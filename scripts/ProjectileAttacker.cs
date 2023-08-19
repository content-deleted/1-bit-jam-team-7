using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProjectileAttacker : Area3D
{

	[Export]
    public float rate = 2000; // rate in ms

    public float rateMult = 1;

    [Export]
    public int damage = 1; // the damage of a single shot

    public bool EnableShooting = true;

    private float timer;
    public override void _Ready()
	{

    }
	public override void _Process(double delta)
	{
        if(!EnableShooting) return;

        timer += rateMult * (float)delta * 1000f;
        if(timer > rate) {
            timer = 0;
            CheckForTargets();
        }
	}

    public void CheckForTargets() {
        var areas = GetOverlappingAreas().Select(x=> x as Hurtbox).Where(x => x != null && x.targetable);
        if(areas.Count() != 0) {
            var target = areas.FirstOrDefault();
            if(target != null) {
                Action callback = () => {
                    target.TakeDamage(damage);
                };
            
                ProjectilePool.StartProjectile(GlobalPosition, target, 2f, callback);
            }
        }
    }
}
