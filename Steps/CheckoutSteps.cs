using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumTestChallenge.Pages;

namespace SeleniumTestChallenge.Steps
{
    [Binding]
    public class CheckoutSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;
        private readonly CheckoutPage _checkoutPage;
        
        public CheckoutSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = (IWebDriver)_scenarioContext["driver"];
            _checkoutPage = new CheckoutPage(_driver);
        }

        //[When(@"the user tries to continue without filling in a mandatory field")]
        //public void WhenTheUserTriesToContinueWithoutFillingInAMandatoryField()
        //{
        //    Hooks.Hooks.LogInfo("Trying to continue without filling the mandatory fields");
        //    _checkoutPage.ClickContinue();
        //}

        [When(@"the user tries to continue without filling ""([^""]*)"", ""([^""]*)"" or ""([^""]*)""")]
        public void WhenTheUserTriesToContinueWithoutFillingOr(string p0, string p1, string p2)
        {
            Hooks.Hooks.LogInfo("Trying to continue without filling the mandatory fields");
            _checkoutPage.FillForm(p0, p1, p2);
            _checkoutPage.ClickContinue();
        }


        [Then(@"the user should not be able to proceed to the checkout overview page")]
        public void ThenTheUserShouldNotBeAbleToProceedToTheCheckoutOverviewPage()
        {
            Hooks.Hooks.LogInfo("An error message should have appeared");
            bool isElementDisplayed = _checkoutPage.IsErrorMessageDisplayed();
            Assert.That(isElementDisplayed, Is.EqualTo(true));
        }

        //[When(@"the user completes the form")]
        //public void WhenTheUserCompletesTheForm()
        //{
        //    Hooks.Hooks.LogInfo("Filling the form");
        //    _checkoutPage.FillForm("John", "Travolta", "65404");
        //    _checkoutPage.ClickContinue();
        //}

        [When(@"the user completes the form with firstname ""([^""]*)"">, lastname ""([^""]*)""> and postal code ""([^""]*)""")]
        public void WhenTheUserCompletesTheFormWithFirstnameLastnameAndPostalCode(string p0, string p1, string p2)
        {
            Hooks.Hooks.LogInfo("Filling the form");
            _checkoutPage.FillForm(p0,p1,p2);
            _checkoutPage.ClickContinue();
        }


    }
}
