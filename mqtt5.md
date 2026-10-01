# MQTT 5 client integration

CoreMQ accepts MQTT 5 clients for connections, persistent sessions, QoS 0/1/2 messaging, retained messages and subscription configuration. Select an MQTT 5-capable client and test its settings against your broker and listener.

## Connect and resume a session

Choose a stable client ID when using persistent sessions. Configure Clean Start and Session Expiry on CONNECT, then verify reconnect and offline replay with the same client ID. Use [endpoints](endpoints.md) to configure TLS and authentication.

## Publish and subscribe

Choose QoS and retained-message behavior deliberately. Configure bounded client-side in-flight limits rather than relying on server-enforced Receive Maximum. Send each singleton MQTT property once, use nonzero Subscription Identifiers, and provide a valid Will Topic. These choices keep client packets interoperable and your queues bounded.

Use full topic names when publishing; outbound topic aliases and server redirects are not part of this broker's client workflow. Configure session lifetime on CONNECT and reconnect to change authentication or session settings. Do not depend on post-connect AUTH reauthentication or a Will-on-DISCONNECT extension.

## Plan delivery behavior

Exercise reconnect, retained retrieval and offline-session replay using your application's actual QoS and payloads. Design consumers to handle duplicate delivery and measure ordering for the sessions and topology you use. See [capacity and availability](sizing.md), [backup and restore](backup-restore.md), and [bridges](bridges.md) for dependency and recovery planning.
