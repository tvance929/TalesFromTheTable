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

    public override void _Ready()
    {
        mainText = GetNode<RichTextLabel>("Main/MainLeft/MainText");
        mainImage = GetNode<TextureRect>("Main/MainLeft/MainImage/RoomImage");

        GameService.AdventureLoaded += _OnAdventureLoaded;
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

    public void ShowMessage()
    {
        var text = GameService.GetRoomMessage();
        //GD.Print(text);
        mainText.Text = text;
    }
}
