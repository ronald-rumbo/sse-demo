import React from 'react';

export const EmptyState: React.FC = () => {
  return (
    <div className="empty-state">
      <div className="empty-state-icon">📝</div>
      <h3 className="empty-state-title">No tasks found</h3>
      <p className="empty-state-message">
        There are no tasks to display at the moment.
      </p>
    </div>
  );
};