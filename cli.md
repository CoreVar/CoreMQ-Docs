# CoreMQ CLI

The product and CLI are **CoreMQ**. The executable is `coremq` (`coremq.exe` on
Windows).

For remote management, use the compatible `corevar coremq` module with an explicitly selected CoreControl deployment. Standalone `coremq` connects directly to a broker; these routes have distinct credentials and revision formats. See [remote management](remote-management.md).

## Connect and get help

```powershell
coremq connect https://mqtt.example.com --username admin
coremq status
coremq status --output json
coremq --help
coremq users --help
coremq endpoints add --schema
coremq disconnect
```

When no password source is supplied, an interactive terminal prompts for the password without echoing characters. Use `--no-prompt` to disable this fallback for a command. Redirected or noninteractive execution must supply `--password-file` or `--password-stdin`; it fails instead of waiting for keyboard input. Explicit sources bypass prompting, multiple sources are rejected, and cancellation preserves the saved connection.

The URL is the HTTP(S) **management origin**, not the MQTT listener. The username
defaults to `admin`. `connect` also accepts `--password-stdin` (one line), or the
legacy `--password <value>` option. File/stdin input avoids putting a password
in command history. Password files may end with a newline.

The saved connection contains the target URL and bearer token, never the
password. Its default location is `~/.coremq/config.json`; set
`COREMQ_CONFIG_PATH` to use a different protected file. Protect this file like a
password. On Unix it is created with owner-only read/write access; on Windows
it inherits the containing directory's ACL. `disconnect` deletes the saved
connection. It does not revoke the server token or clear environment variables.

For CI, set **both** `COREMQ_URL` and `COREMQ_TOKEN`. For a one-command override,
use **both** `--url <origin>` and `--token-file <path>`. Override pairs are
required so a saved token is never accidentally sent to a different broker.
HTTP redirects are not followed. Reconnect when the bearer token expires.

## Complete configuration input

Every JSON configuration command accepts exactly one of:

- `--file <path>`: read a UTF-8 JSON API request.
- `--file -`: read the JSON request from stdin in a one-shot invocation.
- `--data <json>`: pass an inline JSON request using your shell's quoting rules.

Use `--schema` on any JSON configuration command to print its JSON Schema.
Schemas come from the broker's shared, source-generated request contracts and
include nested fields, enums and polymorphic authentication/policy types. The
CLI validates JSON against the request contract and transmits all supplied
fields. It moves polymorphic `type` discriminators before ordinary properties,
as required by the broker's JSON reader. The server performs semantic
validation and enforces deployment-specific restrictions.

```powershell
coremq endpoints add --file endpoint.json
Get-Content -Raw uplink.json | coremq uplinks validate --file -
coremq uplinks put site-uplink --file uplink.json
coremq security identities put device-17 --file identity.json
coremq schema-policies put telemetry --file schema-policy.json
```

For revisioned configuration, include `expectedRevision` in an update body.
Use `null` for creation where the API permits it. Deletion requires
`--expected-revision <revision>`; the CLI never fetches or substitutes a newer
revision and never silently retries a mutation after a conflict.

API responses are emitted as JSON, except an empty response produces no output.
`status` retains its compact text display; `status --output json` returns all
status fields, including cluster and relay information. Successful `connect`,
`disconnect`, `start` and `stop` print short text confirmations.

Exit codes: **0** success; **1** HTTP/runtime failure or a validation/refresh
response with `valid: false` or `succeeded: false`; **2** invalid arguments or
request JSON. HTTP error bodies go to stderr. A rejected mutation never prints
a success message.

## Commands

`<id>` is the ID returned by the corresponding API, not necessarily its name.
Commands below marked **JSON** take `--file`, `--file -`, or `--data`.

| Command | Input / purpose |
|---|---|
| `connect <url>` | Authenticate; password options above |
| `disconnect` | Remove saved connection |
| `start` | Start the local broker executable |
| `stop` | Stop the local broker process |
| `status` | Broker counters; `--output json` for complete status |
| `about` | Broker version/edition |
| `setup show` | Read deployment setup information |
| `health live` | Liveness; optional anonymous `--url <origin>` |
| `health ready` | Readiness/dependency health; optional anonymous `--url <origin>` |
| `watch <read-only command>` | Repeat monitoring with `--interval` and `--count` |
| `diagnostics run` | DNS/TCP/health/API-access report; optional `--report <path>` |
| `endpoints list` | List listeners |
| `endpoints get <id>` | Read listener |
| `endpoints add` | **JSON**: all TCP, TLS, WebSocket and authentication settings |
| `endpoints remove <id>` | Remove listener |
| `endpoints certificate show <id>` | Read certificate metadata |
| `endpoints certificate upload <id> --file <pfx>` | Upload binary, passwordless PKCS#12; no stdin |
| `endpoints certificate refresh <id>` | Refresh certificate |
| `users list` | List users |
| `users get <id>` | Read user |
| `users add` | **JSON**: create user |
| `users remove <id>` | Delete user |
| `users enable <id>` | Enable user |
| `users disable <id>` | Disable user |
| `users password set <id>` | **JSON**: set password |
| `users certificate set <id>` | **JSON**: register client certificate using `certificateBase64` |
| `users roles list <id>` | Read assigned roles |
| `users roles update <id>` | **JSON**: `add`/`remove` arrays of role names |
| `users admin grant <id>` | Assign `User Manager` and `Endpoint Manager` |
| `users admin revoke <id>` | Remove those two roles, preserving other roles |
| `users policies list <id>` | List user policies |
| `users policies add <id>` | **JSON**: add user access policy |
| `users policies remove <id> <policyId>` | Remove user policy |
| `roles list` | List roles |
| `roles get <id>` | Read role |
| `roles add` | **JSON**: create role |
| `roles update <id>` | **JSON**: rename role |
| `roles remove <id>` | Delete role |
| `roles policies list <id>` | List role policies |
| `roles policies add <id>` | **JSON**: add role access policy |
| `roles policies remove <id> <policyId>` | Remove role policy |
| `policies list` | List global access policies |
| `policies get <id>` | Read global policy |
| `policies add` | **JSON**: create global access policy |
| `policies update <id>` | **JSON**: update global message policy |
| `policies remove <id>` | Delete global policy |
| `connections list` | List connected MQTT clients |
| `connections disconnect <id>` | Disconnect MQTT client |
| `subscriptions list` | List MQTT subscriptions |
| `uplinks status` | Runtime uplink status |
| `uplinks list` | List configured uplinks; pagination |
| `uplinks validate` | **JSON**: validate uplink without saving |
| `uplinks put <id>` | **JSON**: create/update all uplink settings |
| `uplinks remove <id>` | Requires `--expected-revision` |
| `security authorities list` | List trusted authorities; pagination |
| `security authorities get <id>` | Read trusted authority |
| `security authorities put <id>` | **JSON**: create/update authority and PEM bundle |
| `security authorities remove <id>` | Requires `--expected-revision` |
| `security identities list` | List workload identities; pagination |
| `security identities put <id>` | **JSON**: create/update identity, certificate identity, client/site IDs, roles and enabled state |
| `security identities remove <id>` | Requires `--expected-revision` |
| `security audit list` | Audit records; pagination |
| `fleet list` | Pagination, `--site-id`, `--query` |
| `fleet disconnect <id>` | Disconnect devices for a workload identity |
| `schema-policies list` | List payload policies; pagination |
| `schema-policies validate` | **JSON**: validate payload policy without saving |
| `schema-policies put <id>` | **JSON**: create/update policy, schema, mode, topic and limits |
| `schema-policies remove <id>` | Requires `--expected-revision` |
| `schema-policies metrics` | Read validation counters |

Pagination options are `--offset <n>` and `--limit <n>`. Each invocation returns
one API page; advance the offset to enumerate large collections.

For a simple listener, the existing shortcut remains available:

```powershell
coremq endpoints add edge --hostname mqtt.example.com --mqtt-port 1883 --mqtts-port 8883
coremq endpoints add web --mqtt-port null --http-websocket-port 8081 --websocket-path /mqtt
```

The shortcut also accepts `--https-websocket-port`. Use full JSON for IP address
bindings and Basic, client-certificate or OAUTHBEARER authentication settings:

```json
{
  "name": "secure-edge",
  "hostname": "mqtt.example.com",
  "mqttsPort": 8883,
  "httpsWebSocketPort": 8443,
  "websocketPath": "/mqtt",
  "authentication": [
    { "type": "BasicAuthentication" },
    { "type": "OAuthBearerAuthentication" }
  ]
}
```

Role membership and access policy examples:

```json
{ "add": ["Device operator"], "remove": [] }
```

```json
{ "type": "message", "publish": "Allow", "subscribe": "Allow", "order": 10, "topics": ["site/17/#"] }
```

An access-policy update uses the message-policy fields without the `type`
discriminator. Uplink credentials are secret **references** exactly as in the
API, not plaintext connection secrets.

## Monitoring and troubleshooting

```powershell
coremq health live
coremq health ready --url https://mqtt.example.com
coremq watch status --interval 5 --count 12
coremq watch connections list --interval 10
coremq watch uplinks status --interval 5
coremq watch schema-policies metrics --interval 10
coremq security audit list --offset 0 --limit 100
coremq diagnostics run --timeout 10 --report coremq-diagnostics.json
```

`watch` accepts only read-only API commands, never mutations. It emits one JSON
line per sample with an observation timestamp, exit code and result. `--count 0`
(the default) runs until Ctrl+C; `--interval` defaults to five seconds. A finite
watch returns failure if any sample failed, even if later samples recovered.

`diagnostics run` checks local DNS resolution, TCP reachability of the management
port, HTTP/TLS access, broker liveness/readiness, and access to status, about,
uplink and schema-metrics APIs. It detects an HTML placeholder returned instead
of a JSON management API and provides suggested next checks. `--timeout` applies
to each check, not the report as a whole. Non-ready, unreachable and denied API
checks result in exit code 1; a denied check can mean insufficient role access
rather than a broker outage.

Diagnostics use the saved target/credentials by default. Supplying `--url` alone
probes anonymously; add `--token-file` to test authenticated API access. The
report contains timestamps, target origin, durations, status codes, content types
and advice. It excludes response bodies, tokens, passwords, configuration files
and message payloads. `--report` saves the same JSON printed to stdout. Broker
logs remain in the deployment's existing logging system; the CLI cannot retrieve
logs through an API the broker does not expose.

## Interactive shell and compatibility

Run `coremq` without arguments in a terminal. Type `help`, a command group plus
`--help`, or `exit`/`quit`. Paths with spaces can use single or double quotes;
backslashes in Windows paths are preserved. Use files for complex JSON in the
interactive shell. Piped stdin is reserved for one-shot invocations.

The legacy `start --setup`/`-s` switch is explicitly rejected: it was previously
accepted but ignored by the local process launcher. Use `start` and the management
commands. `setup show` reads the API; it is not the obsolete Azure ARM setup utility.

Some operations depend on the broker version and deployment policy. In particular,
admin grant/revoke require the corrected broker implementation included with this
change; older brokers returned success without changing those roles. Certificate
upload can be disabled for Kubernetes-mounted certificate deployments. The CLI
reports the API rejection and does not bypass that restriction.

Deployment-only settings (database connections, Redis, mounted secrets, CoreControl
enrollment, and process environment) still belong to the deployment configuration
when no broker UI/API exists for changing them. This CLI does not invent live-edit
APIs for those settings or act as a separate CoreControl portal client.

## Organization sign-in and identities

Use the current [provider guide](sign-in-providers.md) for authorization and activation requirements.

`coremq sign-in providers list` reads saved configuration and its revision; `coremq sign-in providers status` reads runtime provider status. Save a reviewed provider definition with `coremq sign-in providers put --file provider.json`. Use `--schema` on the save command for the JSON contract. Saving requires HTTPS and the current configuration revision. Manually saved changes require a broker restart; the guided CoreID flow is separate.

`coremq organization identities list` lists observed organizational identities. `coremq organization identities access <providerId> <identityId>` reads current CoreID application roles and effective broker permissions for an observed identity. Organizational identities remain independent of local broker accounts.

## Feedback and device revocation

`coremq feedback context` reads the feedback notice, available environment information and context version. `coremq feedback send --file feedback.json` submits the explicitly selected feedback and consent. The CLI obtains the antiforgery token automatically, without adding consent or environment details. `coremq feedback consent revoke --file revoke.json` revokes previously recorded consent at its expected revision.

`coremq security identities revoke <identityId> --file request.json` and `coremq security identities revocation <identityId> <operationId> --tenant-id <tenant> --deployment-id <deployment>` use a scoped workload service token. A normal administrator login token does not authorize device revocation. These commands preserve the service's tenant, deployment, operation ID and revision requirements.

## System event configuration

`coremq system-events show` reads the event publishing setting and revision. `coremq system-events put --file settings.json` saves `{ "enabled": true, "expectedRevision": 7 }` using the revision returned by the read command; the initial unsaved setting has a null revision. A stale revision is rejected.

The same command names work through the CoreVar CLI module on a selected CoreControl route. Remote writes additionally require a stable `--operation-id`. The broker's configuration read/write policies apply. Settings persist and peer instances refresh shared configuration every two seconds.

## UI and CLI parity boundaries

The September 30 source-bound local verification built both broker and standalone CLI at `e5b5eb1`. Reads and request-schema discovery are scoped checks, not production qualification. The local editors and CLI configuration reads were checked with an isolated synthetic session. Browser cookies are keyed by hostname/path, not port: simultaneous brokers on localhost can overwrite each other's session and cause apparent editor failures while CLI bearer-token access still works. Use isolated browser profiles/hostnames for parallel tests. Never silently switch a failed remote command to direct broker access.

A CLI prompt does not imply a browser/portal terminal has a supported secret-input adapter. Portal management uses its own protected input/upload controls. An unavailable input channel must fail safely, without echoing a secret or reading the server console. Deployment-only database, Redis, secret mounts and enrollment settings remain operator configuration; the CLI does not invent live-edit APIs for them.

Next: [monitoring and diagnostics](operations.md), [topic policies](policies.md), [release status](release-status.md).
