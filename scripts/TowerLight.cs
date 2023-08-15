using Godot;
using System;

public partial class TowerLight : Node3D
{

	Camera3D mainCamera;
	Vector3 rayOrigin = new Vector3();
	Vector3 rayDirection = new Vector3();

	private float focalLength = 1f;

	private const float RayLength = 1000f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		mainCamera = GetNode<Camera3D>("../Camera3D");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		

	}

	public override void _PhysicsProcess(double delta){

		MoveLight();

	}


	public override void _Input(InputEvent @event){

		if(@event is InputEventMouseButton inputEventMouse && inputEventMouse.Pressed && inputEventMouse.ButtonIndex == MouseButton.WheelDown){

			focalLength -= 0.1f;

		} else if (@event is InputEventMouseButton inputEventMouse2 && inputEventMouse2.Pressed && inputEventMouse2.ButtonIndex == MouseButton.WheelUp){

			focalLength += 0.1f;

		}

		focalLength = Mathf.Clamp(focalLength, 0.2f, 1f);
	
	}

	public void MoveLight(){

		Vector3 from = mainCamera.ProjectRayOrigin(GetViewport().GetMousePosition());
		Vector3 to = from + mainCamera.ProjectRayNormal(GetViewport().GetMousePosition()) * RayLength;

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;

		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);

		Godot.Collections.Dictionary result = spaceState.IntersectRay(query);

		((Sprite3D)this.GetChild(0)).Scale = new Vector3(focalLength, focalLength, focalLength);
		((Sprite3D)this.GetChild(0)).Visible = true;

		if (result.Count > 0){

			this.Position = new Vector3(((Vector3)result["position"]).X, 0, ((Vector3)result["position"]).Z);
		} else {

			((Sprite3D)this.GetChild(0)).Visible = false;

		}
	}




}
