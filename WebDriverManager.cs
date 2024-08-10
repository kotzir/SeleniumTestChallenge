using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace SeleniumTestChallenge
{
    public static class WebDriverManager
    {
        [ThreadStatic]
        private static IWebDriver _webDriver;

        public static IWebDriver GetDriver()
        {
            if (_webDriver == null)
            {
                _webDriver = new EdgeDriver();
            }
            return _webDriver;
        }

        public static void CloseDriver()
        { 
            _webDriver?.Quit();
            _webDriver = null;
        }
    }
}
