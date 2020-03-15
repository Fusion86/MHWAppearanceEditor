using Avalonia.Threading;
using DynamicData;
using MHWAppearanceEditor.ViewModels;
using Serilog.Core;
using Serilog.Events;

namespace MHWAppearanceEditor
{
    public class LogSink : ILogEventSink
    {
        public SourceList<LogEventViewModel> Events { get; } = new SourceList<LogEventViewModel>();

        public void Emit(LogEvent logEvent)
        {
            var x = new LogEventViewModel(logEvent);
            Dispatcher.UIThread.Post(() => Events.Add(x));
        }
    }
}
