# Users and certificate identities

Open **Configure > Identities**. Local user accounts and certificate identities represent principals authorized to use the broker. They are not a list of currently connected clients.

## Create a user

Choose **Create User**, enter the account information and password, and assign the required roles. Grant **User Manager** or **Endpoint Manager** only when the account needs management access. MQTT publishing and subscribing are controlled by message policies; management privileges and message permissions serve different purposes.

Edit the account to manage its basic information, credentials, roles and policies. The technical details identify the stable local user ID, which is also used for explicit external sign-in assignments.

## Certificate identities

Choose **Create Identity** to configure a device or service that authenticates with a certificate. Certificate identities use backing accounts so their roles and direct message policies can be managed through the same authorization model. Configure compatible certificate authentication on the listener and the required trust material.

A **certificate authority** establishes certificate trust. It is not a user account or a connected device. Manage authorities separately under **Certificate authorities** and upload the required public certificate material.

Authentication support depends on the listener configuration and build; CoreMQ is no longer limited to username/password authentication. External login to the management UI is described in [sign-in providers](sign-in-providers.md); that login flow is separate from MQTT client authentication.

See [roles](roles.md), [policies](policies.md), and [endpoints](endpoints.md).
