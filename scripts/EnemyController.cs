using Godot;
using System;
using System.Collections.Generic;
using System.IO;


public partial class EnemyController : Path3D
{	



	//visualization var
	[Export]
	float lightCount = 300;
	PackedScene pathLight;


	//enemy spawn var

	public int enemyCount = 0;


	double enemyTimer;
	double enemyTime = 2;

	DefenseMode defenseMode;
	Base playerBase;
	Path3D enemyPath;

	PackedScene testEnemy;

	List<PathFollow3D> pathLights = new List<PathFollow3D>();
	public List<PathFollow3D> enemies = new List<PathFollow3D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		defenseMode = (DefenseMode)GetNode("/root/DefenseMode");
		testEnemy = GD.Load<PackedScene>("res://scenes/enemies/TestEnemy.tscn");
		pathLight = GD.Load<PackedScene>("res://scenes/PathLight.tscn");
		
		playerBase = (Base)GetNode("//root/DefenseMode/World/PlayerNodes/Base");

		SetPathLights();

		//testEnemy = (PathFollow3D)this.GetChild(0);


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


		if (enemyTimer > 0){

			enemyTimer -= delta;

		} else {
			
			if (defenseMode.waveCountDownTimer <= 0 && defenseMode.waveState && enemyCount > 0){

				SpawnEnemy();
				enemyTimer = enemyTime;

			}
		}

	}

	public void SpawnEnemy(){

		var enemy = (PathFollow3D)testEnemy.Instantiate();
		enemies.Add(enemy);
		this.AddChild(enemy);
		
		enemyCount--;

	}

	public void SetPathLights(){

		for (int i = 0; i < lightCount; i ++){

			var lightInstance = (PathFollow3D)pathLight.Instantiate();
		
			pathLights.Add(lightInstance);
			this.AddChild(lightInstance);
			lightInstance.ProgressRatio = i / lightCount;
			

		}


	}


}
