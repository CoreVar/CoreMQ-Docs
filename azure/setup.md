# Deploy CoreMQ on Azure Kubernetes Service

The [public CoreMQ Azure Marketplace offer](https://marketplace.microsoft.com/en-us/product/container/core-var.coremq-kubernetes?tab=Overview) installs a Kubernetes application into **your existing AKS cluster**. It is not the retired Express virtual-machine offer. You operate the cluster, data services, networking, DNS, and certificates. The Marketplace software charge is currently USD $0.19 per running broker pod-hour; the default two replicas are $0.38/hour before Azure infrastructure and data-service charges. Check the plan price shown in Marketplace before creating the deployment.

## Before you deploy

- Have an AKS cluster with capacity for at least two Linux AMD64 broker pods. Select the **resource group containing that cluster** on the Azure deployment form. CoreMQ does not create the cluster.
- Provide a PostgreSQL database and a Redis service reachable from broker pods. The published 0.1.6 Marketplace form asks for their connection strings; it does not create either service. Store and handle those strings as secrets.
- Choose the MQTT DNS hostname you will use. Arrange DNS and a network path to the broker separately. The package's default Kubernetes Service is `ClusterIP`, so installing it does not expose a public MQTT address.
- For an encrypted first connection, have a PFX certificate valid for that hostname. The form accepts a base64-encoded PFX when **Enable MQTTS on port 8883** is selected. Deployment does not issue a certificate or create DNS records.

## Install from Marketplace

1. Open [CoreMQ's live Azure deployment page](https://portal.azure.com/#create/core-var.coremq-kubernetescoremq-hourly), select the public hourly plan, and sign in to the Azure subscription that contains your AKS cluster.
2. On **Basics**, select the cluster's resource group and region. In **CoreMQ configuration**, enter the **Existing AKS cluster name** exactly as it appears in Azure. The published 0.1.6 form uses a name field; a newer form may offer a cluster selector.
3. Select the broker replica count (two by default), enter the MQTT hostname, and create an initial administrator username and a unique password of at least 12 characters. Keep these credentials private.
4. Enter the reachable PostgreSQL and Redis connection strings. To enable MQTTS at installation, select **Enable MQTTS on port 8883** and provide the base64-encoded PFX in the protected certificate field. The certificate must cover the MQTT hostname. Do not put a certificate, password, or connection string in a resource name, tag, command history, or support ticket.
5. Review the Marketplace plan, Azure resources, and terms displayed by Azure, then create the deployment. Wait for the ARM deployment and CoreMQ cluster extension to report **Succeeded**.

The published package installs into the `coremq` namespace. From a workstation authorized for the cluster, verify that the broker pods are Ready and inspect the Service ports:

```sh
az aks get-credentials --resource-group <cluster-resource-group> --name <cluster-name>
kubectl -n coremq get deployments,pods,services
kubectl -n coremq rollout status deployment/<coremq-deployment>
```

Choose the deployment and Service names from the `kubectl` output. The default Service is internal to the cluster. Expose only the required MQTTS port through a network design you control, such as a private TCP load balancer, and point your MQTT hostname to that endpoint. Preserve TLS through to CoreMQ and verify the certificate hostname. Do not expose the plaintext MQTT port 1883 or the management port 8080 to the public Internet as a shortcut.

## Send a first encrypted message

1. Reach the browser management interface through an authorized private route. For a local administration session, you can forward the internal management Service to loopback with `kubectl -n coremq port-forward service/<coremq-service> 8080:8080` and open `http://127.0.0.1:8080`. Log in with the initial administrator account. The port-forward is for local administration; it is not a public HTTPS endpoint.
2. Under **Configure > Identities**, create a separate test client account. Configure its publish and subscribe message policies for a narrow test topic, such as `demo/first`. Administrator login and MQTT client permissions are separate. Review [users](../users.md), [policies](../policies.md), and [endpoints](../endpoints.md).
3. Confirm that the MQTTS listener is enabled, your TCP endpoint is reachable on port 8883, DNS resolves to it, and the certificate validates for the hostname. Use an MQTT client that verifies TLS; never use its insecure-certificate option to make this check pass.
4. With Eclipse Mosquitto clients **2.1 or later**, place the test username, password, hostname, port, and certificate authority settings in a local file accessible only to you. For example, `client.conf` can contain the following options, one per line. Replace every placeholder with your deployment's values and protect or delete the file after the test:

   ```text
   -h broker.example.com
   -p 8883
   -u test-client
   -P <test-client-password>
   --cafile /path/to/trusted-ca.pem
   ```

5. In one terminal, subscribe, then publish from another terminal. Both commands read credentials from the protected config file rather than putting the password on the command line:

   ```sh
   mosquitto_sub -o client.conf -t demo/first -q 1 -C 1 -W 30
   mosquitto_pub -o client.conf -t demo/first -q 1 -m 'hello from CoreMQ'
   ```

The subscriber should print `hello from CoreMQ`. If it does not, check pod readiness, the Service and network path, DNS, the certificate chain and hostname, client authentication, and the topic policies. A healthy Kubernetes deployment alone does not prove client access.

The [CoreMQ configuration guides](../README.md) cover later endpoint, identity, and policy changes. Features in those development guides may require a newer broker build than the currently installed Marketplace package.
