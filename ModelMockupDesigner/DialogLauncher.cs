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
using System.Windows.Media;

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
        public DialogResult Result;
        public bool AffirmativeResponse
        {
            get
            {
                bool res = false;
                if (Result == DialogResult.Yes || Result == DialogResult.OK || Result == DialogResult.Accept)
                {
                    res = true;
                }
                return res;
            }
        }
    }
    public class DialogLauncher<T> where T : class, new()
    {
        public EventHandler<DialogEventArgs> OnClose;

        public Window Window = new Window();
        public ScrollViewer ScrollViewer = null;

        public DialogResult DialogResult;
        public bool OpenOnDifferentScreen { get; set; }
        public DialogLauncher(object owner, ResizeMode resizeMode, bool openOnDifferentScreen = false)
        {
            OpenOnDifferentScreen = openOnDifferentScreen;
            Window.SnapsToDevicePixels = true;
            Window.UseLayoutRounding = true;
            Window.ResizeMode = resizeMode;

            if (owner is Window)
            {
                Window.Owner = (Window)owner;
            }
            else if (owner is UserControl)
            {
                Window.Owner = Window.GetWindow((FrameworkElement)owner);
                if (Window.Owner == null)
                {
                    Window.Owner = Application.Current.MainWindow;
                }
            }

            Window.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(window_MouseLeftButtonDown);

            ScrollViewer = new ScrollViewer()
            {
                Name = "scrollViewerContainer",
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new T()
            };

            Window.Title = ScrollViewer.Content.GetType().ToString();
            if (openOnDifferentScreen)
                Window.WindowStartupLocation = WindowStartupLocation.Manual;
            else
                Window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Window.Content = ScrollViewer;
            Window.SizeToContent = SizeToContent.WidthAndHeight;

            if (((ScrollViewer)Window.Content).Content is IDialogClient dialogClient)
            {
                dialogClient.OnClose += new EventHandler<DialogEventArgs>(DialogClient_Close);
            }
        }

        private void DialogClient_Close(object sender, DialogEventArgs e)
        {
            if (((ScrollViewer)Window.Content).Content is IDialogClient dialogClient)
            {
                dialogClient.OnClose -= new EventHandler<DialogEventArgs>(DialogClient_Close);
            }
            DialogResult = e.Result;
            Close();


        }
        public void ShowDialog()
        {
            
            Window.ShowDialog();
        }
        public void Show()
        {
            if (OpenOnDifferentScreen && System.Windows.Forms.Screen.AllScreens.Count() > 1)
            {
                System.Windows.Forms.Screen primary = System.Windows.Forms.Screen.FromRectangle(new System.Drawing.Rectangle((int)Window.Owner.Left, (int)Window.Owner.Top, (int)Window.Owner.Width, (int)Window.Owner.Height));

                System.Windows.Forms.Screen second = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(x => x.DeviceName != primary.DeviceName);

                Window.Left = second.WorkingArea.Left + (second.WorkingArea.Width / 4);
                Window.Top = second.WorkingArea.Top + (second.WorkingArea.Height / 6);

            }

            Window.Show();
            
        }
        private void Close()
        {
            if (DialogResult != Enums.DialogResult.None)
            {
                if (Window.DialogResult == null)
                    Window.DialogResult = false;
            }

            Window.MouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(window_MouseLeftButtonDown);
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

        public T Control => ((ScrollViewer)Window.Content).Content as T;
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
