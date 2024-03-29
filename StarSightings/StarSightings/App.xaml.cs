﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using StarSightings.Events;
using System.Device.Location;

namespace StarSightings
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();
                
                return viewModel;
            }
        }

        private static Configuration config = null;
        public static Configuration Config
        {
            get
            {
                // Delay creation of the view model until necessary
                if (config == null)
                    config = new Configuration();

                return config;
            }
        }

        private static ServerAPI ssapi = null;

        /// <summary>
        /// A static ServerAPI used by the views to bind against.
        /// </summary>
        /// <returns>The ServerAPI object.</returns>
        public static ServerAPI SSAPI
        {
            get
            {
                // Delay creation of the view model until necessary
                if (ssapi == null)
                    ssapi = new ServerAPI();

                return ssapi;
            }
        }

        private static GeoCoordinateWatcher watcher;
        public static GeoCoordinateWatcher GeoWatcher
        {
            get
            {
                if (watcher == null)
                {
                    watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default); // using high accuracy
                    watcher.MovementThreshold = 200; // use MovementThreshold to ignore noise in the signal
                    watcher.Start();
                }
                return watcher;
            }
        }

        
        private static WPClogger logger = null;

        /// <summary>
        /// A static ServerAPI used by the views to bind against.
        /// </summary>
        /// <returns>The ServerAPI object.</returns>
        public static WPClogger Logger
        {
            get
            {
                // Delay creation of the view model until necessary
                if (logger == null)
                    logger = new WPClogger(LogLevel.debug);

                return logger;
            }
        }

        /// <summary>
        /// Component used to handle unhandle exceptions, to collect runtime info and to send email to developer.
        /// </summary>
		public RadDiagnostics diagnostics;
        /// <summary>
        /// Component used to raise a notification to the end users to rate the application on the marketplace.
        /// </summary>
        public RadRateApplicationReminder rateReminder;

        
		/// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

				// Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

			//Creates an instance of the Diagnostics component.
            diagnostics = new RadDiagnostics();

            //Defines the default email where the diagnostics info will be send.
            diagnostics.EmailTo = "zhouzhin@gmail.com";

            //Initializes this instance.
            diagnostics.Init();
        
		      //Creates a new instance of the RadRateApplicationReminder component.
            rateReminder = new RadRateApplicationReminder();

            //Sets how often the rate reminder is displayed.
            rateReminder.RecurrencePerUsageCount = 50;
    
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            //Before using any of the ApplicationBuildingBlocks, this class should be initialized with the version of the application.
            ApplicationUsageHelper.Init("1.0");

            // send previous crash report
            if (App.Logger.hasCriticalLogged())
            {
                App.Logger.emailReport();
                App.Logger.clearEventsFromLog();
            }            
		}   

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                //This will ensure that the ApplicationUsageHelper is initialized again if the application has been in Tombstoned state.
                ApplicationUsageHelper.OnApplicationActivated();
            } 
 	
		    // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            App.SSAPI.InitialTansferStatusCheck();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
			// Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new RadPhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        private void OnAcceptTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        private void GoSettings(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        private LoginEventHandler myLoginEventHandler;
        private void GoLogin(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!App.ViewModel.NeedLogin)
            {
                MessageBoxResult result =
                    MessageBox.Show("Are you sure you want to logout of your StarSightings account?",
                    "Logout", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    myLoginEventHandler = new LoginEventHandler(LogoutCompleted);
                    App.SSAPI.LoginHandler += myLoginEventHandler;
                    App.SSAPI.Logout();
                }

            }
            else
            {

                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/LoginOptionsPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void LogoutCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (e.Successful)
            {
                App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_DEVICE;
                App.ViewModel.User = null;
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);
                Utils.RemoveIsolatedStorageSettings("User");

                App.ViewModel.NeedLogin = true;
                App.Config.Login();
            }
            else
            {
                App.ViewModel.NeedLogin = false;
                MessageBox.Show("Cannot logout, please try again.");
            }
        }
    }
}
