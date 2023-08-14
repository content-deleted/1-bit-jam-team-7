using Godot;
using System;

public partial class Base : Node2D
{
	EnemyController enemyController;

	Sprite2D light;
	Area2D lightArea;
	//int weaponMode = 1;

	float lightLength = 150;
	float lightscale;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		this.Position = new Vector2(500, 250);

		light = (Sprite2D)GetNode("Light");
		lightArea = (Area2D)light.GetChild(0);
		enemyController = (EnemyController)GetNode("//root/DefenseMode/EnemyController");
 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		LightPosition();
		//Slay();

	}

	public override void _Input(InputEvent @event){
		
		if (@event is InputEventMouseButton eventMouseButton){
			if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.WheelUp){
				//ChangeLight(1);

			} else if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.WheelDown){

				//ChangeLight(-1);
			}
		}

	}

	public void LightPosition(){

		lightscale = (light.GlobalPosition.Y / GetViewportRect().Size[1]) * 2;
		lightLength = 100 * lightscale;
		light.Scale = new Vector2(lightscale, lightscale);
		

		light.GlobalPosition = GetGlobalMousePosition();
		//light.Rotation = (GetGlobalMousePosition() - this.GlobalPosition).Angle();
		
		
		
	}


	public void Slay(){

		
	}



}
