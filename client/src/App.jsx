import ThreatAlertFeed from './components/ThreatAlertFeed';

function App() {
  return (
    <div className="app-shell">
      <header className="app-header">
        <h1>CyberGuard Threat Alert Feed</h1>
        <p>Live network threat alerts streamed from .NET Core SignalR.</p>
      </header>

      <main>
        <ThreatAlertFeed />
      </main>
    </div>
  );
}

export default App;
