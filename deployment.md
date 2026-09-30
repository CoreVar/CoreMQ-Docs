# Choose a deployment

Choose the distribution before creating resources. A cloud provider's image, billing identity and installer are a unit; substituting another provider's image is unsupported. Installation prerequisites do not establish Marketplace availability or production qualification.

| Distribution | Prerequisites | Current boundary |
| --- | --- | --- |
| Local/Docker local edition | Approved package, writable data directory, isolated listener access | Standalone SQLite; no CoreControl connector; development defaults must be replaced |
| Azure Marketplace AKS | Existing supported AKS cluster, database and Redis, DNS/network controls, certificate and subscription purchase permissions | Public offer has package-specific exposure risk; read the [Azure guide](azure/setup.md) before installing |
| AWS Marketplace EKS | Entitled private offer/account, approved version, two schedulable AMD64 nodes plus rollout space, PostgreSQL/Redis, existing shared state PVC, IRSA, certificate, ECR/STS/metering egress | Limited/private release; candidate 1754 has EKS evidence, newer CoreControl enrollment remains unqualified |
| Google Cloud Marketplace GKE | Approved offer metadata, GKE and provider identities, Artifact Registry access, database/backplane/state, TLS and DNS | Packaging implementation; publication, commercial metric and customer acceptance remain held |
| Provider-neutral Kubernetes | Approved image index/digest and runtime chart, dependencies and protected Secrets | Internal qualification candidate; no Marketplace entitlement or billing route |

Use the package README shipped with the exact version for supported values and Secret names. Do not reuse Azure labels on AWS/GCP, invent a generic Helm download URL, or assume all database providers are qualified by one PostgreSQL test.

## AWS preparation and installation

The reviewed AWS chart defaults to internal `ClusterIP` management and TLS MQTT. Networking and TLS termination remain your responsibility. The chart creates neither the EKS cluster nor IAM roles, database, Redis, PVC or credential Secrets.

1. Obtain the approved chart and image coordinates from the private release receipt. Verify checksums and immutable image/platform provenance. Confirm AWS entitlement and image-pull access in the target account/region.
2. Use IRSA for hourly `RegisterUsage`, with the exact namespace/service-account subject and STS audience in its OIDC trust. EKS Pod Identity, node-role credentials and static access keys are not substitutes for this metering path.
3. Pre-create protected Secrets: database/backplane `connection-string`, administrator `username`/`password`, and certificate `endpoint-1.pfx`. Reference their names in values, keeping secret values out of Git and command arguments.
4. Supply `state.existingClaim`: a persistent ReadWriteMany claim writable by UID/GID 1654 from every replica. Validate permissions and the actual storage driver. Keep provider files, uploads and protected CoreControl state across replacements.
5. Supply region, IRSA role ARN, MQTT hostname and approved image coordinates. Start with CoreControl disabled; its registration is a separate opt-in lifecycle.
6. Render and review the exact package before installation:

```text
helm lint ./approved-coremq-chart --strict -f private-values.yaml
helm template coremq ./approved-coremq-chart --namespace coremq -f private-values.yaml
helm upgrade --install coremq ./approved-coremq-chart --namespace coremq -f private-values.yaml --wait --timeout 10m
kubectl -n coremq get deployments,pods,services
```

Choose names and namespace from your release. Rendered output can contain sensitive configuration: review it locally and never attach it wholesale to a support ticket. A timeout may leave resources installed; inspect state before retrying. Verify per-replica metering, image IDs, readiness, external port exposure, certificate validation, a client round trip, authorization denial and persistent state before accepting the deployment.

AWS candidate 1754 acceptance run 1813 passed its messaging, persistence/replay, management authorization, dependency outage and upgrade checks. Its independent cleanup receipt is `DELETE_VERIFIED`. It explicitly left CoreControl incomplete and does not qualify production horizontal capacity or candidate 1863's enrollment Job. [Release status](release-status.md) records the evidence boundaries.

## Google Cloud preparation

Use only the approved GCP package. Confirm its Producer Portal service name, selected usage metric/unit, consumption-tracking label, image digest, chart version and product URL; sentinel metadata must not be deployed. Google's pod-runtime annotation is the package's billing mechanism, not AWS `RegisterUsage` or Azure labels. The target price is not a finalized commercial offer until provider approval. Complete a real GKE/customer acceptance before describing this as a supported production workflow.

## Cost review before deployment

Inventory existing clusters, databases, Redis, storage, registries, networking and their actual bills. Reuse suitable infrastructure; choose the minimum viable capacity while retaining rollout and failure headroom. Estimate software replica-hours plus cluster/node, storage, backup, logs, egress and network charges. Two replicas do not by themselves establish HA. Record owner, environment and expiry for experiments, arrange independent cleanup verification, and review billing again after charges settle. Use [sizing](sizing.md) to define a measured capacity decision.

Next: [CoreControl](remote-management.md), [backup](backup-restore.md), [operations](operations.md).
