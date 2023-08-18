using Godot;
using System;

public partial class TowerMatapacos : Node3D
{

    public Tower tower;
    public ProjectileAttacker shooter;
	public override void _Ready()
	{
        tower = GetParent<Tower>();
        shooter = tower.GetNode<ProjectileAttacker>("range");
	}
	public override void _Process(double delta)
	{
        shooter.EnableShooting = tower.isOnline;
        if(!tower.isOnline) {
            tower.Hide();
        } else {
            // may need to make this faster
            shooter.rateMult = tower.GetTotalPower();
        }
        // if(tower.isOnline && tower.light.LightColor.A != 1f) {
        //     tower.light.LightColor = new Color(tower.light.LightColor, 1);
        // } else if(!tower.isOnline && tower.light.LightColor.A != 0f) {
        //     tower.light.LightColor = new Color(tower.light.LightColor, 0);
        // }
	}
}
