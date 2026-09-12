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

The CoreVar-hosted CoreMQ module also provides CoreControl commands such as `control bridges-list`, `control bridges-validate`, `control bridges-put` and `control bridges-delete`. They use the remote command queue and require the target deployment's advertised capability and authorization. A failed remote command does not fall back to direct broker access.

Coverage is not yet universal: browser sign-in provider configuration and system-event settings do not have dedicated CLI commands. See [management coverage](remote-management.md). Installation/distribution of a CLI module is separate from updating these documentation files.

> The commands below are included in the current release candidate; Marketplace publication is still pending qualification.

## Organization sign-in and identities

`coremq sign-in providers list` reads saved configuration and its revision; `coremq sign-in providers status` reads runtime provider status. Save a reviewed provider definition with `coremq sign-in providers put --file provider.json`. Use `--schema` on the save command for the JSON contract. Saving requires HTTPS and the current configuration revision. Manually saved changes require a broker restart; the guided CoreID flow is separate.

`coremq organization identities list` lists observed organizational identities. `coremq organization identities access <providerId> <identityId>` reads current CoreID application roles and effective broker permissions for an observed identity. Organizational identities remain independent of local broker accounts.

## Feedback and device revocation

`coremq feedback context` reads the feedback notice, available environment information and context version. `coremq feedback send --file feedback.json` submits the explicitly selected feedback and consent. The CLI obtains the antiforgery token automatically, without adding consent or environment details. `coremq feedback consent revoke --file revoke.json` revokes previously recorded consent at its expected revision.

`coremq security identities revoke <identityId> --file request.json` and `coremq security identities revocation <identityId> <operationId> --tenant-id <tenant> --deployment-id <deployment>` use a scoped workload service token. A normal administrator login token does not authorize device revocation. These commands preserve the service's tenant, deployment, operation ID and revision requirements.

## System event configuration

`coremq system-events show` reads the event publishing setting and revision. `coremq system-events put --file settings.json` saves `{ "enabled": true, "expectedRevision": 7 }` using the revision returned by the read command; the initial unsaved setting has a null revision. A stale revision is rejected.

The same command names work through the CoreVar CLI module on a selected CoreControl route. Remote writes additionally require a stable `--operation-id`. The broker's configuration read/write policies apply. Settings persist and peer instances refresh shared configuration every two seconds.
