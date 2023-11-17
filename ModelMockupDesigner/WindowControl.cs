using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModelMockupDesigner
{
    public class WindowControl
    {
        public static event EventHandler<WindowParameters> OnWindowDisplay;
        public static Grid MainWindow
        {
            get => mainWindow;
            set => mainWindow = value;
        }
        private static int WindowIndex = 0;

        public static void DisplayWindow(UserControl control)
        {
            if (MainWindow != null && MainWindow.Children.Count > 0)
            {
                MainWindow.Children[0].Visibility = System.Windows.Visibility.Collapsed;
                MainWindow.Children.Clear();
            }
            windowStack.Add(control);

            DisplayTopWindow();
        }

        private static void DisplayTopWindow()
        {
            if (windowStack.Count > 0 && MainWindow != null)
            {
                UserControl control = windowStack[windowStack.Count-1];
                MainWindow.Children.Add(control);
                MainWindow.Children[0].Visibility = System.Windows.Visibility.Visible;

                OnWindowDisplay?.Invoke(null, ((IWindowStack)control).GetWindowParameters());
            }
        }

        public static void CloseTopWindow()
        {
            if (windowStack.Count > 0)
            {
                WindowIndex = windowStack.Count - 1;
                if (windowStack[WindowIndex] is IWindowStack)
                {
                    ((IWindowStack)windowStack[WindowIndex]).OnClosed += CloseTopWindow_OnClosed;
                    ((IWindowStack)windowStack[WindowIndex]).CloseAsync();
                }
            }
        }
        public static IWindowStack GetTopWindow()
        {
            if (windowStack.Count > 0)
            {
                WindowIndex = windowStack.Count - 1;
                if (windowStack[WindowIndex] is IWindowStack)
                {
                    return ((IWindowStack)windowStack[WindowIndex]);
                }
            }

            return null;
        }
        private static void CloseTopWindow_OnClosed(object sender, EventArgs e)
        {
            if (WindowIndex < windowStack.Count && MainWindow != null)
            {
                MainWindow.Children.Clear();
                windowStack.RemoveAt(WindowIndex);
                DisplayTopWindow();
            }
        }

        #region Private Properties

        private static Grid mainWindow;
        private static List<UserControl> windowStack = new List<UserControl>();

        #endregion

    }
}
