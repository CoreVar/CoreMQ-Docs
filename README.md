# CoreMQ documentation

CoreMQ is an MQTT broker with a browser management interface, a `coremq` CLI, and optional CoreControl remote management in cloud builds. Start with your task below. These guides describe the reviewed development implementation; [release status](release-status.md) distinguishes live evidence from held features.

## Start and deploy

- [Install locally or with Docker; send a first message](get-started.md)
- [Choose a deployment: Azure, AWS, GCP or private Kubernetes](deployment.md)
- [Azure Marketplace AKS installation and network prerequisites](azure/setup.md)
- [Secure endpoints and manage certificates](endpoints.md)

## Configure access and messaging

- [Find your way around Configure](configure.md)
- [Local users, certificate identities and authorities](users.md)
- [Roles](roles.md) and [topic policies with worked examples](policies.md)
- [CoreID and other OIDC sign-in providers](sign-in-providers.md)
- [MQTT bridges and Kafka/Event Hubs/CoreStream destinations](bridges.md)
- [Payload schemas](schemas.md) and [system events](system-events.md)
- [CLI authentication, input and task reference](cli.md)
- [CoreControl registration, status, unregistration and portal coverage](remote-management.md)

## Operate and get help

- [Monitoring, troubleshooting and support handoff](operations.md)
- [Persistence, backup and restore](backup-restore.md)
- [HA and measured sizing constraints](sizing.md)
- [Upgrade, rollback and uninstall](upgrade-uninstall.md)
- [MQTT 5 support limits](mqtt5.md)
- [Feedback and consent](feedback.md)
- [Release behavior and known limitations](release-status.md)
- [Changes](CHANGELOG.md) and [documentation assessment](quality-review.md)

## How the parts fit

![MQTT clients use broker listeners; database and Redis support cloud state, bridges forward selected messages, and CoreControl carries outbound authorized management.](assets/coremq-architecture.svg)

The CLI uses the HTTP(S) management origin. Redis is the cloud backplane; bridges route selected messages to other systems. CoreControl carries authorized remote commands, not application payloads. These paths have different credentials and permissions.

Record the broker edition/version under **Configure > About** and compare approved image/chart identities. Updating docs does not upgrade installed Marketplace packages or portal controls. No production capacity or complete MQTT 5 qualification is implied by a successful build.
