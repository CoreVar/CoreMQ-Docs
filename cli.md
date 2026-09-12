# CoreMQ CLI

The executable is `coremq` (`coremq.exe` on Windows). Use the HTTP(S) management origin rather than the MQTT port:

```text
coremq connect https://broker.example.com --username admin
coremq --help
coremq status --output json
coremq endpoints add --schema
coremq disconnect
```

When no password source is supplied, `connect` prompts for the password in an interactive terminal without displaying the characters. Press Enter to submit; Backspace edits the input; Escape or Ctrl+C cancels. No password is added to the command text or saved connection.

Use `--no-prompt` to disable the interactive fallback. Redirected or noninteractive execution fails promptly when the password is missing. For automation, supply a protected password file or `--password-stdin`. Explicit password sources bypass the prompt; supplying multiple sources is rejected. Cancelling leaves an existing saved connection unchanged.

```text
coremq connect https://broker.example.com --no-prompt --password-file broker-password.txt
```

This behavior uses the shared CLI-Tools prompt service. Framework command authors can opt individual arguments/options into missing-value prompting and mark secret values for hidden input. Host-wide prompt policy can disable interaction; a portal host requires its own supported input adapter rather than reading the server console.

Avoid putting credentials directly in shell history. The saved connection contains a bearer token; protect it as a credential. `disconnect` removes the saved connection, not the server token.

JSON configuration commands accept one of `--file <path>`, `--file -` for stdin, or `--data <json>`. Use `--schema` for the request contract and `--help` for commands supported by your installed version.

Command areas include endpoints, users, roles, policies, connections, subscriptions, bridges/uplinks, security authorities and identities, uploaded files, schema policies, status and diagnostics. Revisioned updates carry `expectedRevision`; removals require `--expected-revision`. Read again after a conflict rather than silently substituting a newer revision.

CoreControl transports authorized management commands to the selected deployment. Bridges are a separate CoreMQ capability. Older module versions expose bridge operations under legacy names such as `control bridges-list`; those names describe remote routing, not a CoreControl bridge. Use the installed module's help to check its command names. A failed remote command does not fall back to direct broker access.

Current candidate commands include browser sign-in providers, organizational identities and system-event settings. End-to-end portal coverage is still being qualified. See [management coverage](remote-management.md). Installation/distribution of a CLI module is separate from updating these documentation files.

> The commands below are included in the current release candidate; Marketplace publication is still pending qualification.

## Organization sign-in and identities

`coremq sign-in providers list` reads saved configuration and its revision; `coremq sign-in providers status` reads runtime provider status. Save a reviewed provider definition with `coremq sign-in providers put --file provider.json`. Use `--schema` on the save command for the JSON contract. Saving requires HTTPS and the current configuration revision. Manually saved changes require a broker restart; the guided CoreID flow is separate.

Remove a manually configured provider with `coremq sign-in providers remove <id> --revision <revision>` using its current saved revision. Saving or removing a provider requires a restart before that change becomes active.

On a CoreControl route, provider list/put/remove and organizational identity list/access use the product's dedicated capabilities. Remote writes require a stable `--operation-id` and the numeric `expectedRevision` returned by the remote snapshot; direct broker provider configuration uses its string revision. Do not interchange the two revision formats. A client secret supplied through a protected input file or stdin is sealed for the specific provider before the command is sent. Permission checks remain enforced by both CoreControl and the broker.

`coremq organization identities list` lists observed organizational identities. `coremq organization identities access <providerId> <identityId>` reads current CoreID application roles and effective broker permissions for an observed identity. Organizational identities remain independent of local broker accounts.

## Feedback and device revocation

`coremq feedback context` reads the feedback notice, available environment information and context version. `coremq feedback send --file feedback.json` submits the explicitly selected feedback and consent. The CLI obtains the antiforgery token automatically, without adding consent or environment details. `coremq feedback consent revoke --file revoke.json` revokes previously recorded consent at its expected revision.

`coremq security identities revoke <identityId> --file request.json` and `coremq security identities revocation <identityId> <operationId> --tenant-id <tenant> --deployment-id <deployment>` use a scoped workload service token. A normal administrator login token does not authorize device revocation. These commands preserve the service's tenant, deployment, operation ID and revision requirements.

## System event configuration

`coremq system-events show` reads the event publishing setting and revision. `coremq system-events put --file settings.json` saves `{ "enabled": true, "expectedRevision": 7 }` using the revision returned by the read command; the initial unsaved setting has a null revision. A stale revision is rejected.

The same command names work through the CoreVar CLI module on a selected CoreControl route. Remote writes additionally require a stable `--operation-id`. The broker's configuration read/write policies apply. Settings persist and peer instances refresh shared configuration every two seconds.
