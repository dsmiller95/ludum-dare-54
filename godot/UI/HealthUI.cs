using Godot;

public partial class HealthUI : Control
{
	private AnimatedSprite2D animation;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("HealthImage");
	}

	public override void _Process(double delta)
	{
		if (Health.HealthValue > 80)
		{
			animation.Animation = "full";
		}
		else if (Health.HealthValue > 60)
		{
			animation.Animation = "mostly-full";
		}
		else if (Health.HealthValue > 40)
		{
			animation.Animation = "half";
		}
		else if (Health.HealthValue > 20)
		{
			animation.Animation = "below-half";
		}
		else if (Health.HealthValue > 1)
		{
			animation.Animation = "low";
		}
		else
		{
			animation.Animation = "empty";
		}
	}
}
