# Documentation quality review

Review date: September 30, 2026. Baseline: CoreMQ-Docs `d1bebe7`; implementation: Broker `e5b5eb1`. Scale: 1 unusable, 5 partial/development guidance, 8 usable task guidance with explicit limits and validation, 10 complete independently qualified guidance. Equal weighting across the 11 factors. High scores are not awarded to unsupported product claims.

## Before changes

| Factor | Score | Concrete evidence |
| --- | --- | --- |
| Readability | 7 | Clear modern provider/bridge prose, but roles/policies repeat numbered headings and generic conclusions |
| Understandability | 6 | Identities distinguish account types; policy precedence lacks worked outcomes and storage boundaries are missing |
| Accuracy/currentness | 5 | System events incorrectly says no UI toggle; policies says Users tab; AWS source guide still says EKS never ran |
| Discoverability/navigation | 5 | Root index lists guides, without a start-to-operate task path or recovery links |
| Task/use-case coverage | 4 | Azure install exists; local/Docker, AWS/GCP prerequisites, recovery and retirement absent |
| CLI guidance/examples | 6 | Hidden prompt/input/revision notes good, but command coverage and operational examples incomplete |
| UI guidance | 6 | Configure labels useful; no screenshots or qualified local/remote task walkthrough |
| Screenshots/diagrams | 1 | No current product screenshots or architecture diagrams |
| Troubleshooting | 3 | Scattered caveats; no symptom-to-check/verification runbook |
| Accessibility | 5 | Mostly text, but uneven heading structure and no visual alternatives or rendered review |
| Production operations | 2 | No consistent backup/restore, sizing, rollback or uninstall runbook |

**Baseline overall: 4.5/10 (50/11).** Product/source evidence weaknesses cap accuracy and operational confidence.

## After changes

These scores assess the revised guides and scoped local rendering/behavior checks. DEV publication is independently verified from the immutable source; a saved editing ref is not publication. Product qualification remains separate.

| Factor | Before | After | Evidence and remaining limit |
| --- | --- | --- | --- |
| Readability | 7 | 8 | Task headings, concise steps and consistent tables; long CLI reference remains searchable |
| Understandability | 6 | 8 | Architecture channels, edition/storage boundaries and worked authorization outcomes |
| Accuracy/currentness | 5 | 8 | Exact source baseline, corrected UI/dependency/policy claims and historical/current evidence separated; installed package still needs verification |
| Discoverability/navigation | 5 | 8 | Start/deploy/access/operate index and cross-links; preview title filter and hosted version navigation |
| Task/use-case coverage | 4 | 8 | Local/container plus Azure/AWS/GCP/private prerequisites and operate/retire path; non-Azure commercial acceptance still held |
| CLI guidance/examples | 6 | 8 | Command inventory, schemas, native/module distinction, revision and exit behavior; 49 scoped executable checks |
| UI guidance | 6 | 8 | Actual labels, seven synthetic captures, provider/bridge steps, System events save/read-back; external tenants/remote writes unqualified |
| Screenshots/diagrams | 1 | 8 | Seven current screens with provenance/alt text and architecture PNG with alternate text and retained described SVG source |
| Troubleshooting | 3 | 8 | Symptom/check/verification table, cookie isolation, diagnostics and support handoff |
| Accessibility | 5 | 8 | One H1 per page, labeled code/alt text, high-contrast responsive preview reviewed at desktop/390px; no full assistive-technology audit |
| Production operations | 2 | 8 | Backup consistency, recovery drill, monitored rollout/abort, retention and sizing/cost criteria; no promised RPO/RTO or capacity certification |

**After overall: 8.0/10 (88/11), up from 4.5/10.** Eight means usable, scoped task guidance under this rubric; it does not mean every described product/provider path passed production acceptance.

## Validation evidence

- Markdown build: 24 pages, local targets/anchors, one H1, image alternatives and fenced command paths checked with pinned Markdig 0.40.0. Preview styles differ from the hosted portal; hosted navigation/assets require a separate publication check.
- Candidate 2177 owner receipts add six scoped browser workflow rows and 12 native/API policy checks, bound to exact source/image/Windows CLI archive hashes. Native changes were reconciled in fresh browser reads; six fixtures were removed with 404 absence. No runtime helpers were rerun by the documentation task.
- Native CLI source build passed. Forty-nine live reads, request schemas and synthetic role/global/role-policy/user/membership mutations passed at Broker `e5b5eb1`.
- MQTT 5 loopback first-message proof passed: allowed-topic publish/observer delivery and denied outside-topic publish/subscribe (`0x87`). This is not complete MQTT conformance.
- Isolated local System events UI save and CLI read-back passed; stale revision rejected; CLI update to revision 2 was visible after UI reload. Provider/bridge editor reads passed; no external tenant/bridge was provisioned.
- Local desktop and 390-pixel preview inspected. Mobile document width did not exceed viewport. Code/tables have bounded scrolling and navigation stays usable.
- External links: OASIS MQTT specification and Azure Portal returned 200. Marketplace offer returned automated 403 and needs entitled-browser verification; it is not called broken or qualified. DEV docs returned 200; PROD product docs returned 404 before this DEV-only change.
- Build and publication receipts are retained with task release evidence, including source SHA and job outcome. A pending job is not counted as successful publication.

## Residual product gaps

- Candidate 2133's missing policy route remains historical. Exact candidate 2177 local browser/direct/native edits passed with read-back, ownership/input rejection and cleanup. Remote/security changes, restart persistence and multi-replica qualification remain open; the later development endpoint guard is excluded from 2177.

- An apparent local editor/API mismatch was traced to concurrent broker cookies sharing localhost across ports; isolated session recheck passed. No product defect is inferred from that initial failure.
- CoreControl live lifecycle/rotation, cross-portal authorized writes, protected upload and multi-replica activation remain separately qualified.
- Current production capacity and ordered-runtime recovery are held; no sizing guarantee comes from 1813 or CI.
- Real Event Hubs/CoreStream, other OIDC tenants, provider-specific restore drills and customer Marketplace paths need exact-release evidence.

The new procedures describe operator checks and recovery planning, not a tested production RPO/RTO or certification. Historical reports keep original outcomes. See [release status](release-status.md).
