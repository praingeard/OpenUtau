
using System;
using System.IO;
using System.Linq;
using System.Text;
// using Newtonsoft.Json;
using OpenUtau.Core.Ustx;
// using Melanchall.DryWetMidi.Core;
// using Melanchall.DryWetMidi.Interaction;
using OpenUtau.Core.Render;
using OpenUtau.Core;
using NAudio.Wave;
using OpenUtau.Core.Format;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using OpenUtau.Classic;
using OpenUtau.Core;
using Serilog;

class Program {
    static async Task Main(string[] args) {
        if (args.Length < 2) {
            Console.WriteLine("Usage: dotnet run <input.ustx> <output.wav>");
            return;
        }

        string inputUstxPath = args[0];
        string outputAudioPath = args[1];
        var mainThread = Thread.CurrentThread;
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("shift_jis");
            if (encoding == null) {
            Console.WriteLine("Failed to load shift_jis encoding.");
        } else {
            Console.WriteLine($"Encoding loaded: {encoding.EncodingName}");
        }
       ;

        // Schedule a task using the custom TaskScheduler
        await Task.Run(async () =>
        {
             SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var mainScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            ToolsManager.Inst.Initialize();
            SingerManager.Inst.Initialize();
            DocManager.Inst.Initialize(Thread.CurrentThread, mainScheduler);
            DocManager.Inst.PostOnUIThread = action => Avalonia.Threading.Dispatcher.UIThread.Post(action);
            InitAudio();
            OpenUtau.Core.Format.Formats.LoadProject([inputUstxPath]);
            DocManager.Inst.ExecuteCmd(new ValidateProjectNotification());
            await PlaybackManager.Inst.RenderToFiles(DocManager.Inst.Project, outputAudioPath);
            Console.WriteLine($"Audio rendered to {outputAudioPath}");

        });

        Console.WriteLine("Main thread finished");
        // Console.ReadLine(); // Keep the console open

        
        

        // Console.WriteLine($"Singer path: {PathManager.Inst.SingersPath}");
        // Console.WriteLine($"Plugin path: {PathManager.Inst.PluginsPath}");
        // // DocManager.Inst.Initialize(MainThread, MainScheduler);

        // // DocManager.Inst.PostOnUIThread = async action => {
        // //     if (Thread.CurrentThread == MainThread) {
        // //         action();
        // //     } else {
        // //         await Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, MainScheduler);
        // //     }
        // // };

        // // SingerManager.Inst.Initialize();
        // foreach (var factory in SingerManager.Inst.Singers) {
        //     Console.WriteLine($"Singer: {factory.Value.Name}, Type: {factory.Value.SingerType}");
        // }

        // // Debugging: Check phonemizer factories
        // Console.WriteLine($"PhonemizerFactories count: {DocManager.Inst.PhonemizerFactories?.Length ?? 0}");
        // foreach (var factory in DocManager.Inst.PhonemizerFactories) {
        //     Console.WriteLine($"Phonemizer: {factory.type.FullName}");
        // }

        // // Load the UProject from the .ustx file
        

        // // Debugging: Check tracks, singers, and phonemizers
        // foreach (var track in project.tracks) {
        //     Console.WriteLine($"Track: {track.TrackName}, Singer: {track.Singer?.Name ?? "None"}, Phonemizer: {track.Phonemizer?.GetType().Name ?? "None"}");
        // }

        // // Render the audio
        
    }

    static async Task RenderAudio(UProject project, string outputAudioPath) {
        Console.WriteLine($"Rendering audio to {outputAudioPath}...");
        await PlaybackManager.Inst.RenderToFiles(project, outputAudioPath);
    }

    private void Start() {
            var mainThread = Thread.CurrentThread;
            var mainScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Run(() => {
                ToolsManager.Inst.Initialize();
                SingerManager.Inst.Initialize();
                DocManager.Inst.Initialize(mainThread, mainScheduler);
                DocManager.Inst.PostOnUIThread = action => Avalonia.Threading.Dispatcher.UIThread.Post(action);
                InitAudio();
            });
        }

        private static void InitAudio() {
                    PlaybackManager.Inst.AudioOutput = new OpenUtau.Audio.NAudioOutput();
            }
}
