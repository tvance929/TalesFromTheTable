using System;
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

	private List<VisitedRoom> roomsVisited = new();
	private List<MapControlWithRoomID> mapImageControlsWithIDs = new();

	private AudioStreamPlayer soundPlayer;

	private List<GameButton> gameButtons;


	public override void _Ready()
	{
		soundPlayer = GetNode<AudioStreamPlayer>("SoundPlayer");
		mainText = GetNode<RichTextLabel>("Main/MainLeft/MainText");
		mainImage = GetNode<TextureRect>("Main/MainLeft/MainImage/RoomImage");
		tabContainer = GetNode<TabContainer>("Main/TabContainer");

		GameService.AdventureLoaded += _OnAdventureLoaded;

		SetTabContainerDefaults();
		SetGameButtonsList();
	}

	public override void _Process(double delta)
	{
	}

	//Enter Adventure
	private void _on_begin_adventure_pressed()
	{
		GetNode<Button>("Main/MainLeft/BeginButton").Visible = false;
		GameService.StartAdventure();
		SetMapImageControlsList();
		DisplayRoom();
	}

	/// <summary>
	/// In here we grab the Map Array for the level the player is on
	/// Then loop through the 30 map texture rects and grab each control while assigning the room id 
	/// if it exists to that control
	/// </summary>
	private void SetMapImageControlsList()
	{
		var arrayCount = 0;
		mapImageControlsWithIDs = new List<MapControlWithRoomID>();

		var mapArray = GameService.ReturnCurrentMapArray();

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
		mainText.Text = MainBBText(GameService.GetRoomMessage());

        ShowImageIfExists();

		if (!roomsVisited.Exists(r => r.RoomID == GameService.PlayerLocation))
		{
			roomsVisited.Add(new VisitedRoom { RoomID = GameService.PlayerLocation, DrawnOnMap = false });
		}
		else
		{
			roomsVisited.Where(r => r.RoomID == GameService.PlayerLocation).FirstOrDefault().DrawnOnMap = true;
		}

		//So now we have a list of all the map controls with their room ids and a list of visited room IDs.
		//We need to iterate through the visited room ids and set the texturerect to the room image
		foreach (var visitedRoom in roomsVisited)
		{
			var mapControl = mapImageControlsWithIDs.Where(m => m.roomID == visitedRoom.RoomID).FirstOrDefault();
			if (mapControl != null)
			{
				if (visitedRoom.RoomID == GameService.PlayerLocation && visitedRoom.DrawnOnMap == false)
				{
					//We only play the drawing sound if the player is in the room and it hasn't been drawn on the map yet
					PlaySound(SoundsEnum.Scribble);
				}

				var texture = (Texture2D)GD.Load($"res://Adventures/{GameService.AdventureName}/Assets/Images/map/{visitedRoom.RoomID}x.jpg");

				if (visitedRoom.RoomID != GameService.PlayerLocation)
				{
					texture = (Texture2D)GD.Load($"res://Adventures/{GameService.AdventureName}/Assets/Images/map/{visitedRoom.RoomID}.jpg");
				}

				mapControl.textureRect.Texture = texture;
				//Here I want to position the blank fake map image over the room image and then fade out the fake map image to give the illusion of drawing the map
				// this works below by the way - however it is fading out the actual image of the room...need a fake one over the top and the position
				//var tween = CreateTween();
				//tween.TweenProperty(mapControl.textureRect, "modulate", new Color(1, 1, 1, 0), 1);
			}
		}

		EnableValidButtons();
	}

    #region Button Events
    private void _OnAdventureLoaded()
	{		
		mainText.Modulate = new Color(1, 1, 1, 0); //Making main text invisible so we can fade it in
		mainText.Text = MainBBText(GameService.Adventure.Description, true);
		var beginButton = GetNode<Button>("Main/MainLeft/BeginButton");
		beginButton.Modulate = new Color(1, 1, 1, 0); //Making start button invisible so we can fade it in
		beginButton.Visible = true;

		var tween = CreateTween();
		tween.TweenProperty(mainText, "modulate", new Color(1, 1, 1, 1), 2); //fade in  
		tween.TweenProperty(beginButton, "modulate", new Color(1, 1, 1, 1), 2); //fade in

		mainImage.Modulate = new Color(1, 1, 1, 0); //Making main image invisible so we can fade it in 

		ShowImageIfExists();
	}

	private void EnableValidButtons()
	{
		//Enable all buttons that can be used in this room ( exits, unlocks, search, combat, etc) and change the modulations
		foreach (var button in gameButtons)
		{
			button.Button.Modulate = new Color(1, 1, 1, 0.3f);
			button.Button.Disabled = true;
		}
		foreach (var exit in GameService.CurrentRoomExits())
		{
			var button = gameButtons.Where(b => b.Action == exit.directionAction).FirstOrDefault();
			button.Button.Modulate = new Color(1, 1, 1, 1);
			button.Button.Disabled = false;
		}

		//Search button
		if (GameService.CurrentRoomHasLootOrActiveTraps())
		{
			var button = gameButtons.Where(b => b.Action == ActionsEnum.SEARCH).FirstOrDefault();
			button.Button.Modulate = new Color(1, 1, 1, 1);
			button.Button.Disabled = false;
		}

		if(GameService.CurrentRoomHasLootableChest())
		{
            var button = gameButtons.Where(b => b.Action == ActionsEnum.CHEST).FirstOrDefault();
            button.Button.Modulate = new Color(1, 1, 1, 1);
            button.Button.Disabled = false;
        }
	}

	private void OnDirectionButtonPressed(string compassDirection)
	{
		var compassUpper = compassDirection.ToUpper();

		//Test to see if this is a valid CompassDirectionEnum
		if (Enum.TryParse<CompassDirectionEnum>(compassUpper, out CompassDirectionEnum compass))
		{
			GameService.MovePlayer((ActionsEnum)Enum.Parse(typeof(ActionsEnum), compassUpper));
			DisplayRoom();
		}
		else
		{
			throw new Exception($"Invalid Compass Direction: {compassDirection}");  //do something with this later
		}

	}

	private void OnActionButtonPressed(string action)
	{
		var actionUpper = action.ToUpper();

		if (Enum.TryParse<ActionsEnum>(actionUpper, out ActionsEnum actionPerformed))
		{
			switch (actionPerformed)
			{
                case ActionsEnum.CHEST:
					mainText.Text += MainBBText(GameService.SearchRoom());
                    break;
                case ActionsEnum.SEARCH:  //CHANGE LOOT which is incoming from the button
                    mainText.Text += MainBBText(GameService.SearchRoom());
                    break;
                case ActionsEnum.SOUTH:
                    //GD.Print("south");
                    break;
                case ActionsEnum.EAST:
                    //GD.Print("east");
                    break;
                case ActionsEnum.WEST:
                    //GD.Print("west");
                    break;
                default:
                    break;
            }
		}
        else
        {
            throw new Exception($"Invalid Action Sent: {action}");  //do something with this later
        }
    }
	#endregion

	#region Utility Methods
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

	private void ShowImageIfExists()
	{
		var tween = CreateTween();
		tween.TweenProperty(mainImage, "modulate", new Color(1, 1, 1, 0), 1); //fade out
		
		var imageURL = GameService.CurrentRoomImageUrl();
		if (File.Exists(imageURL) && Utilities.IsPng(imageURL)) //only aceept pngs for now (could also accept pngs and jpgs)
		{
			var texture = (Texture2D)GD.Load(imageURL);
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
	
	private void SetGameButtonsList()
	{
		gameButtons = new List<GameButton>
		{
			new GameButton { Button = GetNode<Button>("Main/MainLeft/MainButtonControls/CompassContainer/West"), Action = ActionsEnum.WEST },
			new GameButton { Button = GetNode<Button>("Main/MainLeft/MainButtonControls/CompassContainer/East"), Action = ActionsEnum.EAST },
			new GameButton { Button = GetNode<Button>("Main/MainLeft/MainButtonControls/CompassContainer/VBox/North"), Action = ActionsEnum.NORTH },
			new GameButton { Button = GetNode<Button>("Main/MainLeft/MainButtonControls/CompassContainer/VBox/South"), Action = ActionsEnum.SOUTH },
			new GameButton { Button = GetNode<Button>("Main/MainLeft/MainButtonControls/ActionsContainer/Chest"), Action = ActionsEnum.CHEST },
            new GameButton { Button = GetNode<Button>("Main/MainLeft/MainButtonControls/ActionsContainer/VBox/Search"), Action = ActionsEnum.SEARCH }
        };

		foreach (var button in gameButtons)
		{
			button.Button.Modulate = new Color(1, 1, 1, 0.5f);
			button.Button.Disabled = true;
		}
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow1/TextureRect5/Button"), Action = ActionsEnum.PickLock });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow2/TextureRect1/Button"), Action = ActionsEnum.West });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow2/TextureRect2/Button"), Action = ActionsEnum.East });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow2/TextureRect3/Button"), Action = ActionsEnum.North });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow2/TextureRect4/Button"), Action = ActionsEnum.South });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow2/TextureRect5/Button"), Action = ActionsEnum.PickLock });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow3/TextureRect1/Button"), Action = ActionsEnum.West });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow3/TextureRect2/Button"), Action = ActionsEnum.East });
		//gameButtons.Add(new GameButton { Button = GetNode<Button>("Main/TabContainer/Map/MapRow3/TextureRect3/Button"), Action = ActionsEnum.N})
	}

	private string MainBBText(string text, bool adventureStart = false)
	{
		if (!adventureStart)
		{
            return $"[center]{text}[/center]";
        }
		else
		{
			return $"[center][b]{TITLE_FONT_SIZE}{TITLE_FONT_COLOR}{GameService.Adventure.Title}[/color][/font_size][/b]\n" +
			$"{text}[/center]";
        }
    }
	#endregion
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

public class GameButton
{
	public Button Button { get; set; }
	public ActionsEnum Action { get; set; }
}

public class VisitedRoom
{
	public string RoomID { get; set; }
	public bool DrawnOnMap { get; set; }
}
