using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Globalization;
using System.IO;

namespace MHWAppearanceEditor
{
    // Hacky logging stuff
    public class InMemorySink : ILogEventSink
    {
        private readonly ITextFormatter _textFormatter = new MessageTemplateTextFormatter("{Timestamp} [{Level}] {Message}{Exception}", CultureInfo.InvariantCulture);

        public string Log = "";

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            var renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);
            Log += renderSpace.ToString() + Environment.NewLine;
        }
    }
}
