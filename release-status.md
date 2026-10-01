# Versions and packages

Select the documentation version that matches your broker. Find the broker version under **Configure > About**, and use the corresponding CLI and deployment package.

## Choose a package

- **Local / Docker:** use the local edition and its persistent SQLite data directory.
- **Azure:** use the package and plan presented in your Azure Marketplace subscription, or the custom chart supplied for your installation.
- **AWS and Google Cloud:** obtain the package, image coordinates and provider configuration from CoreVar for your account or project.
- **Kubernetes:** use the runtime chart and immutable image supplied with your release.

Keep the image digest, chart version and database schema information with your deployment records. The database and certificate fields in a Marketplace form can differ from a custom chart; follow the README included with that package.

## Match management tools

Use the broker's installed CLI help and request schemas. For remote administration, keep CoreVar Portal, the CoreMQ module and the broker on compatible package versions. The deployment's advertised capabilities determine which remote controls are available.

## Upgrade deliberately

Read the package's upgrade instructions, take consistent backups, test restoration, and prepare rollback before changing the running image. See [upgrade and uninstall](upgrade-uninstall.md), [backup and restore](backup-restore.md), and [deployment choices](deployment.md).
