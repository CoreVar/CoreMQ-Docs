# Release behavior and known limitations

This documentation review is dated **September 30, 2026**. Its implementation baseline is Broker commit `e5b5eb112f8b7c2d8a4821fbaba99778d81741e7`. A feature in that source is not automatically present in an installed Marketplace image. Read **Configure > About**, record immutable artifacts and consult the exact release's support statement before enabling it.

| Area | Evidence / implementation | Supported-use limit |
| --- | --- | --- |
| AWS messaging/replay/outage/upgrade | EKS acceptance 1813 for candidate 1754, private promotion 1763; independent cleanup `DELETE_VERIFIED` | Scoped acceptance; CoreControl incomplete and production horizontal capacity unqualified |
| Newer AWS candidate | Builds 1862/1863 at reviewed `e5b5eb1` passed; enrollment Job now meters its paid pod | Fresh real DEV CoreControl enrollment, catalog/health reads and two-local-replica takeover passed against Azure image config `a7b18d1a…`; private AWS publication, lifecycle writes, replay, upgrade, rotation/revocation remain separately gated |
| CoreControl | Opt-in cloud connector, encrypted credentials, capability/revision contracts and coordinated transport | Local edition excludes connector; complete cross-portal mutation and multi-replica qualification remains open |
| Local Configure editors | September 30 source-bound synthetic capture verifies identities/endpoints and system-event/provider reads; narrow CLI policy samples plus MQTT publish/delivery and out-of-scope publish/subscribe denial pass | Scoped local checks do not qualify remote writes, guided tenant onboarding or cloud packages |
| Browser OIDC / CoreID | Local provider editor/CLI, independent organization directory and role mappings | Guided local CoreID versus remote onboarding have distinct qualification; manual changes require restart |
| MQTT 5 | MQTTnet fork and broker handling; historical partial runs and later scoped transport fixtures | No complete MQTT 5/certification claim; gaps must be resolved against the exact shipped dependency |
| Stream bridges | Outbound Kafka-compatible producer profiles and disposable Kafka acceptance | Real CoreStream/Event Hubs, reverse consumption and cluster failover not qualified |
| Persistence providers | Local SQLite; cloud relational/document implementations | PostgreSQL/Redis evidence does not qualify arbitrary SQL, SQL Server or every document provider |
| HA/capacity / ordered runtime | Exploratory tests and ongoing recovery work | No linear scaling or production ceiling; ordered-runtime candidate held |
| Certificates | Upload/reload and deployment-mounted channels | No automatic certificate issuance promise; permitted channel depends on deployment policy |
| GCP | Provider-specific packaging and runtime metadata | Provider/commercial approval and real customer acceptance pending |

Historical reports retain their original outcomes. A strict recovery report with 48 passes and 2 NOT_RUN remains incomplete even if a separate full-transport fixture passed 50/50. A build, package scan, capability manifest or visible portal control cannot substitute for an authorized end-to-end action with read-back.

For production use, retain qualified immutable artifacts, repeat your workload's reliability/capacity checks, test restore and rollback, verify provider entitlement and management authorization, and resolve relevant gaps before promotion. These documentation improvements do not change product qualification.

See [MQTT limits](mqtt5.md), [remote coverage](remote-management.md), [bridge limits](bridges.md), [sizing](sizing.md), and the [documentation assessment](quality-review.md) for residual documentation gaps.
