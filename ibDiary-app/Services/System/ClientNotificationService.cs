using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.System
{
    public class ClientNotificationService
    {
        public event Action<(string Title, string Message)>? Notified;
        public void Register(Action<(string Title, string Message)> handler) => Notified += handler;
        public void UnRegister(Action<(string Title, string Message)> handler) => Notified -= handler;
        public void ShowNotification(string title, string message) => Notified?.Invoke((title, message));
    }
}
