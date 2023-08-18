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
        public PackedScene prefab;
        public string dialogueName;
        public int dialogueCount;
	}

	public towerInfo[] towerTypes = {
		new towerInfo { 
			name = "Fool's Bird",
			description = "Cast's light about itself in a 1-space radius. Charges adjacent towers.",
			flavor = "\"I swear it moves when I'm not looking.\"",
			cost = 20,
			maxHealth = 5,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/FoolsBird.tscn"),
            dialogueName = "foolsbird",
            dialogueCount = 1,
		},
        new towerInfo { 
			name = "Matapacos",
			description = "Fires a single shot at a fixed rate while powered",
			flavor = "\"Alicanto's best friend, and a miner's worst nightmare.\"",
			cost = 15,
			maxHealth = 10,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/Matapacos.tscn"),
            dialogueName = "matapacos",
            dialogueCount = 1,
		},
        new towerInfo { 
			name = "Flaming-Go",
			description = "Casts a beam of light forward that does constant damage while enemies are in contact with it.",
			flavor = "\"The threatened Chilean Flaming-Go has evolved an incendeary tactic to defend itself.\"",
			cost = 30,
			maxHealth = 10,
            prefab = ResourceLoader.Load<PackedScene>("res://scenes/towers/FlamingGo.tscn"),
            dialogueName = "flaminggo",
            dialogueCount = 1,
		},
	};
}
