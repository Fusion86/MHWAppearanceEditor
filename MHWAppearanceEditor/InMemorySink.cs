using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Controls.Primitives;

namespace MHWAppearanceEditor
{
    // Hacky logging stuff
    public class InMemorySink : ILogEventSink
    {
        private readonly ITextFormatter _textFormatter = new MessageTemplateTextFormatter("{Timestamp} [{Level}] {Message}{Exception}", CultureInfo.InvariantCulture);

        public List<string> Events = new List<string>();

        private StatusBarItem _item;

        public InMemorySink(StatusBarItem item)
        {
            _item = item;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            var renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);

            Events.Add(renderSpace.ToString());
            _item.Dispatcher.Invoke(() => _item.Content = logEvent.MessageTemplate.Text);
        }
    }
}
