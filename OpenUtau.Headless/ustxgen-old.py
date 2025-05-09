import yaml
from collections import OrderedDict
from mido import MidiFile

# Ensure YAML uses block style instead of flow style
def ordered_dump(data, stream=None, Dumper=yaml.SafeDumper, **kwargs):
    class OrderedDumper(Dumper):
        pass

    def _dict_representer(dumper, data):
        return dumper.represent_mapping(
            yaml.resolver.BaseResolver.DEFAULT_MAPPING_TAG,
            data.items()
        )

    OrderedDumper.add_representer(OrderedDict, _dict_representer)
    return yaml.dump(data, stream, Dumper=OrderedDumper, default_flow_style=False, **kwargs)

# Function to parse MIDI file and extract notes
def parse_midi(file_path):
    midi = MidiFile(file_path)
    tracks = []
    for i, track in enumerate(midi.tracks):
        notes = []
        current_time = 0
        for msg in track:
            current_time += msg.time
            if msg.type == 'note_on' and msg.velocity > 0:
                notes.append({
                    "position": current_time,
                    "duration": 480,  # Default duration, adjust as needed
                    "tone": msg.note,
                    "lyric": "a",  # Default lyric, adjust as needed
                    "pitch": {
                        "data": [{"x": -40, "y": 0, "shape": "io"}, {"x": 40, "y": 0, "shape": "io"}],
                        "snap_first": True
                    },
                    "vibrato": {"length": 0, "period": 175, "depth": 25, "in": 10, "out": 10, "shift": 0, "drift": 0, "vol_link": 0},
                    "phoneme_expressions": [],
                    "phoneme_overrides": []
                })
        tracks.append({"track_no": i, "notes": notes})
    return tracks

# Data definition based on your specification
ustx_data = OrderedDict({
    "name": "Test Project",
    "comment": "",
    "output_dir": "Vocal",
    "cache_dir": "UCache",
    "ustx_version": "0.6",
    "resolution": 480,
    "bpm": 115,
    "beat_per_bar": 4,
    "beat_unit": 4,
    "expressions": OrderedDict({
        # (Expressions remain unchanged)
    }),
    "exp_selectors": ["dyn", "pitd", "clr", "eng", "vel"],
    "exp_primary": 0,
    "exp_secondary": 1,
    "key": 0,
    "time_signatures": [{"bar_position": 0, "beat_per_bar": 4, "beat_unit": 4}],
    "tempos": [{"position": 0, "bpm": 115}],
    "tracks": [],
    "voice_parts": [],
    "wave_parts": []
})

# Parse MIDI file and add tracks
midi_file_path = "example.mid"  # Replace with your MIDI file path
parsed_tracks = parse_midi(midi_file_path)

for parsed_track in parsed_tracks:
    track_no = parsed_track["track_no"]
    notes = parsed_track["notes"]
    ustx_data["tracks"].append({
        "phonemizer": "OpenUtau.Core.DefaultPhonemizer",
        "renderer_settings": {},
        "track_name": f"Track {track_no + 1}",
        "track_color": "Blue",
        "mute": False,
        "solo": False,
        "volume": 0,
        "pan": 0,
        "track_expressions": [],
        "voice_color_names": [""]
    })
    ustx_data["voice_parts"].append({
        "duration": sum(note["duration"] for note in notes),
        "name": f"Part {track_no + 1}",
        "comment": "",
        "track_no": track_no,
        "position": 0,
        "notes": notes,
        "curves": []
    })

# Write to file
with open("TestProject.ustx", "w", encoding="utf-8") as f:
    ordered_dump(ustx_data, f)

print("USTX file 'TestProject.ustx' created.")
