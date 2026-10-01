# Payload schemas

Payload schema policies validate selected application messages; they do not authenticate publishers or replace [topic policies](policies.md). Open **Configure > Payload Schemas**. Choose a narrow topic filter, the supported schema/mode and limits, validate the definition, then save with the current revision.

```text
coremq schema-policies list
coremq schema-policies validate --schema
coremq schema-policies put telemetry --schema
coremq schema-policies validate --file schema-policy.json
coremq schema-policies put telemetry --file schema-policy.json
coremq schema-policies metrics
```

Use the schema from your installed CLI for the exact policy shape, discriminator, mode names and resource bounds. These request schemas describe configuration; they are distinct from the payload schema you supply inside the policy. Start in the supported observation mode when the release offers it, verify counters against representative valid/invalid payloads, then adopt enforcing behavior only after compatibility review. Do not infer schema-registry serialization, transformations or downstream validation from a successful broker policy check.

Check invalid JSON, missing required fields, oversized payloads and wildcard boundaries. Confirm the intended rejection result at the publishing client's QoS and inspect metrics. Schema validation adds CPU and latency; include it in [sizing tests](sizing.md). Keep policy revisions and test payloads with your deployment record, using sample data.

Remove only the selected policy using its current expected revision. A conflict requires rereading and reviewing the competing change. Remote editor availability depends on broker capabilities and portal packages; [remote management](remote-management.md) explains package compatibility and capability-based controls.

Operational notifications use their own versioned [system event schema](system-events.md); they are best-effort observations, not a durable schema-validation audit.
