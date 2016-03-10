﻿using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Spider_web.Utils
{
    public sealed class TextToastManager
    {
        public static TextToastManager Instance { get; } = new TextToastManager();
        private TextToastManager() { }

        public void ShowToast(string line1, string line2 = null)
        {
            var template = $@"
<toast>
<visual>
<binding template=""ToastGeneric"">
  <text>{line1}</text>
  <text>{line2}</text>
</binding>
</visual>
</toast>
";
            var xml = new XmlDocument();
            xml.LoadXml(template);
            var toast = new ToastNotification(xml) {ExpirationTime = DateTimeOffset.Now.AddSeconds(5)};
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
