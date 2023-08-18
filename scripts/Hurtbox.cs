using Godot;
using System;

public partial class Hurtbox : Area3D
{
	[Signal]
	public delegate void OnDamageTakenEventHandler(int damage);

	public void TakeDamage(int amount)
	{
		EmitSignal(SignalName.OnDamageTaken, amount);
	}
}
