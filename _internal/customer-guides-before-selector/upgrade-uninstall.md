# Upgrade, roll back and uninstall

Only upgrade to an approved release for your provider/edition/architecture. Record the current digest/chart/CLI/portal versions, backup and restore-test evidence, dependency versions, accepted feature limits and maintenance owner. [Release status](release-status.md) separates passed builds from live qualification.

## Upgrade

1. Read the exact release's notes, schema/migration requirements and feature flags. Preserve current values, Secret references, state volumes, certificates and CoreControl protection key. Download and verify immutable artifacts before the maintenance window.
2. Test the change in DEV/TEST with representative sessions, retained messages, replay, policies, TLS and bridges. Confirm compatible portal/CLI packages and capability versions. Define abort conditions.
3. Take a consistent backup and prevent conflicting configuration writers. Roll one controlled instance or use the release's qualified rollout procedure; do not assume concurrent startup/migrations are safe on every provider.
4. For Helm deployments, render the exact approved chart with reviewed private values, then use `helm upgrade ... --wait --timeout ...`. Read the running image IDs and replica readiness after rollout. Helm success alone does not prove messaging or retained state.
5. Test allowed and denied client actions, persistent-session replay, certificate-verified connections, bridge delivery, schema/system-event behavior, and CoreControl catalog/health freshness. Track latency/errors/backlog against predeclared thresholds. Retain the old image and recoverable state until the observation window ends.

For the newer AWS opt-in CoreControl chart, a bootstrap Secret is for one enrollment Job. Remove it after successful enrollment; subsequent upgrades retain the key/PVC and omit the redeemed artifact. Reusing an expired/redeemed artifact cannot recover an empty volume.

## Rollback

Stop the rollout on readiness loss, unacceptable delivery/replay results or failed authorization. Determine whether migrations or writes changed persistent state before switching binaries. **An older image may not understand the new schema.** A Helm rollback restores Kubernetes manifests, not database/Redis/file snapshots or revoked credentials.

Use the pretested compatible rollback release. If state restore is required, quiesce traffic and follow [backup and restore](backup-restore.md), explicitly accounting for messages accepted since the backup. Verify local recovery access, policies, certificate validity, client sessions, bridge duplicate handling and remote command state before restoring traffic. Keep failure evidence and record the observed recovery point/time.

> [!WARNING]
> Preserve the required backup and prove resource ownership and dependencies before removing storage or shared infrastructure. An uninstall can remove the recovery path.

## Uninstall without losing customer data

1. Inventory what the installation owns: release/namespace, workloads, service accounts/IRSA, temporary Secrets, state claims, databases, Redis, ingress/IPs, DNS, registries and log storage. Identify shared resources, retention obligations and a recovery path.
2. Stop or drain producers and bridges; take and verify backups. Revoke CoreControl deployment access server-side when retiring it. Remove local encrypted credentials through the supported process as well.
3. Remove the owned application via its provider's supported uninstall route. For Helm, target the exact owned release/namespace; verify pods, Jobs and Services are gone. Uninstalling the chart does not authorize deleting pre-created PVCs or shared databases.
4. Remove only separately owned, obsolete credentials/IAM grants, DNS/networking and temporary infrastructure after dependency checks. Retain required customer data and rollback backups. Local evaluation container removal and data-volume deletion are separate actions.
5. Verify the resource inventory again, including billable storage/IPs and orphaned revisions; save a cleanup receipt. Revisit actual charges after billing catches up. Expiry tags and a pipeline timeout do not establish that charges stopped.

If uninstall fails, preserve data and collect safe resource IDs/status for support. Do not force-delete a namespace or shared claim to make the status look clean.
