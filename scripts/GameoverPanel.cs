using Godot;
using System;

public partial class GameoverPanel : Panel
{
	public override void _Ready()
	{
        GetNode<Label>("restartlabel").Hide();
	}

	
    int canRestartTimer = 0;
	public override void _Process(double delta)
	{
        if(this.Visible) {
            if(canRestartTimer == 0) {
                GetNode<Label>("scorelabel").Text = "Score: " + ArenaController.controller.score;
            }
            canRestartTimer++;
            if(canRestartTimer == 100){
                GetNode<Label>("restartlabel").Show();
            }
            if(canRestartTimer >= 100) {
                if(Input.IsActionJustPressed("ui_select")) {
                    canRestartTimer = 0;
                    Cage.resetCages();
                    ArenaController.Restart();
                    GetNode<Label>("restartlabel").Hide();
                    Hide();
                }
            }
        }
	}
}
