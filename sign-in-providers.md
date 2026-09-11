# Browser sign-in providers

CoreMQ can use OpenID Connect (OIDC) for login to its own management UI while retaining local accounts. This is separate from MQTT authentication, CoreControl registration, and the portal's own login.

## Configure a provider

1. Open the broker over trusted HTTPS and select **Configure > Sign-in providers** as a User Manager.
2. Choose **Add provider** and the provider type. The chooser supplies guidance; it does not register an application with that provider.
3. Register an OIDC application with the identity provider. Use the exact HTTPS callback displayed by the broker, `/signin-oidc/<provider-id>`, including the broker host and port.
4. Enter the issuer, client ID and client secret where required. Configure an exact tenant claim/value restriction when appropriate.
5. Assign the provider's stable subject identifier to an existing local account with management access. An email address is not a substitute for the subject.
6. Save, observe the restart-required indication, restart through your deployment process, and test sign-in. Retain a working local recovery account.

The broker uses authorization code flow with PKCE and validates the issuer and subject. It does not automatically match by email or import provider roles as local administrative permissions. Settings are revision-checked. Read operations do not return the client secret; leaving the secret empty while editing retains the existing one.

The chooser includes CoreID, Microsoft Entra ID, Auth0, Google Workspace, Amazon Cognito and generic OIDC. These are configuration starting points, not certification of every tenant configuration. AWS IAM keys are not browser login identities. Direct SAML support is not included.

**CoreID automatic registration is not yet available in the broker.** Use a registered client; do not expect the CoreID choice to provision one automatically.

The local configuration API is `/api/BrowserIdentity/Configuration`. Writes require HTTPS and the broker's origin. Dedicated CLI commands, CoreControl management and reusable portal configuration surfaces are still missing. See [management coverage](remote-management.md).
