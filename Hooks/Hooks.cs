using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using AventStack.ExtentReports.Gherkin.Model;
using ScreenRecorderLib;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SeleniumTestChallenge.Hooks
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

        public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultPath = dir.Replace("bin\\x64\\Debug\\net6.0", "TestResults");

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

            // Define the log file path
            var logFilePath = Path.Combine(testResultPath, "application.log");

            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = logFilePath,
                Layout = "${longdate}|${level:uppercase=true}|${message}${exception:format=toString}"
            };

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
        }

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
        public void BeforeScenario()
        {
            scenarioName = featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            Logger.Info("Opening browser");
            var driver = WebDriverManager.GetDriver();
            driver.Manage().Window.Maximize();
            _scenarioContext["driver"] = driver;
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            Logger.Info("Closing the browser.");

            if (scenarioContext["driver"] is IWebDriver)
            {
                WebDriverManager.CloseDriver();
            }
        }

        [BeforeStep]
        public void BeforeStep()
        {

        }

        [AfterStep]
        public void AfterStep()
        {
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            if (_scenarioContext.TestError == null)
            {
                switch (stepType)
                {
                    case "Given":
                        scenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case "When":
                        scenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case "Then":
                        scenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case "And":
                        scenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                }   
            }
            else
            {
                var errorMessage = _scenarioContext.TestError.Message;
                switch (stepType)
                {
                    case "Given":
                        scenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                        break;
                    case "When":
                        scenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                        break;
                    case "Then":
                        scenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                        break;
                    case "And":
                        scenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                        break;
                }
            }
        }

        // Centralized logging methods
        public static void LogInfo(string message)
        {
            Logger.Info(message);
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
        }

        public static void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        public static void LogWarn(string message)
        {
            Logger.Warn(message);
        }
    }
}