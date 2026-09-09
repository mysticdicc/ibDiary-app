using ibDiary_app.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.System
{
    public class FirstLaunchService
    {
        private readonly AppSettings _settings;

        public FirstLaunchService(AppSettings settings)
        {
            _settings = settings;
            if (_settings.IsFirstLaunch)
            {
                FirstTimeSetup();
            }
        }

        public void FirstTimeSetup()
        {
            _settings.TimeZoneId = TimeZoneInfo.Local.Id;
            _settings.IsFirstLaunch = false;
            _settings.Save();
        }
    }
}
