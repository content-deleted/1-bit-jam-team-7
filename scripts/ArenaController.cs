using Godot;
using System;

public partial class ArenaController : Node
{
	[Export]
	public PackedScene trianglePrefab { get; set; }

	private int score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
        if(triangleSpawnTimer <= 0) {
            triangleSpawnTimer = triangleSpawnTimerMax;
            var triangle = trianglePrefab.Instantiate<Triangle>();
            Vector2 spawnPos = (60 + GD.Randf()) * 5 * new Vector2(GD.Randf() - 0.5f, GD.Randf() - 0.5f).Normalized();
            triangle.GlobalPosition = spawnPos;
            AddChild(triangle);
        }
	}
	
	public void NewGame()
	{
		score = 0;

		var player = GetNode<PlayerMovement>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		//GetNode<Timer>("StartTimer").Start();
	}
}
