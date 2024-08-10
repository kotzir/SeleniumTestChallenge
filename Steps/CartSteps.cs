using OpenQA.Selenium;
using SeleniumTestChallenge.Pages;

namespace SeleniumTestChallenge.Steps
{
    [Binding]
    public class CartSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly CartPage _cartPage;

        public CartSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = (IWebDriver)_scenarioContext["driver"];
            _cartPage = new CartPage(_driver);
        }

        [When(@"the user goes to the cart")]
        public void WhenTheUserGoesToTheCart()
        {
            Hooks.Hooks.LogInfo("Going to the cart");
            _cartPage.GotoCart();
        }

        [When(@"the user clicks the checkout button")]
        public void WhenTheUserClicksTheCheckoutButton()
        {
            Hooks.Hooks.LogInfo("Clicking the checkout button");
            _cartPage.ClickCheckoutButton();
        }

    }
}
