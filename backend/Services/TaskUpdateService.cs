using System.Collections.Concurrent;
using System.Text.Json;
using TodoApp.Models;

namespace TodoApp.Services;

public interface ITaskUpdateService
{
    Task AddClientAsync(string clientId, HttpResponse response);
    Task RemoveClientAsync(string clientId);
    Task NotifyTasksUpdatedAsync(IEnumerable<TaskModel> tasks);
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}

public class TaskUpdateService : ITaskUpdateService, IHostedService
{
    private readonly ConcurrentDictionary<string, StreamWriter> _clients = new();
    private readonly ILogger<TaskUpdateService> _logger;

    public TaskUpdateService(ILogger<TaskUpdateService> logger)
    {
        _logger = logger;
    }

    public async Task AddClientAsync(string clientId, HttpResponse response)
    {
        response.Headers["Content-Type"] = "text/event-stream";
        response.Headers["Cache-Control"] = "no-cache";
        response.Headers["Connection"] = "keep-alive";
        response.Headers["Access-Control-Allow-Origin"] = "*";

        var writer = new StreamWriter(response.Body);
        _clients.TryAdd(clientId, writer);

        await writer.WriteLineAsync("data: {\"type\": \"connected\"}\n");
        await writer.FlushAsync();

        _logger.LogInformation("SSE client {ClientId} connected", clientId);
    }

    public Task RemoveClientAsync(string clientId)
    {
        if (_clients.TryRemove(clientId, out var writer))
        {
            writer?.Dispose();
            _logger.LogInformation("SSE client {ClientId} disconnected", clientId);
        }
        return Task.CompletedTask;
    }

    public async Task NotifyTasksUpdatedAsync(IEnumerable<TaskModel> tasks)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var data = JsonSerializer.Serialize(new { type = "tasks_updated", tasks }, options);
        var message = $"data: {data}\n\n";

        var disconnectedClients = new List<string>();

        foreach (var kvp in _clients)
        {
            try
            {
                await kvp.Value.WriteLineAsync(message);
                await kvp.Value.FlushAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send update to client {ClientId}", kvp.Key);
                disconnectedClients.Add(kvp.Key);
            }
        }

        foreach (var clientId in disconnectedClients)
        {
            await RemoveClientAsync(clientId);
        }

        _logger.LogInformation("Notified {ClientCount} clients of task updates", _clients.Count);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TaskUpdateService started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var client in _clients.Values)
        {
            client?.Dispose();
        }
        _clients.Clear();
        _logger.LogInformation("TaskUpdateService stopped");
        return Task.CompletedTask;
    }
}