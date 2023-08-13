using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyController : Node2D
{	
	Random randEnemy = new Random();
	List<Enemy> enemies;
	double enemyTimer;
	double enemyTime = 3;

	Base playerBase;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		enemies = new List<Enemy>();

		playerBase = (Base)GetNode("//root/DefenseMode/Base");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (enemyTimer > 0){

			enemyTimer -= delta;

		} else {

			SpawnEnemy();
			enemyTimer = enemyTime;

		}

		foreach (Enemy enemy in enemies){

			MoveEnemy(enemy, delta);

		}

	}

	public void SpawnEnemy(){

		Enemy newEnemy = new Enemy();

		newEnemy.type = randEnemy.Next(1, 4);
		Sprite2D newEnemySprite = new Sprite2D();
		newEnemy.sprite = newEnemySprite;
		this.AddChild(newEnemy.sprite);
		newEnemy.sprite.Texture = (Texture2D)ResourceLoader.Load("res://gfx/" + enemyTypes[newEnemy.type] + ".png");
		Area2D enemyArea = new Area2D();
		CollisionShape2D enemyCollision = new CollisionShape2D();
		CircleShape2D enemycircle = new CircleShape2D();
		enemycircle.Radius = 15;
		enemyCollision.Shape = enemycircle;
		enemyArea.AddChild(enemyCollision);
		enemyArea.AddToGroup("Enemy" + enemyTypes[newEnemy.type]);
		newEnemy.sprite.AddChild(enemyArea);
		
		

		int quadrant = randEnemy.Next(1, 5);

		if (quadrant == 1){

			newEnemy.position = new Vector2(randEnemy.Next(0, 100), randEnemy.Next(0, 50));

		} else if (quadrant == 2){

			newEnemy.position = new Vector2(randEnemy.Next(900, 1000), randEnemy.Next(0, 50));

		} else if (quadrant == 3){

			newEnemy.position = new Vector2(randEnemy.Next(0, 100), randEnemy.Next(450, 500));

		} else if (quadrant == 4){

			newEnemy.position = new Vector2(randEnemy.Next(900, 1000), randEnemy.Next(450, 500));

		}

		newEnemy.sprite.Position = newEnemy.position;

		enemies.Add(newEnemy);
	}

	public void MoveEnemy(Enemy enemy, double delta){

		if (enemy.type == 1){ // Triangle enemy

			enemy.direction = playerBase.Position - enemy.position;
			enemy.speed = 80;

			enemy.sprite.Translate(enemy.direction.Normalized() * enemy.speed * (float)delta);

			enemy.position = enemy.sprite.Position;

		} else if (enemy.type == 2){ // Square enemy

			enemy.direction = playerBase.Position - enemy.position;
			enemy.speed = 40;

			enemy.sprite.Translate(enemy.direction.Normalized() * enemy.speed * (float)delta);

			enemy.position = enemy.sprite.Position;

		} else if (enemy.type == 3){ // Hexagon enemy

			enemy.direction = playerBase.Position - enemy.position;
			enemy.speed = 60;

			enemy.sprite.Translate(enemy.direction.Normalized() * enemy.speed * (float)delta);

			enemy.position = enemy.sprite.Position;

		}


	}

	public void KillEnemy(Sprite2D enemySprite){

		GD.Print("Killed enemy");

		enemies.Remove(enemies.Find(x => x.sprite == enemySprite));
		enemySprite.QueueFree();

	}

	public class Enemy{

		public int type;
		public Sprite2D sprite;
		public Vector2 position;
		public Vector2 direction;
		public float speed;


	}

	public Dictionary<int, string> enemyTypes = new Dictionary<int, string>(){
		{1, "Triangle"},
		{2, "Square"},
		{3, "Hexagon"}
	};

}
