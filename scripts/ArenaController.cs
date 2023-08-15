using Godot;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;

public partial class ArenaController : Node
{
	[Export]
	public PackedScene[] enemyPrefabs { get; set; }

    private int _score;
	public int score {
        get { return _score; }
        set { 
            _score = value;
            scoreLabel.Text = _score.ToString(); 
        }
    }

    Node enemyPool;

    Label scoreLabel;

    public static ArenaController controller;
	public override void _Ready()
	{
        // Singleton
		if(controller != null) {
			QueueFree();
			return;
		}
		controller = this;

        enemyPool = GetNode("EnemyPool");
        TextMacros.dialogueRunner = GetNode("/root/Base/Camera2D/DialogueRunner") as Node2D;
        scoreLabel = GetNode("/root/Base/Camera2D/HUD/Score") as Label;

        //FPS
        Engine.MaxFps = 60;
	}

    [Export]
    int triangleSpawnTimerMax = 100;
	int triangleSpawnTimer = 0;

    public static bool spawnEnemies = false;
	public override void _Process(double delta)
	{
        if (!spawnEnemies) {
            return;
        }
        triangleSpawnTimer--;
        if(triangleSpawnTimer <= 0 && Cage.cages.Count > 0) {
            triangleSpawnTimer = triangleSpawnTimerMax;

            SpawnTriangle();

            // easy way to make it get harder over time
            if(GD.Randf() < 100.0 / triangleSpawnTimerMax) triangleSpawnTimerMax -= 1;
        }
	}

    public void SpawnTriangle() {
        var triangle = enemyPrefabs[(int)(GD.Randi() % enemyPrefabs.Length)].Instantiate<Triangle>();
        Vector2 spawnPos = (60 + GD.Randf()) * 5 * new Vector2(GD.Randf() - 0.5f, GD.Randf() - 0.5f).Normalized();
        triangle.GlobalPosition = spawnPos;
        enemyPool.AddChild(triangle);
    }
	
    public static void Restart() => controller.NewGame();

	public void NewGame()
	{
		score = 0;

        // clear enemies
        foreach (var child in enemyPool.GetChildren())
        {
            child.QueueFree();
        }

		var player = GetNode<PlayerMovement>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		//GetNode<Timer>("StartTimer").Start();
	}
}
