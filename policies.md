# Topic policies

Message policies control publishing and subscribing independently. They have global, role and user scopes. Management privileges are separate from message authorization.

## Precedence

Matching policies are evaluated in ascending order within a scope; the last matching explicit Allow/Deny wins there. NotSet leaves the action undecided. Use unique order numbers for overlapping rules rather than relying on tie ordering.

1. Use the global result if any policy matched.
2. Evaluate assigned roles. An allow from **any** role wins over denies from other roles; a decided role result replaces the global result.
3. A matching user policy replaces the earlier result.
4. If no scope decided, ordinary topics default to allowed. System subscriptions default to denied. Clients/bridges cannot publish into the reserved local `$SYS` namespace.

A global deny is a baseline that narrower role/user grants can override. It is not an unconditional deny across scopes. Inspect all roles and direct policies when diagnosing an unexpected grant.

## Worked telemetry example

Set global publish/subscribe Deny on `#` at order 0. Create a `Telemetry client` role with publish Allow on `devices/17/telemetry` and subscribe Allow on `devices/17/commands/#`. Use separate rules and NotSet for the action each rule does not decide. Assign only that role and remove unintended direct grants.

| Client action | Expected result |
| --- | --- |
| Publish `devices/17/telemetry` | Role allow |
| Publish `devices/18/telemetry` | Global deny |
| Subscribe `devices/17/commands/#` | Role allow |
| Subscribe `devices/18/commands/#` | Global deny |
| Subscribe `$SYS/coremq/v1/#` | Deny unless explicitly granted |

MQTT `+` matches one level and `#` matches remaining levels. `#` does not match `$`-prefixed system topics. Test actual delivery as well as subscription requests; broad subscriptions do not guarantee access to every matching publication.

## UI and CLI

For global rules open **Configure > Global Policies > Create policy**. For role rules select the role under **Configure > Roles** and add its policy. For user rules edit the local account under **Configure > Identities** and open its policies. Select Publish/Subscribe, order and filters; save and test with the affected account.

```text
coremq policies add --schema
coremq policies add --file default-deny.json
coremq roles policies add <role-id> --file telemetry-publish.json
coremq users policies list <user-id>
```

`default-deny.json`:

```json
{ "type": "message", "publish": "Deny", "subscribe": "Deny", "order": 0, "topics": ["#"] }
```

`telemetry-publish.json`:

```json
{ "type": "message", "publish": "Allow", "subscribe": "NotSet", "order": 10, "topics": ["devices/17/telemetry"] }
```

**Browser policy editing is held for candidate 2133.** A role-policy **Edit** click was verified to open an unregistered route; the corresponding user-policy route is also absent in the reviewed source. Direct broker APIs lack scoped single-policy GET/PUT, and its native CLI provides policy list/add/remove. The remote module already has source-level policy list/put/delete with scope, scope ID and expected revision, but actual deployed remote edit/read-back remains separately held. The create/list/membership and MQTT checks in this review do not qualify editing an existing policy. Do not assume a correction is deployed from a source fix or passing create/delete check; require the corrected immutable candidate and authorized edit/read-back plus persistence evidence.

Use the installed CLI `--schema` for the documented operations; this guide does not promise a single-policy update command. Read back after writing and test real authorization. Keep a tested local recovery login before changing administrative/provider mappings.

Next: [Roles](roles.md), [system permissions](system-events.md), [troubleshooting](operations.md).
