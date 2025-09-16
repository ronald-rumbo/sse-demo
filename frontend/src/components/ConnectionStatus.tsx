import React from 'react';

interface ConnectionStatusProps {
  isConnected: boolean;
  error: string | null;
}

export const ConnectionStatus: React.FC<ConnectionStatusProps> = ({ isConnected, error }) => {
  if (error) {
    return (
      <div className="connection-status error">
        <span className="status-indicator">⚠️</span>
        <span className="status-text">{error}</span>
      </div>
    );
  }

  return (
    <div className={`connection-status ${isConnected ? 'connected' : 'disconnected'}`}>
      <span className="status-indicator">
        {isConnected ? '🟢' : '🔴'}
      </span>
      <span className="status-text">
        {isConnected ? 'Real-time updates active' : 'Connecting...'}
      </span>
    </div>
  );
};