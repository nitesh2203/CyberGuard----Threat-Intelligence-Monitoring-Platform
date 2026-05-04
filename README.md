# CyberGuard — Threat Intelligence & Monitoring Platform

This repository contains a minimal end-to-end demo for the CyberGuard threat alert feed feature.

## What is included

- `.NET 8` Web API backend with SignalR
- EF Core SQLite persistence for network events and threat alerts
- React + Vite frontend subscribing to live alerts

## Run the backend

```bash
cd src/CyberGuard.Api
dotnet restore
dotnet run
```

The backend listens on `http://localhost:5000`.

## Run the frontend

```bash
cd client
npm install
npm run dev
```

The frontend listens on `http://localhost:5173`.

## Quick demo flow

- Submit a network event from the React form
- Backend ingests it in `NetworkEventController`
- `ThreatEngine` detects suspicious activity
- If a threat is found, it saves a `ThreatAlert`
- SignalR pushes `NewThreat` to the browser
- React app updates in real time

## Notes

- The quick demo uses SQLite by default for local development.
- You can change the database provider in `Program.cs` to SQL Server for production-style demo scenarios.
