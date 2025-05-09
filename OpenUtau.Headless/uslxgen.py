import json
from mido import MidiFile

def midi_to_notes(midi_path):
    midi = MidiFile(midi_path)
    notes = []
    time = 0

    for track in midi.tracks:
        for msg in track:
            time += msg.time
            if msg.type == 'note_on' and msg.velocity > 0:
                notes.append({
                    "position": time,
                    "duration": 480,  # Default duration, adjust as needed
                    "tone": msg.note,
                    "lyric": "a",  # Default lyric, adjust as needed
                    "pitch": {
                        "data": [],
                        "snap_first": True
                    },
                    "vibrato": {
                        "length": 0,
                        "period": 175,
                        "depth": 25,
                        "in": 10,
                        "out": 10,
                        "shift": 0,
                        "drift": 0,
                        "vol_link": 0
                    },
                    "phoneme_expressions": [],
                    "phoneme_overrides": []
                })
    return notes

def create_ustx_file_from_midi(midi_path, output_path):
    notes = midi_to_notes(midi_path)
    ustx_data = {
        "name": "Test Project",
        "comment": "",
        "output_dir": "Vocal",
        "cache_dir": "UCache",
        "ustx_version": "0.6",
        "resolution": 480,
        "bpm": 115,
        "beat_per_bar": 4,
        "beat_unit": 4,
        "expressions": {
            "dyn": {
                "name": "dynamics (curve)",
                "abbr": "dyn",
                "type": "Curve",
                "min": -240,
                "max": 120,
                "default_value": 0,
                "is_flag": False,
                "flag": ""
            },
            "pitd": {
                "name": "pitch deviation (curve)",
                "abbr": "pitd",
                "type": "Curve",
                "min": -1200,
                "max": 1200,
                "default_value": 0,
                "is_flag": False,
                "flag": ""
            },
            "clr": {
                "name": "voice color",
                "abbr": "clr",
                "type": "Options",
                "min": 0,
                "max": -1,
                "default_value": 0,
                "is_flag": False,
                "options": []
            },
            "eng": {
                "name": "resampler engine",
                "abbr": "eng",
                "type": "Options",
                "min": 0,
                "max": 1,
                "default_value": 0,
                "is_flag": False,
                "options": ["", "worldline"]
            },
            "vel": {
                "name": "velocity",
                "abbr": "vel",
                "type": "Numerical",
                "min": 0,
                "max": 200,
                "default_value": 100,
                "is_flag": False,
                "flag": ""
            }
        },
        "exp_selectors": ["dyn", "pitd", "clr", "eng", "vel"],
        "exp_primary": 0,
        "exp_secondary": 1,
        "key": 0,
        "time_signatures": [
            {
                "bar_position": 0,
                "beat_per_bar": 4,
                "beat_unit": 4
            }
        ],
        "tempos": [
            {
                "position": 0,
                "bpm": 115
            }
        ],
        "tracks": [
            {
                "phonemizer": "OpenUtau.Core.DefaultPhonemizer",
                "renderer_settings": {},
                "track_name": "New Track",
                "track_color": "Blue",
                "mute": False,
                "solo": False,
                "volume": 0,
                "pan": 0,
                "track_expressions": [],
                "voice_color_names": [""]
            }
        ],
        "voice_parts": [
            {
                "duration": 7680,
                "name": "New Part",
                "comment": "",
                "track_no": 0,
                "position": 0,
                "notes": notes,
                "curves": [],
                "wave_parts": []
            }
        ]
    }

    # Write the JSON data to a file
    with open(output_path, "w", encoding="utf-8") as f:
        json.dump(ustx_data, f, indent=2)

# Example usage
create_ustx_file_from_midi("example.mid", "test.ustx")
