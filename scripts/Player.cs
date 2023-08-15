using Godot;
using System;

public partial class Player : Node3D
{
	float playerSpeed = 0;
	double stamina = 3;

	double staminaCoolDown = 5;
	double staminaCoolDownTimer = 0;

	float playerDefaultSpeed = 2;
	Vector3 playerDirection = new Vector3(0, 0, 0);

	Sprite3D playerSprite;
	Sprite3D lampLight;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		playerSprite = GetNode<Sprite3D>("PlayerSprite");
		lampLight = GetNode<Sprite3D>("LampLight");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Move(delta);

	}


	public void Move(double delta)
	{

		playerSpeed = playerDefaultSpeed;

		if(Input.IsActionPressed("Sprint") && stamina > 0 && staminaCoolDownTimer >= 0){

			playerSpeed *= 1.8f;
			stamina -= delta;

		} else if (stamina >= 0){

			staminaCoolDownTimer = staminaCoolDown;

		} else {

			staminaCoolDownTimer -= delta;
			stamina += delta;

			Mathf.Clamp(staminaCoolDownTimer, 0, 5);
			Mathf.Clamp(stamina, 0, 3);

		}


		if(Input.IsActionPressed("MovePlayerUp") && !Input.IsActionPressed("MovePlayerDown")){

			playerDirection.Z = -1;

		} else if (Input.IsActionPressed("MovePlayerDown") && !Input.IsActionPressed("MovePlayerUp")){

			playerDirection.Z = 1;

		} else {

			playerDirection.Z = 0;

		}

		if(Input.IsActionPressed("MovePlayerLeft") && !Input.IsActionPressed("MovePlayerRight")){

			playerDirection.X = -1;
			playerSprite.FlipH = true;
			playerSprite.Position = new Vector3(0.005f, 0.15f, 0);
			lampLight.Position = new Vector3(-0.075f, 0, 0);

		} else if (Input.IsActionPressed("MovePlayerRight") && !Input.IsActionPressed("MovePlayerLeft")){

			playerDirection.X = 1;
			playerSprite.FlipH = false;
			playerSprite.Position = new Vector3(-0.005f, 0.15f, 0);
			lampLight.Position = new Vector3(0.075f, 0, 0);

		} else {

			playerDirection.X = 0;
			


		}

		Vector3 playerMove = playerDirection.Normalized() * playerSpeed * (float)delta;

		this.Translate(playerMove);

	}
}
