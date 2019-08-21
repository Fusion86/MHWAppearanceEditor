using Cirilla.Core.Models;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.ViewModels.SaveSlotEditors
{
    public class SaveSlotToolsViewModel : ViewModelBase
    {
        [Reactive] public SaveData SourceSaveData { get; private set; }
        public List<SaveSlotViewModel> SourceSaveSlots { [ObservableAsProperty]get; }

        public ReactiveCommand<SaveSlot, Unit> ImportFromSaveSlotCommand { get; }

        private SaveSlot SaveSlotContext; // aka target to where we apply the imported content

        public SaveSlotToolsViewModel(SaveSlot saveSlotContext)
        {
            SaveSlotContext = saveSlotContext;

            ImportFromSaveSlotCommand = ReactiveCommand.CreateFromTask<SaveSlot, Unit>(ImportFromSaveSlot);

            this.WhenAnyValue(x => x.SourceSaveData)
                .Where(x => x != null)
                .Select(saveData => saveData.SaveSlots.Select(x => new SaveSlotViewModel(x)).ToList())
                .ToPropertyEx(this, x => x.SourceSaveSlots);

            // Set source context to currently opened SaveData
            SourceSaveData = SaveSlotContext.SaveData;
        }

        private async Task<Unit> ImportFromSaveSlot(SaveSlot saveSlot)
        {
            return Unit.Default;
        }
    }
}
