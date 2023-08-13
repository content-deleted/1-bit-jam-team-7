using Godot;
using System;

public partial class Base : Node2D
{
	EnemyController enemyController;

	Sprite2D weapon;
	Area2D weaponArea;
	int weaponMode = 1;

	float weaponLength = 75;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		this.Position = new Vector2(500, 250);

		weapon = (Sprite2D)GetNode("Weapon");
		weaponArea = (Area2D)weapon.GetChild(0);
		enemyController = (EnemyController)GetNode("//root/DefenseMode/EnemyController");
 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		WeaponPosition();
		Slay();

	}

	public override void _Input(InputEvent @event){
		
		if (@event is InputEventMouseButton eventMouseButton){
			if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.WheelUp){
				ChangeWeapon(1);

			} else if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.WheelDown){

				ChangeWeapon(-1);
			}
		}

	}

	public void WeaponPosition(){

		weapon.Position = (GetGlobalMousePosition() - this.GlobalPosition).Normalized() * weaponLength;
		weapon.Rotation = (GetGlobalMousePosition() - this.GlobalPosition).Angle();
		
	}

	public void ChangeWeapon(int direction){

		weaponMode += direction;

		if (weaponMode > 3){
			weaponMode = 1;

		} else if (weaponMode < 1){
			weaponMode = 3;
		}

		if (weaponMode == 1){

			weapon.Texture = (Texture2D)ResourceLoader.Load("res://gfx/triangle.png");

		} else if (weaponMode == 2){

			weapon.Texture = (Texture2D)ResourceLoader.Load("res://gfx/square.png");


		} else if (weaponMode == 3){

			weapon.Texture = (Texture2D)ResourceLoader.Load("res://gfx/hexagon.png");

		}
	}

	public void Slay(){

		if (weaponArea.GetOverlappingAreas().Count > 0){

			foreach (Area2D shape in weaponArea.GetOverlappingAreas()){

				if (shape.IsInGroup("Enemy" + enemyController.enemyTypes[weaponMode])){

					enemyController.KillEnemy((Sprite2D)shape.GetParent());

				}
			}

		}

	}



}
