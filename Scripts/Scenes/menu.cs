using Godot;

public partial class menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("MarginContainer/VBoxContainer/NewButton").GrabFocus(); //for keyboard control
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_new_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}

	private void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
}
