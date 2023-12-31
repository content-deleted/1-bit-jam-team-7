using Godot;
using System;
using System.Collections.Generic;

public partial class TowerLight : Node3D
{

	Camera3D mainCamera;
	Vector3 rayOrigin = new Vector3();
	Vector3 rayDirection = new Vector3();

	public float focalLength = 1f;

	public float RayLength = 1000f;

	Area3D lightArea;

	double damageTimer;

	// HACK: value of 10 is a hack, see TODO below for enemy.health adj in AOEDamage
	double damageTime = 6; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		lightArea = (Area3D)this.GetChild(1);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		ResizeArea();
		AoEDamage(delta);
	
	}

	public override void _PhysicsProcess(double delta){

	

	}

	public void OnArea3DEntered(Area3D area){

		

	}
	public void OnArea3DExited(Area3D area){

		
	}

	public void AoEDamage(double delta){

		if (damageTimer > 0){

			damageTimer -= delta;

		} else {

			damageTimer = damageTime;

			var enemies = lightArea.GetOverlappingAreas();

			foreach (Area3D enemyArea in enemies){

				if (!enemyArea.IsInGroup("Enemy")){

					continue;

				}

				Enemy enemy = (Enemy)enemyArea.GetParent();

				// TODO: Would prefer to keep damageTime to 1 and reduce this but as is with (int) the lowest this value can go is 1 so it needs to stay at 5
				enemy.health -= (int)(5 /focalLength);

			}

		}
	}


	public void ResizeArea(){

		lightArea.Scale = new Vector3(focalLength, focalLength, focalLength);

	}

}
