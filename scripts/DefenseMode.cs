using Godot;
using System;

public partial class DefenseMode : Node3D
{

	// wave control variables

	int wave;
	double waveTimer;
	bool waveState;

	//game Node references

	Player playerNode;
	TowerLight towerLightNode;
	Camera mainCamera;
	EnemyController enemyControllerNode;
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallVariousNodes();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		MovePlayer(delta);
		MoveTowerLight();

	}



//player Input functions

	public void MovePlayer(double delta) // this goes in Process because it's delta dependent (frame indepdenent)
	{

		playerNode.playerSpeed = playerNode.playerDefaultSpeed;

		if(Input.IsActionPressed("Sprint") && playerNode.stamina > 0 && playerNode.staminaCoolDownTimer >= 0){

			playerNode.playerSpeed *= 1.8f;
			playerNode.stamina -= delta;

		} else if (playerNode.stamina >= 0){

			playerNode.staminaCoolDownTimer = playerNode.staminaCoolDown;

		} else {

			playerNode.staminaCoolDownTimer -= delta;
			playerNode.stamina += delta;

			Mathf.Clamp(playerNode.staminaCoolDownTimer, 0, 5);
			Mathf.Clamp(playerNode.stamina, 0, 3);

		}


		if(Input.IsActionPressed("MovePlayerUp") && !Input.IsActionPressed("MovePlayerDown")){

			playerNode.playerDirection.Z = -1;

		} else if (Input.IsActionPressed("MovePlayerDown") && !Input.IsActionPressed("MovePlayerUp")){

			playerNode.playerDirection.Z = 1;

		} else {

			playerNode.playerDirection.Z = 0;

		}

		if(Input.IsActionPressed("MovePlayerLeft") && !Input.IsActionPressed("MovePlayerRight")){

			playerNode.playerDirection.X = -1;
			playerNode.playerSprite.FlipH = true;
			playerNode.playerSprite.Position = new Vector3(0.005f, 0.15f, 0);
			playerNode.lampLight.Position = new Vector3(-0.075f, 0, 0);

		} else if (Input.IsActionPressed("MovePlayerRight") && !Input.IsActionPressed("MovePlayerLeft")){

			playerNode.playerDirection.X = 1;
			playerNode.playerSprite.FlipH = false;
			playerNode.playerSprite.Position = new Vector3(-0.005f, 0.15f, 0);
			playerNode.lampLight.Position = new Vector3(0.075f, 0, 0);

		} else {

			playerNode.playerDirection.X = 0;
			

		}

		Vector3 playerMove = playerNode.playerDirection.Normalized() * playerNode.playerSpeed * (float)delta;

		Vector3 cameraRelativeMove = playerNode.CameraRelativeMove(playerMove);

		playerNode.Translate(cameraRelativeMove);

	}


	public void MoveTowerLight(){

		Vector3 from = mainCamera.ProjectRayOrigin(GetViewport().GetMousePosition());
		Vector3 to = from + mainCamera.ProjectRayNormal(GetViewport().GetMousePosition()) * (float)towerLightNode.RayLength;

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;

		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);

		Godot.Collections.Dictionary result = spaceState.IntersectRay(query);

		((Sprite3D)towerLightNode.GetChild(0)).Scale = new Vector3(towerLightNode.focalLength, towerLightNode.focalLength, towerLightNode.focalLength);
		((Sprite3D)towerLightNode.GetChild(0)).Visible = true;

		if (result.Count > 0){

			towerLightNode.Position = new Vector3(((Vector3)result["position"]).X, 0, ((Vector3)result["position"]).Z);
		} else {

			((Sprite3D)towerLightNode.GetChild(0)).Visible = false;

		}
	}

	public override void _Input(InputEvent @event){

		if(@event is InputEventMouseButton inputEventMouse && inputEventMouse.Pressed && inputEventMouse.ButtonIndex == MouseButton.WheelDown){

			towerLightNode.focalLength -= 0.1f;
			towerLightNode.focalLength = Mathf.Clamp(towerLightNode.focalLength, 0.2f, 1f);


		} else if (@event is InputEventMouseButton inputEventMouse2 && inputEventMouse2.Pressed && inputEventMouse2.ButtonIndex == MouseButton.WheelUp){

			towerLightNode.focalLength += 0.1f;
			towerLightNode.focalLength = Mathf.Clamp(towerLightNode.focalLength, 0.2f, 1f);

			
		} else if (@event is InputEventKey inputEventKey && inputEventKey.Pressed){

			if (inputEventKey.Keycode == Key.Q){

				mainCamera.SetLook("left");
				
			} else if (inputEventKey.Keycode == Key.E){

				mainCamera.SetLook("right");

			}

		}
		
	
	}


//initializing functions

	public void CallVariousNodes(){

		playerNode = GetNode<Player>("World/PlayerNodes/Player");
		
		towerLightNode = GetNode<TowerLight>("World/PlayerNodes/TowerLight");

		mainCamera = GetNode<Camera>("MainCamera");

		enemyControllerNode = GetNode<EnemyController>("World/Level/EnemySpawn");

	}

}
