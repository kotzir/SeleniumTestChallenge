using OpenQA.Selenium;

namespace SeleniumTestChallenge.Pages
{
    public class CartPage
    {

        private readonly IWebDriver _driver;
        private IWebElement CartButton => _driver.FindElement(By.ClassName("shopping_cart_link"));
        private IWebElement CheckoutButton => _driver.FindElement(By.Id("checkout"));
        public CartPage(IWebDriver driver)
        {
            _driver = driver;
        }
        public void ClickCheckoutButton()
        {
            CheckoutButton.Click();
        }
        public void GotoCart()
        {
            CartButton.Click();
        }
    }
}
