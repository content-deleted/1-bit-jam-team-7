using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hurtbox : Area3D
{
	[Signal]
	public delegate void OnDamageTakenEventHandler(int damage);

	public void TakeDamage(int amount)
	{
		EmitSignal(SignalName.OnDamageTaken, amount);
	}

    public float GetTotalPower () {
        var areas = GetOverlappingAreas().Where(x => x.IsInGroup("powerSource"));
        GD.Print("cout"+areas?.Count());
        var power = areas.Sum(x => (float)x.GetMeta("Power", 1f) * (0.5f + 1.0f / x.GlobalPosition.DistanceTo(GlobalPosition)));
        return power;
    }
}
