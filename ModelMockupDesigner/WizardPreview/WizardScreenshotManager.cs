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
            IWizardPageLayout page = await DynamicWizardManager.BuildWizardPage(model, pageIndex);

            return await TakePageSnapshot(page as FrameworkElement);
        }

        private static async Task<ImageSource> DynamicWizardEditorSnapshot(IWizardModel model, int pageIndex)
        {
            EditorSection pageSection = await DynamicWizardManager.BuildDynamicWizardEditorPage(model, pageIndex);

            return await TakePageSnapshot(pageSection);
        }

        private static Task<ImageSource> TakePageSnapshot(FrameworkElement pageElement)
        {
            ImageSource imageSource = null;

            if (pageElement != null)
            {
                pageElement.UpdateLayout();

                pageElement.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                pageElement.Arrange(new Rect(0, 0, pageElement.DesiredSize.Width, pageElement.DesiredSize.Height));

                int desiredWidth = (int)pageElement.DesiredSize.Width;
                int desiredHeight = (int)pageElement.DesiredSize.Height;

                //if (desiredWidth < 715)
                //    desiredWidth = 715;

                //if (desiredHeight < 600)
                //    desiredHeight = 600;

                Canvas canvas = new Canvas();
                canvas.Children.Add(pageElement);

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(desiredWidth, desiredHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);

                renderTargetBitmap.Render(canvas);
                renderTargetBitmap.Freeze();

                imageSource = renderTargetBitmap;
            }

            return Task.FromResult(imageSource);
        }
    }
}
