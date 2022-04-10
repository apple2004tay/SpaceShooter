using Godot;
using System;

public class PowerUp : Area2D
{
    [Export]
    private float speed = 200;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        Translate(Vector2.Down * speed * delta);

        if (Position.y > 1400)
        {
            QueueFree();
        }
    }
}