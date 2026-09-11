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
