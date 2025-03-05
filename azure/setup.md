# Deploying CoreMQ to Microsoft Azure

This guide walks you through the deployment and configuration of CoreMQ using the Azure Marketplace Express setup. By following these steps, you'll deploy a fully configured, secure MQTT broker tailored for IoT communications—all through the intuitive Azure Portal interface.

---

## 1. Accessing the Azure Marketplace

1. **Navigate to CoreMQ Marketplace Listing:**  
   Open your web browser and navigate to the [Azure Portal - CoreMQ](https://portal.azure.com/#create/core-var.coremq-entrycoremq-express). Sign in with your Azure credentials.

2. **Select offering:**  
   Select the "Express" plan and click "Create".

---

## 2. Basics

### Project Details

- **Subscription**
  Select the subscription you want to deploy to.
- **Resource Group**
  Select the resource group you want to deploy to or create a new one.

### Instance Details

Upon selecting the Express version, you’ll be presented with the initial configuration page. This section covers the basic broker setup:

- **Region**
  Select the region you want to deploy to.
- **Broker Account Name:**  
  This name uniquely identifies your CoreMQ instance within Azure. Use only letters, numbers, and dashes; must be between 3 and 24 characters.  

- **Registration Information:**  
  - **Registration Link:**  
    An InfoBox provides a link to register your hostname with CoreVar. Click the link to obtain your registration code. This will open a new browser tab to the CoreMQ registration page. You will need to login, and can do so with your Microsoft Entra ID.
  - **Register a Hostname**
    On the registration page, enter a hostname. This will be the domain your broker will be available at. Once you've entered an available hostname, click "Create Setup Token". This will generate the registration code for your broker. **Do not share this code with anyone.**
  - **Broker Registration Code:**  
    Once you’ve registered, copy and paste your registration code into the designated text box.

### Managed Application Details

- **Application Name**
  This is the name of your application as it will show up in your list of resources. Pick something unique.

- **Managed Resource Group**
  This is the name of the managed resource group where all of the resources that operate CoreMQ are deployed to. This name is automatically generated, but it can be overridden if necessary.

---

## 3. Virtual Machine

CoreMQ is deployed on a virtual machine optimized for performance and security. In this step, you’ll configure the underlying VM:

- **Customize Your VM:**  
  - **VM Size:**  
    Choose a VM size from the Size Selector. The VM must support premium storage. The more powerful VM you select, the more throughput CoreMQ will handle.

---

## 4. MQTT and Endpoint Configuration

The next section allows you to configure CoreMQ’s MQTT-specific settings and endpoints:

### Admin User Setup
- **Admin Username:**  
  This user will have access to the management portal for further broker configuration. Must be 3 to 32 characters long using only letters and numbers.
  
- **Admin Password:**  
    You’ll be required to confirm the password to ensure accuracy. Password must be at least 12 characters long.  

### Endpoint Settings
Configure how your broker accepts connections:

- **Protocol Enablement:**  
  - **MQTT:**  
    Enable if you require unencrypted MQTT traffic (not recommended for production). The default port number for MQTT traffic is 1883.
  - **MQTTS:**  
    Enabled by default to secure MQTT connections with TLS encryption. The default port number for MQTTS traffic is 8883.
  - **HTTP (WebSockets):**  
    Can be enabled for web-based connections. A warning advises that HTTP traffic is not encrypted. The default port number for HTTP traffic is 80.
  - **HTTPS (WebSockets):**  
    Enabled by default for secure, encrypted HTTP connections. The default port number for HTTPS traffic is 443.

- **Websocket Path:**  
  This is the path that MQTT over Websockets will be available at. Must start with a forward slash and contain only lowercase letters and numbers.

- **Authentication Type:**
  Basic authentication is more secure and is recommended over anonymous.

---

## 5. Resource Tagging

In the final configuration step, you can assign tags to your deployed resources. Tags help you organize and manage resources such as storage accounts, virtual machines, network interfaces, and more within your Azure environment.

---

## 6. Review + create

This page contains pricing & terms, contact information, Co-Admin Access Permission, and a summary of the deployment configuration:
- **Basics:**
  - Subscription
  - Resource group
  - Region
  - Broker Account Name
  - Registration Code
  - Application Name
  - Managed Resource Group Name
- **Virtual Machine:**
  - Size
- **MQTT:**
  - Username
  - Admin Password (redacted)
  - MQTT Port (if selected)
  - MQTTS Port (if selected)
  - HTTP Port (if selected)
  - HTTPS Port (if selected)
- **Resource Tags:** Any resource tags you configured.

After reviewing your configuration and agreeing to the terms, click **Create**. Azure will then provision the necessary resources, install CoreMQ on the virtual machine, and apply your customized settings.

Once deployed, your application will be availabe at the hostname you configured in the CoreMQ hostname registration step. You can login to it with the admin username and password you configured.

---

## Final Notes

- **Security Considerations:**  
  Always use encrypted protocols (MQTTS and HTTPS) for production environments.  
- **Customization:**  
  While the default settings are optimized for most scenarios, you can adjust VM sizes to meet specific requirements.
- **Next Steps:**  
  Once deployed, use the [Azure management portal](https://portal.azure.com) to manage the deployed resources, and use the [CoreVar Portal](https://portal.corevar.com) to manage the hostname registration.

This guide provides the essential steps to get CoreMQ running on Azure with minimal hassle. For more detailed instructions on advanced configurations and troubleshooting, refer to the additional documentation sections on our website.

Happy deploying!
