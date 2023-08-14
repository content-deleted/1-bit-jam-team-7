using Godot;
using System;

public partial class CameraController : Camera2D
{
	public override void _Process(double delta)
	{
		GlobalPosition = PlayerMovement.player.GlobalPosition;
	}
}

