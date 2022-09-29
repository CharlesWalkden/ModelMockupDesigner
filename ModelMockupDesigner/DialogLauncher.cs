using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Management;

namespace ModelMockupDesigner
{
    public class DialogEventArgs : EventArgs
    {
        public DialogEventArgs()
        {
        }
        public DialogEventArgs(DialogResult result)
        {
            Result = result;
        }
        public DialogResult? Result;
        public bool AffirmativeResponse
        {
            get
            {
                bool res = false;
                if (Result.HasValue && (Result == DialogResult.Yes || Result == DialogResult.OK || Result == DialogResult.Accept))
                {
                    res = true;
                }
                return res;
            }
        }
    }
    public class DialogLauncher<T> where T : class, new()
    {
        public EventHandler<DialogEventArgs>? OnClose;

        public Window Window = new Window();
        public ScrollViewer? ScrollViewer = null;

        public DialogResult? DialogResult;
        private bool DisplayOnSameScreen;
        public DialogLauncher(object owner, bool sameScreen = true)
        {
            DisplayOnSameScreen = sameScreen;
            Window.SnapsToDevicePixels = true;
            Window.UseLayoutRounding = true;

            if (owner is Window)
            {
                Window.Owner = (Window)owner;
            }
            else if (owner is UserControl)
            {
                Window.Owner = Window.GetWindow((FrameworkElement)owner);
            }

            Window.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(window_MouseLeftButtonDown);

            ScrollViewer = new()
            {
                Name = "scrollViewerContainer",
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new T()
            };

            Window.Title = ScrollViewer.Content.GetType().ToString();
            // Window Loaded
            // Window Style
            Window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Window.Content = ScrollViewer;
            Window.SizeToContent = SizeToContent.WidthAndHeight;

            if (((ScrollViewer)Window.Content).Content is IDialogClient dialogClient)
            {
                dialogClient.OnClose += new EventHandler<DialogEventArgs>(DialogClient_Close);
            }
        }
        private void DialogClient_Close(object? sender, DialogEventArgs e)
        {
            DialogResult = e.Result;
            Close();

        }
        public void ShowDialog()
        {
            
            Window.ShowDialog();
        }
        public void Show()
        {
            
            Window.Show();
            
        }
        private void Close()
        {
            if (DialogResult != Enums.DialogResult.None)
            {
                if (Window.DialogResult == null)
                    Window.DialogResult = false;
            }

            try
            {
                Window.Close();
            }
            catch(Exception e)
            {

            }

            OnClose?.Invoke(this, new DialogEventArgs() { Result = DialogResult });
        }

        //private int MonitorCount()
        //{
        //    ManagementObjectCollection collection = GetManagementCollection("root\\WMI", "Select * from WMIMonitorID");

        //    return collection.Count;
        //}
        //private ManagementObjectCollection GetManagementCollection(string scope,string query)
        //{
        //    ManagementObjectSearcher s = new ManagementObjectSearcher(query);

        //    return s.Get();
        //}
        //private void SetWindowLocation()
        //{
        //    ManagementObjectCollection monitors = GetManagementCollection("root\\WMI", "Select * from WmiMonitorBasicDisplayParams ");

        //    foreach (ManagementObject monitor in monitors)
        //    {

        //    }
        //}

#pragma warning disable CS8603 // We will always have content here. If we don't something has gone wrong and needs fixing.
        public T Control => ((ScrollViewer)Window.Content).Content as T;
#pragma warning restore CS8603 // Possible null reference return.
        private void window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == System.Windows.Input.MouseButton.Left && e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                    Window.DragMove();
            }
            catch { }
        }
    }
}
