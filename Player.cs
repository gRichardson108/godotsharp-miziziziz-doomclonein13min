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

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
