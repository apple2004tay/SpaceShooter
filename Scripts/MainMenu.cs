using Godot;
using System;

public class MainMenu : Node2D
{
    public static float HeighestSec = 0;

    public override void _Ready()
    {
        base._Ready();

        var heighestSecLabel = GetNode<Label>("Control/HighestSecLabel");
        heighestSecLabel.Text = $"Heighest Sec: {Math.Round(HeighestSec, 2)}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed())
        {
            GetTree().ChangeScene("res://Scenes/Game.tscn");
        }
    }
}
