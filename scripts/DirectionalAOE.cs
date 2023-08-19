using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DirectionalAOE : Area3D
{

	[Export]
    public float rate = 800; // rate in ms

    public float rateMult = 1;

    [Export]
    public int damage = 1; // the damage of a single shot

    public bool EnableShooting = true;

    private float timer;

    private Area3D aoeArea;
    private GpuParticles3D particles;
    public override void _Ready()
	{
        aoeArea = GetNode<Area3D>("damage");
        particles = GetNode<GpuParticles3D>("particles");
    }

    Hurtbox target;
	public override void _Process(double delta)
	{
        if(!EnableShooting) return;

        if(target != null && Node.IsInstanceValid(target) && !target.IsQueuedForDeletion()) {
            if(!OverlapsArea(target)) {
                target = null;
                return;
            }
            FaceTarget();
            particles.Emitting = true;

            timer += rateMult * (float)delta * 1000f;
            if(timer > rate) {
                timer = 0;
                DamageAoe();
            }
        } else {
            particles.Emitting = false;
            CheckForTargets();
        }
	}

    public void FaceTarget() {
        LookAt(target.GlobalPosition);
    }

    public void DamageAoe() {
        var areas = aoeArea.GetOverlappingAreas().Select(x=> x as Hurtbox).Where(x => x != null && x.targetable);
        foreach(var enemy in areas) {
            enemy.TakeDamage(damage);
        }
    }

    public void CheckForTargets() {
        var areas = GetOverlappingAreas().Select(x=> x as Hurtbox).Where(x => x != null && x.targetable);
        var t = areas.FirstOrDefault();
        if(t != null) { target = t; }
    }
}
