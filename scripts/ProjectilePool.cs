using Godot;
using System;
using System.Collections.Generic;


public partial class ProjectilePool : Node3D
{
    
    [Export]
    int intialPoolSize = 100;

    public static Queue<Projectile> inactiveProjectiles = new Queue<Projectile>();

    private PackedScene prefab;

	public static ProjectilePool pool;
	public override void _Ready()
	{
        if(pool != null) {
			QueueFree();
			return;
		}
		pool = this;

        prefab = ResourceLoader.Load<PackedScene>("res://scenes/instances/projectile.tscn");

        InitializePool(intialPoolSize);
    }

    public void InitializePool(int size) {
        for(int i = 0; i < size; i++) {
            var p = prefab.Instantiate() as Projectile;
            p.Hide();
            AddChild(p);
            inactiveProjectiles.Enqueue(p);
        }
    }

    public static Projectile StartProjectile(Vector3 origin, Node3D target, float speed = -1, Action callback = null) {
        if(inactiveProjectiles.Count == 0) {
            pool.InitializePool(50);
        }
        var p = inactiveProjectiles.Dequeue();

        p.origin = origin;
        p.target = target;
        p.speed = speed;//* 1 / origin.DistanceTo(target.GlobalPosition);
        p.progress = 0;
        p.callback = callback;

        p.GlobalPosition = origin;
        p.Show();

        return p;
    }
}
