using Godot;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Yarn;
using Yarn.GodotYarn;

public partial class DefenseMode : Node3D
{
    public static LevelSelect.levelInfo levelInfo;

	// wave control variables
    private static int _wave;
    public static int wave {
		get=> _wave;
		set {
			_wave = value;
			waveLabel.Text =  "Wave: " + value;
		}
	}
	public static Label waveLabel;

	int waveAmount;

	// The maximum possible gold obtained from previous waves
	int maxGoldPrev;

	// Matapacos Standardized Difficulty - See Alicento Towers doc for more detail
	float MSD; 

	double waveCountDown = 5;
	public double waveCountDownTimer;

	double waveTimer; 
	private static bool _waveState;

	private static int _score;
	public static bool showRange = false;
    
    public delegate void OnWaveEvent();

    public static OnWaveEvent onWaveStartEventHandler;
    
    public static OnWaveEvent onWaveEndEventHandler;
	public static int score {
		get=> _score;
		set {
			_score = value;
			scoreLabel.Text = "Score: " + value;
		}
	}
	public static Label scoreLabel;

	public static Button toggleRangeButton;
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
				toggleRangeButton.Hide();
                onWaveStartEventHandler?.Invoke();
				towerController.ToggleTowerRange();
				PaletteController.SetNewPallete(sunsetColor, black);
			} else {
				ShopController.Open();
				towerLightNode.Hide();
                DescriptionPanel.HidePanel();
                StartRoundButton.Show();
				toggleRangeButton.Show();
                onWaveEndEventHandler?.Invoke();
				PaletteController.SetNewPallete(sunriseColor, black);
			}
		}
	}

	//game Node references

	Player playerNode;
	static TowerLight towerLightNode;
	Camera mainCamera;
	EnemyController enemyControllerNode;
	WorldEnvironment worldEnvironmentNode;
    Area3D towerLightArea;
	static TowerController towerController;
	EnemyController levelNode;

	// color pallete colors for sunrise and sunset

	static Color black = new Color(0, 0, 0, 1);
	static Color sunriseColor = new Color(1, 0.95f, 0.85f, 1);
	static Color sunsetColor = new Color(0.85f, 0.85f, 1f, 1);
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallVariousNodes();

		SetEnemyPath();

		waveState = false;
		score = 0;
	}

	//initializing functions

	public void CallVariousNodes(){

		playerNode = GetNode<Player>("World/PlayerNodes/Player");

		towerLightNode = GetNode<TowerLight>("World/PlayerNodes/TowerLight");

        towerLightArea = towerLightNode.GetNode<Area3D>("Area3D");

		mainCamera = GetNode<Camera>("MainCamera");

		towerController = GetNode<TowerController>("World/TowerController");

		enemyControllerNode = GetNode<EnemyController>("World/Level");

		worldEnvironmentNode = GetNode<WorldEnvironment>("World/Environment/WorldEnvironment");

		scoreLabel = GetNode<Label>("ViewportOverlay/HUD/Info/score");

        waveLabel = GetNode<Label>("ViewportOverlay/HUD/Info/wave");

		levelNode = GetNode<EnemyController>("World/Level");

        toggleRangeButton = GetNode<Button>("ViewportOverlay/HUD/ToggleVisual");
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

        if(levelInfo.yarnSpinnerNodeToPlayOnLevelStart != null && levelInfo.yarnSpinnerNodeToPlayOnLevelStart != "") {
            var runner = GetNode<DialogueRunner>("//root/DefenseMode/ViewportOverlay/HUD/DialogueRunner");
            runner.Show();
            runner.StartDialogue(levelInfo.yarnSpinnerNodeToPlayOnLevelStart);
            levelInfo.yarnSpinnerNodeToPlayOnLevelStart = "";
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

	public void SetEnemyPath(){

		foreach (Curve3D path in levelInfo.paths) {
			
			var enemyPath = new Path3D();
			enemyPath.Curve = path;
			levelNode.AddChild(enemyPath);
			levelNode.enemyPaths.Add(enemyPath);
			enemyPath.Position = new Vector3(-9, 0, -9); // FUCK YOU PAST VESTED

		}

	}

	public void StartWave(){

		wave += 1;

		waveCountDownTimer = waveCountDown;

		// Determines enemy count per wave
		switch(wave)
		{
			case 1:
				//TODO: connect this with shopcontroller's starting gold
				maxGoldPrev = 60;
                enemyControllerNode.enemyTime = 1;
                enemyControllerNode.enemyCount = 3;
				enemyControllerNode.totalEnemiesLastWave = enemyControllerNode.enemyCount;
				MSD = 1.3f;
                break;
			default:
				// enemyTime must be set b4 enemyCount is updated to use previous wave's count

				maxGoldPrev += (enemyControllerNode.totalEnemiesLastWave * 5);
				MSD -= 0.1f;
				enemyControllerNode.enemyTime = 1/((maxGoldPrev / 15) / (3 * MSD));
				enemyControllerNode.bigEnemyTime = enemyControllerNode.enemyTime * 5;
                enemyControllerNode.enemyCount = enemyControllerNode.totalEnemiesLastWave + 3 * (wave-1);
				enemyControllerNode.totalEnemiesLastWave = enemyControllerNode.enemyCount;

				if (wave % 5 == 0){

					enemyControllerNode.bigEnemyCount = wave / 5;
					enemyControllerNode.bigEnemyTimer = enemyControllerNode.bigEnemyTime;

				}

                break;
        }



        //GD.Print("Wave # is " + wave);
		//GD.Print("Wave Stats are: \nMSD: " + MSD + "\nenemies per second: " + 1/enemyControllerNode.enemyTime + "\nenemyCount: " + enemyControllerNode.enemyCount + "\nmaxGoldLastTurn: " + maxGoldPrev);

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
            playerMoved = true;
		} else if (Input.IsActionPressed("MovePlayerRight") && !Input.IsActionPressed("MovePlayerLeft")){
			playerNode.playerDirection.X = 1;
			playerNode.playerSprite.FlipH = true;
            playerMoved = true;
		} else {
			playerNode.playerDirection.X = 0;
		}

		Vector3 playerMove = playerNode.playerDirection.Normalized() * playerNode.playerSpeed * (float)delta;

        if(playerMove == Vector3.Zero) {
            playerNode.playerSprite.Play("idle");
        } else {
            playerNode.playerSprite.Play("walk");
        }

		Vector3 cameraRelativeMove = playerNode.CameraRelativeMove(playerMove);

		if (playerNode.Position.X + cameraRelativeMove.X < 9.5f 
			&& playerNode.Position.X + cameraRelativeMove.X > -9.5f
			&& playerNode.Position.Z + cameraRelativeMove.Z < 9.5f
			&& playerNode.Position.Z + cameraRelativeMove.Z > -9.5f
			){
		
			playerNode.Translate(cameraRelativeMove);
			
		} else {

			playerNode.playerSprite.Play("idle");
		}
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
            towerLightArea.SetMeta("Power", 1f/towerLightNode.focalLength);


		} else if (@event is InputEventMouseButton inputEventMouse2 && inputEventMouse2.Pressed && inputEventMouse2.ButtonIndex == MouseButton.WheelUp){

			towerLightNode.focalLength += 0.1f;
			towerLightNode.focalLength = Mathf.Clamp(towerLightNode.focalLength, 0.5f, 5f);
            towerLightArea.SetMeta("Power", 1f/towerLightNode.focalLength);

			
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
