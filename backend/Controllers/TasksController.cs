using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TodoContext _context;
    private readonly ITaskUpdateService _taskUpdateService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(TodoContext context, ITaskUpdateService taskUpdateService, ILogger<TasksController> logger)
    {
        _context = context;
        _taskUpdateService = taskUpdateService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
    {
        try
        {
            var tasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks");
            return StatusCode(500, "An error occurred while retrieving tasks");
        }
    }

    [HttpGet("stream")]
    public async Task GetTaskStream()
    {
        var clientId = Guid.NewGuid().ToString();

        try
        {
            await _taskUpdateService.AddClientAsync(clientId, Response);

            var tasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            await _taskUpdateService.NotifyTasksUpdatedAsync(tasks);

            var cancellationToken = HttpContext.RequestAborted;
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("SSE stream cancelled for client {ClientId}", clientId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SSE stream for client {ClientId}", clientId);
        }
        finally
        {
            await _taskUpdateService.RemoveClientAsync(clientId);
        }
    }

    [HttpPost]
    public async Task<ActionResult<TaskModel>> CreateTask([FromBody] CreateTaskRequest request)
    {
        try
        {
            var task = new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description ?? string.Empty,
                Tags = request.Tags ?? Array.Empty<string>(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Notify all SSE clients of the update
            var allTasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            await _taskUpdateService.NotifyTasksUpdatedAsync(allTasks);

            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, "An error occurred while creating the task");
        }
    }

    [HttpPost("trigger-update")]
    public async Task<ActionResult> TriggerUpdate()
    {
        try
        {
            var tasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            await _taskUpdateService.NotifyTasksUpdatedAsync(tasks);
            return Ok(new { message = "SSE update triggered", taskCount = tasks.Count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error triggering update");
            return StatusCode(500, "An error occurred while triggering the update");
        }
    }
}

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string[]? Tags { get; set; }
}