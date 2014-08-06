using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NotificationsExtensions.TileContent;

namespace TdcToast {
    public sealed partial class MainPage : Page {

        public MainPage() {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        //lembrar de habilitar no capabilities!
        private void SendToast(object sender, RoutedEventArgs e) {
            var toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(Message.Text));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(Message.Text));

            //IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            //((XmlElement)toastNode).SetAttribute("duration", "long");

            var toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(3600);

#if WINDOWS_PHONE_APP
           toast.SuppressPopup = Ghost.IsChecked == true;
           toast.Group = "tdcgroup";
           //toast.Tag = "tdctag"; //unique!
#endif
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void UpdateTile(object sender, RoutedEventArgs e) {
            var tileContent = TileContentFactory.CreateTileWide310x150Text09();
            tileContent.TextHeading.Text = "Olá TDC!";
            tileContent.TextBodyWrap.Text = MessageTile.Text;

            //Sempre crie um tile 150x150 tile tambem
            var squareContent = TileContentFactory.CreateTileSquare150x150Text04();
            squareContent.TextBodyWrap.Text = "Olá TDC!";
            tileContent.Square150x150Content = squareContent;

            var xml = new XmlDocument();
            xml.LoadXml(tileContent.GetContent());

            var futureTile = new ScheduledTileNotification(xml, DateTime.Now.AddSeconds(10));

            TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(futureTile);
            var tileNotification = new TileNotification(xml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private void DeleteToast(object sender, RoutedEventArgs e) {
#if WINDOWS_PHONE_APP
            ToastNotificationManager.History.RemoveGroup("tdcgroup");
#endif
        }
    }
}