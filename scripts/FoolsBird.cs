using Godot;
using System;

public partial class FoolsBird : Node3D
{
	public Tower tower;
	public override void _Ready()
	{
        tower = GetParent<Tower>();
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
