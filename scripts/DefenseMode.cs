using Godot;
using System;
using System.Collections;
using System.Linq;


public partial class DefenseMode : Node3D
{

	// wave control variables

	int wave;
	int waveAmount;

	double waveCountDown = 5;
	public double waveCountDownTimer;

	double waveTimer; 
	private static bool _waveState;

	private static int _score;
    
    public delegate void OnWaveEvent();

    public static OnWaveEvent onWaveStartEventHandler;
    
    public static OnWaveEvent onWaveEndEventHandler;
	public static int score {
		get=> _score;
		set {
			_score = value;
			scoreLabel.Text = value.ToString();
		}
	}
	public static Label scoreLabel;

    public static Button StartRoundButton;

	public static bool waveState {
		get { return _waveState; }
		set { 
			_waveState = value;
			if(value) {
				towerLightNode.Show();
				ShopController.Close();
                DescriptionPanel.HidePanel();
                StartRoundButton.Hide();
                onWaveStartEventHandler?.Invoke();
			} else {
				ShopController.Open();
				towerLightNode.Hide();
                DescriptionPanel.HidePanel();
                StartRoundButton.Show();
                onWaveEndEventHandler?.Invoke();
			}
		}
	}

	//game Node references

	Player playerNode;
	static TowerLight towerLightNode;
	Camera mainCamera;
	EnemyController enemyControllerNode;
	WorldEnvironment worldEnvironmentNode;
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallVariousNodes();
		waveState = false;
		score = 0;
	}

	//initializing functions

	public void CallVariousNodes(){

		playerNode = GetNode<Player>("World/PlayerNodes/Player");

		towerLightNode = GetNode<TowerLight>("World/PlayerNodes/TowerLight");

		mainCamera = GetNode<Camera>("MainCamera");

		enemyControllerNode = GetNode<EnemyController>("World/Level/EnemySpawn");

		worldEnvironmentNode = GetNode<WorldEnvironment>("World/Environment/WorldEnvironment");

		scoreLabel = GetNode<Label>("ViewportOverlay/HUD/Info/score");

        StartRoundButton = GetNode<Button>("ViewportOverlay/HUD/StartRoundButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateMouseFieldPos();
		MovePlayer(delta);

		if(waveState){
			MoveTowerLight();
			Wave(delta);
		} else if (!waveState){
			Sunrise(delta);
		}
	}

	public static Vector3 mouseFieldPos = new Vector3();

	public static bool mouseOnField = false;

	public void UpdateMouseFieldPos_old() {
		Vector3 from = mainCamera.ProjectRayOrigin(GetViewport().GetMousePosition());
		Vector3 to = from + mainCamera.ProjectRayNormal(GetViewport().GetMousePosition()) * (float)towerLightNode.RayLength;

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;

		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);

		Godot.Collections.Dictionary result = spaceState.IntersectRay(query);

		if (result.Count > 0) {
			mouseFieldPos = new Vector3(((Vector3)result["position"]).X, 0, ((Vector3)result["position"]).Z); 
			mouseOnField = true;
		} else {
			mouseOnField = false;
		}
	}

	public void UpdateMouseFieldPos() {
		var plane = new Plane(Vector3.Up);
		Vector3 from = mainCamera.ProjectRayOrigin(GetViewport().GetMousePosition());
		Vector3 to = mainCamera.ProjectRayNormal(GetViewport().GetMousePosition());

		var pos = plane.IntersectsRay(from, to);

		if (pos != null) {
			var vec = (Vector3)pos;
			if(Math.Abs(vec.X) < 9.5 && Math.Abs(vec.Z) < 9.5) {
				mouseFieldPos = (Vector3)pos;
				mouseOnField = true;
				return;
			}
		} 

		mouseOnField = false;
	}

	#region Wave State Functions

	public void StartWave(){

		wave += 1;

		waveCountDownTimer = waveCountDown;

		enemyControllerNode.enemyCount = 3; // progression handled here later

		waveState = true;	

	}

	public void EndWave(){

		waveState = false;

	}

	public void Wave(double delta) {

		if (waveCountDownTimer >= 0){
			
			Sunset(delta);

		} else {

			
			if (enemyControllerNode.enemyCount == 0 && enemyControllerNode.enemies.Count == 0){

				EndWave();

			}


		}
	}

	public void Sunrise(double delta){
			
		waveCountDownTimer += delta;

		worldEnvironmentNode.Environment.AmbientLightEnergy += (float)delta;

		worldEnvironmentNode.Environment.AmbientLightEnergy = Mathf.Clamp(worldEnvironmentNode.Environment.AmbientLightEnergy, 0.0f, 1f);

	}

	public void Sunset(double delta){

		waveCountDownTimer -= delta;

		worldEnvironmentNode.Environment.AmbientLightEnergy -= (float)delta;

		worldEnvironmentNode.Environment.AmbientLightEnergy = Mathf.Clamp(worldEnvironmentNode.Environment.AmbientLightEnergy, 0.0f, 1f);

	}

	#endregion

	#region Player Input Functions

    // easy way for interupting the fix animation
    public static bool playerMoved = false;
    // used for disabling moving while talking
    public static bool playerCanMove = true;
	public void MovePlayer(double delta) // this goes in Process because it's delta dependent (frame independent)
	{
        if(!playerCanMove) return;

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
            playerMoved = true;

		} else if (Input.IsActionPressed("MovePlayerDown") && !Input.IsActionPressed("MovePlayerUp")){

			playerNode.playerDirection.Z = 1;
            playerMoved = true;

		} else {

			playerNode.playerDirection.Z = 0;

		}

		if(Input.IsActionPressed("MovePlayerLeft") && !Input.IsActionPressed("MovePlayerRight")){
			playerNode.playerDirection.X = -1;
			playerNode.playerSprite.FlipH = false;
			playerNode.playerSprite.Position = new Vector3(0.005f, 0.15f, 0);
            playerMoved = true;
		} else if (Input.IsActionPressed("MovePlayerRight") && !Input.IsActionPressed("MovePlayerLeft")){
			playerNode.playerDirection.X = 1;
			playerNode.playerSprite.FlipH = true;
			playerNode.playerSprite.Position = new Vector3(-0.005f, 0.15f, 0);
            playerMoved = true;
		} else {
			playerNode.playerDirection.X = 0;
		}

		Vector3 playerMove = playerNode.playerDirection.Normalized() * playerNode.playerSpeed * (float)delta;

		Vector3 cameraRelativeMove = playerNode.CameraRelativeMove(playerMove);

		playerNode.Translate(cameraRelativeMove);

	}

	public void MoveTowerLight() {
		var light = towerLightNode.GetNode("SpotLight3D") as SpotLight3D;
		light.SpotAngle = 23.0f * towerLightNode.focalLength;
		light.LightEnergy = 0.75f / towerLightNode.focalLength;
		towerLightNode.Position = mouseFieldPos;
		if(mouseOnField) {
			towerLightNode.Show();
		} else {
			towerLightNode.Hide();
		}
	}

	public override void _Input(InputEvent @event){

		if(@event is InputEventMouseButton inputEventMouse && inputEventMouse.Pressed && inputEventMouse.ButtonIndex == MouseButton.WheelDown){

			towerLightNode.focalLength -= 0.1f;
			towerLightNode.focalLength = Mathf.Clamp(towerLightNode.focalLength, 0.5f, 5f);
            towerLightNode.SetMeta("Power", 1f/towerLightNode.focalLength);


		} else if (@event is InputEventMouseButton inputEventMouse2 && inputEventMouse2.Pressed && inputEventMouse2.ButtonIndex == MouseButton.WheelUp){

			towerLightNode.focalLength += 0.1f;
			towerLightNode.focalLength = Mathf.Clamp(towerLightNode.focalLength, 0.5f, 5f);
            towerLightNode.SetMeta("Power", 1f/towerLightNode.focalLength);

			
		} else if (@event is InputEventKey inputEventKey && inputEventKey.Pressed){ // single key press

			if (inputEventKey.Keycode == Key.Q){

				mainCamera.SetLook("left");
				
			} else if (inputEventKey.Keycode == Key.E){

				mainCamera.SetLook("right");

			} else if (!waveState && inputEventKey.Keycode == Key.Enter){


				StartWave();

			}


		}		
	
	}
	#endregion

}
