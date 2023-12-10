using Godot;
using TalesFromTheTable.SystemServices;

public partial class menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("MarginContainer/VBoxContainer/NewButton").GrabFocus(); //for keyboard control
		// How to tween and fade an image
		//var image = GetNode<TextureRect>("tweener");
  //      var tween = CreateTween();
  //      tween.TweenProperty(image, "modulate", new Color(1, 1, 1, 0), 2);
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

	private void _on_load_button_pressed()
	{
		GameService.SkippingCreation = true;
		
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
}
