-- Create the tasks table
CREATE TABLE IF NOT EXISTS tasks (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(255) NOT NULL,
    description TEXT,
    tags TEXT[] DEFAULT '{}',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create a trigger to update modified_at on row updates
CREATE OR REPLACE FUNCTION update_modified_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.modified_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER update_task_modified_at
    BEFORE UPDATE ON tasks
    FOR EACH ROW
    EXECUTE FUNCTION update_modified_at_column();

-- Insert sample data
INSERT INTO tasks (title, description, tags) VALUES
    ('Complete project documentation', 'Write comprehensive documentation for the new feature including API specs and user guide', ARRAY['documentation', 'urgent', 'project']),
    ('Review code changes', 'Review pull requests for the authentication module and provide feedback', ARRAY['code-review', 'backend']),
    ('Design database schema', 'Create ERD and design optimal database schema for user management system', ARRAY['database', 'design', 'architecture']),
    ('Setup CI/CD pipeline', 'Configure automated testing and deployment pipeline using GitHub Actions', ARRAY['devops', 'automation', 'ci-cd']),
    ('Implement user authentication', 'Add JWT-based authentication with refresh tokens and role-based access control', ARRAY['backend', 'security', 'authentication']),
    ('Create responsive UI components', 'Build reusable React components with mobile-first responsive design', ARRAY['frontend', 'ui', 'responsive']),
    ('Optimize database queries', 'Analyze and optimize slow-running queries, add appropriate indexes', ARRAY['database', 'performance', 'optimization']),
    ('Write unit tests', 'Achieve 90% code coverage with comprehensive unit and integration tests', ARRAY['testing', 'quality-assurance']),
    ('Deploy to production', 'Deploy application to production environment with monitoring and alerting', ARRAY['deployment', 'production', 'monitoring']),
    ('Update API documentation', 'Keep OpenAPI/Swagger documentation up to date with latest endpoints', ARRAY['documentation', 'api']);