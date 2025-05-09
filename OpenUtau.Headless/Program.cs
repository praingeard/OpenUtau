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

//class Program {
    
//     static void Main(string[] args) {
//         if (args.Length < 2) {
//             Console.WriteLine("Usage: dotnet run <midi.kar> <output.wav>");
//             return;
//         }

//         var midiPath = args[0];
//         var outputAudioPath = args[1];

//         // Create a new UProject
//         var project = CreateNewProject();

//         // Set singer
//         //ChangeVoice(project, "ExternalVoicebankName");

//         AddRandomNotesPart(project);

//        static void AddRandomNotesPart(UProject project) {
//     if (project.tracks.Count == 0) {
//         Console.WriteLine("No tracks available in the project.");
//         return;
//     }

//     var random = new Random();
//     var part = new UVoicePart {
//         position = 0,
//         trackNo = 0,
//         name = "New Part"
//     };

//         var note = UNote.Create();
//         note.position = 0; // Each note starts 480 ticks after the previous one
//         note.duration = 1800;     // Each note lasts 480 ticks
//         note.tone = 61; // Random MIDI pitch between C4 (60) and B4 (71)
//         note.lyric = "la";       // Default lyric

//         // Configure pitch
//         note.pitch.AddPoint(new PitchPoint(0, 0)); // Start at default pitch
//         note.pitch.AddPoint(new PitchPoint(240, 10)); // Bend up by 1 semitone at 240ms
//         note.pitch.AddPoint(new PitchPoint(480, 0)); // Return to default pitch at the end

//         // Configure vibrato
//         note.vibrato.length = 50;  // Vibrato lasts for 50% of the note's duration
//         note.vibrato.period = 180; // Vibrato period is 180ms
//         note.vibrato.depth = 20;   // Vibrato depth is 20 cents (0.2 semitones)
//         note.vibrato.@in = 10;     // Fade-in over 10% of the vibrato length
//         note.vibrato.@out = 10;    // Fade-out over 10% of the vibrato length

//         // Add the note to the part
//         part.notes.Add(note);
    

//     // Calculate the duration of the part based on the last note's end position
//     part.Duration = 1800;

//     // Add the part to the project
//     project.parts.Add(part);

//     // Validate the part
//     var track = project.tracks[part.trackNo];
//     var validateOptions = new ValidateOptions {
//         SkipTiming = false,
//         SkipPhonemizer = true,
//         SkipPhoneme = true
//     };
//     part.Validate(validateOptions, project, track);

//     Console.WriteLine("Added a part with random notes to the first track.");
// }

//         // Import MIDI and lyrics
//         //ImportMidiAndLyrics(project, midiPath);
//         foreach (var part in project.parts.OfType<UVoicePart>()) {
//             Console.WriteLine($"Part: {part.name}, Duration: {part.Duration}, Notes: {part.notes.Count}");
//             foreach (var note in part.notes) {
//                 Console.WriteLine($"Note: Position={note.position}, Duration={note.duration}, Tone={note.tone}, Error={note.Error}");
//             }
//         }
//         // Export the USTX project
//         var ustxPath = Path.ChangeExtension(outputAudioPath, ".ustx");
//         ExportUstx(project, ustxPath);
//         Console.WriteLine($"USTX project saved to {ustxPath}.");

//         static void ExportUstx(UProject project, string ustxPath) {
//             var settings = new JsonSerializerSettings {
//             Formatting = Formatting.Indented,        };
//         var json = JsonConvert.SerializeObject(project, settings);
//         File.WriteAllText("output.ustx", json);
//         }
        
//         // Render audio
//         // RenderAudio(project, outputAudioPath);

//         Console.WriteLine("Audio rendering complete.");
//     }
//     static UProject CreateNewProject() {
//         var project = new UProject {
//             name = "Test Project",
//             bpm = 115, // Default BPM
//             resolution = 480 // Default resolution
//         };

//         var track = new UTrack {
//             TrackName = "Test Track"
//         };
//         project.tracks.Add(track);

//         Console.WriteLine("Created a new UProject.");
//         return project;
//     }

//     // static void ChangeVoice(UProject project, string newSingerName) {
//     // var singer = USinger.CreateMissing(newSingerName);

//     // foreach (var track in project.tracks) {
//     //     track.Singer = singer;
//     //     if (!singer.Found || !singer.Loaded) {
//     //         Console.WriteLine($"Warning: Singer '{newSingerName}' not found or not loaded.");
//     //     }
//     // }

//     // Console.WriteLine($"Voice changed to: {newSingerName}");
//     // }

//    static void ImportMidiAndLyrics(UProject project, string midiPath) {
//     // Use the MidiWriter.Load method to parse the MIDI file and import its parts
//     var parts = OpenUtau.Core.Format.MidiWriter.Load(midiPath, project);

//     // Ensure the project has at least one track
//     if (project.tracks.Count == 0) {
//         var defaultTrack = new UTrack {
//             TrackName = "Default Track"
//         };
//         project.tracks.Add(defaultTrack);
//     }

//     // Assign each part to a track and calculate its duration
//     for (int i = 0; i < parts.Count; i++) {
//         var part = parts[i];
//         if (i >= project.tracks.Count) {
//             var newTrack = new UTrack {
//             TrackName = $"Track {i + 1}"
//             };
//             project.tracks.Add(newTrack);
//             Console.WriteLine($"Created new track: {newTrack.TrackName}");
//         }
//         part.trackNo = Math.Min(i, project.tracks.Count - 1); // Assign the part to an existing or new track
//         part.Duration = part.notes.LastOrDefault()?.End ?? 0; // Calculate the duration based on the last note

//         // Link notes with Prev and Next attributes
//         // UNote lastNote = null;
//         // foreach (var note in part.notes) {
//         //     note.Prev = lastNote; // Set the previous note
//         //     if (lastNote != null) {
//         //         lastNote.Next = note; // Link the previous note to the current note
//         //     }
//         //     lastNote = note; // Update the last note
//         // }

//         project.parts.Add(part); // Add the part to the project
//     }

//     // Validate the project to ensure all parts are properly initialized
//     // var validateOptions = new ValidateOptions {
//     //     SkipTiming = false,
//     //     SkipPhonemizer = true,
//     //     SkipPhoneme = true
//     // };

//     // foreach (var part in project.parts) {
//     // var track = project.tracks[part.trackNo];
//     // part.Validate(new validateOptions, project, track);
//     // }

//     Console.WriteLine($"Imported {parts.Sum(p => p.notes.Count)} notes from MIDI across {parts.Count} parts.");
// }

//     // static void RenderAudio(UProject project, string outputAudioPath) {
//     //     Console.WriteLine($"Rendering audio to {outputAudioPath}...");

//     //     // Initialize the rendering engine using IRender
//     //     IRenderer renderer = new IRenderer(); // Example: Replace with the actual implementation of IRender

//     //     foreach (var track in project.tracks) {
//     //         if (track.Singer == null) {
//     //             Console.WriteLine($"Track '{track.TrackName}' has no singer assigned. Skipping...");
//     //             continue;
//     //         }

//     //         // Render each part in the track
//     //         foreach (var part in project.parts.OfType<UVoicePart>()) {
//     //             Console.WriteLine($"Rendering part at position {part.position}...");

//     //             // Render the part using IRender
//     //             var renderResult = renderer.Render(project, track, part);

//     //             // Save the rendered audio to the output path
//     //             if (renderResult != null) {
//     //                 File.WriteAllBytes(outputAudioPath, renderResult);
//     //                 Console.WriteLine($"Rendered part saved to {outputAudioPath}");
//     //             } else {
//     //                 Console.WriteLine($"Failed to render part at position {part.position}");
//     //             }
//     //         }
//     //     }

//     //     Console.WriteLine("Audio rendering complete.");
//     // }
// }

class Program {
    static void Main(string[] args) {
        if (args.Length < 2) {
            Console.WriteLine("Usage: dotnet run <input.ustx> <output.wav>");
            return;
        }

        string inputUstxPath = args[0];
        string outputAudioPath = args[1];

        // Load the UProject from the .ustx file
        var project = Ustx.Load(inputUstxPath);
        if (project == null) {
            Console.WriteLine($"Failed to load project from {inputUstxPath}");
            return;
        }

        // Render the audio
        RenderAudio(project, outputAudioPath);
    }

   

    static async Task RenderAudio(UProject project, string outputAudioPath) {
    Console.WriteLine($"Rendering audio to {outputAudioPath}...");

    // Initialize the rendering engine
    var renderer = Renderers.CreateRenderer("CLASSIC");

    foreach (var track in project.tracks) {
        if (track.Singer == null) {
            Console.WriteLine($"Track '{track.TrackName}' has no singer assigned. Skipping...");
            continue;
        }

        int trackNo = project.tracks.IndexOf(track);

        foreach (var part in project.parts.OfType<UVoicePart>()) {
            Console.WriteLine($"Rendering part '{part.name}' at position {part.position}...");

            // Prepare the render phrases
            var renderPhrases = RenderPhrase.FromPart(project, track, part);
            var progress = new Progress(part.phonemes.Count);
            var cancellation = new CancellationTokenSource();

            foreach (var renderPhrase in renderPhrases) {
                try {
                    // Render the phrase
                    var renderResult = await renderer.Render(renderPhrase, progress, trackNo, cancellation, false);

                    if (renderResult.samples != null) {
                        // Use a default wave format (44.1 kHz, mono)
                        var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

                        // Save the rendered audio to the output path
                        using (var waveFileWriter = new WaveFileWriter(outputAudioPath, waveFormat)) {
                            waveFileWriter.WriteSamples(renderResult.samples, 0, renderResult.samples.Length);
                        }
                        Console.WriteLine($"Rendered part '{part.name}' saved to {outputAudioPath}");
                    } else {
                        Console.WriteLine($"Failed to render part '{part.name}' at position {part.position}");
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"Error rendering part '{part.name}': {ex.Message}");
                }
            }
        }
    }

    Console.WriteLine("Audio rendering complete.");
}
}
