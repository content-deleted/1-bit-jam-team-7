using Godot;
using System;

public partial class Player : Node3D
{
	public float playerSpeed = 0;
	public double stamina = 3;

	public double staminaCoolDown = 5;
	public double staminaCoolDownTimer = 0;

	public float playerDefaultSpeed = 2;
	public Vector3 playerDirection = new Vector3(0, 0, 0);

	public Sprite3D playerSprite;
	public Sprite3D lampLight;
	public Camera mainCamera;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		playerSprite = GetNode<Sprite3D>("PlayerSprite");
		lampLight = GetNode<Sprite3D>("LampLight");
		mainCamera = GetNode<Camera>("//root/DefenseMode/MainCamera");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    // not needed, all sprites have this built in
    // public override void _PhysicsProcess(double delta)
    // {
        
	// 	BillBoard();

    // }

    // public void BillBoard(){

	// 	playerSprite.LookAt(mainCamera.Position, new Vector3(0, 1, 0));

	// }

	public Vector3 CameraRelativeMove(Vector3 movement){

		if (mainCamera.cameraOrientation == 1){

			return new Vector3(movement.Z, movement.Y, -movement.X);

		} else if (mainCamera.cameraOrientation == 2){

			return new Vector3(-movement.X, movement.Y, -movement.Z);

		} else if (mainCamera.cameraOrientation == 3){

			return new Vector3(-movement.Z, movement.Y, movement.X);

		} else {
			
			return movement;


		}

	}

	
}
