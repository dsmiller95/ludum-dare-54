using Godot;

public partial class RotatingSpotlight : PointLight2D
{
	// Max pixel radius of rotation
	[Export] public float MaxDistance { get; set; } = 400;

	// Degrees per second
	[Export] public float MaxSpeed { get; set; } = 10;

	private float distance = 0;
	private float speed = 0;
	private float angle = 0;
	private Vector2 startPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		distance = GD.Randf() * MaxDistance;
		speed = GD.Randf() * MaxSpeed;
		angle = GD.Randf() * 360;
		startPosition = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		distance = Mathf.Clamp(distance + ((GD.Randf() * 10f - 5f) * (float)delta), 0f, MaxDistance);
		speed = Mathf.Clamp(speed + ((GD.Randf() * 10f - 5f) * (float)delta), 0f, MaxSpeed);
		angle += speed * (float)delta;
		Position = startPosition + (Vector2.FromAngle(Mathf.DegToRad(angle)) * distance);
	}
}
