using Godot;
using System;
using Yarn.Compiler;


public partial class TowerController : Node3D
{
	
    public static TowerController controller;
	public override void _Ready()
	{
        if(controller != null) {
			QueueFree();
			return;
		}
		controller = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public static void PlaceTower(ShopItem.towerInfo tower, Vector3 pos) {
        var tow = tower.prefab.Instantiate() as Tower;
        tow.info = tower;
        controller.AddChild(tow);
        tow.GlobalPosition = pos;
        tow.maxHealth = tower.maxHealth;
        tow.currentHealth = tower.maxHealth;
		tow.range = tower.range;

		((TorusMesh)tow.rangeMesh.Mesh).OuterRadius = tow.range;
        ((TorusMesh)tow.rangeMesh.Mesh).InnerRadius = tow.range - 0.2f;
		((SphereShape3D)tow.rangeCollider.Shape).Radius = tow.range;
		tow.rangeMesh.Show();
	
    }

	public void ToggleTowerRange(){

		foreach(Tower tower in controller.GetChildren()){
			tower.rangeMesh.Visible = !tower.rangeMesh.Visible;
		}

	}

}
