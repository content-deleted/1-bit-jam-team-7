using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ShopController : Panel
{
    Vector2 origin;

    float offscreenDistance = 100;

    private static int _gold;
    public static int gold {
        get => _gold;
        set {
            _gold = value;
            controller.goldLabel.Text = "Gold: "+_gold;
        }
    }

    public static ShopController controller;

    // starting gold
    public int startingGold = 60;

    private static bool _towerPlacing = false;
    public static bool towerPlacing {
        get => _towerPlacing;
        set {
           _towerPlacing = value;
           if(towerPlacementTest != null) {
             if(!value) {
                towerPlacementTest.Hide();
                //towerPlacementTest.GetNode<CollisionShape3D>("hitbox/CollisionShape3D").SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
             } else {
                //towerPlacementTest.GetNode<CollisionShape3D>("hitbox/CollisionShape3D").SetDeferred(CollisionShape3D.PropertyName.Disabled, false);
             }
           }
        }
    }

    public static ShopItem.towerInfo currentTower;

    public static Tower towerPlacementTest;

    public Label goldLabel;

	public override void _Ready()
	{
        if(controller != null && IsInstanceValid(controller)) {
			QueueFree();
			return;
		}
		controller = this;

        origin = GlobalPosition;
        GlobalPosition -= new Vector2(0, offscreenDistance);

        goldLabel = GetParent().GetNode("Info/gold") as Label;
        gold = startingGold;
	}
    
    float openness = 0;
    bool open = false;
    public override void _Process(double delta)
	{
        if(openness < 1.0 && open) {
            openness+=(float)delta;
            updateOpening();
            return;
        } else if(openness > 0 && !open) {
            openness-=(float)delta;
            updateOpening();
            // not sure why we need to do this probably UI input
            if(towerPlacing) towerPlacing = false;
            return;
        } else if(!open) {
            Hide();
        }

        if(towerPlacementTest != null && DefenseMode.mouseOnField) {
            towerPlacementTest.GlobalPosition = DefenseMode.mouseFieldPos;
        }
        if(towerPlacing) {
            if(DefenseMode.mouseOnField && !towerPlacementTest.hitbox.HasOverlappingAreas()) {
                towerPlacementTest.Show();
            } else {
                towerPlacementTest.Hide();
            }
        }
	}

	public override void _Input(InputEvent @event){
        if(@event is InputEventMouseButton inputEventMouse && inputEventMouse.Pressed) {
            if(towerPlacing && towerPlacementTest.Visible) {
                if(inputEventMouse.ButtonIndex == MouseButton.Left){
                    if(currentTower.cost <= gold) {
                        TowerController.PlaceTower(currentTower, towerPlacementTest.GlobalPosition);
                        gold-= currentTower.cost;
                    }
                }
                if(inputEventMouse.ButtonIndex == MouseButton.Right){
                    towerPlacing = false;
                }
            } else if(!towerPlacing)  {
                if(towerPlacementTest != null && inputEventMouse.ButtonIndex == MouseButton.Left) {
                    var areas = towerPlacementTest.hitbox.GetOverlappingAreas();
                    var tower = areas.Select(a => a.GetParentOrNull<Tower>()).Where(t => t != null).FirstOrDefault();
                    if(tower != null) {
                        CurrentlyViewingTower = tower;
                        var mousePos = GetViewport().GetMousePosition();
                        var screenSize = GetViewport().GetVisibleRect().Size;
                        var relative = mousePos / screenSize;
                        if(relative.Y > 0.7) {
                            mousePos.Y -= screenSize.Y / 10f;
                        }
                        // Todo: maybe print the power being providce?
                        var powerText = tower.isPowerSource ? "Power Source" : "Power: " + tower.GetTotalPower();
                        //only sell in day
                        if(DefenseMode.waveState) {
                            DescriptionPanel.ShowPanel(mousePos, tower.info.name, powerText, false);
                        } else { 
                            DescriptionPanel.ShowPanel(mousePos, tower.info.name, powerText, true, " Sell ", SellCurrentlyViewingTower);
                        }
                    }
                }
                if(inputEventMouse.ButtonIndex == MouseButton.Right){
                    DescriptionPanel.HidePanel();
                }
            }
        }
    }

    public Tower CurrentlyViewingTower;

    public void SellCurrentlyViewingTower() {
        gold += (int)Math.Floor(CurrentlyViewingTower.info.cost * 0.8f);
        CurrentlyViewingTower.QueueFree();
        DescriptionPanel.HidePanel();
    }

    public void updateOpening() {
        GlobalPosition = origin - new Vector2(0, (1.0f - openness) * offscreenDistance);
    }

    public static void Open() {
        controller.open = true;
        controller.Show();
    }

    public static void Close() {
        controller.open = false;
        towerPlacing = false;
    }
}
