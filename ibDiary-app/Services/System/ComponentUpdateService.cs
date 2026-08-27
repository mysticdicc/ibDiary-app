using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.System
{
    public class ComponentUpdateService
    {
        private event EventHandler<ICalendarUpdate>? OnUpdateReceived;

        public void SubscribeToComponentUpdates(EventHandler<ICalendarUpdate> handler)
        {
            OnUpdateReceived += handler;
        }

        public void UnsubscribeFromComponentUpdates(EventHandler<ICalendarUpdate> handler)
        {
            OnUpdateReceived -= handler;
        }

        public void NotifiyComponentUpdate(ICalendarUpdate update)
        {
            OnUpdateReceived?.Invoke(this, update);
        }
    }
}
