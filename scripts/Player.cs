using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Yarn;
using Yarn.GodotYarn;

public partial class Player : Node3D
{
	public float playerSpeed = 0;
    [Export]
	public double stamina = 3;

    [Export]
	public double staminaCoolDown = 5;
    [Export]
	public double staminaCoolDownTimer = 0;

    [Export]
	public float playerDefaultSpeed = 2;
	public Vector3 playerDirection = new Vector3(0, 0, 0);

    [Export]
    public float repairTime = 0.1f; // time to add one health

	public AnimatedSprite3D playerSprite;
	public Camera mainCamera;
    private Area3D towerInteractionRange;
    private Sprite3D interactionSprite;
    private DialogueRunner runner;

    private CompressedTexture2D dayInteractSprite;
    private CompressedTexture2D nightInteractSprite;

	public override void _Ready()
	{
		playerSprite = GetNode<AnimatedSprite3D>("PlayerSprite");
        playerSprite.Play("idle");
		mainCamera = GetNode<Camera>("//root/DefenseMode/MainCamera");
        towerInteractionRange = GetNode<Area3D>("TowerInteractionRange");
        interactionSprite =  GetNode<Sprite3D>("InteractionSprite");
        runner = GetNode<DialogueRunner>("//root/DefenseMode/ViewportOverlay/HUD/DialogueRunner");

        dayInteractSprite = ResourceLoader.Load<CompressedTexture2D>("res://gfx/QuestionMark.png");
        nightInteractSprite = ResourceLoader.Load<CompressedTexture2D>("res://gfx/shit_wrench.png");
	
        runner.onDialogueComplete += () => runner.Hide();
        runner.onDialogueStart+= () => runner.Show();
    }

	public override void _Process(double delta)
	{
        HandleTowerInteractions(delta);
	}


    private double repairTimer = 0f;

    private bool isRepairing = false;

    private Tower repairTarget = null;

    public static int justStartedTalking =0;
    public void HandleTowerInteractions(double delta) {
        if(justStartedTalking > 0) {
            justStartedTalking--;
        }
        if(isRepairing) {
            if(DefenseMode.playerMoved) {
                isRepairing = false;
                repairTimer = 0;
            }

            // flip back and forth until we have a better animation
            interactionSprite.FlipH = (int)(repairTimer * 100) % 2 == 0;

            repairTimer += delta;
            if(repairTimer > repairTime) {
                repairTimer = 0;
                repairTarget.currentHealth++;
                if(repairTarget.currentHealth >= repairTarget.maxHealth) {
                    isRepairing = false;
                }
            }
        } else {
            var tower = towerInteractionRange.GetOverlappingAreas().FirstOrDefault()?.GetParent() as Tower;
            if (tower != null) {
                if(DefenseMode.waveState) {
                    if(tower.currentHealth < tower.maxHealth) {
                        ShowInteractionSprite(nightInteractSprite);
                        if(Input.IsActionJustPressed("PlayerInteract") ) {
                            isRepairing = true;
                            repairTarget = tower;
                            DefenseMode.playerMoved = false;
                        }
                    } else {
                        interactionSprite.Hide();
                    }
                } else {
                    ShowInteractionSprite(dayInteractSprite);
                    if(!runner.IsDialogueRunning && Input.IsActionJustPressed("PlayerInteract") ) {
                        justStartedTalking = 10;
                        runner.StartDialogue(tower.info.dialogueName + "_" + (GD.Randi() % tower.info.dialogueCount));
                    }
                }
            } else {
                if(runner.IsDialogueRunning) {
                    runner.Stop();
                }
                interactionSprite.Hide();
            }
        }
    }

    public void ShowInteractionSprite(CompressedTexture2D s) {
        interactionSprite.Show();
        interactionSprite.Texture = s;
    }

	public Vector3 CameraRelativeMove(Vector3 movement){

		if (mainCamera.cameraOrientation == 1){

			return new Vector3(movement.Z, movement.Y, -movement.X);

		} else if (mainCamera.cameraOrientation == 2){

			return new Vector3(-movement.X, movement.Y, -movement.Z);

		} else if (mainCamera.cameraOrientation == 3){

			return new Vector3(-movement.Z, movement.Y, movement.X);

		} else {
			
			return movement;


		}

	}

	
}
