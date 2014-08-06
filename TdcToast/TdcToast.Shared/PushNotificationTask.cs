using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;

namespace TdcToast {
    public class PushNotificationTask : IBackgroundTask {
        private const string TaskName = "pushtask";

        public void Run(IBackgroundTaskInstance taskInstance) {
            var notification = (RawNotification) taskInstance.TriggerDetails;
            string content = notification.Content;

            var toastDescriptor = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var txtNodes = toastDescriptor.GetElementsByTagName("text");

            txtNodes[0].AppendChild(toastDescriptor.CreateTextNode("Raw Notification"));
            txtNodes[1].AppendChild(toastDescriptor.CreateTextNode(content));

            var toast = new ToastNotification(toastDescriptor);
            var toatNotifier = ToastNotificationManager.CreateToastNotifier();
            toatNotifier.Show(toast);
        }

        public static async void RegisterAsync() {
            if (IsTaskRegistered()) {
                return;
            }
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            var builder = new BackgroundTaskBuilder();
            builder.Name = TaskName;
            builder.TaskEntryPoint = typeof (PushNotificationTask).FullName;
            builder.SetTrigger(new PushNotificationTrigger());
            var task = builder.Register();
        }

        public static bool IsTaskRegistered() {
            var taskRegistered = false;
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);

            if (entry.Value != null) {
                taskRegistered = true;
            }
            return taskRegistered;
        }

        public static void DisableTask() {
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);
            if (entry.Value != null) {
                entry.Value.Unregister(true);
            }
        }
    }
}