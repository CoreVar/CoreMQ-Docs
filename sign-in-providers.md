# Browser sign-in providers

CoreMQ supports OpenID Connect for its management UI. Organization identities and local accounts are separate. An organization sign-in never creates, links to, or inherits permissions from a local broker account. MQTT authentication and the Management Portal login are separate capabilities.

## Connect CoreID

Open **Configure > Sign-in providers > Connect CoreID** using the broker's HTTPS address. Sign in to CoreID, select an organization you are authorized to administer, and approve the broker application. CoreID returns you to CoreMQ; the broker verifies the approving identity and saves the application credential on the server.

Registration does not grant broker access. In CoreID, assign application roles to the intended people or groups for this particular broker application. In CoreMQ, edit the provider and map those application roles to CoreMQ permissions. At least one mapped **User Manager** or **Endpoint Manager** role is required to open the management UI. Organization membership or tenant administration alone does not grant broker administration.

**User Manager** manages accounts and security configuration. **Endpoint Manager** manages endpoints and operational views. Map both only when both privileges are intended. Local recovery accounts retain their separately configured permissions.

The CoreID application-role editor is linked from the provider's configuration. Role assignments belong to the selected organization and application; they are not local CoreMQ user records. CoreMQ verifies current CoreID access on each authenticated request. Revocation, an inactive identity, or failure to verify access ends the external session. It never falls back to local permissions.

The organization is an identity membership boundary, not a deployment, region or cloud subscription. Cancel returns to CoreMQ. Registration approval and MFA requirements are enforced by CoreID. Guided activation is immediate; subsequent manual provider changes require a broker restart.

## Organization identities

**Configure > Identities** shows local accounts, organization identities, and certificate identities in one searchable list with an Account type column. Click a column heading to sort all types together by name, type, provider, status, or last sign-in. Local accounts and certificate identities offer Edit and Delete; organization identities offer View permissions, with assignments managed by their provider. Certificate authorities remain in their own trust configuration tab. After a successful organization sign-in, CoreMQ records its provider, display name when supplied, issuer, tenant, subject, last sign-in and observed permissions. **View permissions** opens an independent identity view. There is no associated local account and no local role editor for that identity.

CoreID identities show current application roles and effective CoreMQ permissions. Other providers show permissions observed at the last sign-in, explicitly labeled as observations. Display information is not an authorization grant. Identity keys use the exact issuer, configured tenant and subject, not email matching.

## Other OpenID Connect providers

Choose **Add provider** to configure an existing OIDC registration. Register the exact HTTPS callback `/signin-oidc/{provider-id}` and use the issuer from its discovery metadata. Configure the client ID, protected client secret if required, tenant restriction, and session duration.

Choose **Provider token roles**, specify the signed ID-token claim containing application roles (default `roles`), and map its exact values to CoreMQ permissions. The identity provider must issue that claim to this application. Providers that do not supply suitable application-role claims require an appropriate federation or claim configuration; an organization email address alone grants no access. Changes to token-based assignments apply at the next sign-in or session expiration. CoreID uses its dedicated current-access endpoint instead.

CoreMQ supports authorization code with S256 PKCE, state/nonce, signature, issuer, audience and lifetime validation. Access and refresh tokens are not stored. Sessions are nonpersistent, do not slide, and expire no later than the signed ID token or configured 5–480-minute limit. Provider configuration changes invalidate existing external sessions after restart.

Supported integration uses OIDC; OAuth alone, AWS IAM credentials and SAML-only providers are not browser sign-in protocols for this feature. Provider-specific client registration and claim configuration must be tested in the intended tenant.

## Deployment configuration

Guided registration requires `BrowserIdentity:CoreId:Authority` and `BrowserIdentity:CoreId:BrokerOrigin`. These are trusted deployment settings. A registration in progress belongs to its initiating broker process and browser session.

Example CoreID provider (use your actual issuer, tenant and client):

```json
{
  "BrowserIdentity": {
    "Providers": [{
      "Id": "coreid",
      "DisplayName": "CoreID",
      "Enabled": true,
      "Authority": "https://identity.example.com/",
      "ClientId": "registered-broker-client",
      "ClientSecretFile": "/run/secrets/coremq-coreid-client",
      "TenantClaim": "corevar:tenant_id",
      "TenantId": "11111111-1111-1111-1111-111111111111",
      "Scopes": ["openid", "profile", "email", "roles"],
      "SessionMinutes": 60,
      "AuthorizationSource": "CoreID",
      "RoleMappings": [
        { "ApplicationRole": "broker-operator", "BrokerRole": "Endpoint Manager" }
      ]
    }]
  }
}
```

`Bindings` and `AuthorizationSource: Local` are no longer supported. Configure role mappings and provider-side assignments instead. The provider API does not expose stored secrets. Organization directory observations are stored independently beside the provider configuration in `organization-identities.json`; they have no local account foreign key and never determine permission grants.

The local broker UI provides provider configuration. CoreControl/portal configuration parity must be verified separately; this document does not claim that provider changes are remotely editable through every management surface.
