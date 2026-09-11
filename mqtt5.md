# MQTT 5 support

CoreMQ implements MQTT 5 behavior on MQTTnet 5.2.0.1603 with additional broker policy, routing and persistence handling. Do not interpret this as complete MQTT 5 conformance or formal certification.

A recent development TCP conformance run completed 50 cases: **41 passed, 0 failed, 6 unsupported, 3 not run**. The unrun cases concerned WebSocket/TLS/mTLS in that particular run; other transport checks do not make this run complete.

Verified areas include MQTT 5 connection negotiation, session resume, ordinary QoS 0/1/2 exchange, retained message operations, and several MQTT 5 properties and subscription options. Crash durability, cross-node behavior and configured limits have additional qualification requirements.

## Known protocol-engine gaps

- Client Receive Maximum enforcement.
- Updating Session Expiry from DISCONNECT.
- Rejecting zero or repeated Subscription Identifier properties.
- Rejecting repeated singleton PUBLISH properties.
- Rejecting an invalid Will Topic in the audited case.

Post-connect AUTH reauthentication and the requested Will-on-DISCONNECT lifecycle also remain incomplete. Outbound topic aliases and server redirects are not implemented; those are optional features rather than proof by themselves of protocol nonconformance.

Proposed upstream fixes are not a released, qualified broker dependency. A newer broker release must repeat the relevant wire tests before removing these limitations from its documentation.
