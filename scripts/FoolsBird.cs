using Godot;
using System;

public partial class FoolsBird : Node3D
{
	public Tower tower;
    bool isActive = true;
	public override void _Ready()
	{
        tower = GetParent<Tower>();
		// tower.GetNode<OmniLight3D>("light").OmniRange = 1.5f * tower.range; FUCK YOU FUTURE CORY

        var light = tower.GetNode<OmniLight3D>("light");
        var range = tower.GetNode<Area3D>("range");
        tower.OnHealthChangedEventHandler += (int cur, int prev) => {
            if(!isActive && cur > 0) {
                light.Show();
                range.Monitorable = true;
            } else if(isActive && cur <= 0) {
                light.Hide();
                range.Monitorable = false;
                isActive = false;
            }
        };
	}

	public override void _Process(double delta)
	{
        // if(tower.isOnline && tower.light.LightColor.A != 1f) {
        //     tower.light.LightColor = new Color(tower.light.LightColor, 1);
        // } else if(!tower.isOnline && tower.light.LightColor.A != 0f) {
        //     tower.light.LightColor = new Color(tower.light.LightColor, 0);
        // }
	}
}
