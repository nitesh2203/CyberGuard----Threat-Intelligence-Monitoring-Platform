import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api',
});

const initialAlerts = [];

function ThreatAlertFeed() {
  const [alerts, setAlerts] = useState(initialAlerts);
  const [status, setStatus] = useState('Connecting...');
  const [ip, setIp] = useState('203.0.113.42');
  const [eventType, setEventType] = useState('PortScan');
  const [source, setSource] = useState('Firewall-01');

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/threathub')
      .withAutomaticReconnect()
      .build();

    connection.on('NewThreat', (alert) => {
      setAlerts((prevAlerts) => [alert, ...prevAlerts]);
    });

    connection
      .start()
      .then(() => setStatus('Connected'))
      .catch((error) => setStatus(`Connection failed: ${error}`));

    return () => {
      connection.stop();
    };
  }, []);

  const sendEvent = async (event) => {
    event.preventDefault();

    try {
      const response = await api.post('/networkevent', {
        ip,
        eventType,
        source,
        timestamp: new Date().toISOString(),
      });

      if (response.data?.reason) {
        setStatus('Threat generated and pushed to feed.');
      } else {
        setStatus('Event recorded, no threat generated.');
      }
    } catch (error) {
      setStatus('Failed to send event.');
      console.error(error);
    }
  };

  return (
    <div className="feed-shell">
      <section className="feed-panel">
        <div className="panel-header">
          <h2>Live Threat Alerts</h2>
          <span className="status-chip">{status}</span>
        </div>

        <form className="event-form" onSubmit={sendEvent}>
          <label>
            IP Address
            <input value={ip} onChange={(e) => setIp(e.target.value)} />
          </label>
          <label>
            Event Type
            <select value={eventType} onChange={(e) => setEventType(e.target.value)}>
              <option>PortScan</option>
              <option>FailedLogin</option>
              <option>MalwareSignature</option>
              <option>DataExfiltration</option>
              <option>NormalTraffic</option>
            </select>
          </label>
          <label>
            Source
            <input value={source} onChange={(e) => setSource(e.target.value)} />
          </label>
          <button type="submit">Send Network Event</button>
        </form>

        <div className="alerts-list">
          {alerts.length === 0 ? (
            <div className="empty-state">No alerts received yet.</div>
          ) : (
            alerts.map((alert) => (
              <article key={`${alert.id}-${alert.detectedAt}`} className="alert-card">
                <div className="alert-header">
                  <strong>{alert.eventType}</strong>
                  <span className={`severity-chip severity-${alert.severity.toLowerCase()}`}>
                    {alert.severity}
                  </span>
                </div>
                <div className="alert-body">
                  <p>{alert.reason}</p>
                  <div className="metadata">
                    <span>{alert.ip}</span>
                    <span>{new Date(alert.detectedAt).toLocaleString()}</span>
                  </div>
                </div>
              </article>
            ))
          )}
        </div>
      </section>
    </div>
  );
}

export default ThreatAlertFeed;
