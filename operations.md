# Operate and diagnose CoreMQ

Keep a deployment record with the broker/CLI/portal versions, immutable image digests, chart version, architecture, endpoint names, dependency versions, storage owners, certificate expiry, backup location and restore-test date. Store secret references separately from credentials. Check [versions and packages](release-status.md) before adopting a new feature.

## Daily checks

```text
coremq about
coremq health live
coremq health ready
coremq status --output json
coremq connections list
coremq bridges status
coremq schema-policies metrics
coremq security audit list --offset 0 --limit 100
coremq watch status --interval 5 --count 12
```

Liveness means the process can respond. Readiness includes configured dependencies. Compare counters and node status with the expected inventory; inspect all replicas rather than only the load-balanced response. Watch emits timestamped samples; a finite watch fails if any sample fails, even after recovery. Audit pagination returns one page per invocation.

Set alerts for readiness loss, connection churn, publish/delivery errors, queue saturation, replay backlog, bridge retries, schema rejection, Redis memory/evictions/persistence, database failures, disk usage and certificate expiry. Establish workload-specific thresholds and an on-call owner. Record dropped system events separately: the event stream is best-effort and cannot replace a durable audit or health probe. Retain logs for the required support/security period with volume and cost limits; avoid unrestricted payload logging.

## Troubleshooting by symptom

| Symptom | Check in this order | Verification |
| --- | --- | --- |
| Broker is alive but not Ready | Database/Redis reachability, credentials via secret references, TLS and dependency health; migrations/permissions | Each replica returns Ready and a real client round trip succeeds |
| Client cannot connect | DNS, TCP path/Service ports, listener enabled, certificate chain/hostname/expiry, authentication type, account enabled | Certificate-verified connection succeeds without an insecure option |
| Connected but message missing | Exact topic/filter, policy precedence, subscription QoS/session settings, schema rejects, routing/queue counters | Allowed narrow topic arrives; denied topic remains blocked |
| CLI returns HTML | Wrong management origin, ingress routing, frontend placeholder | Management API returns JSON; CLI connects to HTTP(S), not 1883/8883 |
| UI reads fail while CLI works in parallel local tests | Browser cookie collision across brokers sharing hostname/path despite different ports; use isolated profiles/hostnames and fresh sign-in | Editor and native CLI both read the intended broker |
| HTTP 401 / 403 | Expired token versus missing permission; broker local roles versus organization mappings and remote policy | Reauthenticate for 401; verify intended permissions for 403, without broadening them blindly |
| Revision conflict | Another writer changed state; saved provider revision differs from CoreControl numeric revision | Reread, review diff, use current revision; never silently retry |
| Provider saved but login unchanged | Pending restart, exact callback/issuer/client/audience, tenant and role claims, trusted broker origin | Restart safely, use a fresh private browser session, test mapped and denied identities |
| CoreControl disconnected | Enabled cloud build, enrolled credential plus separate key, environment/audience/gateway trust, HTTPS egress and Redis coordination | Catalog/health freshness and reconnect verified in the portal; MQTT remains healthy |
| Bridge connected but no forwarding | Explicit direction/filter, destination topic exists, Secret reference, queue/retry/rate/hop limits | Destination consumer sees a uniquely identified message; inspect duplicate handling |
| Certificate refresh fails | Deployment uses mounted material or disallows API upload; supplied PFX/hostname/trust | Apply through the supported deployment channel and verify a new TLS handshake |

Do not repeatedly restart a dependency outage into apparent recovery. Preserve failure timestamps and the exact broker version, inspect queue and durable state, then run the recovery checks appropriate to your configuration.

## Safe diagnostics and support handoff

```text
coremq diagnostics run --timeout 10 --report coremq-diagnostics.json
```

The timeout applies to each check. The report excludes response bodies, tokens, passwords and payloads, but includes the target origin and timestamps. Inspect it for private hostnames before sharing. Denied API checks can indicate insufficient role access rather than an outage. Broker logs remain in the existing deployment logging system; the CLI does not retrieve them.

Send support the exact versions/digests, platform, environment, symptom and UTC time range, expected versus observed result, safe diagnostics, relevant redacted logs and operation IDs. Include reproduction using sample data and indicate whether the problem occurs locally or through CoreControl. Never send saved CLI tokens, bootstrap artifacts, connection strings, private keys, PFX files, raw message payloads or full Kubernetes Secret/Helm output. Use your contracted support channel; [feedback](feedback.md) is a consent-based report, not an incident SLA.

For data recovery use [backup and restore](backup-restore.md). For a version failure use [upgrade and rollback](upgrade-uninstall.md).
