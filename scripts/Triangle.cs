using Godot;
using System;

public partial class Triangle : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public int baseSpeed = 150;
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();

		var target = Cage.GetRandomCage();

		var dir = target.GlobalPosition - GlobalPosition;

		LinearVelocity = dir.Normalized() * (float)(baseSpeed + GD.RandRange(0.0, 50.0));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
