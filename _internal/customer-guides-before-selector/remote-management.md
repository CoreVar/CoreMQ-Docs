# CoreControl and configuration coverage

CoreControl provides the backplane for authorized remote commands to a CoreMQ deployment. Portal login and support access do not replace the broker's own local login.

The product requirement is that every configurable enhancement works through the local Configure UI, the CLI, and CoreControl in both management and administration portals. Shared controls and contracts should provide matching validation, permissions, revision conflicts, protected secret handling and restart state.

## Register, verify and unregister

The connector is compiled into cloud builds and disabled by default. Local edition excludes it. Deploy first, then create a deployment enrollment in the Management Portal using an authorized organization/product role. Save the short-lived bootstrap artifact in a protected file/Secret, never in arguments, environment values or tickets. Mount a separate Base64-encoded 32-byte protection key. Configure the trusted service origin, environment, audience and complete gateway allowlist from the approved deployment contract; DEV URLs must not be substituted for PROD.

On the newer AWS chart, opt-in enrollment runs once in a Helm Job against the existing shared state PVC. Install with CoreControl disabled first, or pre-create the IRSA ServiceAccount when first-install enrollment is selected. The Job meters its own paid pod. Broker replicas retain only the encrypted credential and mounted protection key. Remove the bootstrap Secret after successful enrollment and omit it on later upgrades; retain the key/PVC. A redeemed artifact cannot enroll empty replacement storage. This implementation still needs fresh live qualification for candidate 1863.

After enrollment, select the deployment in the Management Portal and confirm connected status, catalog capabilities, version and health freshness. Restart a controlled broker and verify reconnect without a second bootstrap exchange. Check local messaging while the control service is unavailable. A visible deployment row is not evidence of a successful authorized command.

For retirement, revoke server-side deployment access in the portal. Stop the broker and use the same cloud image/storage settings with `--cloud --corecontrol-unregister` to remove its local encrypted credential. Local deletion alone does not revoke access. Preserve customer state and separate key material until ownership/retention/recovery checks permit cleanup. Rotation and empty-storage recovery require the exact release contract; do not assume an expired artifact is reusable.

## Manage through the portals

Select the intended organization and deployment, inspect advertised capabilities and their enabled policy groups, then open the product configuration area supported by that portal version. Remote reads/writes require both human authorization and the broker local policy. Configuration-write, security and high-impact permissions are distinct; 403 is an authorization result, not a request to bypass it. Read, review, write with current expected revision and stable operation ID, then read back and verify runtime behavior. Check command status after a timeout before retrying.

A compatible CoreVar CLI host supplies the selected route and token to `corevar coremq` without command-line credentials. The module ignores standalone saved credentials. JSON files/stdin are supported; direct connection overrides, inline secret arguments and local process-control commands are rejected. Use its installed help because capability names differ by module version.

## Current development coverage

| Feature | Current coverage | Remaining work |
| --- | --- | --- |
| Bridge settings, including stream profiles | Local editor/API, CLI and CoreControl configuration contracts | Portal package adoption, new profile presentation and remote service qualification |
| Browser sign-in providers | Local editor/HTTPS API/CLI; CoreControl read/write/delete and shared portal editor | Live authorized remote round trips; multi-replica persistence and activation; guided remote onboarding |
| System event emission settings | Configure editor, CLI, revisioned CoreControl contract and shared portal editor | Complete live portal write/read-back qualification |
| Identity directory | Shared searchable/sortable list of local, certificate and observed organization identities; unified directory and current CoreID access reads verified through the development Management Portal | Authorized mutation coverage, certificate editor and multi-page live acceptance, production qualification |
| System event subscription permissions | Existing message-policy mechanisms | Full cross-portal round-trip qualification |

A portal must consume compatible versions of the internal CoreMQ management controls and command packages, and the deployment must advertise the required capabilities. Updating the broker alone does not update a portal's controls. A visible control or capability manifest is not proof of an end-to-end working mutation.

The unified directory uses `coremq.security.identity-directory.read` version 1 with configuration-read permission. Compatible portals retain the individual views when an older broker does not advertise it. Results are paged; if the inventory changes between pages, reload the list. Directory visibility grants no permission to edit an entry.

If a command times out, inspect its status before retrying: it may still execute remotely. Revision and idempotency checks protect configuration changes. Never send a secret through an unprotected command payload or substitute a browser workstation path for a managed upload.

Configuration qualification is ongoing. Protocol and container scan results do not establish Marketplace readiness or complete portal coverage. Provider changes currently report restart-required; shared durable storage alone does not prove concurrent updates or activation across replicas.

## Local and remote evidence

September 30 local screenshots verify identity/endpoint navigation and system-event/provider editor reads under an isolated synthetic session. Local system-event UI save, direct CLI read-back, stale-revision rejection and CLI restoration are checked separately. Browser cookies can collide between brokers sharing a hostname even at different ports; isolate evaluation sessions before diagnosing an API/editor mismatch.

Cross-portal mutations, protected upload and activation must still pass against the exact compatible portal and broker packages. Local evidence does not establish remote or multi-replica qualification.
