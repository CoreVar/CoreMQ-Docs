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

#### 2. Configure a New Global Policy
1. Click the Add Global Policy button
2. Select permissions for Publish/Subscribe. The options are: NotSet, Allow, and Deny.
3. Set the order to a number representing the order to apply the policy, lowest-to-highest.
4. Add any number of topics to the rule. MQTT wildcards (+ and #) can be used. A catch-all topic would be `#`
5. Click OK.

## Role-based Policies

Role-based policies enable policies to be configured at a role level. When a user is assigned a role, the policy is applied as a part of publish/subscribe actions. Because a user can have many roles associated with them, the highest-allowed policy is the one that is applied.

### Add a Role-based Policy

Adding a role-based policy can be done within the CoreMQ dashboard.

#### 1. Navigate to the Role's Page
1. Click the Setup link on the left navigation bar
2. Click the Roles tab button
3. Select the role you want to add the policy to

#### 2. Configure a New Role-based Policy
1. Click the Add Policy button
2. Select permissions for Publish/Subscribe. The options are: NotSet, Allow, and Deny.
3. Set the order to a number representing the order to apply the policy, lowest-to-highest.
4. Add any number of topics to the rule. MQTT wildcards (+ and #) can be used. A catch-all topic would be `#`
5. Click OK.

## User Policies

Policies can be configured at the user level. These policies are the last to be applied.

### Add a User Policy

Adding a user policy can be done within the CoreMQ dashboard.

#### 1. Navigate to the User's Page
1. Click the Setup link on the left navigation bar
2. Click the Users tab button
3. Select the user you want to add the policy to
4. Click the Policies tab button

#### 2. Configure a New User Policy
1. Click the Add Policy button
2. Select permissions for Publish/Subscribe. The options are: NotSet, Allow, and Deny.
3. Set the order to a number representing the order to apply the policy, lowest-to-highest.
4. Add any number of topics to the rule. MQTT wildcards (+ and #) can be used. A catch-all topic would be `#`
5. Click OK.

## Conclusion

By being able to optionally deny/allow publish/subscribe actions at a global, role-based, and user level, many different types of permission models can be applied.
