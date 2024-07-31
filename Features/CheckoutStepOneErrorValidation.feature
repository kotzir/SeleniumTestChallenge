Feature: Checkout Step One Error Validation

Checkout Step One Error Validation

@Test1
Scenario: User cannot continue to checkout overview page if mandatory field is missing
    Given the user navigates to the application URL
    And the user logs in with a valid username and password
    When the user adds a product to the cart
    And the user goes to the cart
    And the user clicks the checkout button
    And the user tries to continue without filling in a mandatory field
    Then the user should not be able to proceed to the checkout overview page