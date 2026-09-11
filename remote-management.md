# CoreControl and configuration coverage

CoreControl provides the backplane for authorized remote commands to a CoreMQ deployment. Portal login and support access do not replace the broker's own local login.

The product requirement is that every configurable enhancement works through the local Configure UI, the CLI, and CoreControl in both management and administration portals. Shared controls and contracts should provide matching validation, permissions, revision conflicts, protected secret handling and restart state.

## Current development coverage

| Feature | Current coverage | Remaining work |
| --- | --- | --- |
| Bridge settings, including stream profiles | Local editor/API, CLI and CoreControl configuration contracts | Portal package adoption, new profile presentation and remote service qualification |
| Browser sign-in providers | Local editor and HTTPS API | Dedicated CLI, CoreControl and shared portal editor |
| System event emission settings | Deployment file/environment | Configure editor, CLI and CoreControl settings |
| System event subscription permissions | Existing message-policy mechanisms | Full cross-portal round-trip qualification |

A portal must consume compatible versions of the internal CoreMQ management controls and command packages, and the deployment must advertise the required capabilities. Updating the broker alone does not update a portal's controls. A visible control or capability manifest is not proof of an end-to-end working mutation.

If a command times out, inspect its status before retrying: it may still execute remotely. Revision and idempotency checks protect configuration changes. Never send a secret through an unprotected command payload or substitute a browser workstation path for a managed upload.
