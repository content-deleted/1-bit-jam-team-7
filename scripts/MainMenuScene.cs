using Godot;
using System;

public partial class MainMenuScene : Control {
	void _on_start_pressed() {
		GetTree().ChangeSceneToFile("res://scenes/DefenseMode.tscn");
	}
}