using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumTestChallenge.Pages
{
    public class ProductsPage
    {
        private readonly IWebDriver _driver;

        private IWebElement product1 => _driver.FindElement(By.Name("add-to-cart-sauce-labs-backpack"));
        private IList<IWebElement> products => _driver.FindElements(By.ClassName("inventory_item"));
        public (string name, string price, IWebElement button) FirstItem { get; private set; }
        public (string name, string price, IWebElement button) SecondItem { get; private set; }

        public ProductsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void AddProductToCart()
        {
            product1.Click();
            Console.WriteLine("Product Added");
        }
        public void AddTwoRandomProductsToCart()
        {
            var randomIndices = GetTwoRandomIndices(products.Count);

            FirstItem = GetItemDetails(products[randomIndices.Item1]);
            SecondItem = GetItemDetails(products[randomIndices.Item2]);

            FirstItem.button.Click();
            SecondItem.button.Click();
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
        private (string name, string price, IWebElement button) GetItemDetails(IWebElement item)
        {
            string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text;
            string price = item.FindElement(By.CssSelector(".inventory_item_price")).Text;
            IWebElement button = item.FindElement(By.CssSelector("button[data-test^='add-to-cart']"));
            return (name, price, button);
        }

    }

    
}
