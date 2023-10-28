using System.Collections.Generic;
using Godot;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Scripts.Utilities;
using TalesFromTheTable.Services;
using TalesFromTheTable.Utilities;
using TalesFromTheTable.Utilities.Enums;

public partial class char_creation : Control
{
	private LineEdit nameLineEdit;
	private Button rollAbilitiesButton;
	public Adventurer adventurer;
	private AdventurerService adventurerService;
	private Label rollOneLabel, rollTwoLabel, rollThreeLabel, rollFourLabel, rollFiveLabel, rollSixLabel;
	private Timer buttonCooldownTimer;
	private int abilitiesReRolled = 0;

	private Button reRollButton1;
	private Button reRollButton2;
	private Button reRollButton3;
	private Button reRollButton4;
	private Button reRollButton5;
	private Button reRollButton6;

	private List<OptionButton> optionButtons;

	private PopupMenu popupMenu;

	private const int REROLL_COOLDOWN = 1;
	private const int REROLL_MAX = 2;

	public override void _Ready()
	{
		adventurerService = new AdventurerService(new Dice());
		adventurer = new Adventurer();
		nameLineEdit = GetNode<LineEdit>("AdventurerNameInput");
		rollAbilitiesButton = GetNode<Button>("RollAbilitiesButton");

		rollOneLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel1");
		rollTwoLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel2");
		rollThreeLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel3");
		rollFourLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel4");
		rollFiveLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel5");
		rollSixLabel = GetNode<Label>("GridContainerRolls/RollContainer/RollLabel6");

		reRollButton1 = GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton1");
		reRollButton2 = GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton2");
		reRollButton3 = GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton3");
		reRollButton4 = GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton4");
		reRollButton5 = GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton5");
		reRollButton6 = GetNode<Button>("GridContainerRolls/RollButtonContainer/ReRollButton6");

		buttonCooldownTimer = GetNode<Timer>("ButtonCooldownTimer");
		//popupMenu = GetNode<PopupMenu>("PopupPlaceholder/PopupMenu");

		optionButtons = new List<OptionButton>
		{
			GetNode<OptionButton>("GridContainerRolls/AttributesDropdownContainer/OptionButton0"), GetNode<OptionButton>("GridContainerRolls/AttributesDropdownContainer/OptionButton1"),
			GetNode<OptionButton>("GridContainerRolls/AttributesDropdownContainer/OptionButton2"), GetNode<OptionButton>("GridContainerRolls/AttributesDropdownContainer/OptionButton3"),
			GetNode<OptionButton>("GridContainerRolls/AttributesDropdownContainer/OptionButton4"), GetNode<OptionButton>("GridContainerRolls/AttributesDropdownContainer/OptionButton5")
		};

		ResetUI();

	}

	public override void _Process(double delta)
	{
	}

	//private void _on_start_create_button_pressed()
	//{
	//    string adventurerName = nameLineEdit.Text;
	//    //GD.Print($"Adventurer's Name Submitted: {adventurerName}");		
	//    adventurer = new Adventurer(adventurerName);
	//    //GD.Print(JsonSerializer.Serialize(todd));
	//    rollAbilitiesButton.Visible = true;


	//    //ReRollButtonTextChange("REROLL");
	//}

	private void _on_adventurer_name_input_text_changed(string new_text)
	{
		adventurer.Name = nameLineEdit.Text;

		if (string.IsNullOrWhiteSpace(new_text))
		{
			ResetUI();
		}
		else
		{
			GetNode<RichTextLabel>("RollNoteLabel").Visible = true;
			rollAbilitiesButton.Visible = true;
		}
	}

	private void _on_roll_abilities_button_pressed()
	{
		RollAttributesUI();

		GetNode<Button>("RollAbilitiesButton").Disabled = true;
		buttonCooldownTimer.Start(REROLL_COOLDOWN);

		var rolls = adventurerService.RollAttributes(adventurer);

		//GD.Print($"Here");
		foreach (var attributeRoll in rolls)
		{
			var roll = AbilityWithModifier(attributeRoll.Value);
			switch (attributeRoll.Key)
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
		GetNode<Label>("ReRollCount").Text = $"ReRolls left: {REROLL_MAX - abilitiesReRolled}";
		DisableEnableReRollButtonsWithTimer(REROLL_COOLDOWN, true);
	}

	private void _on_button_cooldown_timer_timeout()
	{
		GetNode<Button>("RollAbilitiesButton").Disabled = false;
		reRollButton1.Disabled = false;
		reRollButton2.Disabled = false;
		reRollButton3.Disabled = false;
		reRollButton4.Disabled = false;
		reRollButton5.Disabled = false;
		reRollButton6.Disabled = false;
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
			reRollButton1.Disabled = false;
			reRollButton2.Disabled = false;
			reRollButton3.Disabled = false;
			reRollButton4.Disabled = false;
			reRollButton5.Disabled = false;
			reRollButton6.Disabled = false;
		}
		else
		{
			reRollButton1.Disabled = true;
			reRollButton2.Disabled = true;
			reRollButton3.Disabled = true;
			reRollButton4.Disabled = true;
			reRollButton5.Disabled = true;
			reRollButton6.Disabled = true;
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

		if (abilitiesReRolled == REROLL_MAX)
		{
			GetNode<Container>("GridContainerRolls/RollButtonContainer").Visible = false;
			GetNode<Container>("GridContainerRolls/RollLabelContainer").Visible = false;
			GetNode<Container>("GridContainerRolls/AttributesDropdownContainer").Visible = true;
			GetNode<Label>("ReRollCount").Visible = false;
			//GetNode<Label>("AssignAbility").Visible = true;
			GetNode<Button>("ContinueButton").Visible = true;

			//DisableEnableReRollButtonsWithTimer(0);

		}
		else
		{
			DisableEnableReRollButtonsWithTimer(REROLL_COOLDOWN);
		}

		var reRoll = adventurerService.ReRollAbility(abilityRollNumber, adventurer);
		rollLabel.Text = AbilityWithModifier(reRoll);

		GetNode<Label>("ReRollCount").Text = $"ReRolls left: {REROLL_MAX - abilitiesReRolled}";

		return reRoll;
	}

	private string AbilityWithModifier(int abilityScore) =>
	$" {abilityScore}  ({(Rules.AbilityBonus(abilityScore) >= 0 ? "+" : "")}{Rules.AbilityBonus(abilityScore)})";

	//private void ReRollButtonTextChange(string change)
	//{
	//	reRollButton1.Text = change;
	//	reRollButton2.Text = change;
	//	reRollButton3.Text = change;
	//	reRollButton4.Text = change;
	//	reRollButton5.Text = change;
	//	reRollButton6.Text = change;
	//}

	private void _on_random_name_pressed()
	{
		nameLineEdit.Text = RandomFantasyName.GenerateFantasyName();
		_on_adventurer_name_input_text_changed(nameLineEdit.Text);
	}

	private void _on_option_button_0_item_selected(long index)
	{
		CheckAbilitiesAssigned(0, (int)index);
	}

	private void _on_option_button_1_item_selected(long index)
	{
		CheckAbilitiesAssigned(1, (int)index);
	}

	private void _on_option_button_2_item_selected(long index)
	{
		CheckAbilitiesAssigned(2, (int)index);
	}

	private void _on_option_button_3_item_selected(long index)
	{
		CheckAbilitiesAssigned(3, (int)index);
	}

	private void _on_option_button_4_item_selected(long index)
	{
		CheckAbilitiesAssigned(4, (int)index);
	}

	private void _on_option_button_5_item_selected(long index)
	{
		CheckAbilitiesAssigned(5, (int)index);
	}

	/// <summary>
	/// If the user picks (str) and there is another button that has already chosen (str) this will clear that dropdown
	/// </summary>
	/// <param name="buttonNumber"></param>
	/// <param name="abilityIndex"></param>
	private void CheckAbilitiesAssigned(int buttonNumber, int abilityIndex)
	{
		var allSet = true;

		//GD.Print($"IN - buttonNumber: {buttonNumber} abilityIndex: {abilityIndex}");
		foreach (var optionButton in optionButtons)
		{
			if (optionButton.GetSelectedId() == abilityIndex)
			{
				if (optionButton != optionButtons[buttonNumber])
				{
					optionButton.Select(-1);
				}
			}

			if (optionButton.GetSelectedId() == -1)
			{
				allSet = false;
			}
		}

		GetNode<Button>("ContinueButton").Visible = allSet;
	}

	private void _on_continue_button_pressed()
	{
		GetNode<Container>("RaceSavingThrowsContainer").Visible = true;
		GetNode<Button>("ContinueButton").Disabled = true;

		foreach (var optionButton in optionButtons)
		{
			optionButton.Disabled = true;
			var optionButtonIndex = optionButtons.IndexOf(optionButton);
			var attributeID = (AttributeEnum)optionButton.GetSelectedId();
			var value = adventurerService.AttributeRolls[ConvertIndexToRollsKey(optionButtonIndex)];
			GD.Print($"Option Value: {optionButton.Text} Attribute: {attributeID} Value: {value}");
			adventurer.SetAttribute(attributeID, adventurerService.AttributeRolls[ConvertIndexToRollsKey(optionButtonIndex)]);
		}

		SetAttributeLabels();
	}

	private void _on_race_options_item_selected(long index)
	{
		var skillContainer = GetNode<Container>("RaceSavingThrowsContainer/SkillContainer");
		var skillOne = GetNode<OptionButton>("RaceSavingThrowsContainer/SkillContainer/SkillOneOption");
		var skillTwo = GetNode<OptionButton>("RaceSavingThrowsContainer/SkillContainer/SkillTwoOption");
		skillOne.Disabled = false;
		skillTwo.Disabled = false;

		var raceOption = GetNode<OptionButton>("RaceSavingThrowsContainer/RaceOptions");
		skillContainer.Visible = true;
		var selectedID = raceOption.GetSelectedId();

		if (selectedID == 0)
		{
			skillContainer.Visible = false;
		}
		else if (selectedID == (int)RaceEnum.Human)
		{
			skillOne.Visible = true;
			//skillTwo.Visible = true;  -- they get to choose two abilities to increase and ONE skill - make its own container for this
			//adventurer.SetRace(RaceEnum.Human); -- wait to set this 
		}
		else if (selectedID == (int)RaceEnum.Dwarf || selectedID == (int)RaceEnum.Elf)
		{
			skillOne.Visible = true;
			skillTwo.Visible = false;
		}
		else if (selectedID == (int)RaceEnum.Halfling) // Halfing gets thief and sneak ... er something.
		{
			skillOne.Visible = true;
			skillTwo.Visible = true;
			skillOne.Select(1);
			skillTwo.Select(2);
			skillOne.Disabled = true;
			skillTwo.Disabled = true;
			//adventurer.SetRace(RaceEnum.Halfling);
		}
	}

	private void ResetUI()
	{
		GetNode<Container>("GridContainerRolls/RollButtonContainer").Visible = false;
		GetNode<Container>("GridContainerRolls/RollLabelContainer").Visible = false;
		GetNode<Container>("GridContainerRolls/AttributesDropdownContainer").Visible = false;
		GetNode<Container>("GridContainerRolls").Visible = false;
		GetNode<Label>("ReRollCount").Visible = false;
		GetNode<Label>("AssignAbility").Visible = false;
		GetNode<Button>("ContinueButton").Visible = false;
		GetNode<Button>("ContinueButton").Disabled = false;
		GetNode<Container>("RaceSavingThrowsContainer").Visible = false;
		GetNode<Button>("RollAbilitiesButton").Visible = false;
		GetNode<RichTextLabel>("RollNoteLabel").Visible = false;

		foreach (var optionButton in optionButtons)
		{
			optionButton.Disabled = false;
		}
	}

	private void RollAttributesUI()
	{
		rollAbilitiesButton.Visible = true;
		GetNode<RichTextLabel>("RollNoteLabel").Visible = true;
		GetNode<Container>("GridContainerRolls/RollButtonContainer").Visible = true;
		GetNode<Container>("GridContainerRolls/RollLabelContainer").Visible = true;
		GetNode<Container>("GridContainerRolls/AttributesDropdownContainer").Visible = false;
		GetNode<Container>("GridContainerRolls").Visible = true;
		GetNode<Label>("ReRollCount").Visible = true;
		GetNode<Label>("AssignAbility").Visible = false;
		GetNode<Button>("ContinueButton").Visible = false;
		GetNode<Button>("ContinueButton").Disabled = false;
		GetNode<Container>("RaceSavingThrowsContainer").Visible = false;

		foreach (var optionButton in optionButtons)
		{
			optionButton.Disabled = false;
		}
	}

	private string ConvertIndexToRollsKey(int index)
	{
		switch (index)
		{
			case 0:
				return "one";
			case 1:
				return "two";
			case 2:
				return "three";
			case 3:
				return "four";
			case 4:
				return "five";
			case 5:
				return "six";
			default:
				return "one";
		}
	}

	private void SetAttributeLabels()
	{
		var path = "RaceSavingThrowsContainer/FinalStatsGrid/AttributesGrid/Values/";
		GetNode<Label>($"{path}StrengthValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Strength]);
		GetNode<Label>($"{path}DexterityValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Dexterity]);
		GetNode<Label>($"{path}ConstitutionValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Constitution]);
		GetNode<Label>($"{path}IntelligenceValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Intelligence]);
		GetNode<Label>($"{path}WisdomValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Wisdom]);
		GetNode<Label>($"{path}CharismaValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Charisma]);
	}
}
