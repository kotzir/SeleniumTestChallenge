using OpenQA.Selenium;
using SeleniumTestChallenge.Pages;

namespace SeleniumTestChallenge.Steps
{
    [Binding]
    public class CheckoutOverviewSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly CheckoutOverviewPage _checkoutOverviewPage;

        public CheckoutOverviewSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = (IWebDriver)_scenarioContext["driver"];
            _checkoutOverviewPage = new CheckoutOverviewPage(_driver);
        }

        [Then(@"the products and their prices are verified")]
        public void ThenTheProductsAndTheirPricesAreVerified()
        {
            Hooks.Hooks.LogInfo("Verifying the products and their prices");
            var productsPage = (ProductsPage)_scenarioContext["ProductsPage"];
            _checkoutOverviewPage.VefiryProductsAndPrices(productsPage);
        }

    }
}
