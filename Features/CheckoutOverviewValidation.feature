Feature: Checkout Overview Validation

Checkout Overview Validation

@Test2
Scenario Outline: Verify the products added to the cart and their prices in the checkout overview page
    Given the user navigates to the application URL
    And the user logs in with username "<username>" and password "<password>"
    When the user adds two random products to the cart
    And the user goes to the cart
    And the user clicks the checkout button
    And the user completes the form with firstname "<firstname>", lastname "<lastname>" and postal code "<postalcode>"
    Then the products and their prices are verified

    Examples: Valid Credentials and Info
    | username                | password     | firstname | lastname | postalcode |
    | standard_user           | secret_sauce | John      | Travolta | 65404      |
    #| locked_out_user         | secret_sauce | Jane      | Doe      | 90210      |
    #| problem_user            | secret_sauce | Mary      | Poppins  | 12345      |
    #| performance_glitch_user | secret_sauce | Peter     | Parker   | 67890      |
    #| error_user              | secret_sauce | Tony      | Stark    | 11111      |
    #| visual_user             | secret_sauce | Clark     | Kent     | 22222      |