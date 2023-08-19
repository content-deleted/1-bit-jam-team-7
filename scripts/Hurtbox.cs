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
        var power = areas.Sum(x => (float)x.GetMeta("Power", 1f) * (0.5f + Mathf.Clamp(1 / x.GlobalPosition.DistanceTo(GlobalPosition), 0, 1)));
        return power;
    }

    public bool targetable = true;
}
