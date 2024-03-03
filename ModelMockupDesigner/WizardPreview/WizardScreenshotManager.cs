using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModelMockupDesigner.WizardPreview
{
    public class WizardScreenshotManager
    {
        public static async Task<ImageSource> TakeWizardSnapshot(IWizardModel wizardModel, bool editor, int pageIndex) 
        {
            ImageSource screenShot;

            switch (wizardModel.WizardType)
            {
                case Enums.WizardType.Dynamic:
                    {
                        if (editor)
                        {
                            screenShot = await DynamicWizardEditorSnapshot(wizardModel, pageIndex);
                        }
                        else
                        {
                            screenShot = await DynamicWizardSnapshot(wizardModel, pageIndex);
                        }
                        break;
                    }
                case Enums.WizardType.Static:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }


            return screenShot;
        }

        private static async Task<ImageSource> DynamicWizardSnapshot(IWizardModel model, int pageIndex)
        {
            WizardDesignPreview preview = new WizardDesignPreview();
            await preview.LoadWizard(model, pageIndex);
            preview.Background = System.Windows.Media.Brushes.White;

            return await TakePageSnapshot(preview);
        }

        private static async Task<ImageSource> DynamicWizardEditorSnapshot(IWizardModel model, int pageIndex)
        {
            EditorSection pageSection = await DynamicWizardManager.BuildDynamicWizardEditorPage(model, pageIndex);

            return await TakePageSnapshot(pageSection);
        }

        private static Task<ImageSource> TakePageSnapshot(FrameworkElement element, int width = 0, int height = 0)
        {
            ImageSource imageSource = null;

            if (element != null)
            {

                element.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));

                int desiredWidth = width > 0 ? width : (int)element.DesiredSize.Width;
                int desiredHeight = height > 0 ? height : (int)element.DesiredSize.Height;

                element.UpdateLayout();

                Canvas canvas = new Canvas();
                canvas.Children.Add(element);
                canvas.Width = desiredWidth;
                canvas.Height = desiredHeight;

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(desiredWidth, desiredHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);

                renderTargetBitmap.Render(canvas);
                renderTargetBitmap.Freeze();

                imageSource = renderTargetBitmap;
            }

            return Task.FromResult(imageSource);
        }
    }
}
