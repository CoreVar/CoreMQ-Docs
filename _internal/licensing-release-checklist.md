# Exact-release licensing clearance

Updated October 5, 2026 from the nonbinding CoreMQ licensing recommendation.
The single canonical CoreMQ Product Plan owns commercial decisions. This
checklist is a release record, not customer terms or an activated EULA.

- [ ] Identify the adopted Marketplace billing model and exact broker-pod scope;
  verify reporting, rounding, reconciliation, cancellation, refunds and boundaries.
- [ ] Verify replacement/standby/upgrade-overlap metering with actual receipts.
- [ ] Record entitlement outage and revocation behavior without bypassing tenant
  or role authorization, and confirm the support purchase/activation route.
- [ ] Review final legal-entity/environment/distribution terms through the actual
  purchase channel before publishing binding terms.
- [ ] For each final image, chart, CLI package, four management NuGet packages and
  bundled acceptance tool, record component/version, source SHA, digest, license
  expression, distribution boundary, notices/source offers and reviewer disposition.
- [ ] Include base-image OS packages, transitive dependencies and browser assets.
  An SBOM or vulnerability pass does not itself establish license clearance.
- [ ] Verify the exact MQTTnet fork's MIT notice and modifications; preserve the
  html2canvas notice and all other applicable notices in shipped artifacts.
- [ ] Record the exact Redis server version and selected license terms separately
  from StackExchange.Redis. Distinguish customer-supplied services from redistributed
  servers. Do not assume automatic clearance or automatic CoreMQ copyleft obligations.
- [ ] Retain unresolved license expressions as release blockers. An alternate
  backend needs its own API/Lua, recovery, failover, security and performance tests.
- [ ] Preserve final adopted terms, third-party notices, SBOM dispositions and
  metering/support receipts alongside the immutable release.

No current-image/native ACK/Redis restart/SMB/capacity/Portal end-to-end acceptance
is inferred from DEV2771/2780 unit results or the historical V12B recovery fixture.
