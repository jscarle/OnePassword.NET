# Running tests

Due to the fact that this library acts as a wrapper for the CLI and in order for tests to have any significant value,
the majority of tests are integration tests which must run against an active 1Password account (preferably a Business
account).

### Effects on the active account

The integration tests will sign in to the specified account, then create and update a test group, a test user
(optional), and a test vault. Items will then be created and update in the test vault. Finally, the test vault, user,
and group will be deleted. _Note: If the integration tests fail, test data may remain in the specified account._

### Configuration

The integration tests are configured using the environment variables which are prefixed with `OPT_` (**O**ne**P**assword
**T**ests).
Environment variables which are integers must contain only numbers and those which are booleans must contain `true` or
`false` as a string.

| Environment Variable          | Description                                                                   | Type   | Default Value         |
|-------------------------------|-------------------------------------------------------------------------------|--------|-----------------------|
| OPT_COMMAND_TIMEOUT           | The timeout (in minutes) for each CLI command.                                | int    | `2`                   |
| OPT_RATE_LIMIT                | The rate (in milliseconds) at which commands are executed.                    | int    | `250`                 |
| OPT_RUN_LIVE_TESTS            | Activates or deactivates integration tests.                                   | bool   | `false`               |
| OPT_CREATE_TEST_USER          | Activates or deactivates the creation of the test user and its related tests. | bool   | `false`               |
| OPT_ACCOUNT_ADDRESS           | The account address. Should be the host name only.                            | string |                       |
| OPT_ACCOUNT_EMAIL             | The email to use when authenticating.                                         | string |                       |
| OPT_ACCOUNT_NAME              | The account name. Used to test account related commands.                      | string |                       |
| OPT_ACCOUNT_PASSWORD          | The password to use when authenticating.                                      | string |                       |
| OPT_ACCOUNT_SECRET_KEY        | The secret key to use when authenticating.                                    | string |                       |
| OPT_TEST_USER_EMAIL           | The test user's email address.                                                | string |                       |
| OPT_TEST_USER_CONFIRM_TIMEOUT | The time (in minutes) to wait for manual confirmation of the test user.       | int    | `OPT_COMMAND_TIMEOUT` |
