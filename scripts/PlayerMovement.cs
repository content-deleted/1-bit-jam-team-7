using Godot;
using System;
using Yarn.GodotYarn;


public partial class PlayerMovement : Area2D
{
	[Export]
	public int Speed { get; set; } = 10; // How fast the player will move (pixels/sec).
	
	private AnimatedSprite2D animatedSprite2D;

	public static PlayerMovement player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Singleton
		if(player != null) {
			QueueFree();
			return;
		}
		player = this;

		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
		animatedSprite2D.Animation = "default";
	}

	// Custom start used for positioning I guess
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = getMovementVector();

		velocity = HandleDash(velocity);

		if(!dashing) {
			if (velocity.Length() > 0)
			{
				velocity = velocity.Normalized() * Speed;
			}

			if (velocity.X < 0)
			{
				animatedSprite2D.FlipH = true;
			}
			else
			{
				animatedSprite2D.FlipH = false;
			}
		}
	
		Position += velocity * (float)delta;
	}

	public Vector2 getMovementVector() {
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("ui_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("ui_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("ui_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("ui_up"))
		{
			velocity.Y -= 1;
		}

		return velocity;
	}

	#region Collision
	[Signal]
	public delegate void HitEventHandler();

	private void OnBodyEntered(PhysicsBody2D body)
	{
		EmitSignal(SignalName.Hit);

		GD.Print("Hit!");
        // why does this not work?
	}

    
    private void _on_player_body_entered(Node2D body)
    {
        // Probably more stuff goes here
        body.QueueFree();
    }


	#endregion

	#region Dash

	// After images?
	// public static int totalAfterImages = 8;    
	// private List<SpriteRenderer> PlayerAfterImages = new List<SpriteRenderer>();
	// public GameObject afterImagePrefab;
	// public void populateAfterImage() {
	//     foreach(SpriteRenderer s in PlayerAfterImages) {
	//         GameObject.Destroy( s.gameObject );
	//     }

	//     PlayerAfterImages.Clear();

	//     for(int i = 0; i < totalAfterImages; i++) {
	//         PlayerAfterImages.Add(GameObject.Instantiate(afterImagePrefab).GetComponent<SpriteRenderer>() );
	//         PlayerAfterImages[i].gameObject.SetActive(false);
	//     }
	// }

	// private int curAfterImage = 0;
	// public void updateAfterImages() {
	//     PlayerAfterImages[curAfterImage].gameObject.SetActive(true);
	//     PlayerAfterImages[curAfterImage].sprite = sprite.sprite;
	//     PlayerAfterImages[curAfterImage].flipX = sprite.flipX;
	//     PlayerAfterImages[curAfterImage].gameObject.transform.position = transform.position;
	//     curAfterImage=(curAfterImage+1)%totalAfterImages;
	// }

	private Vector2 cacheVel;

	public static bool dashEnabled = false;
	
	public bool _dashing = false;
	public bool dashing {
		get => _dashing;
		set {
			_dashing = value;
			animatedSprite2D.Animation = value ? "dash" : "default";
		}
	}
	[Export]
	public float dashSpeed = 2;
	public int framesDashing = 0; // current frames in dash
	[Export]
	public int totalDashFrames = 10; // frames a dash will last
	[Export]
	public int extraDashIFrames = 5;
	[Export]
	public float sweetSpotMin = 10; // If greater than this we can dash again

	Vector2 DashDir = Vector2.Zero;

	public Vector2 HandleDash(Vector2 velocity) {
		if(Input.IsActionJustPressed("ui_select")) {
		   if(CanDash()) {
				//Begin Dash
				dashing = true;
	
				framesDashing = 0;

				if(velocity != Vector2.Zero) {
					DashDir = velocity.Normalized();
				} else {
					DashDir = Vector2.Right;
				}
				playDashSFX();
		   }
		}

		// Handle already dashing 
		if(dashing) {
			if(rateAdjustedFrames(framesDashing) <= totalDashFrames) {
				velocity = DashDir * dashSpeed;

				//updateAfterImages();
			} else {
				EndDash();
			}
		}

		framesDashing++;
		return velocity;
	}

	public void EndDash() {
		dashing = false;
	}

	public bool CanDash() {
		return dashEnabled && (!dashing || (dashing && framesDashing > sweetSpotMin));
	}

	public void playDashSFX() {
		// play
	}

	public int rateAdjustedFrames(int frames) {
		return frames;
	}

	#endregion
}
