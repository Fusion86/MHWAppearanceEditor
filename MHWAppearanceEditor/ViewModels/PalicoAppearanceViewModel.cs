using Cirilla.Core.Models;
using System.ComponentModel;
using System.Drawing;

namespace MHWAppearanceEditor.ViewModels
{
    public class PalicoAppearanceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SaveSlot _saveSlot;

        public PalicoAppearanceViewModel(SaveSlot saveSlot)
        {
            _saveSlot = saveSlot;
        }

        public Color PatternColor1
        {
            get => _saveSlot.PalicoAppearance.PatternColor1;
            set => _saveSlot.PalicoAppearance.PatternColor1 = value;
        }

        public Color PatternColor2
        {
            get => _saveSlot.PalicoAppearance.PatternColor2;
            set => _saveSlot.PalicoAppearance.PatternColor2 = value;
        }

        public Color PatternColor3
        {
            get => _saveSlot.PalicoAppearance.PatternColor3;
            set => _saveSlot.PalicoAppearance.PatternColor3 = value;
        }

        public Color FurColor
        {
            get => _saveSlot.PalicoAppearance.FurColor;
            set => _saveSlot.PalicoAppearance.FurColor = value;
        }

        public Color LeftEyeColor
        {
            get => _saveSlot.PalicoAppearance.LeftEyeColor;
            set => _saveSlot.PalicoAppearance.LeftEyeColor = value;
        }

        public Color RightEyeColor
        {
            get => _saveSlot.PalicoAppearance.RightEyeColor;
            set => _saveSlot.PalicoAppearance.RightEyeColor = value;
        }

        public Color ClothingColor
        {
            get => _saveSlot.PalicoAppearance.ClothingColor;
            set => _saveSlot.PalicoAppearance.ClothingColor = value;
        }

        public float FurLength
        {
            get => _saveSlot.PalicoAppearance.FurLength;
            set => _saveSlot.PalicoAppearance.FurLength = value;
        }

        public float FurThickness
        {
            get => _saveSlot.PalicoAppearance.FurThickness;
            set => _saveSlot.PalicoAppearance.FurThickness = value;
        }

        public byte PatternType
        {
            get => _saveSlot.PalicoAppearance.PatternType;
            set => _saveSlot.PalicoAppearance.PatternType = value;
        }

        public byte EyeType
        {
            get => _saveSlot.PalicoAppearance.EyeType;
            set => _saveSlot.PalicoAppearance.EyeType = value;
        }

        public byte EarType
        {
            get => _saveSlot.PalicoAppearance.EarType;
            set => _saveSlot.PalicoAppearance.EarType = value;
        }

        public byte TailType
        {
            get => _saveSlot.PalicoAppearance.TailType;
            set => _saveSlot.PalicoAppearance.TailType = value;
        }

        public byte VoiceType
        {
            get => _saveSlot.PalicoAppearance.VoiceType;
            set => _saveSlot.PalicoAppearance.VoiceType = value;
        }

        public byte VoicePitch
        {
            get => _saveSlot.PalicoAppearance.VoicePitch;
            set => _saveSlot.PalicoAppearance.VoicePitch = value;
        }
    }
}
