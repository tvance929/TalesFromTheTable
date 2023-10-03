using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using TalesFromTheTable.Entities;

public partial class char_creation : Node2D
{
	private LineEdit nameLineEdit;

	public override void _Ready()
	{
		nameLineEdit = GetNode<LineEdit>("Control/AdventurerNameInput");
		if (nameLineEdit != null)
		{
			GD.Print($"nameLineEdit is not null");
		}
		else
		{
			GD.Print("Ya its null");
		}

		Button submitButton = GetNode<Button>("Control/StartCreateButton");

		//submitButton.Connect("pressed", this, "_on_StartCreateButton_pressed");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//private void _on_line_edit_text_changed(string new_text)
	//{
	//	GD.Print($"Text changed : {new_text}");
	//}

	private void _on_start_create_button_pressed()
	{
		string adventurerName = nameLineEdit.Text;
		GD.Print($"Adventurer's Name Submitted: {adventurerName}");
		
		var todd = new Adventurer("Toddicus");
		
		GD.Print(JsonSerializer.Serialize(todd));
	}
}
