using NUnit.Framework;
using OpenQA.Selenium;
using NLog;

namespace SeleniumTestChallenge.StepDefinitions
{
    [Binding]
    [Scope(Tag = "Test2")]
    public class CheckoutOverviewValidationStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string userName = "standard_user";
        private readonly string password = "secret_sauce";
        private IList<IWebElement> products;
        private IList<IWebElement> cartItems;
        private (string name, string price, IWebElement button) firstItem;
        private (string name, string price, IWebElement button) secondItem;
        private (string name, string price) cartItem1;
        private (string name, string price) cartItem2;

        public CheckoutOverviewValidationStepDefinitions(ScenarioContext scenarioContext)
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

        [When(@"the user adds two random products to the cart")]
        public void WhenTheUserAddsTwoRandomProductsToTheCart()
        {
            Logger.Info("Adding two random products to the cart");
            products = driver.FindElements(By.XPath("//*[@class='inventory_item']"));
            var randomIndices = GetTwoRandomIndices(products.Count);

            firstItem = GetItemDetails(products[randomIndices.Item1]);
            secondItem = GetItemDetails(products[randomIndices.Item2]);

            firstItem.button.Click();
            secondItem.button.Click();

            Console.WriteLine($"Products: {products.Count}");
            Console.WriteLine($"Chosen: {randomIndices.Item1} {randomIndices.Item2}");
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

        [When(@"the user completes the form")]
        public void WhenTheUserCompletesTheForm()
        {
            Logger.Info("Filling the form");
            CompleteCheckoutForm("John", "Travolta", "65504");
            Thread.Sleep(2000);
        }

        [Then(@"the products and their prices are verified")]
        public void ThenTheProductsAndTheirPricesAreVerified()
        {
            Logger.Info("Verifying the products and their prices");
            cartItems = driver.FindElements(By.CssSelector(".cart_item"));

            cartItem1 = GetCartItemDetails(cartItems[0]);
            cartItem2 = GetCartItemDetails(cartItems[1]);

            VerifyItemDetails(firstItem, cartItem1);
            VerifyItemDetails(secondItem, cartItem2);
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

        private (string name, string price, IWebElement button) GetItemDetails(IWebElement item)
        {
            string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text;
            string price = item.FindElement(By.CssSelector(".inventory_item_price")).Text;
            IWebElement button = item.FindElement(By.CssSelector("button[data-test^='add-to-cart']"));
            return (name, price, button);
        }

        private (string name, string price) GetCartItemDetails(IWebElement item)
        {
            string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text;
            string price = item.FindElement(By.CssSelector(".inventory_item_price")).Text;
            return (name, price);
        }

        private void ClickElement(By by)
        {
            driver.FindElement(by).Click();
        }

        private void EnterText(By by, string text)
        {
            driver.FindElement(by).SendKeys(text);
        }

        private void CompleteCheckoutForm(string firstName, string lastName, string postalCode)
        {
            EnterText(By.XPath("//*[@name='firstName']"), firstName);
            EnterText(By.XPath("//*[@name='lastName']"), lastName);
            EnterText(By.XPath("//*[@name='postalCode']"), postalCode);
            ClickElement(By.XPath("//*[@name='continue']"));
            Thread.Sleep(2000);
        }

        private (int, int) GetTwoRandomIndices(int length)
        {
            Random random = new Random();
            int index1 = random.Next(length);
            int index2;
            do
            {
                index2 = random.Next(length);
            } while (index1 == index2);

            return (index1, index2);
        }

        private void VerifyItemDetails((string name, string price, IWebElement button) expectedItem, (string name, string price) actualItem)
        {
            Assert.That(expectedItem.name, Is.EqualTo(actualItem.name));
            Assert.That(expectedItem.price, Is.EqualTo(actualItem.price));
        }
    }
}
