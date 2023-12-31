using Godot;
using System;

public partial class Base : Node3D

{

	int health = 5;
	int maxHealth = 5;

	public Sprite3D towerSprite;
	public Camera mainCamera;
	public EnemyController enemyControllerNode;
	public OmniLight3D baseLight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		towerSprite = (Sprite3D)GetNode("TowerSprite");
		mainCamera = (Camera)GetNode("//root/DefenseMode/MainCamera");
		enemyControllerNode = (EnemyController)GetNode("//root/DefenseMode/World/Level");
		baseLight = (OmniLight3D)GetNode("baselight");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		BaseLight();

	}

	public void OnArea3DEntered(Area3D area) {
		if (area.IsInGroup("Enemy")){

			var enemy = (Enemy)area.GetParent();

			health -= enemy.damage;
            GD.Print(health);
            if(health <= 0) {
                GetTree().ChangeSceneToFile("res://scenes/GameOver.tscn");
            }

			area.GetParent().QueueFree();
			enemyControllerNode.enemies.Remove((PathFollow3D)area.GetParent());
		}
	}

	public void BaseLight(){

		baseLight.LightEnergy = health / (float)maxHealth;


	}

}
