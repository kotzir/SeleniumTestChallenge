using OpenQA.Selenium;

namespace SeleniumTestChallenge.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private IWebElement InputFirstName => _driver.FindElement(By.Id("first-name"));
        private IWebElement InputLastName => _driver.FindElement(By.Id("last-name"));
        private IWebElement InputPostalCode => _driver.FindElement(By.Id("postal-code"));
        private IWebElement ContinueButton => _driver.FindElement(By.Id("continue"));
        private IWebElement ErrorMessage => _driver.FindElement(By.ClassName("error-message-container"));
        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ClickContinue()
        {
            ContinueButton.Click();
        }

        public bool IsErrorMessageDisplayed()
        {
            return ErrorMessage.Displayed;
        }

        public void FillForm(string fname, string lastname, string postal_code)
        {
            InputFirstName.SendKeys(fname);
            InputLastName.SendKeys(lastname);
            InputPostalCode.SendKeys(postal_code);
        }
    }
}
