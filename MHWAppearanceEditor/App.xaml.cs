using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml;

namespace MHWAppearanceEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("MHWAppearanceEditor.SyntaxHighlighting.JSON.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    HighlightingManager.Instance.RegisterHighlighting("JSON", new string[0], HighlightingLoader.Load(reader, HighlightingManager.Instance));
                }
            }
        }
    }
}
