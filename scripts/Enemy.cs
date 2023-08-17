using Godot;
using System;

public partial class Enemy : PathFollow3D
{

	public int health = 10;
	public int damage = 18;

	EnemyController enemyControllerNode;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		enemyControllerNode = (EnemyController)GetNode("/root/DefenseMode/World/Level/EnemySpawn");


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		this.ProgressRatio += (float)delta / 40;

		if (this.health <= 0)
		{
			enemyControllerNode.enemies.Remove(this);
			this.QueueFree();
			

		}

	}
}
