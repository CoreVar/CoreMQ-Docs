# Browser sign-in providers

CoreMQ can use OpenID Connect (OIDC) for login to its own management UI while retaining local accounts. This is separate from MQTT authentication, CoreControl registration, and the portal's own login.

## Connect CoreID

On deployments with guided CoreID registration enabled, open **Configure > Sign-in providers > Connect CoreID**. If you are using HTTP, **Continue securely with CoreID** opens the configured HTTPS broker address first.

1. Click **Connect CoreID**. This explicitly links your CoreID identity to the local broker account you are currently using.
2. Sign in to CoreID, choose an existing organization where you are a tenant administrator, and approve the broker application. Complete any required MFA.
3. Return to the broker using the CoreID consent screen. The broker verifies an ordinary OIDC sign-in for the same issuer, subject and organization before enabling the provider.

The broker registers the application and stores its credential server-side. You do not copy client IDs, secrets or subject IDs. Successful verification activates CoreID immediately, without a restart. Local roles remain authoritative; CoreID does not grant additional broker privileges.

A deployment administrator enables this flow with `BrowserIdentity:CoreId:Authority` (the exact trusted HTTPS issuer, including any trailing slash) and `BrowserIdentity:CoreId:BrokerOrigin` (the broker's public HTTPS origin). CoreID must support the guided broker-registration API. These are deployment settings, not values accepted from the browser. Use a stable address for the broker handling registration; an in-progress registration is local to that process.

Registration expires after ten minutes; the final sign-in verification has a five-minute window. **Cancel pending connection** clears the broker's pending attempt. It does not delete an application already approved in CoreID. If verification fails, the broker restarts, or a credential response is lost, review the application in CoreID before registering again. The broker does not retry a one-time credential redemption or silently replace an existing CoreID provider. Keep a working local recovery account.

The guided flow is available in the current local development build. Live organization approval is performed by the user; this is not a claim of production or Marketplace qualification.

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
