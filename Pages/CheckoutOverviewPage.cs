using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumTestChallenge.Pages
{
    public class CheckoutOverviewPage
    {
        private readonly IWebDriver _driver;
        IWebElement FinishButton => _driver.FindElement(By.Name("finish"));

        public CheckoutOverviewPage(IWebDriver driver)
        {
            _driver = driver;
        }    
        
        public void ClickContinue()
        {
            FinishButton.Click();
        }
        public void VefiryProductsAndPrices(ProductsPage productsPage)
        {
            IList<IWebElement> cartItems = _driver.FindElements(By.ClassName("cart_item"));

            Console.WriteLine(cartItems.Count);

            (string name, string price) cartItem1 = GetCartItemDetails(cartItems[0]);
            (string name, string price) cartItem2 = GetCartItemDetails(cartItems[1]);

            VerifyItemDetails(productsPage.FirstItem, cartItem1);
            VerifyItemDetails(productsPage.SecondItem, cartItem2);

        }
        private static (string name, string price) GetCartItemDetails(IWebElement item)
        {
            string name = item.FindElement(By.ClassName("inventory_item_name")).Text;
            string price = item.FindElement(By.ClassName("inventory_item_price")).Text;
            return (name, price);
        }
        private static void VerifyItemDetails((string name, string price, IWebElement button) expectedItem, (string name, string price) actualItem)
        {
            Assert.That(expectedItem.name, Is.EqualTo(actualItem.name));
            Assert.That(expectedItem.price, Is.EqualTo(actualItem.price));
        }
    }
}
