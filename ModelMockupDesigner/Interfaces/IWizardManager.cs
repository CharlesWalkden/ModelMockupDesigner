using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IWizardManager
    {
        Task Reload();
        Task LoadWizard(IWizardModel wizard, int pageIndex = 0);
        double Width { get; set; }
        double Height { get; set; }
    }
}