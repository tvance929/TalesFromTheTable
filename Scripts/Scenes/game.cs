using Godot;
using System;
using TalesFromTheTable.Scripts.SystemServices;

public partial class game : Control
{
    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowMessage()
	{
        var text = GameService.GetRoomMessage();
    }
}
