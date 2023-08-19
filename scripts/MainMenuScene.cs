using Godot;
using System;

public partial class MainMenuScene : Control {
	void _on_start_pressed() {
		GetTree().ChangeSceneToFile("res://scenes/LevelSelect.tscn");
    }

    void quitGame() {
        GetTree().Quit();
    }

    void gotoCredits() {
        GetTree().ChangeSceneToFile("res://scenes/Credits.tscn");
    }

    void gotoTitle() {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    }
}
