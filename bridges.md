# Bridges and stream destinations

A bridge routes messages between CoreMQ and another messaging system. It does not merge broker users, sessions or configuration, and it does not automatically enroll or pair the remote system.

## MQTT bridges

Open **Bridges**, choose **Create bridge**, and enter a name and destination. MQTT profiles are **Upstream**, **Peer**, and **External**; these classify the connection while using the same MQTT routing engine.

Configure TLS, authentication and uploaded trust material as required by the remote broker. Add explicit topic routes for **Local to remote** or **Remote to local**. Configure separate routes for each direction. JSON direction values remain `LocalToUpstream` and `UpstreamToLocal` for compatibility.

Validate the configuration, save it, and inspect runtime status. Queue limits, retention, retry limits, maximum hops and message rate are configurable. Do not infer lossless delivery or arbitrary mesh interoperability from a successful connection.

## Kafka, Azure Event Hubs and CoreStream

The development implementation adds **Kafka**, **EventHubs**, and **CoreStream** profiles for outbound delivery to Kafka-compatible listeners. For example, map MQTT filter `devices/+/telemetry` to fixed Kafka topic `device-telemetry`.

- Only local-to-remote delivery is implemented for these profiles.
- The destination topic must already exist; automatic creation is disabled.
- Payload bytes are preserved. The route name is the Kafka key; message ID and origin are record headers.
- Kafka supports TLS and optional SASL/PLAIN credentials. Event Hubs requires TLS and uses `$ConnectionString` as the username with the configured connection-string secret.
- Use CoreStream's Kafka listener. Actual CoreStream and Azure Event Hubs service acceptance remains outstanding.
- Reverse consumption, SCRAM selection, OAuth credential refresh, Kafka client-certificate authentication, arbitrary payload transformations and schema-registry serialization are not implemented.

Messages leave the durable bridge queue after acknowledged production. Uncertain acknowledgements, restarts and lease changes can cause duplicates. Queue saturation can skip forwarding. Producer idempotence is not an end-to-end exactly-once guarantee. A disposable Kafka producer/consumer test passed; cluster lease/failover qualification remains outstanding.

## Management

```text
coremq bridges list
coremq bridges status
coremq bridges validate --file bridge.json
coremq bridges put <id> --file bridge.json
coremq bridges remove <id> --expected-revision <revision>
```

Use `--schema` to obtain the request format. Local APIs use `/api/Bridges/Configuration` and `/api/Bridges/Validate`; `/api/Uplinks` and CLI `uplinks` aliases remain compatible. CoreControl uses the existing versioned uplink capabilities. See [remote management](remote-management.md).

To export system events, explicitly match their `$SYS/coremq/v1` filter. Incoming bridges cannot forge local `$SYS` publications; map remote system topics into an ordinary application namespace.
