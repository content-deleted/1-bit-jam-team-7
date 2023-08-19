using Godot;
using System;

public partial class DescriptionPanel : Panel
{
	public static DescriptionPanel singleton;

	private Label title;
	private Label body;
	private Button button;

	public override void _Ready()
	{
		if(singleton != null && IsInstanceValid(singleton)) {
			QueueFree();
			return;
		}
		singleton = this;

        title = GetNode<Label>("title");
		body = GetNode<Label>("body");
		button = GetNode<Button>("button");
	}

	public static void ShowPanel(Vector2 pos, string title, string body, bool enableButton, string buttonText = "", Action callback = null) {
		singleton.Show();
        singleton.GlobalPosition = pos;
		singleton.title.Text = title;
		singleton.body.Text = body;
		if(enableButton) {
			singleton.button.Show();
			singleton.button.Text = buttonText;
			singleton.buttonCallback = callback;
		} else {
			singleton.button.Hide();
		}
	}

	private Action buttonCallback;

	void OnButtonPressed() {
        Hide();
		if(buttonCallback != null) buttonCallback.Invoke();
	}

    public static void HidePanel() {
        singleton.Hide();
    }
}
