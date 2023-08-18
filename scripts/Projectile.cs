using Godot;
using System;

public partial class Projectile : Sprite3D
{
	public Action callback;
	public Vector3 origin;
	public Node3D target;

	public float speed;
	public float progress;
	public override void _Process(double delta)
	{
		if(!Visible) return;

        if(!Node.IsInstanceValid(target) || target.IsQueuedForDeletion()) {
            ReturnToPool();
        }

		if(progress >= 1.0) {
			callback?.Invoke();
			ReturnToPool();
		}

		progress += speed * (float)(delta);
		GlobalPosition = origin.Lerp(target.GlobalPosition, progress);
	}

	public void ReturnToPool() {
		ProjectilePool.inactiveProjectiles.Enqueue(this);
		Hide();
	}
}
