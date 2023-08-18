using Godot;
using System;

public partial class Camera : Camera3D
{

	public int rotateCount = 60;

    public int rotateStep;

    public string rotateDirection;

	string panningDirection;

	float cameraHeight = 5f;

	float cameraDistance = 8f;

	public int cameraOrientation = 0;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta){

		Look();

	}

	public void SetLook(string panDirection){

		if(rotateStep == 0 && panDirection == "left"){

			rotateStep = rotateCount;
			panningDirection = "left";

			cameraOrientation += 1;

			if (cameraOrientation > 3){

				cameraOrientation = 0;

			}


		} else if (rotateStep == 0 && panDirection == "right"){

			rotateStep = rotateCount;
			panningDirection = "right";

			cameraOrientation -= 1;

			if (cameraOrientation < 0){

				cameraOrientation = 3;

			}

		}


	}

	public void Look(){

		if (rotateStep > 0){

			float angle = (Mathf.Pi / 2) / rotateCount;

			if (panningDirection == "left"){
			
				this.Position = this.Position.Rotated(new Vector3(0, 1, 0), angle);

			} else if (panningDirection == "right"){

				this.Position = this.Position.Rotated(new Vector3(0, 1, 0), -angle);

			}

			rotateStep -= 1;

		}

		this.LookAt(new Vector3(0, 1, 0), new Vector3(0, 1, 0));

	}
}
