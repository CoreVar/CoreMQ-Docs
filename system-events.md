# Broker system events

CoreMQ publishes versioned operational notifications beneath `$SYS/coremq/v1`.
MQTT defines special wildcard handling for `$` topics; `$SYS` is the established
convention for broker information, rather than an MQTT-defined event schema.

## Subscriptions

| Interest | Filter |
| --- | --- |
| All CoreMQ events | `$SYS/coremq/v1/#` |
| All client events | `$SYS/coremq/v1/nodes/+/endpoints/+/clients/+/#` |
| One client across nodes and endpoints | `$SYS/coremq/v1/nodes/+/endpoints/+/clients/sensor-17/#` |
| All client connections | `$SYS/coremq/v1/nodes/+/endpoints/+/clients/+/connected` |
| Endpoint lifecycle | `$SYS/coremq/v1/nodes/+/endpoints/+/lifecycle/endpoint/+` |
| Bridge connectivity | `$SYS/coremq/v1/nodes/+/bridges/+/+` |

A subscription to `#` does **not** include system events. Grant the monitoring
account an explicit subscribe policy for the desired `$SYS/coremq/v1` filter.
System subscriptions default to denied even when ordinary topics default to
allowed. Existing global, role, and account policy precedence applies. Clients
cannot publish under `$SYS`, even with an allow policy. Incoming bridges cannot
inject `$SYS` messages either. This reserves the local system namespace against
forgery; map a remote broker's system events into an ordinary application namespace.

Node IDs, bridge names, and client IDs are URI-escaped as individual topic levels:
`a/b` becomes `a%2Fb`, `%` becomes `%25`, and `+` becomes `%2B`. Ordinary IDs such as
`sensor-17` remain readable. Escape once; MQTT does not decode topic names.

## Implemented events

- Clients: `connected`, `disconnected`, `subscribed`, `unsubscribed`.
- Endpoint lifecycle: `started`, `stopped`.
- Bridges: `connected`, `disconnected`, for MQTT and Kafka-compatible connections.

An accepted connection followed by an administrative disconnect can produce both
events. Subscription restoration does not produce a new subscribed event.
Unsubscribe events describe processed unsubscribe requests, including requests
for filters that no longer exist. These are operational observations, not proof
of durable state transitions. A stopped endpoint cannot deliver its own shutdown
notification to clients whose transports have already closed.

Payloads use JSON with `application/json` content type:

```json
{
  "schemaVersion": 1,
  "eventId": "cc209236-bbf5-42a8-9c89-6c785b443d65",
  "occurredAt": "2026-09-11T20:00:00Z",
  "type": "clients.connected",
  "nodeId": "node-1",
  "endpointId": 1,
  "resourceId": "sensor-17",
  "clientId": "sensor-17",
  "topicFilter": null
}
```

Client IDs and subscription filters may themselves contain customer-sensitive
values; they are intentionally included for correlation. Passwords, access
tokens, usernames, certificate contents, network addresses, and message payloads
are not included. Events remain on the broker unless an administrator configures
a bridge route to export them. A Kafka/CoreStream route can explicitly match the
system filter and preserve the JSON payload in a fixed stream topic.

## Delivery and limits

Notifications are non-retained, QoS 0, best-effort, with a 1,024-item in-memory
queue. Queue overflow, delivery failure, shutdown, and process failure can lose
events. `BrokerSystemEvents.DroppedEvents` exposes a process-local loss counter;
delivery failures also produce rate-limited warnings. An escaped topic exceeding
MQTT's 65,535-byte limit is dropped and counted. There is no durable event journal,
offline replay guarantee, or total ordering across nodes. Consumers should use
`eventId` for deduplication and query current state when reconciling status.

Disable emission with `SystemEvents:Enabled=false` (environment variable
`SystemEvents__Enabled=false`). This setting does not remove the reserved-topic
publish protection. Configuration currently uses the broker configuration file
or environment; there is no dedicated Configure UI toggle yet.

Message publish/delivery events are deliberately absent to avoid recursive
monitoring traffic. Authentication failures, certificate expiry, policy changes,
CoreControl enrollment changes, and audit events are not yet connected to this
stream. They must not be inferred from the lifecycle events above.

## Validation status

Focused tests cover topic encoding, wildcard matching and authorization. A local Docker test verified connection/disconnection notifications, escaped client identifiers and blocked forged publications. Bridge lifecycle and multi-node event delivery have not yet been verified end to end.

See [MQTT 5 topic rules](https://docs.oasis-open.org/mqtt/mqtt/v5.0/os/mqtt-v5.0-os.html) and [management coverage](remote-management.md).
