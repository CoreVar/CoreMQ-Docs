# Policies

Policies can be defined to restrict control over publishing/subscribing to various topics.

Policies exist in three scopes: global, role, and user. They are applied in that order. Within each scope, each policy can define an order (lowest-to-highest) in which it is applied within that scope. For role-based policies, because a user can have many roles assigned to them, the highest-allowed access is applied. Multiple topics can be specified for a single policy, as well as MQTT wildcards (+ and #) can be specified within the topic.

By default, no policies exist and any user can publish/subscribe to any topic. It is recommended to add a default global policy which denies publishing/subscribing to all topics.

## Global Policies

Global policies are the first policies applied for any publish/subscribe action.

### Add a Global Policy

Adding a global policy can be done within the CoreMQ dashboard.

#### 1. Navigate to the Global Policies
1. Click the Setup link on the left navigation bar
2. Click the Global Policies tab button

### 2. Configure a Global Policy
1. Click the Add Global Policies button
2. Select permissions for Publish/Subscribe. The options are: NotSet, Allow, and Deny.
3. Set the order to a number representing the order to apply the policy, lowest-to-highest.
4. Add any number of topics to the rule. MQTT wildcards (+ and #) can be used. A catch-all topic would be `#`
5. Click OK.

