Feature: Checkout Overview Validation

Checkout Overview Validation

@Test2
Scenario: Verify the products added to the cart and their prices in the checkout overview page
    Given the user navigates to the application URL
    And the user logs in with a valid username and password
    When the user adds two random products to the cart
    And the user goes to the cart
    And the user clicks the checkout button
    And the user completes the form
    Then the products and their prices are verified
