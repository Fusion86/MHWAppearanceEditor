using Cirilla.Core.Models;
using Splat;
using System;

namespace MHWAppearanceEditorNext2.ViewModels
{
    public class SaveSlotViewModel : ViewModelBase, IEnableLogger
    {
        private SaveSlot SaveSlot { get; }

        public SaveSlotViewModel(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;
        }

        public string HunterName
        {
            get => SaveSlot.HunterName;
            set
            {
                try
                {
                    // The setter will throw an exception if the name is too large (large as in: number of bytes) or invalid (when not UTF-8)
                    SaveSlot.HunterName = value;
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex);
                }
            }
        }

        public string PlayTime => "todo";
    }
}
