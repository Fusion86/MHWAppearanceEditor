using Cirilla.Core.Models;
using MHWAppearanceEditor.Interfaces;

namespace MHWAppearanceEditor.ViewModels.SaveSlotEditors
{
    public class SaveSlotFaceViewModel : ViewModelBase
    {
        public string Title => "Face";

        private readonly SaveSlot SaveSlot;

        public SaveSlotFaceViewModel(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;
        }
    }
}
