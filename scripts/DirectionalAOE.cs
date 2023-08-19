using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DirectionalAOE : Area3D
{
	[Export]
    public float rate = 800; // rate in ms

    [Export]
    public float powerScaling = 0.5f;

    public float rateMult = 1;

    [Export]
    public int damage = 1; // the damage of a single shot

    public bool EnableShooting = true;

    private float timer;

    private Area3D aoeArea;
    private GpuParticles3D particles;
    private Vector3 sizeArea;
    private Vector3 sizeParticles;

    private Tower tower;
    public override void _Ready()
	{
        aoeArea = GetNode<Area3D>("damage");
        sizeArea = aoeArea.Scale * powerScaling;
        particles = GetNode<GpuParticles3D>("particles");
        sizeParticles = particles.Scale * powerScaling;
        tower = GetParent() as Tower;
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
            UpdateAreaFromPower();
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

    public void UpdateAreaFromPower() {
        var power = tower.GetTotalPower();
        power = power > 0 ? power : 0.0001f;
        particles.Scale = sizeParticles * power;
        aoeArea.Scale = sizeArea * power;
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
