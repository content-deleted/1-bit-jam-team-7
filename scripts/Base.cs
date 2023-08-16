using Godot;
using System;

public partial class Base : Node3D

{

	public Sprite3D towerSprite;
	public Camera mainCamera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		towerSprite = (Sprite3D)GetNode("TowerSprite");
		mainCamera = (Camera)GetNode("//root/DefenseMode/MainCamera");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		BillBoard();

	}

	public void BillBoard(){

		towerSprite.LookAt(mainCamera.Position, new Vector3(0, 1, 0));

	}

}
