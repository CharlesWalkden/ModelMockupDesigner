using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IWizardManager
    {
        Task Reload();
        Task LoadWizard(IWizardModel wizard);
    }
}