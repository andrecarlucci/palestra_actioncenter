using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TdcToast {
    public sealed partial class App : Application {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif
        public App() {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            PushNotificationHelper.StartAsync();
            PushNotificationTask.RegisterAsync();

#if DEBUG
            if (Debugger.IsAttached) {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) {
                rootFrame = new Frame();
                rootFrame.CacheSize = 1;
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null) {
#if WINDOWS_PHONE_APP
                 // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                if (!rootFrame.Navigate(typeof (MainPage), e.Arguments)) {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}