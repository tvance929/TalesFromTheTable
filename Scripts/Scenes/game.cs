using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using TalesFromTheTable.Scripts.Utilities;
using TalesFromTheTable.Scripts.Utilities.Enums;
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

	private AudioStreamPlayer soundPlayer;

	public override void _Ready()
	{
		soundPlayer = GetNode<AudioStreamPlayer>("SoundPlayer");
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
		mainText.Modulate = new Color(1, 1, 1, 0); //Making main text invisible so we can fade it in
		mainText.Text = bbString;
		var beginButton = GetNode<Button>("Main/MainLeft/BeginButton");
		beginButton.Modulate = new Color(1, 1, 1, 0); //Making start button invisible so we can fade it in
		beginButton.Visible = true;

        var tween = CreateTween();
		tween.TweenProperty(mainText, "modulate", new Color(1, 1, 1, 1), 3); //fade in  
		tween.TweenProperty(beginButton, "modulate", new Color(1, 1, 1, 1), 3); //fade in

		mainImage.Modulate = new Color(1, 1, 1, 0); //Making main image invisible so we can fade it in 

		ShowImageIfExists();
	}

	private void ShowImageIfExists()
	{
		var imageURL = GameService.CurrentRoomImageUrl();
		if (File.Exists(imageURL) && Utilities.IsJpeg(imageURL)) //only aceept jpgs for now
		{
			var texture = (Texture2D)GD.Load(imageURL);

			var tween = CreateTween();
			tween.TweenProperty(mainImage, "modulate", new Color(1, 1, 1, 0), 1); //fade out
			tween.TweenCallback(Callable.From(() => mainImage.Texture = texture)); //this waits for tween to finish before changing image
			tween.TweenProperty(mainImage, "modulate", new Color(1, 1, 1, 1), 2); //fade in          
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

	private void _on_begin_adventure_pressed()
	{
        GetNode<Button>("Main/MainLeft/BeginButton").Visible = false;
        GameService.StartAdventure();
		SetMapImageControlsList();
		DisplayRoom();
	}

	/// <summary>
	/// In here we grab the Map Array for the level the player is on
	/// Then loop through the 30 map texturerects and grab each control while assigning the room id 
	/// if it exists to that control
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

	private void DisplayRoom()
	{
		var text = GameService.GetRoomMessage();
		mainText.Text = text;

		ShowImageIfExists();

		roomsVisited.Add(GameService.PlayerLocation);

		//So now we have a list of all the map controls with their room ids and a list of visited room IDs.
		//We need to iterate through the visited room ids and set the texturerect to the room image
		foreach (var roomID in roomsVisited)
		{
			var mapControl = mapImageControlsWithIDs.Where(m => m.roomID == roomID).FirstOrDefault();
			if (mapControl != null)
			{
				PlaySound(SoundsEnum.Scribble);
				var texture = (Texture2D)GD.Load($"res://Adventures/{GameService.AdventureName}/Assets/Images/map/{roomID}.jpg");
				mapControl.textureRect.Texture = texture;
				//Here I want to position the blank fake map image over the room image and then fade out the fake map image to give the illusion of drawing the map
				// this works below by the way - however it is fading out the actual image of the room...need a fake one over the top and the position
				//var tween = CreateTween();
				//tween.TweenProperty(mapControl.textureRect, "modulate", new Color(1, 1, 1, 0), 1);
			}
		}

		//Enable all buttons that can be used in this room ( exits, unlocks, search, combat, etc)
		foreach(var exit in GameService.CurrentRoomExits())
		{
            //var button = GetNode<Button>($"Main/TabContainer/Map/MapRow{exit.direction.GetRow()}/TextureRect{exit.direction.GetColumn()}/Button");
        //    button.Visible = true;
        //    button.Disabled = false;
        }
	}

	private void PlaySound(SoundsEnum sound)
	{
		if (soundPlayer == null)
		{
			soundPlayer = GetNode<AudioStreamPlayer>("SoundPlayer");
		}

		if (sound == SoundsEnum.Scribble)
		{
			soundPlayer.Stream = (AudioStream)GD.Load("res://Assets/Sounds/Effects/pencilscribble.mp3");
		}

		soundPlayer.VolumeDb = -5;
		soundPlayer.Play();
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
