using ProjectsTM.Service;
using ProjectsTM.ViewModel;

namespace ProjectsTM.UI.Main
{
    public class UndoServiceFactory : IUndoServiceFactory
    {
        public IUndoService Create()
        {
            return new UndoService();
        }
    }
}