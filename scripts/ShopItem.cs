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
		DescriptionPanel.ShowPanel(GlobalPosition + new Vector2(0, 60), info.name, textBody(), false, " Buy ", BuyTower);
	}

	public void HideDescriptionPanel() {
		DescriptionPanel.HidePanel();
	}

	public void BuyTower() {
		GD.Print("This is where I'd create a tower _IF I HAD ONE_");
	}
	
	public string textBody () => $"Health: {info.maxHealth}\n{info.description}\n\n{info.flavor}";

	public struct towerInfo {
		public string name;
		public string description;
		public string flavor;
		public int cost;
		public int maxHealth;
	}

	public towerInfo[] towerTypes = {
		new towerInfo { 
			name = "Fool's Bird",
			description = "Cast's light about itself in a 1-space radius. Charges adjacent towers.",
			flavor = "\"I swear it moves when I'm not looking.\"",
			cost = 20,
			maxHealth = 20
		},
	};
}
