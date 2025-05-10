using System.Text;
using OpenUtau.Core.Ustx;
using OpenUtau.Core;
using OpenUtau.Classic;
using Serilog;
class Program {
    static async Task Main(string[] args) {
        InitLogging();
        if (args.Length < 2) {
            Log.Information("Usage: dotnet run <input.ustx> <output.wav>");
            return;
        }

        string inputUstxPath = args[0];
        string outputAudioPath = args[1];

        var synchronizationContext = new SynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        // Schedule a task using the custom TaskScheduler
        var mainScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        var mainThread = Thread.CurrentThread;
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        Encoding encoding = Encoding.GetEncoding("shift_jis");
        if (encoding == null) {
            Log.Information("Failed to load shift_jis encoding.");
        } else {
            Log.Information($"Encoding loaded: {encoding.EncodingName}");
        }

        ToolsManager.Inst.Initialize();
        SingerManager.Inst.Initialize();
        DocManager.Inst.Initialize(mainThread, mainScheduler);
        DocManager.Inst.PostOnUIThread = action => {
            synchronizationContext.Post(_ => action(), null);
        };
        OpenUtau.Core.Format.Formats.LoadProject([inputUstxPath]);
        DocManager.Inst.WaitPhonemizerFinish();
        Log.Information("Phonemizer processing completed.");

        await PlaybackManager.Inst.RenderToFiles(DocManager.Inst.Project, outputAudioPath);
        Log.Information($"Audio rendered to {outputAudioPath}");

        Log.Information("Main thread finished");
    }

    public static void InitLogging() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Debug()
            .WriteTo.Logger(lc => lc
                .MinimumLevel.Debug()
                .WriteTo.File(PathManager.Inst.LogFilePath, rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8))
            .CreateLogger();
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, args) => {
            Log.Error((Exception)args.ExceptionObject, "Unhandled exception");
        });
        Log.Information("Logging initialized.");
    }

}
