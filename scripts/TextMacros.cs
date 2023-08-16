using Godot;
using System;
using Yarn.GodotYarn;


public partial class TextMacros : Node
{
	public static Node2D dialogueRunner;
	[YarnCommand]
	public static void enableDash() {
		//PlayerMovement.dashEnabled = true;
		//ArenaController.spawnEnemies = true;
		if(dialogueRunner != null) {
			dialogueRunner.Hide();
		}
	}
}
