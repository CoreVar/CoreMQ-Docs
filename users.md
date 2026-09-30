# Users and certificate identities

Open **Configure > Identities** to see local accounts, certificate identities and observed organization identities in one searchable, sortable list. The account type and provider identify each entry. Available actions depend on its type and your permissions. Live client sessions appear under **Connections**.

## Create a user

Choose **Create User**, enter the account information and password, and assign the required roles. Grant **User Manager** or **Endpoint Manager** only when the account needs management access. MQTT publishing and subscribing are controlled by message policies; management privileges and message permissions serve different purposes.

Edit the account to manage its basic information, credentials and roles, and to view its policy collection. Candidate 2133's **Edit** link for an existing user policy leads to an unregistered route; that policy-edit workflow remains held. See [policy limitations](policies.md#ui-and-cli). The technical details identify the stable local user ID. Organization sign-in identities are independent and are not attached to a local account.

## Organization identities

Organization identities appear after they have been observed signing in through a configured provider. Their provider-managed identity and roles are read-only here. Displayed roles are the last observed values; they do not prove current access. Where supported and authorized, **Check current access** refreshes the access decision separately. Provider availability also does not establish whether an individual still has access.

The list never merges identities merely because their names or email addresses match. Certificate identities with an explicit backing-account relationship appear once instead of duplicating that account.

## Certificate identities

Choose **Create Identity** to configure a device or service that authenticates with a certificate. Certificate identities use backing accounts so their roles and direct message policies can be managed through the same authorization model. Configure compatible certificate authentication on the listener and the required trust material.

A **certificate authority** establishes certificate trust. It is not a user account or a connected device. Manage authorities separately under **Certificate authorities** and upload the required public certificate material.

Authentication support depends on the listener configuration and build; CoreMQ is no longer limited to username/password authentication. External login to the management UI is described in [sign-in providers](sign-in-providers.md); that login flow is separate from MQTT client authentication.

See [roles](roles.md), [policies](policies.md), and [endpoints](endpoints.md).

## Certificate authority and identity checks

Upload public CA certificates under **Certificate authorities**, then configure the listener's intended trust/authentication settings. A trusted chain alone does not assign a device role. Create the certificate identity/backing account, map the supported certificate identity field and client-ID rules, and assign only its message role. Test an authorized certificate, an untrusted certificate, a revoked/disabled identity and an unrelated client ID.

```text
coremq security authorities list
coremq security authorities put example-ca --schema
coremq security identities put device-17 --schema
coremq users certificate set <user-id> --schema
```

The registered-account certificate command expects `certificateBase64` for a DER client certificate; endpoint-server upload uses PFX and is a different operation. Follow your deployment's renewal/revocation process. Device revocation service commands need a scoped workload token; an ordinary administrator token does not authorize them. Never attach private key material to support reports.
