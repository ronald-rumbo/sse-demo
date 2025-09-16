import React from 'react';
import { useSSE } from './hooks/useSSE';
import { TaskCard } from './components/TaskCard';
import { LoadingSpinner } from './components/LoadingSpinner';
import { EmptyState } from './components/EmptyState';
import { ConnectionStatus } from './components/ConnectionStatus';
import './App.css';

const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000';

function App() {
  const { tasks, isConnected, error } = useSSE(`${API_URL}/api/tasks/stream`);

  return (
    <div className="app">
      <header className="app-header">
        <h1 className="app-title">TO-DO List</h1>
        <p className="app-subtitle">Real-time task management with live updates</p>
        <ConnectionStatus isConnected={isConnected} error={error} />
      </header>

      <main className="app-main">
        {!isConnected && tasks.length === 0 ? (
          <LoadingSpinner />
        ) : tasks.length === 0 ? (
          <EmptyState />
        ) : (
          <div className="tasks-container">
            <div className="tasks-header">
              <h2 className="tasks-title">Tasks ({tasks.length})</h2>
            </div>
            <div className="tasks-grid">
              {tasks.map((task) => (
                <TaskCard key={task.id} task={task} />
              ))}
            </div>
          </div>
        )}
      </main>

      <footer className="app-footer">
        <p>Powered by React, .NET 9.0, and Server-Sent Events</p>
      </footer>
    </div>
  );
}

export default App;