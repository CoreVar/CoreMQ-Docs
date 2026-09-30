# Endpoints

Open **Configure > Endpoints** to add or edit MQTT listeners. A deployment may restrict endpoint editing according to edition or policy.

An endpoint defines its host and ports, TCP/TLS or WebSocket listeners, authentication settings, and certificate material. Enable only the transports required by your clients. TLS encrypts the connection; client authentication determines who may connect. Enabling TLS alone does not create a client identity or grant message permissions.

Use browser upload controls for certificate material where offered. A path on your workstation is not a path inside the broker container. Existing operator-managed server references must resolve in the broker deployment.

Changing a listener can disconnect clients. Review its address, certificate and authentication settings before saving, then test a client connection using that transport.

CLI examples:

```text
coremq endpoints list
coremq endpoints add --schema
coremq endpoints add --file endpoint.json
coremq endpoints update <id> --file endpoint.json
```

The CLI management URL is an HTTP(S) origin, not the MQTT listener port. See [CLI](cli.md).

## TLS setup and renewal

1. Obtain a certificate for the exact MQTT hostname from your approved issuer. CoreMQ configuration does not promise automatic certificate issuance or DNS creation.
2. Keep its private key protected. For the direct certificate-upload command, the reviewed contract accepts a **passwordless PKCS#12/PFX** containing the required private key. Treat that file as a credential and remove its temporary copy after upload. Protected mounted certificate channels can impose different package requirements.
3. Upload via the browser's managed file control where offered, or use the deployment's supported mounted Secret. A workstation path is not a container path. API upload can be disabled for mounted-certificate Kubernetes deployments.
4. Enable the needed TLS/TLS-WebSocket listener, retain hostname verification at the client, and close plaintext external routes. Listener changes can disconnect clients; plan reconnects.
5. Inspect certificate metadata and perform a new client handshake. Test chain, hostname and expiry; for mTLS also verify client CA, client-auth EKU, identity mapping and revocation behavior.

```text
coremq endpoints certificate show <id>
coremq endpoints certificate upload <id> --file server.pfx
coremq endpoints certificate refresh <id>
```

Track expiry and renew early enough to test and roll back. Stage new material through the approved upload/mount channel, refresh/restart as that release requires, then verify every replica's listener with a new TLS connection. Existing sessions alone cannot prove the renewed certificate is served. Retain the old valid material for the agreed rollback window under protected access. `refresh` reloads available material; it is not proof that a CA issued a new certificate.

![Endpoint list showing listener hostname, ports, authentication, and Edit/Delete controls in the current CoreMQ UI.](assets/configure-endpoints.jpg){width=800 style="max-width:100%;height:auto"}

Captured in a disposable local build on September 30, 2026; its plaintext loopback evaluation listener is not a production configuration. See [backup](backup-restore.md), [upgrade](upgrade-uninstall.md), and [release limits](release-status.md).
