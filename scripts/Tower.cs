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

    [Export]
    public CompressedTexture2D mainSprite;

    [Export]
    public CompressedTexture2D downSprite;

	public int currentHealth {
        get => _currentHealth;
        set {
            _currentHealth = value;
            // check the health and disable if its below threshold
            if(currentHealth > 0) {
                Show();
                hitbox.targetable = true;
                sprite.Texture = mainSprite;
            } else {
                hitbox.targetable = false;
                sprite.Texture = downSprite;
            }
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

        DefenseMode.onWaveEndEventHandler += () => currentHealth = maxHealth;
	}

	public float GetTotalPower () => isPowerSource ? 0 : hitbox.GetTotalPower();

	public void TakeDamage(int dmg) => currentHealth -= dmg;
}
