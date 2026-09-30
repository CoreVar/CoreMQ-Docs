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

## Edit an existing role or user policy

The corrected local/browser and direct/native path passed against **candidate `0.0.0-ci.2177`**, source `93790dc9b1a17390b0433bd3422355f76c584695`, using the verified Azure image and Windows CLI. Install the matching approved broker/CLI artifacts before using these commands. Other packages and deployed remote editing need their own evidence.

Open the role's policy collection under **Configure > Roles**, or the local account's policies under **Configure > Identities**. Choose **Edit** to open the policy modal. Review topics, order, Publish and Subscribe; save, reopen and read back the values. Use an affected test account to verify allowed and denied messaging before applying a changed policy to active traffic.

For native direct management, obtain both IDs from the owning collection. The update request contains `topics`, `order`, `publish` and `subscribe`:

```text
coremq roles policies get <role-id> <policy-id>
coremq roles policies update <role-id> <policy-id> --file policy-update.json
coremq roles policies get <role-id> <policy-id>
coremq users policies get <user-id> <policy-id>
coremq users policies update <user-id> <policy-id> --file policy-update.json
coremq users policies get <user-id> <policy-id>
```

Example `policy-update.json` for a narrow test-topic deny:

```json
{ "topics": ["qualification/test/#"], "order": 20, "publish": "Deny", "subscribe": "Deny" }
```

Select the intended permissions rather than copying a test deny into an active policy. Keep the owner and policy IDs together; a policy belonging to another role/user was rejected with 404, and empty topics with 400, in the 2177 checks. Fresh API and browser reads verified topic/order/Deny values after native edits. The six disposable policy/user/role fixtures were removed and absence confirmed.

**Historical candidate 2133 failure remains recorded:** role-policy Edit opened an unregistered route; the corresponding user-policy route and direct single-policy GET/PUT were absent in source. Its native CLI had list/add/remove. The 2177 corrected modal and native get/update evidence supersedes that local edit hold only for the tested artifacts.

The remote module uses separate scoped list/put/delete operations with scope ID and expected revision. Deployed remote/security mutations, restart persistence, multi-replica behavior and full release qualification remain separately gated. Candidate 2177 does not contain the later development endpoint `--include-etag`/`--if-match` guard changes.

Use installed help/request schemas, read back every write and test real authorization. Retain a tested recovery login before administrative/provider changes.

Next: [Roles](roles.md), [system permissions](system-events.md), [troubleshooting](operations.md).
