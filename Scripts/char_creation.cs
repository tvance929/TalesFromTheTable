using System.Collections.Generic;
using System.Linq;
using Godot;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Scripts.SystemServices;
using TalesFromTheTable.Scripts.Utilities;
using TalesFromTheTable.Services;
using TalesFromTheTable.Utilities;
using TalesFromTheTable.Utilities.Enums;

public partial class char_creation : Control
{
	private LineEdit nameLineEdit;
	private Button rollAbilitiesButton;
	public Adventurer adventurer = new Adventurer();
	private AdventurerService adventurerService = new AdventurerService(new Dice());
	private SaveService saveService = new SaveService();
	private Label rollOneLabel, rollTwoLabel, rollThreeLabel, rollFourLabel, rollFiveLabel, rollSixLabel;
	private Timer buttonCooldownTimer;
	private int abilitiesReRolled = 0;

	private Button reRollButton1;
	private Button reRollButton2;
	private Button reRollButton3;
	private Button reRollButton4;
	private Button reRollButton5;
	private Button reRollButton6;

	private List<OptionButton> assignAttributeOptions;

	//Race and Background Options
	private OptionButton raceOption;
	private OptionButton humanAttrOne;
	private OptionButton humanAttrTwo;
	private OptionButton skillOne;
	private OptionButton skillTwo;
	private OptionButton backgroundOption;
	private Container skillContainer;
	private Container attrContainer;
	private Button finalSaveButton;

	private const double REROLL_COOLDOWN = 0.5;
	private const int REROLL_MAX = 2;
	private const int DEFAULT_OPTIONS_ID = 99;
	private const int DEFAULT_OPTIONS_INDEX = 0;

	public override void _Ready()
	{	
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

		raceOption = GetNode<OptionButton>("RaceSavingThrowsContainer/RaceOptions");
		humanAttrOne = GetNode<OptionButton>("RaceSavingThrowsContainer/AttributeBonusContainer/AttributeOneOption");
		humanAttrTwo = GetNode<OptionButton>("RaceSavingThrowsContainer/AttributeBonusContainer/AttributeTwoOption");
		skillOne = GetNode<OptionButton>("RaceSavingThrowsContainer/SkillContainer/SkillOneOption");
		skillTwo = GetNode<OptionButton>("RaceSavingThrowsContainer/SkillContainer/SkillTwoOption");
		backgroundOption = GetNode<OptionButton>("RaceSavingThrowsContainer/BackgroundContainer/BackgroundOption");
		skillContainer = GetNode<Container>("RaceSavingThrowsContainer/SkillContainer");
		attrContainer = GetNode<Container>("RaceSavingThrowsContainer/AttributeBonusContainer");

		finalSaveButton = GetNode<Button>("RaceSavingThrowsContainer/FinalSaveContainer/FinalSaveButton");

		buttonCooldownTimer = GetNode<Timer>("ButtonCooldownTimer");
		//popupMenu = GetNode<PopupMenu>("PopupPlaceholder/PopupMenu");

		assignAttributeOptions = new List<OptionButton>
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

	#region Name And Attribute rolls
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
		adventurer.ResetAttributes();
		ResetUI();
		RollAttributesUI();

		GetNode<Button>("RollAbilitiesButton").Disabled = true;
		buttonCooldownTimer.Start(REROLL_COOLDOWN);

		var rolls = adventurerService.RollAttributes(adventurer);

		foreach (var attributeRoll in rolls)
		{
			var roll = AbilityWithModifier(attributeRoll.Value);
			switch (attributeRoll.Key)
			{
				case "one":
					rollOneLabel.Text = roll;
					if (attributeRoll.Value == 18) reRollButton1.Visible = false;
					break;
				case "two":
					rollTwoLabel.Text = roll;
					if (attributeRoll.Value == 18) reRollButton2.Visible = false;
					break;
				case "three":
					rollThreeLabel.Text = roll;
					//if (attributeRoll.Value == 18) reRollButton3.Disabled = true; //this maybe should happen... the visible things wont work
					break;
				case "four":
					rollFourLabel.Text = roll;
					if (attributeRoll.Value == 18) reRollButton4.Visible = false;
					break;
				case "five":
					rollFiveLabel.Text = roll;
					if (attributeRoll.Value == 18) reRollButton5.Visible = false;
					break;
				case "six":
					rollSixLabel.Text = roll;
					if (attributeRoll.Value == 18) reRollButton6.Visible = false;
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
	private void DisableEnableReRollButtonsWithTimer(double timeoutTime, bool enable = false)
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
	$" {abilityScore}  ({(Rules.AttributeBonus(abilityScore) >= 0 ? "+" : "")}{Rules.AttributeBonus(abilityScore)})";

	private void _on_random_name_pressed()
	{
		nameLineEdit.Text = RandomFantasyName.GenerateFantasyName();
		_on_adventurer_name_input_text_changed(nameLineEdit.Text);
	}

	private void _on_continue_button_pressed()
	{

		GetNode<Container>("RaceSavingThrowsContainer").Visible = true;
		GetNode<Button>("ContinueButton").Disabled = true;

		//Set each attribute to the selected value
		foreach (var optionButton in assignAttributeOptions)
		{
			optionButton.Disabled = true;
			var optionButtonIndex = assignAttributeOptions.IndexOf(optionButton);
			var attributeID = (AttributeEnum)optionButton.GetSelectedId();
			var value = adventurerService.AttributeRolls[ConvertIndexToRollsKey(optionButtonIndex)];
			adventurer.SetAttribute(attributeID, adventurerService.AttributeRolls[ConvertIndexToRollsKey(optionButtonIndex)]);
		}

		SetAttributeAndSavingThrowLabels();
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

		reRollButton1.Visible = true;
		reRollButton2.Visible = true;
		reRollButton3.Visible = true;
		reRollButton4.Visible = true;
		reRollButton5.Visible = true;
		reRollButton6.Visible = true;               //DO THIS FOR REROLLS too that add up to 18 - remove the button

		var optionIndex = 0;
		foreach (var optionButton in assignAttributeOptions)
		{
			optionButton.Disabled = false;
			optionButton.Select(optionIndex);
			optionIndex++;
		}

		skillOne.Disabled = true;
		skillTwo.Disabled = true;
		backgroundOption.Disabled = true;
		raceOption.Select(DEFAULT_OPTIONS_INDEX);
		skillOne.Select(DEFAULT_OPTIONS_INDEX);
		skillTwo.Select(DEFAULT_OPTIONS_INDEX);
		humanAttrOne.Select(DEFAULT_OPTIONS_INDEX);
		humanAttrTwo.Select(DEFAULT_OPTIONS_INDEX);
		backgroundOption.Select(DEFAULT_OPTIONS_INDEX);
		finalSaveButton.Disabled = true;
		attrContainer.Visible = false;
		skillContainer.Visible = false;
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

		foreach (var optionButton in assignAttributeOptions)
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

	private void SetAttributeAndSavingThrowLabels()
	{
		var attributePath = "RaceSavingThrowsContainer/FinalStatsGrid/AttributesGrid/Values/";
		var savingThrowPath = "RaceSavingThrowsContainer/FinalStatsGrid/SavingThrowGrid/Values/";
		GetNode<Label>($"{attributePath}StrengthValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Strength]);
		GetNode<Label>($"{attributePath}DexterityValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Dexterity]);
		GetNode<Label>($"{attributePath}ConstitutionValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Constitution]);
		GetNode<Label>($"{attributePath}IntelligenceValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Intelligence]);
		GetNode<Label>($"{attributePath}WisdomValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Wisdom]);
		GetNode<Label>($"{attributePath}CharismaValue").Text = AbilityWithModifier(adventurer.Attributes[AttributeEnum.Charisma]);
		GetNode<Label>($"{savingThrowPath}PoisonValue").Text = adventurer.SavingThrows.PoisonOrDeathRay.ToString();
		GetNode<Label>($"{savingThrowPath}PetrificationValue").Text = adventurer.SavingThrows.Petrification.ToString();
		GetNode<Label>($"{savingThrowPath}WandsValue").Text = adventurer.SavingThrows.MagicWand.ToString();
		GetNode<Label>($"{savingThrowPath}DragonBreathValue").Text = adventurer.SavingThrows.DragonBreath.ToString();
		GetNode<Label>($"{savingThrowPath}SpellsStavesValue").Text = adventurer.SavingThrows.SpellsOrMagicStaff.ToString();
	}

	private void _on_option_button_0_item_selected(long index)
	{
		CheckAttributesAssigned(0, (int)index);
	}

	private void _on_option_button_1_item_selected(long index)
	{
		CheckAttributesAssigned(1, (int)index);
	}

	private void _on_option_button_2_item_selected(long index)
	{
		CheckAttributesAssigned(2, (int)index);
	}

	private void _on_option_button_3_item_selected(long index)
	{
		CheckAttributesAssigned(3, (int)index);
	}

	private void _on_option_button_4_item_selected(long index)
	{
		CheckAttributesAssigned(4, (int)index);
	}

	private void _on_option_button_5_item_selected(long index)
	{
		CheckAttributesAssigned(5, (int)index);
	}

	/// <summary>
	/// If the user picks (str) and there is another button that has already chosen (str) this will clear that dropdown
	/// </summary>
	/// <param name="buttonNumber"></param>
	/// <param name="abilityIndex"></param>
	private void CheckAttributesAssigned(int buttonNumber, int abilityIndex)
	{
		var allSet = true;

		//GD.Print($"IN - buttonNumber: {buttonNumber} abilityIndex: {abilityIndex}");
		foreach (var optionButton in assignAttributeOptions)
		{
			if (optionButton.GetSelectedId() == abilityIndex)
			{
				if (optionButton != assignAttributeOptions[buttonNumber])
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

	#endregion

	#region Race And Saving Throws 
	private void _on_race_options_item_selected(long index)
	{
		//resetting everything
		skillOne.Disabled = false;
		skillTwo.Disabled = false;
		skillOne.Select(DEFAULT_OPTIONS_INDEX);
		skillTwo.Select(DEFAULT_OPTIONS_INDEX);
		humanAttrOne.Select(DEFAULT_OPTIONS_INDEX);
		humanAttrTwo.Select(DEFAULT_OPTIONS_INDEX);
		backgroundOption.Select(DEFAULT_OPTIONS_INDEX);
		finalSaveButton.Disabled = true;
		attrContainer.Visible = false;

		skillContainer.Visible = true;
		var selectedID = raceOption.GetSelectedId();
		var race = (RaceEnum)selectedID;

		adventurer.SetRace(race);

		if (selectedID == 4) //random default number that is NOT a race
		{
			skillContainer.Visible = false;
		}
		else if (race == RaceEnum.Human)
		{
			skillOne.Visible = true;
			skillTwo.Visible = false;
			attrContainer.Visible = true;
			backgroundOption.Disabled = true;
		}
		else if (selectedID == (int)RaceEnum.Dwarf || selectedID == (int)RaceEnum.Elf)
		{
			skillContainer.Visible = false;
			skillOne.Disabled = true;
			skillTwo.Disabled = true;
			backgroundOption.Disabled = false;
		}
		else if (selectedID == (int)RaceEnum.Halfling)
		{
			skillOne.Visible = true;
			skillTwo.Visible = true;
			skillOne.Select(1);
			skillTwo.Select(2);
			skillOne.Disabled = true;
			skillTwo.Disabled = true;
			backgroundOption.Disabled = false;
		}
		SetAttributeAndSavingThrowLabels();
	}

	private void _on_race_options_selected(long index)
	{
		//Check the race chosen and if all the options for that race have a selection, if so ENABLE the Background radio. 
		var selectedRace = raceOption.GetSelectedId();

		if (selectedRace == (int)RaceEnum.Human)
		{
			if (humanAttrOne.GetSelectedId() != DEFAULT_OPTIONS_ID && humanAttrTwo.GetSelectedId() != DEFAULT_OPTIONS_ID && skillOne.GetSelectedId() != DEFAULT_OPTIONS_ID)
			{
				adventurer.AttributeAddBonus((AttributeEnum)humanAttrOne.GetSelectedId(), 1);
				adventurer.AttributeAddBonus((AttributeEnum)humanAttrTwo.GetSelectedId(), 1);
				adventurer.AddSkill(adventurerService.skills.Where(s => s.Name == skillOne.GetItemText(skillOne.GetSelectedId())).FirstOrDefault());
				backgroundOption.Disabled = false;
				SetAttributeAndSavingThrowLabels();
			}
		}
		// Dont need to do anything for other races and there is nothing to choose
	}

	private void _on_attribute_one_option_focus_entered()
	{
		//Set all back to enabled
		for (var i = 1; i < 7; i++)
		{
			humanAttrOne.SetItemDisabled(i, false);
		}

		if (humanAttrTwo.GetSelectedId() != DEFAULT_OPTIONS_ID)
		{
			humanAttrOne.SetItemDisabled(humanAttrOne.GetItemIndex(humanAttrTwo.GetSelectedId()), true);
		}
	}

	private void _on_attribute_two_option_focus_entered()
	{
		//Set all back to enabled
		for (var i = 1; i < 7; i++)
		{
			humanAttrTwo.SetItemDisabled(i, false);
		}

		if (humanAttrOne.GetSelectedId() != DEFAULT_OPTIONS_ID)
		{
			humanAttrTwo.SetItemDisabled(humanAttrTwo.GetItemIndex(humanAttrOne.GetSelectedId()), true);
		}
	}

	private void _on_background_option_item_selected(long index)
	{
		if (backgroundOption.GetSelectedId() != DEFAULT_OPTIONS_ID)
		{
			var chosenFullBackgroundName = backgroundOption.GetItemText(backgroundOption.GetSelectedId());
			var nameBreakIndex = chosenFullBackgroundName.IndexOf(" +1");
			var chosenBackgroundName = chosenFullBackgroundName[..nameBreakIndex].TrimEnd();
			var chosenBackground = adventurerService.backgrounds.Where(b => b.Name == chosenBackgroundName).FirstOrDefault();

			adventurer.SetBackground(chosenBackground);

			//If race is human we will need to add the attribute bonuses back since SetBackground calls SetRace which clears them
			if (adventurer.Race == RaceEnum.Human)
			{
				adventurer.AttributeAddBonus((AttributeEnum)humanAttrOne.GetSelectedId(), 1);
				adventurer.AttributeAddBonus((AttributeEnum)humanAttrTwo.GetSelectedId(), 1);
			}

			SetAttributeAndSavingThrowLabels();
		}

		finalSaveButton.Disabled = false;
	}

	private void _on_final_save_button_pressed()
	{
		saveService.SaveGame(adventurer);
	}
	#endregion
}






