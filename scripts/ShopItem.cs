using Godot;
using System;
using System.Collections.Generic;


public partial class ShopItem : Button
{
	public towerInfo info;
	public override void _Ready()
	{
		// placeholder
		info = towerTypes[0];
	}

	public void OpenDescriptionPanel() {
		DescriptionPanel.ShowPanel(GlobalPosition + new Vector2(0, 60), info.name, textBody(), false, " Buy ", null);
	}

	public void HideDescriptionPanel() {
		DescriptionPanel.HidePanel();
	}

	public void SelectTower() {
        ShopController.towerPlacing = true;
        if(ShopController.currentTower.name != info.name) {
            ShopController.currentTower = info;
            // not very efficent but do I care?
            if(ShopController.towerPlacementTest != null) ShopController.towerPlacementTest.QueueFree();
            ShopController.towerPlacementTest = info.prefab.Instantiate() as Tower;
            TowerController.controller.AddChild(ShopController.towerPlacementTest);
        }
	}
	
	public string textBody () => $"Health: {info.maxHealth}\n{info.description}\n\n{info.flavor}";

	public struct towerInfo {
		public string name;
		public string description;
		public string flavor;
		public int cost;
		public int maxHealth;
        public PackedScene prefab;
	}

	public towerInfo[] towerTypes = {
		new towerInfo { 
			name = "Fool's Bird",
			description = "Cast's light about itself in a 1-space radius. Charges adjacent towers.",
			flavor = "\"I swear it moves when I'm not looking.\"",
			cost = 20,
			maxHealth = 20,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/test_tower.tscn")
		},
	};
}
