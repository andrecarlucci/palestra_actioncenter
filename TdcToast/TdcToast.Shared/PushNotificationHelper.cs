using System;
using System.Diagnostics;
using Windows.Networking.PushNotifications;

namespace TdcToast {
    public static class PushNotificationHelper {
        private static PushNotificationChannel _channel;

        public static async void StartAsync() {
            _channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            _channel.PushNotificationReceived += channel_PushNotificationReceived;
            Debug.WriteLine(_channel.Uri);
        }

        private static void channel_PushNotificationReceived(PushNotificationChannel sender,
            PushNotificationReceivedEventArgs args) {
            Debug.WriteLine(args);
        }
    }
}