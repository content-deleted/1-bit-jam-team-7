using Godot;
using System;

public partial class Enemy : PathFollow3D
{
	private int _health = 10;
	public int health {
		get => _health;
		set {
			HandleHealth(value, _health);
			_health = value;
		}
	}

	public int damage = 18;

	[Export]
	public int gold = 20;
	[Export]
	public int score = 20;

	AnimatedSprite3D spriteAnimation;
	EnemyController enemyControllerNode;

	public override void _Ready()
	{
		enemyControllerNode = (EnemyController)GetNode("/root/DefenseMode/World/Level/EnemySpawn");
		spriteAnimation = (AnimatedSprite3D)GetChild(0);
		spriteAnimation.Play("walk");

	}

	public override void _Process(double delta)
	{
		this.ProgressRatio += (float)delta / 40;
		
	}

    public void TakeDamage(int dmg) => health -= dmg;

	private void HandleHealth(int cur, int prev) {
		if (cur <= 0)
		{
			enemyControllerNode.enemies.Remove(this);
			this.QueueFree();

			// give drops
			ShopController.gold += gold;
			DefenseMode.score += score;
		}
	}
}
