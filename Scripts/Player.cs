using Godot;
using System;

public class Player : Area2D
{
    public event Action OnDeadEvent;

    [Export]
    private float speed = 200;

    private LaserManager laserManager; private Sprite sprite;
    private AudioStreamPlayer explosionAudio;
    private AudioStreamPlayer upgradeAudio;
    private AnimatedSprite animatedSprite;
    private bool isHitted;
    private bool canControl = false;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite>("Sprite");
        laserManager = GetNode<LaserManager>("LaserManager");
        explosionAudio = GetNode<AudioStreamPlayer>("ExlosionAudio");
        explosionAudio.Connect("finished", this, nameof(OnExplosionAudioFinished));
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        upgradeAudio = GetNode<AudioStreamPlayer>("UpgradeAudio");
        Connect("area_entered", this, nameof(OnAreaEntered));
    }

    public void StartControl()
    {
        canControl = true;
        laserManager.StartShootLaser();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (!canControl)
        {
            return;
        }

        var movement = Vector2.Zero;

        if (Input.IsActionPressed("ui_up") && Position.y > 0)
        {
            movement += Vector2.Up;
        }

        if (Input.IsActionPressed("ui_down") && Position.y < 1280)
        {
            movement += Vector2.Down;
        }

        if (Input.IsActionPressed("ui_left") && Position.x > 0)
        {
            movement += Vector2.Left;
        }

        if (Input.IsActionPressed("ui_right") && Position.x < 720)
        {
            movement += Vector2.Right;
        }

        var isSprint = Input.IsActionPressed("ui_accept");

        if (movement.LengthSquared() > 0)
        {
            var move = movement.Normalized();
            Translate(move * speed * delta * (isSprint ? 2 : 1));
        }
    }

    private void OnAreaEntered(Area2D area2D)
    {
        if (area2D.CollisionLayer == 32 && !isHitted)
        {
            area2D.QueueFree();
            laserManager.UpgradeLaser();
            upgradeAudio.Play();
            return;
        }

        if (!isHitted)
        {
            isHitted = true;
            canControl = false;
            sprite.Visible = false;
            laserManager.QueueFree();
            animatedSprite.Play("Explosion");
            explosionAudio.Play();
            OnDeadEvent?.Invoke();
        }
    }

    private void OnExplosionAudioFinished()
    {
        OnDeadEvent = null;
        QueueFree();
    }
}
