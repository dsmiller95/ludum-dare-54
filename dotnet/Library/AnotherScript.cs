//using Godot; Godot is not available in a class library project

public partial class AnotherScript //: Node
{
	public double Time { get; set; }
	public void _Ready()
	{
		Time = 0;
	}

	public void _Process(double delta)
	{
		Time += delta;
	}
}
