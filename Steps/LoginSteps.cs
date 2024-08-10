using SeleniumTestChallenge.Pages;
using OpenQA.Selenium;

namespace SeleniumTestChallenge.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = (IWebDriver)_scenarioContext["driver"];
            _loginPage = new LoginPage(_driver);
        }

        [Given(@"the user navigates to the application URL")]
        public void GivenTheUserNavigatesToTheApplicationURL()
        {
            Hooks.Hooks.LogInfo("Navigating to the Saucedemo site.");
            _loginPage.NavigateToPage();
        }
        
        [Given(@"the user logs in with username ""([^""]*)"" and password ""([^""]*)""")]
        public void GivenTheUserLogsInWithUsernameAndPassword(string p0, string p1)
        {
            Hooks.Hooks.LogInfo($"Logging in with username '{p0}' and password '{p1}'.");
            _loginPage.LoginWithValidCredentials(p0, p1);
        }

    }
}
