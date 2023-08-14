using Godot;
using System;
using Yarn.GodotYarn;


public partial class TextMacros : Node
{
    [YarnCommand]
	public static void enableDash() {
        PlayerMovement.dashEnabled = true;
        ArenaController.spawnEnemies = true;
    }
}
