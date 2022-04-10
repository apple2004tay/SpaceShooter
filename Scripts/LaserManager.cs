using Godot;
using System;

public class LaserManager : Node2D
{
    [Export]
    private PackedScene normalLaser;

    [Export]
    private float laserPosOffsetY = 0;

    private Node2D parentNode;
    private Node laserHolder;

    private Timer timer;

    private AudioStreamPlayer laserAudio;

    public int LaserLevel { get; set; } = 0;

    private float laserWaitTime = 1;

    public override void _Ready()
    {
        base._Ready();
        var gameManager = GetTree().Root.GetNode("GameManager");
        parentNode = GetParent<Node2D>();
        laserHolder = gameManager.GetNode("LaserHolder");
        laserAudio = GetNode<AudioStreamPlayer>("LaserAudio");
    }

    public void StartShootLaser()
    {
        if (LaserLevel < 3)
        {
            StartShootNormalLaser();
        }
        else if (LaserLevel >= 3)
        {
            StartShootLevel2Laser();
        }
    }

    private void StartShootNormalLaser()
    {
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = laserWaitTime - (LaserLevel % 3) * 0.2f;
        timer.OneShot = false;
        timer.Connect("timeout", this, nameof(ShootNormalLaser));
        timer.Start();
    }

    private void ShootNormalLaser()
    {
        var laser = normalLaser.Instance<NormalLaser>();
        laser.Position = new Vector2(parentNode.Position.x, parentNode.Position.y + laserPosOffsetY);
        laserHolder.AddChild(laser);
        laserAudio.Play();
    }

    public void UpgradeLaser()
    {
        LaserLevel++;
        timer.QueueFree();
        StartShootLaser();
    }

    private void StartShootLevel2Laser()
    {
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = laserWaitTime - (LaserLevel % 3) * 0.2f; ;
        timer.OneShot = false;
        timer.Connect("timeout", this, nameof(ShootLevel2Laser));
        timer.Start();
    }

    private void ShootLevel2Laser()
    {
        for (int i = -1; i < 2; i++)
        {
            var laser = normalLaser.Instance<NormalLaser>();
            laser.Position = new Vector2(parentNode.Position.x + (50 * i), parentNode.Position.y + laserPosOffsetY);
            laserHolder.AddChild(laser);
        }
        laserAudio.Play();
    }
}
