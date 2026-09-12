# Users and certificate identities

Open **Configure > Identities** to see local accounts, certificate identities and observed organization identities in one searchable, sortable list. The account type and provider identify each entry. Available actions depend on its type and your permissions. Live client sessions appear under **Connections**.

## Create a user

Choose **Create User**, enter the account information and password, and assign the required roles. Grant **User Manager** or **Endpoint Manager** only when the account needs management access. MQTT publishing and subscribing are controlled by message policies; management privileges and message permissions serve different purposes.

Edit the account to manage its basic information, credentials, roles and policies. The technical details identify the stable local user ID. Organization sign-in identities are independent and are not attached to a local account.

## Organization identities

Organization identities appear after they have been observed signing in through a configured provider. Their provider-managed identity and roles are read-only here. Displayed roles are the last observed values; they do not prove current access. Where supported and authorized, **Check current access** refreshes the access decision separately. Provider availability also does not establish whether an individual still has access.

The list never merges identities merely because their names or email addresses match. Certificate identities with an explicit backing-account relationship appear once instead of duplicating that account.

## Certificate identities

Choose **Create Identity** to configure a device or service that authenticates with a certificate. Certificate identities use backing accounts so their roles and direct message policies can be managed through the same authorization model. Configure compatible certificate authentication on the listener and the required trust material.

A **certificate authority** establishes certificate trust. It is not a user account or a connected device. Manage authorities separately under **Certificate authorities** and upload the required public certificate material.

Authentication support depends on the listener configuration and build; CoreMQ is no longer limited to username/password authentication. External login to the management UI is described in [sign-in providers](sign-in-providers.md); that login flow is separate from MQTT client authentication.

See [roles](roles.md), [policies](policies.md), and [endpoints](endpoints.md).
