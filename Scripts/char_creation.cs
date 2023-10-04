using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Services;
using TalesFromTheTable.Utilities;

public partial class char_creation : Control
{
	private LineEdit nameLineEdit;
	private Button rollAbilitiesButton;
	private Adventurer adventurer;
	private Label rollOneLabel, rollTwoLabel, rollThreeLabel, rollFourLabel, rollFiveLabel, rollSixLabel;

	public override void _Ready()
	{
		nameLineEdit = GetNode<LineEdit>("AdventurerNameInput");
		rollAbilitiesButton = GetNode<Button>("RollAbilitiesButton");
		//rollAbilitiesButton.Connect("pressed", this, "_on_rollabilities_pressed");
		Button submitButton = GetNode<Button>("StartCreateButton");
		//submitButton.Connect("pressed", this, "_on_StartCreateButton_pressed
		//
		rollOneLabel = GetNode<Label>("RollContainer/RollOneLabel");
		rollTwoLabel = GetNode<Label>("RollContainer/RollTwoLabel");
		rollThreeLabel = GetNode<Label>("RollContainer/RollThreeLabel");
		rollFourLabel = GetNode<Label>("RollContainer/RollFourLabel");
		rollFiveLabel = GetNode<Label>("RollContainer/RollFiveLabel");
		rollSixLabel = GetNode<Label>("RollContainer/RollSixLabel");
	}

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
		//GD.Print($"Adventurer's Name Submitted: {adventurerName}");		
		adventurer = new Adventurer(adventurerName);
		//GD.Print(JsonSerializer.Serialize(todd));
		rollAbilitiesButton.Visible = true;
		GetNode<Container>("RollContainer").Visible = true;
	}

	private void _on_roll_abilities_button_pressed()
	{
		var adventurerService = new AdventurerService(new Dice());
		var rolls = adventurerService.RollAbilities(adventurer);

		foreach (var abilityRoll in rolls)
		{ 		
			var modifierString = Rules.AbilityBonus(abilityRoll.Value) > 0 ? $"+{Rules.AbilityBonus(abilityRoll.Value)}" : $"{Rules.AbilityBonus(abilityRoll.Value)}";
			var roll = $" {abilityRoll.Value}  ( {modifierString} )";
			switch (abilityRoll.Key)
			{
				case "one":
					rollOneLabel.Text += roll;
					break;
				case "two":
					rollTwoLabel.Text += roll;
					break;
				case "three":
					rollThreeLabel.Text += roll;
					break;
				case "four":
					rollFourLabel.Text += roll;
					break;
				case "five":
					rollFiveLabel.Text += roll;
					break;
				case "six":
					rollSixLabel.Text += roll;
					break;
				default:
					break;
			}
		}

	}
}
