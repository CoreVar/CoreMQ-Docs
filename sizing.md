# HA and sizing constraints

**No production throughput, connection ceiling, linear-scaling guarantee or automatic failover SLO is qualified by the evidence cited in these docs.** More replicas do not establish more usable capacity. Use the exact candidate and workload in your sizing test.

The cloud implementation coordinates brokers through Redis and persistent storage. This **backplane** carries broker coordination/routing state. A [bridge](bridges.md) forwards selected MQTT messages to a different broker or stream destination. Neither replaces the other, and a bridge is not CoreControl remote management.

## Before calling a topology HA

Verify independent node/failure-domain placement, spare scheduling space during rollout, dependency availability, durable volumes and permissions, DNS/ingress health, certificate continuity, and Redis/database recovery. A disruption budget only constrains voluntary disruptions; it does not protect against node or dependency failure. A shared ReadWriteMany mount does not prove safe concurrent configuration or restart activation.

Keep remote/configuration writes disabled where multi-replica coordination and activation are not qualified. Use an explicit maintenance procedure for provider changes that require restart. Do not use an unqualified HPA to scale around a shared Redis or replay bottleneck.

## Measure your workload

Record publishers/subscribers, sustained and burst messages per second, payload sizes, QoS mix, TLS/mTLS, topic fan-out, shared subscriptions, retained count/size, session expiry, offline replay, bridge volume and schema complexity. Set latency/error/duplicate/order objectives and a minimum failure/rollout headroom requirement before testing.

Measure broker CPU, resident memory, garbage collection, network, queue depth, relay lag, Redis CPU/memory/evictions/persistence and database/storage latency. Use sufficient independent generators to show the clients are not the bottleneck. Repeat steady-state, burst, reconnect, dependency-outage, replica-loss and upgrade tests, then compare one/two/more replicas at equal total workload. Retain raw results and immutable image/dependency hashes.

Historical exploratory single observations at approximately 6k/12k/12k/12k durable targets do not define a supported sizing curve. A separate strict VM recovery run recorded 156 failed observations. The ordered-runtime workstream remains held pending hash-bound qualification. AWS EKS run 1813 is a scoped functional/recovery test, not a production capacity benchmark.

## Cost and capacity decision

Reuse a suitable existing cluster and dependencies. Pick the smallest measured viable node/replica settings while retaining the defined headroom and availability. Estimate the entire monthly footprint, including software replica-hours, control plane, nodes, Redis/database, storage/backups, ingress/IP, egress and logs. Tag temporary tests with owner/lifetime and verify teardown. Report expected savings separately from actual billed savings; recheck after billing settles.

Promote cost or capacity changes through DEV and TEST before guarded PROD promotion. Do not shrink PROD simply because current utilization is low. Keep the measured rollback topology and preserved data available.

A fresh September 30 native-default replay check for candidate 1863 delivered all 1,284 accepted messages with zero missing/duplicates, but first-arrival order failed (156–1155 before 0–155). Its receipt hash is `a2b593f53ce31b9c239248dbc9cc47aa4141d2130b3d1b249908ac3ba9a12160`; owned cleanup passed. This current ordering failure is distinct from historical missing-message observations and does not establish permanent loss. The production capacity hold remains.

The matched 1863 other-live-node/default local control passed all 1,284 messages with zero missing, duplicates, unexpected messages or offline first-arrival inversions. Receipt SHA-256: `ac781b7d9ef0a423fabf3d2da6bc4d96036c0bc64d1604fd263bed5f44b893e1`. Both disposable fixtures were cleaned. This is a local correctness control, not an independent VM or capacity result.
