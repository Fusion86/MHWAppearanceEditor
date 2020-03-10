using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MHWAppearanceEditor.ViewModels;
using ReactiveUI;
using Splat;
using System;

namespace MHWAppearanceEditor
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            Type type = typeof(IViewFor<>).MakeGenericType(new Type[] { data.GetType() });
            var control = Locator.Current.GetService(type);

            if (control != null) return (IControl)control;
            return new TextBlock { Text = "Not Found: " + data.GetType().Name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
