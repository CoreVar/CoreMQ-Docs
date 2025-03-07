# Roles

Users can be assigned roles, which will allow properties of those roles to be applied to the user.

### Add a Role

#### 1. Navigate to the Roles
1. Click the Setup link on the left navigation bar
2. Click the Roles tab button

#### 2. Configure a new Role
1. Click the Add Role button
2. Enter a name for the role
3. Click OK

### Add a Role-based policy

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
