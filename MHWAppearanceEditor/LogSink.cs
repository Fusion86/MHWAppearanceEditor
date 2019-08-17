using Avalonia.Threading;
using DynamicData;
using Serilog.Core;
using Serilog.Events;

namespace MHWAppearanceEditor
{
    public class LogSink : ILogEventSink
    {
        public static LogSink AppLogger = new LogSink();

        public SourceList<LogEvent> Events { get; } = new SourceList<LogEvent>();

        public void Emit(LogEvent logEvent)
        {
            Dispatcher.UIThread.InvokeAsync(() => Events.Add(logEvent));
        }
    }
}
