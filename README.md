# TO-DO List Web Application

A modern, real-time TO-DO List web application built with React, .NET 9.0, and PostgreSQL. Features Server-Sent Events (SSE) for live task updates with a clean, responsive design.

## Features

- **Real-time Updates**: Tasks update automatically via Server-Sent Events (SSE)
- **Modern UI**: Clean, light-themed interface with responsive design
- **TypeScript**: Full TypeScript support for type safety
- **Containerized**: Complete Docker setup for easy deployment
- **PostgreSQL**: Robust database with sample data
- **No Authentication**: Simple access for demo purposes

## Technology Stack

- **Frontend**: React 18 with TypeScript
- **Backend**: .NET 9.0 Web API
- **Database**: PostgreSQL 15
- **Real-time**: Server-Sent Events (SSE)
- **Deployment**: Docker & Docker Compose

## Quick Start

### Prerequisites

- Docker Desktop
- Docker Compose

### Running the Application

1. Clone the repository and navigate to the project directory
2. Start all services with Docker Compose:

```bash
docker-compose up --build
```

3. Access the application:
   - **Frontend**: http://localhost:3000
   - **Backend API**: http://localhost:5000
   - **Swagger UI**: http://localhost:5000/swagger

### Development Mode

For development with hot reload:

```bash
# Start database only
docker-compose up database -d

# Run backend in development mode
cd backend
dotnet watch run --urls http://0.0.0.0:5000

# Run frontend in development mode
cd frontend
npm install
npm start
```

## Project Structure

```
demo-sse-net/
├── backend/                 # .NET 9.0 Web API
│   ├── Controllers/         # API controllers
│   ├── Data/               # Entity Framework context
│   ├── Models/             # Data models
│   ├── Services/           # Business logic & SSE service
│   └── Dockerfile
├── frontend/               # React TypeScript app
│   ├── public/             # Static assets
│   ├── src/
│   │   ├── components/     # React components
│   │   ├── hooks/          # Custom hooks (SSE)
│   │   └── types/          # TypeScript definitions
│   └── Dockerfile
├── database/               # Database initialization
│   └── init.sql           # Schema & sample data
└── docker-compose.yml     # Multi-container setup
```

## API Endpoints

- `GET /api/tasks` - Retrieve all tasks
- `GET /api/tasks/stream` - SSE endpoint for real-time updates

## Database Schema

```sql
CREATE TABLE tasks (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(255) NOT NULL,
    description TEXT,
    tags TEXT[] DEFAULT '{}',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

## Features Implemented

- ✅ Real-time task display with SSE
- ✅ Responsive, modern UI design
- ✅ PostgreSQL database with sample data
- ✅ Docker containerization
- ✅ TypeScript support
- ✅ Error handling and connection status
- ✅ Loading states and empty states
- ✅ Tag visualization with dynamic colors

## Browser Support

- Chrome/Chromium (recommended)
- Firefox
- Safari
- Edge

*Note: Server-Sent Events are supported in all modern browsers*

## Troubleshooting

### Common Issues

1. **Port Conflicts**: Ensure ports 3000, 5000, and 5432 are available
2. **Docker Issues**: Try `docker-compose down -v` to reset volumes
3. **SSE Connection**: Check browser developer tools Network tab for SSE stream

### Logs

View service logs:
```bash
docker-compose logs backend
docker-compose logs frontend
docker-compose logs database
```

## Contributing

This is a demo application. Feel free to explore and modify the code to understand the implementation of real-time web applications with SSE.

## License

This project is for demonstration purposes.# sse-demo
# sse-demo
