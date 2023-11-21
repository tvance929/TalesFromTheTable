using Godot;
using System;
using TalesFromTheTable.SystemServices;

public partial class main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GameService.SkippingCreation)
		{
			GameService.StartGame(new TalesFromTheTable.Models.Entities.Adventurer());
			GetNode<Control>("Game").Show();
			GetNode<Control>("Create").Hide();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
