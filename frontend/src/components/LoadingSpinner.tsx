import React from 'react';

export const LoadingSpinner: React.FC = () => {
  return (
    <div className="loading-container">
      <div className="loading-spinner"></div>
      <p className="loading-text">Loading tasks...</p>
    </div>
  );
};