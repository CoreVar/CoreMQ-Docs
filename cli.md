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
