using Godot;
using System;

public class NormalLaser : Area2D
{
    [Export]
    private float speed = 500;
    [Export]
    private bool isPlayerLaser = false;
    private Vector2 direction;

    public override void _Ready()
    {
        direction = isPlayerLaser ? Vector2.Up : Vector2.Down;

        Connect("area_entered", this, nameof(OnAreaEntered));
    }

    public override void _PhysicsProcess(float delta)
    {
        Translate(direction * delta * speed);

        if (Position.y < -200 || Position.y > 1500)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area2D)
    {
        QueueFree();
    }
}
