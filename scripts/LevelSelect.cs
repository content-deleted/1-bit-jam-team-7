using Godot;
using System;

public partial class LevelSelect : Control
{
	public override void _Ready()
	{
        BuildLevelSelect();
	}
    
    void BuildLevelSelect() {
        var prefab = ResourceLoader.Load<PackedScene>("res://scenes/instances/level_option.tscn");
		Vector2 pos = new Vector2(100, 150);
        // + 200 + 800
        foreach(var level in levels) {
            var option = prefab.Instantiate() as Button;

            AddChild(option);
            
            option.Position = pos;
            pos += new Vector2(0, 200);
            if(pos.Y > 900) {
                pos = new Vector2(800, 150);
            }

            option.GetNode<Label>("title").Text = level.name;
            option.GetNode<Label>("body").Text = level.description;

            option.Pressed += () => {
                DefenseMode.levelInfo = level;
                GetTree().ChangeSceneToFile("res://scenes/DefenseMode.tscn");
            };
        }
	}

    public struct levelInfo {
		public string name;
		public string description;
        public Curve3D[] paths;
        public string yarnSpinnerNodeToPlayOnLevelStart;
	}

    public levelInfo[] levels = {
		new levelInfo { 
			name = "Tutorial",
			description = "Try this level if you haven't played before!",
            paths = new Curve3D[] {
                LoadPath("circularPath")
            },
            yarnSpinnerNodeToPlayOnLevelStart = "tutorial_level_start"
		},
        new levelInfo { 
			name = "Spikey Path",
			description = "This path begins visable on the default camera angle before deliberately going out of sight unless the player rotates the camera. Hopefully this familiarizes them with that concept.",
            paths = new Curve3D[] {
                LoadPath("spikeyPath_behindCamera")
            }
		},
        new levelInfo { 
			name = "Wiggly Labryinth",
			description = "Building on spikeyPath's introduction to the path going out of sight, it starts out of the player's view. The whole thing is also not totally visable at any one camera view. Even so, it's a pretty long, single path and so isn't too tough.",
            paths = new Curve3D[] {
                LoadPath("wigglyLabryinthPath")
            }
		},
        new levelInfo { 
			name = "Star",
			description = "I give up",
            paths = new Curve3D[] {
                LoadPath("starPath")
            }
		},
        new levelInfo { 
			name = "Hell Fucker 360",
			description = "You should not have come here",
            paths = new Curve3D[] {
                LoadPath("starPath"),
                LoadPath("circularPath")
            }
		},
        new levelInfo { 
			name = "ACAB",
			description = "ACAB ACAB ACAB ACAB ACAB",
            paths = new Curve3D[] {
                LoadPath("ACABPath"),
               
            }
		},
        new levelInfo { 
			name = "Good Luck",
			description = "I hate you, you the player, specifically",
            paths = new Curve3D[] {
                LoadPath("goodLuck")
            }
		},
	};

    private static Curve3D LoadPath(string f) => ResourceLoader.Load<Curve3D>("res://EnemyPaths/"+f+".tres");
}
