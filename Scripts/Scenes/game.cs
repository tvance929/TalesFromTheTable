using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TalesFromTheTable.Scripts.Utilities;
using TalesFromTheTable.SystemServices;
public partial class game : Control
{
	private const string TITLE_FONT_SIZE = "[font_size=20]";
	private const string TITLE_FONT_COLOR = "[color=#5649FF]";

	private RichTextLabel mainText;
	private TextureRect mainImage;
	private TabContainer tabContainer;

	private List<string> roomsVisited = new();
	private List<MapControlWithRoomID> mapImageControlsWithIDs = new();

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
			$"{GameService.Adventure.Description}[/center]";
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
	
	private void _on_begin_adventure_pressed()
	{
		GameService.StartAdventure();
		roomsVisited.Add(GameService.PlayerLocation);
        SetMapImageControlsList();
        ShowGridMap();
	}

	/// <summary>
	/// In here we grab the Map Array for the level the player is on
	/// Then loop through the 30 map images and grab each control while assigning the room id if it exists to that control
	/// </summary>
	private void SetMapImageControlsList()
	{
		var arrayCount = 0;
        mapImageControlsWithIDs = new List<MapControlWithRoomID>();
        var mapArray = GameService.Adventure.MapArrays.Level1.Split(',').Select(s => s.Trim()).ToList();


		for (var i = 1; i < 7; i++)
		{
			for (var ii = 1; ii < 6; ii++)
			{
				var roomID = "";
				if (arrayCount < mapArray.Count)
				{
					roomID = mapArray[arrayCount];

                }

                mapImageControlsWithIDs.Add(new MapControlWithRoomID(GetNode<TextureRect>($"Main/TabContainer/Map/MapRow{i}/TextureRect{ii}"), roomID));
				arrayCount++;
			}
		}
	}

	private void ShowGridMap()
	{
		//iterate all the map controls... see if the player has been in here!  if so, show the image - for now it should just be 1-1
		//foreach (var control in mapImageControls)
		//{
  //          var controlName = control.Name;
  //          var controlNameSplit = controlName.Split("TextureRect");
  //          var controlNameSplit2 = controlNameSplit[1].Split("x");
  //          var controlRow = controlNameSplit2[0];
  //          var controlColumn = controlNameSplit2[1];

  //          var controlLocation = $"{controlRow}-{controlColumn}";
  //          if (roomsVisited.Contains(controlLocation))
		//	{
  //              var texture = (Texture2D)GD.Load("res://Assets/Icons/room.png");
  //              control.Texture = texture;
  //          }
  //      }
	}
}

public class MapControlWithRoomID
{
	public TextureRect textureRect;
	public string roomID;

	public MapControlWithRoomID(TextureRect textureRect, string roomID)
	{
        this.textureRect = textureRect;
        this.roomID = roomID;
    }
}
