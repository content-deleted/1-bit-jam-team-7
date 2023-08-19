using Godot;
using System;
using System.Collections.Generic;
using System.IO;


public partial class EnemyController : StaticBody3D
{	

	Random randE = new Random();

	//visualization var
	[Export]
	float lightCount = 300;


	//enemy spawn var

	public int enemyCount = 0;
	public int totalEnemiesLastWave;

	public bool drawPath = false;

	double enemyTimer;
	public double enemyTime;

	DefenseMode defenseMode;
	Base playerBase;
	public List<Path3D> enemyPaths = new List<Path3D>();

	PackedScene testEnemy;
	PackedScene pathLight;

	List<PathFollow3D> pathLights = new List<PathFollow3D>();
	public List<PathFollow3D> enemies = new List<PathFollow3D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		defenseMode = (DefenseMode)GetNode("/root/DefenseMode");
		testEnemy = GD.Load<PackedScene>("res://scenes/enemies/TestEnemy.tscn");
		playerBase = (Base)GetNode("//root/DefenseMode/World/PlayerNodes/Base");
		pathLight = GD.Load<PackedScene>("res://scenes/PathLight.tscn");

		
		//testEnemy = (PathFollow3D)this.GetChild(0);


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (!drawPath){

			foreach (Path3D path in enemyPaths){

				DrawPath(path);

			}

			drawPath = true;

		}

		if (enemyTimer > 0){

			enemyTimer -= delta;

		} else {

			if (defenseMode.waveCountDownTimer <= 0 && DefenseMode.waveState && enemyCount > 0){

				Path3D enemyPath = enemyPaths[randE.Next(enemyPaths.Count)];

				SpawnEnemy(enemyPath);
				enemyTimer = enemyTime;

			}
		}

	}

	public void SpawnEnemy(Path3D enemyPath){

		var enemy = (PathFollow3D)testEnemy.Instantiate();
		enemies.Add(enemy);
		enemyPath.AddChild(enemy);
		
		enemyCount--;

	}

	public void DrawPath(Path3D path){

		for (int i = 0; i < lightCount; i ++){

			var lightInstance = (PathFollow3D)pathLight.Instantiate();
		
			pathLights.Add(lightInstance);
			path.AddChild(lightInstance);
			lightInstance.ProgressRatio = i / lightCount;
			

		}


	}


}
