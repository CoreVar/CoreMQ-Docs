# Management capabilities and compatibility

Reviewed **October 4, 2026** against broker source
`c674f84e63d9044ce20365110cf9e731520b958b`, DEV candidate `0.0.0-ci.2727`.
Use this matrix to choose a management route and a compatible broker/module pair.
An installed deployment's advertised capabilities and your permissions determine
which controls are available. Check **Configure > About** before applying a guide
to a different release.

## Choose a route

- **Broker UI:** the broker's own Configure pages, using local broker access.
- **Direct CLI:** `coremq` connects to that broker's management origin.
- **CoreControl CLI:** `corevar coremq` uses an explicitly selected deployment.
- **Portal controls:** reusable CoreMQ controls hosted by CoreVar Portal or an
  authorized administration host. The host must consume compatible packages and
  supply deployment authorization, capability invocation and operation status.

Portal sign-in does not grant broker administration. A denied remote command
returns an error; it never switches to a direct connection. See
[remote management](remote-management.md) for enrollment and access boundaries.

## Implemented surfaces

“Present” below means code and contracts exist in the reviewed source. Portal
controls means a reusable component exists, rather than proof that every deployed
host currently loads it. The live acceptance column is intentionally separate.

| Task | Broker UI/API | Direct CLI | CoreControl | Portal controls | Live mutation acceptance for this candidate |
|---|---|---|---|---|---|
| Users, roles and topic policy reads | Configure views and HTTP reads present | Canonical list/get commands present | 13 canonical read adapters; bounded pages and explicit errors | Users, roles, policies and identity directory surfaces present | Not established by the recorded checks |
| Enable/disable users; change roles; grant/revoke administrative roles | User controls and HTTP writes present | Canonical commands; optional revision and operation flags | Canonical commands require revision and operation ID; security and high-impact policy gates | User administration surface present | Not established by the recorded checks |
| MQTT endpoint configuration | Listener controls and API present | List/get/add/remove and certificate commands | Typed configuration and lifecycle handlers present | Endpoint lifecycle surface present | Not established by the recorded checks |
| Bridges and stream destination profiles | Shared bridge editor and uplink API present | Bridge/uplink commands present | Read/validate/put/delete handlers present | Bridges surface uses the shared editor | Not established by the recorded checks |
| Browser sign-in providers | Saved configuration service, editor and HTTPS API present | List/put/remove; separate runtime status read | Provider read/put/delete and organization identity handlers present | Sign-in provider and organization identity surfaces present | Not established by the recorded checks |
| System event publishing setting | Persisted revisioned service and editor present | `system-events show/put` | Configuration read/put handlers present | System events editor present | Not established by the recorded checks |
| Payload schema policies | Configuration service and editor present | List/validate/put/remove and metrics | Configuration, staged-content and lifecycle handlers present | Schema policy surface present | Not established by the recorded checks |
| Certificate authorities and workload identities | Security controls and HTTP APIs present | Authority/identity commands present | Typed security handlers present; policy gates apply | Authority and identity surfaces present | Not established by the recorded checks |

Command names and request shapes are route-specific outside the documented
canonical adapters. A capability descriptor alone does not establish a matching
CLI command or complete field parity. Use the installed module's help and the
[CLI reference](cli.md), rather than translating a local API URL into a remote
command.

## Automated evidence

[DEV build 2727](https://dev.azure.com/corevar/CoreMq/_build/results?buildId=2727&view=results)
succeeded for the exact source above. Its test results are:

| Test suite | Passed | Skipped | Failed | Evidence scope |
|---|---:|---:|---:|---|
| Broker default graph | 1,090 | 60 | 0 | Automated broker tests |
| Broker incoming graph | 1,098 | 62 | 0 | Alternate automated graph; overlaps the default graph |
| CLI | 140 | 0 | 0 | Commands and transport adapters, including user revisions |
| CoreControl contracts | 11 | 0 | 0 | Product contracts and registration tests |
| Management components | 70 | 0 | 0 | Shared controls and host boundary tests |
| Portal commands | 2 | 0 | 0 | Browser terminal command integration tests |

Do not add the two broker graph counts together. These suites establish their
tested contracts; they do not substitute for live deployment acceptance.
The build also produced four internal management packages with packaged READMEs.
Package creation does not establish adoption by a deployed Portal host.

The user revision checks cover canonical `users get`, unchanged lightweight list
output, omitted legacy revisions, rejected stale writes, denied direct reads,
and disabled CoreControl configuration-read dispatch. Read the selected user's
revision with `users get`; never default an absent revision to zero. Follow the
[user access workflow](cli.md#read-and-update-user-access) for operation IDs and
uncertain outcomes.

## Verify your installed deployment

Before accepting a management workflow for a release:

1. Record the broker version, module version, Portal package versions and selected
   tenant/deployment. Confirm the required capability and policy groups.
2. Read through each intended route, change one disposable configuration through
   an authorized route, then read it back through the others.
3. Check stale-revision rejection and unauthorized access. Confirm secret
   redaction and any restart/activation requirement.
4. Check runtime behavior after the change, and inspect command status after a
   timeout before any further submission.

For this reviewed candidate, the recorded evidence does not yet establish a full
live Broker UI/direct CLI/CoreControl/Portal mutation round trip or PROD release
acceptance. The legacy Marketplace `0.1.5` listing is a separate package history,
rather than evidence for these newer capabilities. See
[versions and packages](release-status.md) when choosing an installation.

The separate 37-case compiled Redis comparator check validates its comparison
logic only. It does not demonstrate broker restart recovery, current-candidate
capacity, or distributed throughput. Older `e953` recovery fixtures likewise do
not establish recovery for `c674f84`. Use workload measurements in the
[sizing guide](sizing.md) and release-specific recovery evidence for those decisions.

## Source audit references

The audit used the reviewed source's `CommandCatalog.cs`, `CoreMqModule.cs`,
`ReadOnlyControlAdapter.cs`, `UserAccessControlAdapter.cs`, `CoreMqManagedProduct.cs`,
the administration/browser-identity/system-event/uplink handlers, shared management
surfaces and Portal command registration. The older September broker inventories
describe an earlier implementation stage: their file-only system-event switch,
missing provider commands and initially deferred administration reads are
superseded by the reviewed source. This page records the current inventory without
converting source availability into live acceptance.
