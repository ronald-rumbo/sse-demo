#!/bin/bash

echo "🔄 Testing Real-time SSE Updates"
echo "================================="
echo

echo "📊 Current task count:"
CURRENT_COUNT=$(curl -s http://localhost:8000/api/tasks | jq length)
echo "Tasks: $CURRENT_COUNT"
echo

echo "🚀 Creating a new task via API..."
curl -X POST http://localhost:8000/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test real-time update",
    "description": "This task tests if SSE updates work when adding tasks via API",
    "tags": ["test", "sse", "real-time"]
  }' -s | jq .
echo

echo "📊 New task count:"
NEW_COUNT=$(curl -s http://localhost:8000/api/tasks | jq length)
echo "Tasks: $NEW_COUNT"
echo

if [ "$NEW_COUNT" -gt "$CURRENT_COUNT" ]; then
    echo "✅ Task successfully created!"
    echo "💡 The frontend should now show the new task in real-time"
    echo "🌐 Check http://localhost:3000 to see the update"
else
    echo "❌ Task creation failed"
fi