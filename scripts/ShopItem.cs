using Godot;
using System;
using System.Collections.Generic;


public partial class ShopItem : Button
{
	public towerInfo info;

    [Export]
    public int type;
	public override void _Ready()
	{
		info = towerTypes[type];
        GetNode<Label>("Panel/pricelabel").Text = $"{info.cost}G";
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
			
      	  	((TorusMesh)ShopController.towerPlacementTest.rangeMesh.Mesh).OuterRadius = info.range;
        	((TorusMesh)ShopController.towerPlacementTest.rangeMesh.Mesh).InnerRadius = info.range - 0.2f;
			ShopController.towerPlacementTest.rangeMesh.Show();
            TowerController.controller.AddChild(ShopController.towerPlacementTest);
            
             // make it so that it cant be targeted
            ShopController.towerPlacementTest.hitbox.Monitorable = false;
            ShopController.towerPlacementTest.hitbox.CollisionMask = (1 << 8) | (1 << 19);
        }
	}
	
	public string textBody () => $"Health: {info.maxHealth}\n{info.description}\n\n{info.flavor}";

	public struct towerInfo {
		public string name;
		public string description;
		public string flavor;
		public int cost;
		public int maxHealth;
		public float range;
        public PackedScene prefab;
        public string dialogueName;
        public int dialogueCount;
	}

	public towerInfo[] towerTypes = {
		new towerInfo { 
			name = "Fool's Bird",
			description = "Cast's light about itself in a 1-space radius. Charges adjacent towers.",
			flavor = "\"I swear it moves when I'm not looking.\"",
			cost = 50,
			maxHealth = 5,
			range = 2.5f,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/FoolsBird.tscn"),
            dialogueName = "foolsbird",
            dialogueCount = 3,
		},
        new towerInfo { 
			name = "Matapacos",
			description = "Fires a single shot at a fixed rate while powered",
			flavor = "\"Alicanto's best friend, and a miner's worst nightmare.\"",
			cost = 15,
			maxHealth = 10,
			range = 2,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/Matapacos.tscn"),
            dialogueName = "matapacos",
            dialogueCount = 4,
		},
        new towerInfo { 
			name = "Flaming-Go",
			description = "Casts a beam of light forward that does constant damage while enemies are in contact with it.",
			flavor = "\"The threatened Chilean Flaming-Go has evolved an incendeary tactic to defend itself.\"",
			cost = 30,
			maxHealth = 10,
			range = 1.5f,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/FlamingGo.tscn"),
            dialogueName = "flaminggo",
            dialogueCount = 3,
		},
		/*
        new towerInfo { 
			name = "DiscoDuck",
			description = "Makes a nifty effect that cycles through different colors. And bragging rights.",
			flavor = "\"This here funky dude is ready to get his groove on!\"",
			cost = 1976,
			maxHealth = 10,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/Discoduck.tscn"),
            dialogueName = "discoduck",
            dialogueCount = 4,
		},
		*/
	};
}
