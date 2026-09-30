# Roles

A local role groups message permissions for accounts and certificate-backed accounts. **User Manager** and **Endpoint Manager** are built-in management roles; grant them only where those operations are needed. A telemetry device normally needs a message role.

## Create and assign a message role

1. Open **Configure > Roles > Create role** and enter `Telemetry client`.
2. Open the role and add a policy for a narrow topic filter. Review [policy precedence](policies.md).
3. Open **Configure > Identities**, edit the local account and assign that role. Inspect direct policies too; they can override role results. Built-in management roles are assigned from the account's **Advanced** tab.
4. Test an allowed and a denied topic with the affected MQTT account. Configured accounts exist independently of live connections.

CLI equivalents use IDs returned by list/create, and role **names** inside membership arrays:

```text
coremq roles list
coremq roles add --schema
coremq roles add --file role.json
coremq roles policies add <role-id> --file telemetry-policy.json
coremq users roles update <user-id> --file membership.json
coremq users roles list <user-id>
```

`role.json`:

```json
{ "name": "Telemetry client" }
```

`membership.json`:

```json
{ "add": ["Telemetry client"], "remove": [] }
```

Candidate 2133's browser **Edit** action on an existing role policy is held because its target route is unregistered. Role creation/assignment and policy creation/listing are separate checks. See the [policy-edit qualification boundary](policies.md#ui-and-cli) before changing an existing policy.

An allow from one role wins over a deny from another. Test combined roles before granting them. Removing administrative roles preserves other roles; verify effective access after the change.

## Organization application roles

Organization identities are independent of local accounts. Assign roles in the provider for this broker application, then map exact application-role values to permitted CoreMQ management roles. Matching names/emails do not merge accounts or inherit local policies. See [sign-in providers](sign-in-providers.md).
