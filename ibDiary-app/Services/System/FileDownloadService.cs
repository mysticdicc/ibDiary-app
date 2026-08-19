using Java.Nio.FileNio.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.System
{
    public class FileDownloadService
    {
        public static void SaveToDownloads(string content, string fileName)
        {
            var context = MainApplication.Context;
            var downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDownloads);

            if (null == downloadsDir) return;

            var path = Path.Combine(downloadsDir.Path, fileName);
            File.WriteAllText(path, content);
        }
    }
}