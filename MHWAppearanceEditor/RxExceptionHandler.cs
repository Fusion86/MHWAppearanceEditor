using ReactiveUI;
using Serilog;
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace MHWAppearanceEditor
{
    public class RxExceptionHandler : IObserver<Exception>
    {
        private static readonly ILogger log = Log.ForContext<RxExceptionHandler>();

        public void OnNext(Exception ex)
        {
            if (Debugger.IsAttached) Debugger.Break();
            log.Error(ex.Message);
            RxApp.MainThreadScheduler.Schedule(() => { throw ex; });
        }

        public void OnError(Exception ex)
        {
            if (Debugger.IsAttached) Debugger.Break();
            log.Error(ex.Message);
            RxApp.MainThreadScheduler.Schedule(() => { throw ex; });
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            log.Error("Unknown error??");
            RxApp.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
        }
    }
}
