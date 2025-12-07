using Godot;
using System;

public partial class Player : Node2D
{
	[Export] public float MoveSpeed = 50f;
	[Export] public int GridSize = 32;

	private Vector2 targetPosition;
	private bool isMoving = false;

	private AnimatedSprite2D anim;

	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		targetPosition = GlobalPosition;
	}

	public override void _Process(double delta)
	{
		if (!isMoving)
			HandleInput();

		MoveTowardsTarget(delta);
	}

	private void HandleInput()
	{
		Vector2 input = Vector2.Zero;

		if (Input.IsActionPressed("ui_up"))
		{
			input = Vector2.Up;
			PlayAnimation("up");
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			input = Vector2.Down;
			PlayAnimation("down");
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			input = Vector2.Left;
			PlayAnimation("left");
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			input = Vector2.Right;
			PlayAnimation("right");
		}

		if (input != Vector2.Zero)
		{
			targetPosition += input * GridSize;
			isMoving = true;
		}
	}

	private void MoveTowardsTarget(double delta)
	{
		if (!isMoving)
			return;

		GlobalPosition = GlobalPosition.MoveToward(targetPosition, MoveSpeed * (float)delta);

		if (GlobalPosition == targetPosition)
		{
			isMoving = false;
			anim.Stop();  
		}
	}

	private void PlayAnimation(string animName)
	{
		if (!anim.IsPlaying() || anim.Animation != animName)
			anim.Play(animName);
	}
}
