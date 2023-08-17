using Godot;
using System;

public partial class Tower : Node3D
{
	public OmniLight3D light;
	public Sprite3D sprite;
	public Area3D hitbox;

	public int maxHealth;
	public int currentHealth;
	public override void _Ready()
	{
		light = GetNode("light") as OmniLight3D;
		sprite = GetNode("sprite") as Sprite3D;
		hitbox = GetNode("hitbox") as Area3D;
	}
}
