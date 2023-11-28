using Godot;
using System;
using System.IO;
using TalesFromTheTable.Scripts.Utilities;
using TalesFromTheTable.SystemServices;
public partial class game : Control
{
	private const string TITLE_FONT_SIZE = "[font_size=20]";
	private const string TITLE_FONT_COLOR = "[color=#5649FF]";

	private RichTextLabel mainText;
	private TextureRect mainImage;
	private TabContainer tabContainer;

	public override void _Ready()
	{
		mainText = GetNode<RichTextLabel>("Main/MainLeft/MainText");
		mainImage = GetNode<TextureRect>("Main/MainLeft/MainImage/RoomImage");
		tabContainer = GetNode<TabContainer>("Main/TabContainer");

		GameService.AdventureLoaded += _OnAdventureLoaded;

		SetTabContainerDefaults();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _OnAdventureLoaded()
	{
		//GD.Print("ADVENTURE LOADED");
		var bbString = $"[center][b]{TITLE_FONT_SIZE}{TITLE_FONT_COLOR}{GameService.Adventure.Title}[/color][/font_size][/b]\n" +
			$"{GameService.Adventure.Description}[/center]" +
			$"\nasdfasd\nasdfsadfasdfasd\nasdfasdfasdf\naDFGASDFGFGSDFGSDF\nASFASDFASDF\nASDFGASDFASDF\nADFASDFSDAFSADF" +
			$"\nasdfasd\nasdfsadfasdfasd\nasdfasdfasdf\naDFGASDFGFGSDFGSDF\nASFASDFASDF\nASDFGASDFASDF\nADFASDFSDAFSADF";
		mainText.Text = bbString;

		ShowImageIfExists();  
	}

	private void ShowImageIfExists()
	{
		var imageURL = GameService.CurrentRoomImageUrl();
		if (File.Exists(imageURL) && Utilities.IsJpeg(imageURL)) //only aceept jpgs for now
		{
			var texture = (Texture2D)GD.Load(imageURL);
			mainImage.Texture = texture;
		}
	}

	private void SetTabContainerDefaults()
	{
		var mapIcon = (Texture2D)GD.Load("res://Assets/Icons/treasure-map.png");       
		tabContainer.SetTabIcon(0, (Texture2D)mapIcon);
		tabContainer.SetTabTitle(0, "");

		tabContainer.SetTabIcon(1, (Texture2D)GD.Load("res://Assets/Icons/swordman.png"));
		tabContainer.SetTabTitle(1, "");

		tabContainer.SetTabIcon(2, (Texture2D)GD.Load("res://Assets/Icons/quill-ink.png"));
		tabContainer.SetTabTitle(2, "");

		tabContainer.SetTabIcon(3, (Texture2D)GD.Load("res://Assets/Icons/gears.png"));
		tabContainer.SetTabTitle(3, "");
	}

	public void ShowMessage()
	{
		var text = GameService.GetRoomMessage();
		//GD.Print(text);
		mainText.Text = text;
	}
}
