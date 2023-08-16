using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyController : Path3D
{	
	Random randEnemy = new Random();
	List<Enemy> enemies;
	double enemyTimer;
	double enemyTime = 2;

	Base playerBase;
	Path3D enemyPath;

	PathFollow3D testEnemy;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{


		enemies = new List<Enemy>(); //inherit from PathFollow3D
		playerBase = (Base)GetNode("//root/DefenseMode/World/PlayerNodes/Base");


		testEnemy = (PathFollow3D)this.GetChild(0);


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		testEnemy.Progress += (float)delta;

		if (enemyTimer > 0){

			enemyTimer -= delta;

		} else {

			//SpawnEnemy();
			enemyTimer = enemyTime;

		}

		foreach (Enemy enemy in enemies){

			MoveEnemy(enemy, delta);

		}

	}

	public void SpawnEnemy(){

		GD.Print("EnemySpawned");

		Enemy newEnemy = new Enemy();

		Sprite3D newEnemySprite = new Sprite3D();
		newEnemy.sprite = newEnemySprite;
		this.AddChild(newEnemy.sprite);
		newEnemy.sprite.Texture = (Texture2D)ResourceLoader.Load("res://gfx/Hexagon.png");
		Area3D enemyArea = new Area3D();
		CollisionShape2D enemyCollision = new CollisionShape2D();
		CircleShape2D enemycircle = new CircleShape2D();
		enemycircle.Radius = 15;
		enemyCollision.Shape = enemycircle;
		enemyArea.AddChild(enemyCollision);
		enemyArea.AddToGroup("Enemy");
		newEnemy.sprite.AddChild(enemyArea);
		newEnemy.speed = 20;
		
		newEnemySprite.Position = this.Position;

		int quadrant = randEnemy.Next(1, 5);

		


		enemies.Add(newEnemy);
	}

	public void MoveEnemy(Enemy enemy, double delta){

	

	}

	public void KillEnemy(Sprite3D enemySprite){

		GD.Print("Killed enemy");

		enemies.Remove(enemies.Find(x => x.sprite == enemySprite));
		enemySprite.QueueFree();

	}

	public class Enemy{

		public Sprite3D sprite;
		public Vector3 position;
		public Vector3 direction;
		public float speed;


	}


}
