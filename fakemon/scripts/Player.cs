using Godot;
using System;

public partial class Player : AnimatedSprite2D 
{
	[Export] public float MoveSpeed = 100f;
	[Export] public int GridSize = 32;

	private Vector2 targetPosition;
	private bool isMoving = false;
	private int last = -1;
	private double pressTime = 0.0;
	private const double tapThreshold = 0.050;
	private bool directionPressed = false;
	private Vector2 lastInputDir = Vector2.Zero;
	private Vector2 heldDir = Vector2.Zero;

	public override void _Ready()
	{		
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

		if (Input.IsActionPressed("ui_up"))      input = Vector2.Up;
		else if (Input.IsActionPressed("ui_down"))  input = Vector2.Down;
		else if (Input.IsActionPressed("ui_left"))  input = Vector2.Left;
		else if (Input.IsActionPressed("ui_right")) input = Vector2.Right;

		if (input == Vector2.Zero)
		{
			directionPressed = false;
			pressTime = 0;
			if (!isMoving) Stop();
			return;
		}

		if (!directionPressed || input != heldDir)
		{
			directionPressed = true;
			heldDir = input;
			pressTime = 0;

			if (input == Vector2.Up)    PlayAnimation("up");
			if (input == Vector2.Down)  PlayAnimation("down");
			if (input == Vector2.Left)  PlayAnimation("left");
			if (input == Vector2.Right) PlayAnimation("right");

			return; 

		pressTime += GetProcessDeltaTime();

		if (pressTime >= tapThreshold && !isMoving)
		{
			targetPosition += input * GridSize;
			isMoving = true;
		}
	}

	private void MoveTowardsTarget(double delta)
	{
		if (!isMoving) return;

		GlobalPosition = GlobalPosition.MoveToward(targetPosition, MoveSpeed * (float)delta);

		if (GlobalPosition == targetPosition)
		{
			isMoving = false;

			if (!Input.IsActionPressed("ui_up") &&
				!Input.IsActionPressed("ui_down") &&
				!Input.IsActionPressed("ui_left") &&
				!Input.IsActionPressed("ui_right"))
			{
				Pause();
			}
		}
	}

	private void PlayAnimation(string animName)
	{
		if (!IsPlaying() || Animation != animName)
			Play(animName);
	}
}
