# Users

Users can be configured with various types of control.

## Authentication
CoreMQ currently only supports basic authentication (username & password).

### Creating a New User

#### 1. Navigate to the Users Page
1. Click the Setup link on the left navigation bar
2. Click the Users tab button
3. Click Add User

#### 2. Configure a New User
1. Enter the username
2. Enter the password
3. Confirm the password
4. (Optionally) Add the user to the system roles: User Manager and Endpoint Manager
5. Click OK

## Roles

Users can be assigned roles, giving them properties of that role.

For the case of policies, because a user can have multiple roles assigned, the highest allowed role is applied.

### Add a Role Assignment to a User

#### 1. Navigate to the User's Basic Info Page
1. Click the Setup link on the left navigation bar
2. Click the Users tab button
3. Select the user
4. Click the Basic Info tab button

#### 2. Add the Role Assignment
1. Click the Add Role button
2. Select the Role you want to assign
3. When asked to confirm, click OK

## Policies

Users can have policies assigned to them. These policies are applied after global policies, and role policies, in that order.

### Add a User Policy

Adding a user policy can be done within the CoreMQ dashboard.

#### 1. Navigate to the User's Policies Page
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

Understanding how to manage users along with roles and policies is an essential part of securing an MQTT broker.
