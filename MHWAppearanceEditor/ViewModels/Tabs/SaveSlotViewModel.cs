using Cirilla.Core.Models;
using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditor.Interfaces;
using MHWAppearanceEditor.ViewModels.SaveSlotEditors;
using System;

namespace MHWAppearanceEditor.ViewModels.Tabs
{
    public class SaveSlotViewModel : ViewModelBase, ITabItemViewModel
    {
        public string Title => HunterName;
        public string ToolTipText => $"{HunterName} (Rank: {HunterRank})";

        // Only used in TabItem header, so no need to be mutable
        public string HunterName => SaveSlot.HunterName;
        public int HunterRank => SaveSlot.HunterRank;

        private readonly SourceList<object> tabs = new SourceList<object>();
        public IObservableCollection<object> TabsBinding { get; } = new ObservableCollectionExtended<object>();

        private readonly SaveSlot SaveSlot;

        public SaveSlotViewModel(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;

            tabs.Connect()
                .Bind(TabsBinding)
                .Subscribe();

            tabs.AddRange(new ViewModelBase[] {
                new SaveSlotInfoViewModel(SaveSlot),
                new SaveSlotFaceViewModel(SaveSlot)
            });
        }
    }
}
