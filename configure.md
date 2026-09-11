# Configure and monitor CoreMQ

Use **Configure** in the broker's left navigation. Existing links under `/Setup` remain valid; the route name does not imply an older UI.

Available tabs depend on your permissions:

- **Identities**: local users and certificate identities for devices or services.
- **Roles** and **Global Policies**: message authorization.
- **Certificate authorities**: trust material for certificate authentication.
- **Payload Schemas**: payload validation policies.
- **Endpoints**: listener and authentication settings.
- **Sign-in providers**: external browser login configuration.
- **About**: broker version and edition.

The dashboard shows broker status and activity charts. The refresh indicator shows the sampling interval. **Connections** shows current connections; **Topics** shows subscription information. A configured identity can exist while disconnected, so it is not the same thing as a connection.

Certificate identity connection status is available with identity management. The current local navigation no longer has a separate Fleet section; older APIs and remote surfaces may retain that name for compatibility.

Use **Bridges** to list existing connections to other brokers or stream destinations, then **Create bridge** to open the editor. Browser certificate and secret inputs use managed file uploads where offered, not paths on the browser user's computer.

Updates may require reconnecting clients or restarting the broker; observe the confirmation and pending-state information for that operation. The browser and management portals can differ when their package versions differ. See [management coverage](remote-management.md).
