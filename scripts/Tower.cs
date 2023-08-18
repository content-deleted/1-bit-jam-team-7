using Godot;
using System;
using System.Diagnostics;


public partial class Tower : Node3D
{
	public OmniLight3D light;
	public Sprite3D sprite;
	public Hurtbox hitbox;

	public int maxHealth;
	private int _currentHealth;

	[Export]
	public bool isPowerSource = false;

	public int currentHealth {
		get => _currentHealth;
		set { 
			_currentHealth = value;
			// check the health and disable if its below threshold
		}
	}

	// This will also take into account the energy
	public bool isOnline => currentHealth > 0;

	public ShopItem.towerInfo info;
	public override void _Ready()
	{
		light = GetNode("light") as OmniLight3D;
		sprite = GetNode("sprite") as Sprite3D;
		hitbox = GetNode("hitbox") as Hurtbox;
	}

	public float GetTotalPower () => isPowerSource ? 0 : hitbox.GetTotalPower();

	public void TakeDamage(int dmg) => currentHealth -= dmg;
}
