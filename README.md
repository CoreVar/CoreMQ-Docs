# CoreMQ documentation

CoreMQ is an MQTT broker with a browser management interface, a `coremq` command-line tool, and optional CoreControl remote management.

These guides describe the 0.1 development implementation. Availability depends on the broker build, edition, deployment policy and portal package version. A local development feature is not automatically available in an Azure Marketplace image or a hosted portal.

## Guides

- [Configure and monitor the broker](configure.md)
- [Users and certificate identities](users.md)
- [Roles](roles.md) and [message policies](policies.md)
- [Endpoints](endpoints.md)
- [MQTT bridges and stream destinations](bridges.md)
- [Browser sign-in providers](sign-in-providers.md)
- [System events](system-events.md)
- [Command-line management](cli.md)
- [CoreControl and management coverage](remote-management.md)
- [Feedback](feedback.md)
- [MQTT 5 support and limitations](mqtt5.md)
- [Azure deployment guide](azure/setup.md)
- [Recent changes](CHANGELOG.md)

The [Azure deployment guide](azure/setup.md) covers the public AKS Marketplace offer, its current network exposure warning, and a conditional encrypted first-message check. Read the warning before installing. Publishing broker source or documentation does not upgrade an installed container or Marketplace deployment.
