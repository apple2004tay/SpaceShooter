using Godot;
using System;

public class Metero : Area2D
{
    public int MoveDuration { get; set; } = 5;

    private AudioStreamPlayer explosionAudio;
    private Tween tween;

    private bool isHitted;

    public override void _Ready()
    {
        base._Ready();

        explosionAudio = GetNode<AudioStreamPlayer>("ExplosionAudio");
        tween = GetNode<Tween>("Tween");
        var endPos = new Vector2((int)GD.RandRange(-100, 820), 1400);
        tween.InterpolateProperty(this, "position", Position, endPos, MoveDuration);
        tween.Connect("tween_completed", this, nameof(OnTweenCompleted));
        tween.Start();

        Connect("area_entered", this, nameof(OnAreaEntered));
    }

    private void OnTweenCompleted(object obj, NodePath nodePath)
    {
        QueueFree();
    }

    private void OnAreaEntered(Area2D area2D)
    {
        if (!isHitted)
        {
            tween.StopAll();

            isHitted = true;
            Visible = false;

            explosionAudio.Connect("finished", this, nameof(OnExplosionAudioFinished));
            explosionAudio.Play();
        }
    }
    private void OnExplosionAudioFinished()
    {
        QueueFree();
    }
}
