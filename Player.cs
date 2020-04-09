using Godot;
using System;

public class Player : KinematicBody
{
	private const float MOVE_SPEED = 4.0f;
	private const float MOUSE_SENSITIVITY = 0.5f;
	
	private AnimationPlayer animPlayer;
	private RayCast rayCast;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		rayCast = GetNode<RayCast>("RayCast");
		Input.SetMouseMode(Input.MouseMode.Captured);
		GetTree().CallGroup("zombies", "SetPlayer", this);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			Vector3 newRotation = new Vector3(RotationDegrees);
			newRotation.y -= MOUSE_SENSITIVITY * mouseMotion.Relative.x;
			RotationDegrees = newRotation;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("exit"))
		{
			GetTree().Quit(0);
		}
		if (Input.IsActionPressed("restart"))
		{
			Kill();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		var moveVec = new Vector3();
		if (Input.IsActionPressed("move_forwards"))
		{
			moveVec.z -= 1;
		}
		if (Input.IsActionPressed("move_backwards"))
		{
			moveVec.z += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			moveVec.x -= 1;
		}
		if (Input.IsActionPressed("move_right"))
		{
			moveVec.x += 1;
		}
		moveVec = moveVec.Normalized(); // prevents faster diagonal movement
		moveVec = moveVec.Rotated(new Vector3(0.0f, 1.0f, 0.0f), Rotation.y);
		MoveAndCollide(moveVec * MOVE_SPEED * delta);

		if (Input.IsActionPressed("shoot") && !animPlayer.IsPlaying())
		{
			animPlayer.Play("shoot");
			// shoot the bullet as a hitscan ray
			var coll = rayCast.GetCollider();
			if (rayCast.IsColliding() && coll.HasMethod("kill"))
			{
				coll.Call("kill");
			}
		}
	}

	/// Represent player being killed or some similar activity that ends the game
	public void Kill()
	{
		GetTree().ReloadCurrentScene();
	}
}
