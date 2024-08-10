using OpenQA.Selenium;
using SeleniumTestChallenge.Pages;


namespace SeleniumTestChallenge.Steps
{
    [Binding]
    public class ProductsSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly ProductsPage _productsPage;

        public ProductsSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = (IWebDriver)_scenarioContext["driver"];
            _productsPage = new ProductsPage(_driver);
            _scenarioContext["ProductsPage"] = _productsPage;
        }

        [When(@"the user adds a product to the cart")]
        public void WhenTheUserAddsAProductToTheCart()
        {
            _productsPage.AddProductToCart();
        }

        [When(@"the user adds two random products to the cart")]
        public void WhenTheUserAddsTwoRandomProductsToTheCart()
        {
            Hooks.Hooks.LogInfo("Adding two random products to the cart");
            _productsPage.AddTwoRandomProductsToCart();
        }
    }
}
