using Godot;
using System;
using System.Collections.Generic;

public partial class Cage : Area2D
{
	public const int maxCages = 3;
	public static List<Cage> cages = new List<Cage>(maxCages);
    public static List<Cage> destroyedCages = new List<Cage>(maxCages);

    const string gameoverNode = "/root/Base/Camera2D/GameoverPanel";

	public override void _Ready()
	{
		cages.Add(this);

        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}

	public static Cage GetRandomCage () => cages[(int)(GD.Randi() % cages.Count)];

	[Signal]
	public delegate void HitEventHandler();

	private void _on_body_entered(Node body)
	{
		Hide();
		EmitSignal(SignalName.Hit);

		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		
		cages.Remove(this);
        destroyedCages.Add(this);
		//QueueFree();
        // hide instead of free
        Hide();
		if(cages.Count <= 0) {
			GetNode<Panel>(gameoverNode).Show();
		}
	}

    public static void resetCages () { 
        destroyedCages.ForEach(cage => {
            cage.Show();
            cages.Add(cage);
            cage.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        });
        destroyedCages.Clear();
    }
}
