using Godot;
using System;

public class Zombie : KinematicBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AnimationPlayer animPlayer;
	private RayCast rayCast;

	private Player player;

	private const float MOVE_SPEED = 3.0f;
	private const float MELEE_RANGE = 1.5f;

	private bool dead = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		rayCast = GetNode<RayCast>("RayCast");
		animPlayer.Play("walk");
		AddToGroup("zombies");
	}

	 public override void _PhysicsProcess(float delta)
	 {
		 if (dead) return;
		 if (player == null) return;

		 var vecToPlayer = player.Translation - Translation;
		 vecToPlayer = vecToPlayer.Normalized();

		 // monster "attack" is just when it's within MELEE_RANGE of player
		 // when this raycast collides the attack is hitting the player
		 rayCast.CastTo = vecToPlayer * MELEE_RANGE;

		 // move towards the player
		 MoveAndCollide(vecToPlayer * MOVE_SPEED * delta);

		 if (rayCast.IsColliding())
		 {
			 var coll = rayCast.GetCollider();
			 if (coll != null && coll is Player)
			 {
				 coll.Call("Kill");
			 }
		 }
	 }

	 public void Kill()
	 {
		 dead = true;
		 GetNode<CollisionShape>("CollisionShape").Disabled = true;
		 animPlayer.Play("die");
	 }

	 public void SetPlayer(Player player)
	 {
		 this.player = player;
	 }
}
