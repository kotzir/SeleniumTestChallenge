Feature: Checkout Step One Error Validation

Checkout Step One Error Validation

@Test1
Scenario Outline: User cannot continue to checkout overview page if mandatory field is missing
    Given the user navigates to the application URL
    And the user logs in with username "<username>" and password "<password>"
    When the user adds a product to the cart
    And the user goes to the cart
    And the user clicks the checkout button
    And the user tries to continue without filling "<First Name>", "<Last Name>" or "<Postal Code>"
    Then the user should not be able to proceed to the checkout overview page

    Examples: Missing Order Details
    | username      | password     | First Name | Last Name | Postal Code |
    | standard_user | secret_sauce |            |           |             | # All Fields Missing
    | standard_user | secret_sauce |            | Travolta  | 65404       | # Missing First Name
    | standard_user | secret_sauce | John       |           | 65404       | # Missing Last Name
    | standard_user | secret_sauce | John       | Travolta  |             | # Missing Postal Code
    | standard_user | secret_sauce |            |           | 65404       | # Missing First & Last Name
    | standard_user | secret_sauce |            | Travolta  |             | # Missing First Name & Postal Code
    | standard_user | secret_sauce | John       |           |             | # Missing Last Name & Postal Code