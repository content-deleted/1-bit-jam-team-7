using Godot;
using System;

public partial class TowerLight : Node3D
{

	Camera3D mainCamera;
	Vector3 rayOrigin = new Vector3();
	Vector3 rayDirection = new Vector3();

	public float focalLength = 1f;

	public float RayLength = 1000f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	
	}

	public override void _PhysicsProcess(double delta){

	

	}



}
