# Persistence, backup and restore

Define an acceptable recovery point (RPO: how much data may be lost) and recovery time (RTO: how long service may be unavailable). A running second broker is not a backup. Do not claim an RPO/RTO until a restore drill on the same release and topology proves it.

## What must survive

| State | Storage boundary | Backup requirement |
| --- | --- | --- |
| Local accounts, roles, policies and endpoint configuration | Local SQLite; cloud relational provider selected by deployment | Consistent database backup, version/schema record and tested restore |
| Sessions, retained messages and cluster relay/replay state | Standalone stores or configured Redis cloud implementation | Redis persistence/replication plus a coordinated recovery plan; process memory is not durable |
| Browser providers and observed organization directory | File-backed state directory | Consistent file snapshot with provider secret files and version/revision metadata protected separately |
| Uploaded certificates/files and other file configuration | Persistent broker storage path | Snapshot referenced files and metadata together, preserving permissions |
| CoreControl credential | Encrypted file on state volume, separate protection key | Protect both independently; server revocation is authoritative and an old artifact cannot be reused |
| Browser session protection material | Build/deployment-specific data-protection key storage | Preserve approved key store when continuity is intended; allow forced reauthentication after recovery |

The reviewed cloud relational implementation explicitly supports PostgreSQL, SQL Server and SQLite code paths; this is not arbitrary SQL support. Local edition permits SQLite only. Marketplace/EKS acceptance cited here uses PostgreSQL and Redis. Document-store implementations are separate candidates and need provider-specific qualification; do not switch providers as a recovery shortcut.

## Create a backup

1. Record exact image/CLI/chart versions and schema/migration state. Inventory every storage mount and external dependency; assign a recovery owner.
2. Quiesce publishers/bridges and configuration writers according to the approved maintenance plan. Coordinate all replicas. Determine how accepted messages and inflight acknowledgements will be reconciled.
3. For local SQLite, stop the broker cleanly and copy the whole data directory, including database sidecar files if present. Do not copy a live SQLite database as an ordinary file and call it a consistent backup.
4. For cloud PostgreSQL/SQL Server and Redis, use the service's supported consistent backup/export tooling. Capture file state and separate secrets/key material within the same recovery plan. Retain original snapshots, encryption and access controls.
5. Store backups away from the live failure domain with retention and expiry appropriate to customer/security requirements. Record checksums and restore instructions. Resume traffic and verify client delivery.

This is an operator procedure; CoreMQ exposes no single atomic backup/restore CLI for all stores. Independent service snapshots can describe incompatible points in time. A consistency strategy and a drill are required before promising recovery semantics.

## Restore drill

Restore into an isolated target with outgoing bridges and CoreControl disabled initially. Never point a test restore at production Redis, database or an active deployment identity. Verify ownership, dependencies and a recovery path before replacing any live data.

1. Restore the database, Redis state, files and required keys using the approved version's tooling and permissions. Use the recorded image first; avoid applying new migrations during the recovery exercise.
2. Start one controlled broker. Check migrations, liveness/readiness, configuration, local recovery login, identities, roles/policies and certificate validity.
3. Test retained-message retrieval, persistent-session reconnect/replay and inflight QoS behavior using the original session settings and synthetic client IDs. Measure losses, duplicates and order against the declared requirements.
4. Add the remaining intended replicas, test replacement/failover, then enable bridges with duplicate-safe downstream consumers. Reconcile remote command state before reconnecting CoreControl; obtain fresh enrollment if restored credentials are no longer valid.
5. Record observed RPO/RTO, exact versions and failures. Teardown only the drill's owned resources and verify their absence. Keep backup evidence per retention policy.

Current production HA/recovery remains unqualified beyond scoped acceptance; see [sizing constraints](sizing.md) and [release evidence](release-status.md). Never delete shared storage as part of a failed drill.
