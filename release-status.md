# Versions and packages

Select the documentation version that matches your broker. Find the broker version under **Configure > About**, and use the corresponding CLI and deployment package.

## Choose a package

- **Local / Docker:** use the local edition and its persistent SQLite data directory.
- **Azure:** use the package and plan presented in your Azure Marketplace subscription, or the custom chart supplied for your installation.
- **AWS and Google Cloud:** obtain the package, image coordinates and provider configuration from CoreVar for your account or project.
- **Kubernetes:** use the runtime chart and immutable image supplied with your release.

Keep the image digest, chart version and database schema information with your deployment records. The database and certificate fields in a Marketplace form can differ from a custom chart; follow the README included with that package.

## Training, support and software billing

Product usage training, documentation, tutorials and labs are free. Support is
provided through shared CoreVar support agreements for eligible purchased
products; use the published purchase route or request a quote for negotiated
scope. There is no separate CoreMQ paid how-to lab or fleet allowance.

For the hourly Marketplace offer, include billable broker pod-hours in your
software estimate and review the selected plan's current price and terms before
purchase. Cluster compute, databases, Redis, storage and networking are separate
cloud costs. Microsoft's [container offer billing models](https://learn.microsoft.com/en-us/partner-center/marketplace-offers/marketplace-containers#licensing-options)
include per-pod pricing reported hourly. Follow your actual offer's metering rules
for standby replicas, replacement pods and upgrade overlap; this guide does not
activate a new price or license agreement.

## Match management tools

Use the broker's installed CLI help and request schemas. For remote administration, keep CoreVar Portal, the CoreMQ module and the broker on compatible package versions. The deployment's advertised capabilities determine which remote controls are available.

## Upgrade deliberately

Read the package's upgrade instructions, take consistent backups, test restoration, and prepare rollback before changing the running image. See [upgrade and uninstall](upgrade-uninstall.md), [backup and restore](backup-restore.md), and [deployment choices](deployment.md).
