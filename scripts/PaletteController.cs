using Godot;
using System;

public partial class PaletteController : ColorRect
{
	public static PaletteController controller;
	public override void _Ready()
	{
		// Singleton
		if(controller != null && IsInstanceValid(controller)) {
			QueueFree();
			return;
		}
		controller = this;

		lerpPos = totalLerpTime;
		
		Color a = (Color)(Material as ShaderMaterial).GetShaderParameter("primaryColor");
		Color b = (Color)(Material as ShaderMaterial).GetShaderParameter("secondaryColor");
		current = new Pallete(a,b);
	}

	public override void _Process(double delta)
	{
		if(lerpPos < totalLerpTime) {
			lerpPos++;

			current = previous.Lerp(next, (float)lerpPos / totalLerpTime);

			// set the palette in the shader
			setShaderPalette(current);
		}
	}

	public void setShaderPalette(Pallete p) {
		(Material as ShaderMaterial).SetShaderParameter("primaryColor", p.primary);
		(Material as ShaderMaterial).SetShaderParameter("secondaryColor", p.secondary);
	}

	int lerpPos = 0;
	[Export]
	int totalLerpTime = 30; // total time to lerp between color in frames

	public struct Pallete {
		public Color primary;
		public Color secondary;
		public Pallete(Color a, Color b) {
			primary = a; secondary = b; 
		}

		public Pallete Lerp(Pallete to, float pos) {
			return new Pallete(primary.Lerp(to.primary, pos), secondary.Lerp(to.secondary, pos));
		}
	}

	Pallete next;

	public Pallete current;

	Pallete previous;


	public static void SetNewPallete(Color a, Color b) {
		controller.previous = controller.current;
		controller.next = new Pallete(a, b);
		controller.lerpPos = 0;
	}
}
