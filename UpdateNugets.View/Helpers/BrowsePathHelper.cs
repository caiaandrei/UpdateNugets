using Microsoft.WindowsAPICodePack.Dialogs;

namespace UpdateNugets.UI.Helpers
{
    public class BrowsePathHelper
    {
        public string GetFolderPath()
        {
            return GetPath(true);
        }

        public string GetFilePath()
        {
            return GetPath(false);
        }

        private string GetPath(bool isFolderPicker)
        {
            var dialog = new CommonOpenFileDialog();
            try
            {
                dialog.IsFolderPicker = isFolderPicker;
                dialog.EnsurePathExists = true;
                dialog.ShowDialog();
                return dialog.FileName;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
