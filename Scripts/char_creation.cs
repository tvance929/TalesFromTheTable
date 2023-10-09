using Godot;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Services;
using TalesFromTheTable.Utilities;

public partial class char_creation : Control
{
	private LineEdit nameLineEdit;
	private Button rollAbilitiesButton;
	public Adventurer adventurer;
	private AdventurerService adventurerService;
	private Label rollOneLabel, rollTwoLabel, rollThreeLabel, rollFourLabel, rollFiveLabel, rollSixLabel;
	private Timer buttonCooldownTimer;
	private int abilitiesReRolled = 0;

	private const int REROLL_COOLDOWN = 2;

	public override void _Ready()
	{
		adventurerService = new AdventurerService(new Dice());
		nameLineEdit = GetNode<LineEdit>("AdventurerNameInput");
		rollAbilitiesButton = GetNode<Button>("RollAbilitiesButton");
		//rollAbilitiesButton.Connect("pressed", this, "_on_rollabilities_pressed");
		Button submitButton = GetNode<Button>("StartCreateButton");
		//submitButton.Connect("pressed", this, "_on_StartCreateButton_pressed
		//
		rollOneLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel1");
		rollTwoLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel2");
		rollThreeLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel3");
		rollFourLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel4");
		rollFiveLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel5");
		rollSixLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel6");

		buttonCooldownTimer = GetNode<Timer>("ButtonCooldownTimer");
	}

	public override void _Process(double delta)
	{
	}

	private void _on_start_create_button_pressed()
	{
		string adventurerName = nameLineEdit.Text;
		//GD.Print($"Adventurer's Name Submitted: {adventurerName}");		
		adventurer = new Adventurer(adventurerName);
		//GD.Print(JsonSerializer.Serialize(todd));
		rollAbilitiesButton.Visible = true;
		GetNode<RichTextLabel>("RollNoteLabel").Visible = true;
	}

	private void _on_roll_abilities_button_pressed()
	{
		GetNode<Container>("GridContainerRolls").Visible = true;
		GetNode<Label>("ReRollCount").Visible = true;

		GetNode<Button>("RollAbilitiesButton").Disabled = true;
		buttonCooldownTimer.Start(REROLL_COOLDOWN);

		var rolls = adventurerService.RollAbilities(adventurer);

		foreach (var abilityRoll in rolls)
		{
			var roll = AbilityWithModifier(abilityRoll.Value);
			switch (abilityRoll.Key)
			{
				case "one":
					rollOneLabel.Text = roll;
					break;
				case "two":
					rollTwoLabel.Text = roll;
					break;
				case "three":
					rollThreeLabel.Text = roll;
					break;
				case "four":
					rollFourLabel.Text = roll;
					break;
				case "five":
					rollFiveLabel.Text = roll;
					break;
				case "six":
					rollSixLabel.Text = roll;
					break;
				default:
					break;
			}
		}

		abilitiesReRolled = 0;
		DisableEnableReRollButtonsWithTimer(REROLL_COOLDOWN, true);
	}

	private void _on_button_cooldown_timer_timeout()
	{
		GetNode<Button>("RollAbilitiesButton").Disabled = false;
		GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton1").Disabled = false;
		GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton2").Disabled = false;
		GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton3").Disabled = false;
		GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton4").Disabled = false;
		GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton5").Disabled = false;
		GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton6").Disabled = false;
	}
	private void _on_re_roll_button_1_pressed()
	{
		ReRollAbility("one", rollOneLabel);
	}

	private void _on_re_roll_button_2_pressed()
	{
		ReRollAbility("two", rollTwoLabel);
	}


	private void _on_re_roll_button_3_pressed()
	{
		ReRollAbility("three", rollThreeLabel);
	}


	private void _on_re_roll_button_4_pressed()
	{
		ReRollAbility("four", rollFourLabel);
	}


	private void _on_re_roll_button_5_pressed()
	{
		ReRollAbility("five", rollFiveLabel);
	}


	private void _on_re_roll_button_6_pressed()
	{
		ReRollAbility("six", rollSixLabel);
	}

	/// <summary>
	/// Disable or Enable all the reroll buttons - defaults to Disable
	/// A Timeout Time of 0 will disable the buttons indefinitely
	/// </summary>
	/// <param name="timeoutTime"></param>
	/// <param name="enable"></param>
	private void DisableEnableReRollButtonsWithTimer(int timeoutTime, bool enable = false)
	{
		if (enable)
		{
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton1").Disabled = false;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton2").Disabled = false;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton3").Disabled = false;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton4").Disabled = false;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton5").Disabled = false;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton6").Disabled = false;
		}
		else
		{
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton1").Disabled = true;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton2").Disabled = true;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton3").Disabled = true;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton4").Disabled = true;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton5").Disabled = true;
			GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton6").Disabled = true;
		}

		if (timeoutTime > 0)
		{
			//this renables after the timeout
			buttonCooldownTimer.Start(timeoutTime);
		}
	}

	private int ReRollAbility(string abilityRollNumber, Label rollLabel)
	{
		abilitiesReRolled++;

		if (abilitiesReRolled == 2)
		{
			DisableEnableReRollButtonsWithTimer(0);
		}
		else
		{
			DisableEnableReRollButtonsWithTimer(REROLL_COOLDOWN);
		}

		var reRoll = adventurerService.ReRollAbility(abilityRollNumber, adventurer);
		rollLabel.Text = AbilityWithModifier(reRoll);

		GetNode<Label>("ReRollCount").Text = $"ReRolls left: {2 - abilitiesReRolled}";

		return reRoll;
	}

    private string AbilityWithModifier(int abilityScore) =>
    $" {abilityScore}  ({(Rules.AbilityBonus(abilityScore) >= 0 ? "+" : "")}{Rules.AbilityBonus(abilityScore)})";

}
