# Browser sign-in providers

CoreMQ can use OpenID Connect (OIDC) for login to its own management UI while retaining local accounts. This is separate from MQTT authentication, CoreControl registration, and the portal's own login.

## Connect CoreID

On deployments with guided CoreID registration enabled, open **Configure > Sign-in providers > Connect CoreID**. If you are using HTTP, **Continue securely with CoreID** opens the configured HTTPS broker address first.

1. Click **Connect CoreID**. This explicitly links your CoreID identity to the local broker account you are currently using.
2. Sign in to CoreID, choose an existing organization where you are a tenant administrator, and approve the broker application. Complete any required MFA.
3. CoreID redirects you back to CoreMQ after approval. If the redirect does not happen within 30 seconds, select **Return to CoreMQ**. The broker verifies an ordinary OIDC sign-in for the same issuer, subject and organization before enabling the provider.

The organization is a CoreID membership boundary. It is not the broker deployment, hosting region or cloud subscription. A CoreID account can belong to several organizations; this selection binds the broker application to one eligible organization you administer. Membership alone does not grant a broker role.

Cancel on the CoreID consent screen returns to the broker configuration screen and clears the pending connection. It does not approve or activate a provider.

The broker registers the application and stores its credential server-side. You do not copy client IDs, secrets or subject IDs. Successful verification activates CoreID immediately, without a restart. New guided connections use local broker roles by default. An administrator can explicitly select CoreID application roles and configure mappings afterward.

A deployment administrator enables this flow with `BrowserIdentity:CoreId:Authority` (the exact trusted HTTPS issuer, including any trailing slash) and `BrowserIdentity:CoreId:BrokerOrigin` (the broker's public HTTPS origin). CoreID must support the guided broker-registration API. These are deployment settings, not values accepted from the browser. Use a stable address for the broker handling registration; an in-progress registration is local to that process.

Registration expires after ten minutes; the final sign-in verification has a five-minute window. **Cancel pending connection** clears the broker's pending attempt. It does not delete an application already approved in CoreID. If verification fails, the broker restarts, or a credential response is lost, review the application in CoreID before registering again. The broker does not retry a one-time credential redemption or silently replace an existing CoreID provider. Keep a working local recovery account.

The guided flow is available in the current local development build. Live organization approval is performed by the user; this is not a claim of production or Marketplace qualification.

## Accounts and permissions

Guided CoreID setup links the approving identity to the existing broker account that started setup. It does not create a second user or grant administrator access merely because someone signs in through CoreID.

In **Configure > Identities**, local accounts appear under **Accounts**, and linked CoreID or other provider identities appear separately under **Organization accounts**. Each organization identity shows its provider, linked broker account, permission source and last recorded sign-in. **View account** opens the linked account and its permissions; it does not edit or delete the identity-provider account.

The broker records an account name from validated sign-in claims (`preferred_username`, `email`, then `name`) after successful sign-in. Earlier sessions have no recorded name, so the provider subject ID is shown until the next provider sign-in. No tokens or passwords are stored with this display information. This list includes explicitly linked identities, not every account in the organization. Identity details on the account include the issuer, organization ID and subject identifier. Provider settings pending restart are identified as saved settings.

Local password sign-in uses the account's CoreMQ roles and policies; local administrative permissions are under its Advanced tab. External sign-in uses the provider's configured **Permission source**. Certificate identities are certificate credentials, not browser sign-in accounts. CoreID organization roles are never automatically imported as broker roles.

### Use CoreID application roles

1. Edit the connected provider under **Configure > Sign-in providers**.
2. Choose **CoreID application roles** as its permission source. The CoreID deployment must have application-role support enabled.
3. Open **Manage application roles in CoreID**. An eligible tenant administrator assigns roles to accounts or organization groups for this particular broker application; required organization MFA still applies.
4. Back in CoreMQ, add explicit mappings from those application-role keys to existing broker roles. For example, map `broker-administrator` to **User Manager** and separately to **Endpoint Manager** when both permissions are intended.
5. Keep exact subject-to-account assignments, save, restart the broker, and sign in again. Maintain a local recovery account.

CoreID-managed external sessions use only the mapped roles. They do not inherit local account roles or tenant administrator status. Unknown or unassigned roles grant no access, and a mapped management role is required to open the broker management UI. Changing this setting does not change local password sign-in permissions.

On account details, **Linked sign-in identities** shows the permission source, current CoreID application roles, mapped broker roles, and check time. CoreID assignments are read-only here; edit them in CoreID. This view is not a directory of every organization user or a sign-in audit history.

The broker checks current CoreID access on each authenticated external HTTP request. Revocation, expiry, disabled access, an unavailable CoreID service, or an invalid response ends external access; it does not fall back to local administrator roles. Saved configuration changes take effect after restart and invalidate sessions using the previous mapping. A request already running is not undone by later revocation.

## Configure another provider or an existing application

1. Open the broker over trusted HTTPS and select **Configure > Sign-in providers** as a User Manager.
2. Choose **Add provider** and the provider type. The chooser supplies guidance; it does not register an application with that provider.
3. Register an OIDC application with the identity provider. Use the exact HTTPS callback displayed by the broker, `/signin-oidc/<provider-id>`, including the broker host and port.
4. Enter the issuer, client ID and client secret where required. Configure an exact tenant claim/value restriction when appropriate.
5. Assign the provider's stable subject identifier to an existing local account with management access. An email address is not a substitute for the subject.
6. Save, observe the restart-required indication, restart through your deployment process, and test sign-in. Retain a working local recovery account.

The broker uses authorization code flow with PKCE and validates the issuer and subject. It does not automatically match by email or import provider roles as local administrative permissions. Settings are revision-checked. Read operations do not return the client secret; leaving the secret empty while editing retains the existing one.

The chooser includes CoreID, Microsoft Entra ID, Auth0, Google Workspace, Amazon Cognito and generic OIDC. These are configuration starting points, not certification of every tenant configuration. AWS IAM keys are not browser login identities. Direct SAML support is not included.

For CoreID, prefer **Connect CoreID** above. Selecting CoreID inside **Add provider** is the advanced path for an application you already registered.

The local configuration API is `/api/BrowserIdentity/Configuration`. Writes require HTTPS and the broker's origin. Dedicated CLI commands, CoreControl management and reusable portal configuration surfaces are still missing. See [management coverage](remote-management.md).


## Verification

The current development integration was verified end to end against CoreID DEV and the local Docker broker with isolated synthetic accounts. Unassigned sign-in was denied. Direct and group assignments granted endpoint access while user administration remained forbidden. Removing either assignment denied the next request from the already-open broker session. The read-only account view showed the current CoreID role and mapped broker permission, with no local roles assigned. This is development acceptance, not qualification of every identity provider or production deployment.
