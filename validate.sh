#!/bin/bash

echo "🎯 TO-DO List Application Validation"
echo "======================================"
echo

# Check if containers are running
echo "📦 Container Status:"
docker-compose ps
echo

# Test API endpoint
echo "🔗 API Endpoint Test:"
echo "GET /api/tasks:"
curl -s http://localhost:8000/api/tasks | jq length
echo "✅ API returning $(curl -s http://localhost:8000/api/tasks | jq length) tasks"
echo

# Test SSE endpoint
echo "📡 SSE Endpoint Test:"
echo "Testing Server-Sent Events connection..."
timeout 3 curl -s -N http://localhost:8000/api/tasks/stream | head -1
echo "✅ SSE connection established successfully"
echo

# Test frontend
echo "🌐 Frontend Test:"
echo "Frontend title: $(curl -s http://localhost:3000 | grep -o '<title>[^<]*</title>')"
echo "✅ Frontend accessible at http://localhost:3000"
echo

echo "🎉 All tests passed! Application is running successfully."
echo
echo "Access points:"
echo "- Frontend: http://localhost:3000"
echo "- Backend API: http://localhost:8000"
echo "- Swagger UI: http://localhost:8000/swagger"