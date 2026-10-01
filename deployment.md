# Choose a deployment

Run CoreMQ in your cloud, your Kubernetes cluster, or locally. Choose a platform and database in the diagram to focus this guide. Leave both unselected to read every deployment path.

[Choose your deployment](deployment-choice.html){.docs-interactive .docs-immersive}

:::docs-section {#deploy-azure data-docs-section=azure data-docs-filter-cloud=azure}
## Azure

The [Azure Marketplace guide](azure/setup.md) takes you from an existing AKS cluster to your first encrypted message. You provide the cluster, backing database, Redis, DNS and certificate. The Marketplace form uses PostgreSQL; use the database fields and networking controls supplied by the package you install.

For a custom CoreMQ chart, select a relational database or Azure Cosmos DB using the backing-store settings below. Keep the management interface private and expose only the client listener ports you need.
:::

:::docs-section {#deploy-aws data-docs-section=aws data-docs-filter-cloud=aws}
## AWS

Obtain the AWS deployment package and version from CoreVar. Use the chart and image supplied for your AWS account and region; cloud images carry their own provider identity and billing configuration.

Prepare an EKS cluster with Linux AMD64 capacity for two broker pods and rollout headroom; a PostgreSQL or SQL Server database, or DynamoDB when the chart exposes that persistence mode; Redis; TLS and DNS; and ECR, STS and metering egress.

Use an IRSA service account with an exact namespace/service-account subject and STS audience, plus the Marketplace metering permissions required by your package. Pre-create protected Secrets for the database/backplane connection strings, initial administrator credentials and MQTT PFX certificate. Provide an existing ReadWriteMany state PVC writable by UID/GID 1654 from every broker replica.

Supply `state.existingClaim`, region, IRSA role ARN, MQTT hostname and the package's image coordinates. Start with CoreControl disabled and connect it separately using [remote management](remote-management.md).

```sh
helm lint ./coremq-chart --strict -f private-values.yaml
helm template coremq ./coremq-chart --namespace coremq -f private-values.yaml
helm upgrade --install coremq ./coremq-chart --namespace coremq -f private-values.yaml --wait --timeout 10m
kubectl -n coremq get deployments,pods,services
```

Use the chart path and namespace from your package. Rendered output can contain secrets: review it locally. If installation times out, inspect the existing deployment before retrying. Confirm pod readiness, certificate validation, a publish/subscribe round trip, authorization denial and persistent state.
:::

:::docs-section {#deploy-gcp data-docs-section=gcp data-docs-filter-cloud=gcp}
## Google Cloud

Obtain the Google Cloud package from CoreVar for your project. Prepare a GKE cluster, Redis, persistent state, TLS and DNS. Choose PostgreSQL, an existing SQL Server database, or Firestore using the database settings exposed by that package.

Use its supplied service name, usage metric, consumption-tracking label, image digest and chart version. Google Cloud's billing identity is distinct from AWS metering and Azure plan labels. Replace package placeholders before rendering or installing the chart.

For managed PostgreSQL, configure the package's Cloud SQL connection settings. For SQL Server, provide an existing reachable database and protected connection-string Secret. For Firestore, grant the broker's workload identity access to the selected project and collection.
:::

:::docs-section {#deploy-kubernetes data-docs-section=kubernetes data-docs-filter-cloud=kubernetes}
## Your Kubernetes cluster

Use the provider-neutral runtime chart and immutable image supplied with your release. Prepare a PostgreSQL or SQL Server database, or Azure Cosmos DB when using the chart's Cosmos mode, alongside Redis, persistent shared state, administrator credentials and a TLS certificate.

The chart references your existing dependencies and Secrets. Render it first, review image coordinates and Service exposure, then install into your chosen namespace. Use private management access and verify the client listener's DNS, certificate and message permissions.
:::

:::docs-section {#deploy-local data-docs-section=local data-docs-filter-cloud=local}
## Local or Docker

Follow [Send your first message](get-started.md). The local edition uses SQLite and a writable data directory. Keep that directory across container replacement, bind management access to the intended local interface, and replace initial administrator defaults.
:::

:::docs-section {#database-postgresql data-docs-section=postgresql data-docs-filter-database=postgresql data-docs-filter-cloud="azure aws gcp kubernetes"}
## PostgreSQL

Create a database reachable from the broker pods and a dedicated database user. Store its connection string in the Secret referenced by the chart. For relational charts:

```yaml
persistence:
  mode: relational
  provider: postgresql
  existingSecret: coremq-database
```

Use your package's Secret name and key; the AWS chart commonly uses `database`, while the Google Cloud chart uses `coremq-database`. The Secret contains the connection string, not the values file. The Azure Marketplace form accepts the connection string directly in its protected field.
:::

:::docs-section {#database-sqlserver data-docs-section=sqlserver data-docs-filter-database=sqlserver data-docs-filter-cloud="azure aws gcp kubernetes"}
## SQL Server

Use an existing reachable SQL Server database and a dedicated login with permissions for schema initialization and broker data. Select SQL Server in a custom chart that exposes the relational provider setting:

```yaml
persistence:
  mode: relational
  provider: sqlserver
  existingSecret: coremq-database
```

Store the connection string in the chart's referenced Secret, enable encrypted database transport and validate the server certificate. A package's PostgreSQL provisioning option does not provision SQL Server; bring the database separately. The Azure Marketplace PostgreSQL form is a separate installation path.
:::

:::docs-section {#database-cosmos data-docs-section=cosmos data-docs-filter-database=cosmos data-docs-filter-cloud="azure kubernetes"}
## Azure Cosmos DB

Provide an existing account, database and container, then configure the custom chart's Cosmos mode:

```yaml
persistence:
  mode: cosmos
  cosmos:
    endpoint: https://your-account.documents.azure.com:443/
    database: coremq
    container: coremq
    existingSecret: coremq-cosmos
```

Put the account credential in the Secret field specified by your chart. Cosmos provides the configuration document store; Redis remains the cloud messaging backplane. Review account throughput, backups and network access before connecting the broker.
:::

:::docs-section {#database-dynamodb data-docs-section=dynamodb data-docs-filter-database=dynamodb data-docs-filter-cloud=aws}
## DynamoDB

Use the AWS chart's DynamoDB persistence mode:

```yaml
persistence:
  mode: dynamodb
```

Create the table named by your package, with string keys `PK` and `SK`. The runtime default table name is `CoreMq`. Grant the broker's IRSA identity the required item read, query, write and delete access to that table. Credentials come from the workload identity. Leave `persistence.dynamodb.serviceUrl` empty for AWS; it is an override for a local test endpoint. Redis remains the messaging backplane.
:::

:::docs-section {#database-firestore data-docs-section=firestore data-docs-filter-database=firestore data-docs-filter-cloud=gcp}
## Firestore

Use the Google Cloud chart's Firestore mode and select your project and collection:

```yaml
persistence:
  mode: firestore
  firestore:
    projectId: your-project
    collection: coremq
```

Configure the broker's workload identity for access to that project and collection. Firestore provides the configuration document store; keep Redis for the cloud messaging backplane. Review access, retention and backups before installation.
:::

:::docs-section {#database-sqlite data-docs-section=sqlite data-docs-filter-database=sqlite data-docs-filter-cloud=local}
## SQLite

The local edition stores its database in the broker's persistent local data directory. Keep a writable volume across restarts and container replacements, and take a consistent [backup](backup-restore.md) before upgrading. Use this single-broker path for local or standalone deployments.
:::

## Before creating resources

Reuse suitable clusters, databases, Redis and storage. Estimate broker replica-hours, cluster/node capacity, database throughput, backups, logs, egress and networking charges. Include rollout and failure headroom; use [sizing](sizing.md) to choose capacity from your workload.

## After installation

Check pod readiness, private management access, DNS, certificate hostname and chain, client authentication, topic authorization and a real publish/subscribe exchange. Keep database and state backups, and follow [operations](operations.md), [backup and restore](backup-restore.md), and [upgrade](upgrade-uninstall.md) for ongoing maintenance.
