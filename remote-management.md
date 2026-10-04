# CoreControl remote management

CoreControl provides the backplane for authorized remote commands to a CoreMQ deployment. Portal login and support access do not replace the broker's own local login.

## Register, verify and unregister

The connector is compiled into cloud builds and disabled by default. Local edition excludes it. Deploy first, then create a deployment enrollment in the Management Portal using an authorized organization/product role. Save the short-lived bootstrap artifact in a protected file/Secret, never in arguments, environment values or tickets. Mount a separate Base64-encoded 32-byte protection key. Configure the trusted service origin, environment, audience and complete gateway allowlist from the approved deployment contract; DEV URLs must not be substituted for PROD.

On the newer AWS chart, opt-in enrollment runs once in a Helm Job against the existing shared state PVC. Install with CoreControl disabled first, or pre-create the IRSA ServiceAccount when first-install enrollment is selected. The Job meters its own paid pod. Broker replicas retain only the encrypted credential and mounted protection key. Remove the bootstrap Secret after successful enrollment and omit it on later upgrades; retain the key/PVC. A redeemed artifact cannot enroll empty replacement storage.

After enrollment, select the deployment in the Management Portal and confirm connected status, catalog capabilities, version and health freshness. Restart a controlled broker and verify reconnect without a second bootstrap exchange. Check local messaging while the control service is unavailable. A visible deployment row is not evidence of a successful authorized command.

For retirement, revoke server-side deployment access in the portal. Stop the broker and use the same cloud image/storage settings with `--cloud --corecontrol-unregister` to remove its local encrypted credential. Local deletion alone does not revoke access. Preserve customer state and separate key material until ownership/retention/recovery checks permit cleanup. Rotation and empty-storage recovery require the exact release contract; do not assume an expired artifact is reusable.

## Manage through the portals

Select the intended organization and deployment, inspect advertised capabilities and their enabled policy groups, then open the product configuration area supported by that portal version. Remote reads/writes require both human authorization and the broker local policy. Configuration-write, security and high-impact permissions are distinct; 403 is an authorization result, not a request to bypass it. Read, review, write with current expected revision and stable operation ID, then read back and verify runtime behavior. Check command status after a timeout before retrying.

A compatible CoreVar CLI host supplies the selected route and token to `corevar coremq` without command-line credentials. The module ignores standalone saved credentials. JSON files/stdin are supported; direct connection overrides, inline secret arguments and local process-control commands are rejected. Use its installed help because capability names differ by module version.

## Compatible portal controls

See the [dated management capability matrix](management-capabilities.md) for the
current source inventory, automated evidence and installed-deployment checks.

Use compatible versions of CoreVar Portal, its CoreMQ management module and the broker. The selected deployment advertises the capabilities and policy groups available to your account. Update the portal controls alongside the broker when introducing a new configuration workflow.

The unified directory uses `coremq.security.identity-directory.read` version 1 with configuration-read permission. Compatible portals retain the individual views when an older broker does not advertise it. Results are paged; if the inventory changes between pages, reload the list. Directory visibility grants no permission to edit an entry.

For user access changes, select the user with `users list` and obtain its current revision with `users get <id>`. Lists omit revisions; older get responses may omit them too. Never substitute zero. See the [CLI user access workflow](cli.md#read-and-update-user-access) for revision and operation-ID handling.

If a command times out, retain its operation ID and inspect its status before another submission: it may still execute remotely. Revision checks reject stale changes, but an operation ID does not promise durable exactly-once execution. Never send a secret through an unprotected command payload or substitute a browser workstation path for a managed upload.

Provider changes can require a broker restart. Apply them during a planned maintenance window and confirm runtime behavior after the restart.
