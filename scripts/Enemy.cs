using Godot;
using System;

public partial class Enemy : PathFollow3D
{
	private int _health = 3;
	public int health {
		get => _health;
		set {
			HandleHealth(value, _health);
			_health = value;
		}
	}

	public int damage = 1;

	[Export]
	public int speed = 40;
	[Export]
	public int gold = 20;
	[Export]
	public int score = 20;

	AnimatedSprite3D spriteAnimation;
	EnemyController enemyControllerNode;
    Hurtbox hurtbox;

	public override void _Ready()
	{
		enemyControllerNode = (EnemyController)GetNode("/root/DefenseMode/World/Level");
		spriteAnimation = (AnimatedSprite3D)GetChild(0);
		spriteAnimation.Play("walk");

        hurtbox = GetNode("hitbox") as Hurtbox;
	}

	public override void _Process(double delta)
	{
		this.Progress += (float)delta * speed;
		
	}

	public void TakeDamage(int dmg) => health -= dmg;

	private void HandleHealth(int cur, int prev) {
		if (cur <= 0)
		{
			enemyControllerNode.enemies.Remove(this);
            hurtbox.targetable = false;
			this.QueueFree();

			// give drops
			ShopController.gold += gold;
			DefenseMode.score += score;
		}
	}
}
