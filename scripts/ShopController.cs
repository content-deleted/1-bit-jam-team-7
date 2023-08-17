using Godot;
using System;

public partial class ShopController : Panel
{
    Vector2 origin;

    float offscreenDistance = 100;

    public static ShopController controller;

    private static bool _towerPlacing = false;
    public static bool towerPlacing {
        get => _towerPlacing;
        set {
           _towerPlacing = value;
           if(!value && towerPlacementTest != null) {
             towerPlacementTest.Hide();
           }
        }
    }

    public static ShopItem.towerInfo currentTower;

    public static Tower towerPlacementTest;

	public override void _Ready()
	{
        if(controller != null) {
			QueueFree();
			return;
		}
		controller = this;

        origin = GlobalPosition;
        GlobalPosition -= new Vector2(0, offscreenDistance);
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

         if(towerPlacing) {
            if(DefenseMode.mouseOnField) {
                towerPlacementTest.GlobalPosition = DefenseMode.mouseFieldPos;
                if(!towerPlacementTest.hitbox.HasOverlappingAreas()) {
                    towerPlacementTest.Show();
                } else {
                    towerPlacementTest.Hide();
                }
            } else {
                towerPlacementTest.Hide();
            }
         }
	}

	public override void _Input(InputEvent @event){
        if(towerPlacing && towerPlacementTest.Visible) {
            if(@event is InputEventMouseButton inputEventMouse && inputEventMouse.Pressed) {
                if(inputEventMouse.ButtonIndex == MouseButton.Left){
                    TowerController.TryPlaceTower(currentTower, towerPlacementTest.GlobalPosition);
                }
                if(inputEventMouse.ButtonIndex == MouseButton.Right){
                    towerPlacing = false;
                }
            }
        }
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
