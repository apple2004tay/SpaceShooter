using Godot;
using System;

public class Enemy : Area2D
{
    public event Action<Vector2> OnDeadEvent;

    [Export]
    private float speed = 150;

    private LaserManager laserManager;
    private Sprite sprite;
    private AudioStreamPlayer explosionAudio;
    private AnimatedSprite animatedSprite;
    private Tween tween = new Tween();

    public Player Player { get; set; }
    private bool isTweenCompleted;
    private bool isHitted;

    public int Level { get; set; } = 0;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite>("Sprite");
        laserManager = GetNode<LaserManager>("LaserManager");
        explosionAudio = GetNode<AudioStreamPlayer>("ExlosionAudio");
        explosionAudio.Connect("finished", this, nameof(OnExplosionAudioFinished));
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        Connect("area_entered", this, nameof(OnAreaEntered));

        AddChild(tween);
        tween.InterpolateProperty(this, "position", Position, Position + (Vector2.Down * 300), 1, Tween.TransitionType.Cubic, Tween.EaseType.Out);
        tween.Start();
        tween.Connect("tween_completed", this, nameof(OnTweenCompleted));
    }

    private void OnTweenCompleted(object obj, NodePath nodePath)
    {
        isTweenCompleted = true;
        laserManager.LaserLevel = Level;
        laserManager.StartShootLaser();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (isTweenCompleted && IsInstanceValid(Player) && !isHitted)
        {
            if (Player.Position.x > Position.x)
            {
                Translate(Vector2.Right * speed * delta);
            }
            else if (Player.Position.x < Position.x)
            {
                Translate(Vector2.Left * speed * delta);
            }
        }
    }

    private void OnAreaEntered(Area2D area2D)
    {
        if (!isHitted)
        {
            tween.StopAll();
            isHitted = true;
            sprite.Visible = false;
            laserManager.QueueFree();
            animatedSprite.Play("Explosion");
            explosionAudio.Play();
            OnDeadEvent?.Invoke(Position);
        }
    }

    private void OnExplosionAudioFinished()
    {
        OnDeadEvent = null;
        QueueFree();
    }
}
