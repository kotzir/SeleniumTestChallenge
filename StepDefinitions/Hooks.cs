using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using AventStack.ExtentReports.Gherkin.Model;
using ScreenRecorderLib;
using NLog;
using TechTalk.SpecFlow;

namespace SeleniumTestChallenge.StepDefinitions
{
    [Binding]
    public class Hooks
    {
        private static ExtentReports extent;
        private static ExtentTest featureName;
        private static ExtentTest scenarioName;

        private static Recorder screenRecorder;

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ScenarioContext _scenarioContext;
        private readonly string applicationUrl = "https://www.saucedemo.com/";
        private readonly string userName = "standard_user";
        private readonly string password = "secret_sauce";

        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static String testResultPath = dir.Replace("bin\\x64\\Debug\\net6.0", "TestResults");

        [BeforeTestRun]

        public static void BeforeTestRun()
        {
            var htmlReporter = new ExtentHtmlReporter(testResultPath);
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.ReportName = "Automation Status Report";
            htmlReporter.Config.DocumentTitle = "Automation Status Report";
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            screenRecorder = Recorder.CreateRecorder(new RecorderOptions
            {
                OutputOptions = new OutputOptions
                {
                    RecorderMode = RecorderMode.Video,
                    //This sets a custom size of the video output, in pixels.
                    OutputFrameSize = new ScreenSize(1920, 1080),
                    //Stretch controls how the resizing is done, if the new aspect ratio differs.
                    Stretch = StretchMode.Uniform,
                },
                VideoEncoderOptions = new VideoEncoderOptions
                {
                    Framerate = 30,
                    IsFixedFramerate = true,
                    //Currently supported are H264VideoEncoder and H265VideoEncoder
                    Encoder = new H264VideoEncoder
                    {
                        BitrateMode = H264BitrateControlMode.CBR,
                        EncoderProfile = H264Profile.Main,
                    },
                    //Fragmented Mp4 allows playback to start at arbitrary positions inside a video stream,
                    //instead of requiring to read the headers at the start of the stream.
                    IsFragmentedMp4Enabled = true,
                    //If throttling is disabled, out of memory exceptions may eventually crash the program,
                    //depending on encoder settings and system specifications.
                    IsThrottlingDisabled = false,
                    //Hardware encoding is enabled by default.
                    IsHardwareEncodingEnabled = true,
                    //Low latency mode provides faster encoding, but can reduce quality.
                    IsLowLatencyEnabled = false,
                    //Fast start writes the mp4 header at the beginning of the file, to facilitate streaming.
                    IsMp4FastStartEnabled = false
                },
                AudioOptions = new AudioOptions
                {
                    IsAudioEnabled = false
                }
            });

            string videoPath = Path.Combine(testResultPath, "TestExecution.mp4");
            screenRecorder.Record(videoPath);

        }

        [AfterTestRun]

        public static void AfterTestRun()
        {
            extent.Flush();
            screenRecorder.Stop();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            scenarioName = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            Logger.Info("Opening browser");
            IWebDriver driver = new EdgeDriver();
            driver.Manage().Window.Maximize();
            scenarioContext["driver"] = driver;
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            Logger.Info("Closing the browser.");

            if (scenarioContext["driver"] is IWebDriver driver)
            {
                driver.Quit();
            }
        }

        [BeforeStep]
        public void BeforeStep()
        {

        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenariocontext)
        {
            var stepType = scenariocontext.StepContext.StepInfo.StepDefinitionType.ToString();
            //Console.WriteLine("Step type: " + stepType);
            //Console.WriteLine("Step text: " + scenariocontext.StepContext.StepInfo.Text);

            if (scenariocontext.TestError == null)
            {
                if (stepType == "Given")
                    scenarioName.CreateNode<Given>(scenariocontext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    scenarioName.CreateNode<When>(scenariocontext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    scenarioName.CreateNode<Then>(scenariocontext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    scenarioName.CreateNode<And>(scenariocontext.StepContext.StepInfo.Text);
            }
            else if (scenariocontext.TestError != null)
            {
                if (stepType == "Given")
                    scenarioName.CreateNode<Given>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.Message);
                else if (stepType == "When")
                    scenarioName.CreateNode<When>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.Message);
                else if (stepType == "Then")
                    scenarioName.CreateNode<Then>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.Message);
                else if (stepType == "And")
                    scenarioName.CreateNode<And>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.Message);
            }

        }
    }
}