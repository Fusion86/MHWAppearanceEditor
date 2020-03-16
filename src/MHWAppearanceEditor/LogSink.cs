using Avalonia.Threading;
using MHWAppearanceEditor.ViewModels;
using Serilog.Core;
using Serilog.Events;
using System.Collections.ObjectModel;

namespace MHWAppearanceEditor
{
    public class LogSink : ILogEventSink
    {
        public ObservableCollection<LogEventViewModel> Events { get; } = new ObservableCollection<LogEventViewModel>();

        public void Emit(LogEvent logEvent)
        {
            var x = new LogEventViewModel(logEvent);
            Dispatcher.UIThread.Post(() => Events.Add(x));
        }
    }
}
