namespace MHWAppearanceEditor.ViewModels
{
    public class CharacterAssetViewModel : ViewModelBase
    {
        public string Name { get; }
        public string? PreviewSource { get; }
        public int Value { get; }

        public CharacterAssetViewModel(string name, string? previewSource, int value)
        {
            Name = name;
            PreviewSource = previewSource;
            Value = value;
        }
    }
}
