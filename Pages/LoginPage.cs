using OpenQA.Selenium;

namespace SeleniumTestChallenge.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly string _url = "https://www.saucedemo.com/";
        private IWebElement UserName => _driver.FindElement(By.Name("user-name"));
        private IWebElement PassWord => _driver.FindElement(By.Name("password"));
        private IWebElement LogInBtn => _driver.FindElement(By.Name("login-button"));

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }
        
        public void NavigateToPage()
        {
            _driver.Navigate().GoToUrl(_url);
        }

        public void LoginWithValidCredentials(string username, string password)
        {
            UserName.SendKeys(username);
            PassWord.SendKeys(password);
            LogInBtn.Click();
        }
    }
}
