using Godot;
using System;

public partial class ShopController : Panel
{
    Vector2 origin;

    float offscreenDistance = 100;

    public static ShopController controller;
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
        if(openness < 100 && open) {
            openness+=2;
            updateOpening();
        } else if(openness > 0 && !open) {
            openness-=2;
            updateOpening();
        } else if(!open) {
            Hide();
        }
	}

    public void updateOpening() {
        GlobalPosition = origin - new Vector2(0, (1 - (openness / 100.0f)) * offscreenDistance);
    }

    public static void Open() {
        controller.open = true;
        controller.Show();
    }

    public static void Close() {
        controller.open = false;
        controller.Show();
    }
}
