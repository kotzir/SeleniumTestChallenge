using NUnit.Framework;
using OpenQA.Selenium;
using NLog;

namespace SeleniumTestChallenge.StepDefinitions
{
    [Binding]
    [Scope(Tag = "Test1")]
    public class CheckoutStepOneErrorValidationStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string userName = "standard_user";
        private readonly string password = "secret_sauce";

        public CheckoutStepOneErrorValidationStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            driver = (IWebDriver)_scenarioContext["driver"];
        }

        [Given(@"the user navigates to the application URL")]
        public void GivenTheUserNavigatesToTheApplicationURL()
        {
            Logger.Info("Navigating to the Saucedemo site.");
            NavigateToUrl("https://www.saucedemo.com/");
            Thread.Sleep(2000);
        }

        [Given(@"the user logs in with a valid username and password")]
        public void GivenTheUserLogsInWithAValidUsernameAndPassword()
        {
            Logger.Info("Logging in with valid credentials.");
            PerformLogin(userName, password);
            Thread.Sleep(2000);
        }

        [When(@"the user adds a product to the cart")]
        public void WhenTheUserAddsAProductToTheCart()
        {
            Logger.Info("Adding one product to the cart");
            ClickElement(By.XPath("//*[@name='add-to-cart-sauce-labs-backpack']"));
            Console.WriteLine("Product Added");
            Thread.Sleep(2000);
        }

        [When(@"the user goes to the cart")]
        public void WhenTheUserGoesToTheCart()
        {
            Logger.Info("Going to the cart");
            ClickElement(By.XPath("//*[@class='shopping_cart_link']"));
            Console.WriteLine("Went to cart");
            Thread.Sleep(2000);
        }

        [When(@"the user clicks the checkout button")]
        public void WhenTheUserClicksTheCheckoutButton()
        {
            Logger.Info("Clicking the checkout button");
            ClickElement(By.XPath("//*[@name='checkout']"));
            Console.WriteLine("Checkout button clicked");
            Thread.Sleep(2000);
        }

        [When(@"the user tries to continue without filling in a mandatory field")]
        public void WhenTheUserTriesToContinueWithoutFillingInAMandatoryField()
        {
            Logger.Info("Trying to continue without filling the mandatory fields");
            ClickElement(By.XPath("//*[@name='continue']"));
            Console.WriteLine("Trying to continue without mandatory fields");
            Thread.Sleep(2000);
        }

        [Then(@"the user should not be able to proceed to the checkout overview page")]
        public void ThenTheUserShouldNotBeAbleToProceedToTheCheckoutOverviewPage()
        {
            Logger.Info("An error message should have appeared");
            bool isElementDisplayed = IsElementDisplayed(By.XPath("//*[@class='error-button']"));
            Assert.That(isElementDisplayed, Is.EqualTo(true));
            Console.WriteLine("Error");
            Thread.Sleep(2000);
        }

        private void NavigateToUrl(string url)
        {
            driver.Url = url;
            Thread.Sleep(2000);
        }

        private void PerformLogin(string username, string password)
        {
            EnterText(By.XPath("//*[@name='user-name']"), username);
            EnterText(By.XPath("//*[@name='password']"), password);
            ClickElement(By.XPath("//*[@name='login-button']"));
        }

        private void ClickElement(By by)
        {
            driver.FindElement(by).Click();
        }

        private void EnterText(By by, string text)
        {
            driver.FindElement(by).SendKeys(text);
        }

        private bool IsElementDisplayed(By by)
        {
            return driver.FindElement(by).Displayed;
        }
    }
}
