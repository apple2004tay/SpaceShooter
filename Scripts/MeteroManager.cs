using Godot;
using System;

public class MeteroManager : Node2D
{
    [Export]
    private PackedScene[] meteros;
    private Timer createMeteroTimer = new Timer();
    private float createMeteroTime = 1.5f;
    private Timer upgradeTimer = new Timer();

    public override void _Ready()
    {
        base._Ready();
        AddChild(createMeteroTimer);
        createMeteroTimer.OneShot = false;
        createMeteroTimer.Connect("timeout", this, nameof(CreateMetero));

        AddChild(upgradeTimer);
        upgradeTimer.WaitTime = 5.0f;
        upgradeTimer.OneShot = false;
        upgradeTimer.Connect("timeout", this, nameof(OnUpgradeTimeout));
        upgradeTimer.Start();
    }

    public void StartCreateMetero()
    {
        createMeteroTimer.WaitTime = createMeteroTime;
        createMeteroTimer.Start();
    }

    private void CreateMetero()
    {
        var metero = meteros[(int)GD.RandRange(0, meteros.Length)].Instance<Metero>();
        metero.Position = new Vector2((int)GD.RandRange(0, 720), -100);
        metero.MoveDuration = (int)GD.RandRange(3, 10);
        AddChild(metero);
    }

    private void OnUpgradeTimeout()
    {
        createMeteroTime -= 0.2f;
        createMeteroTime = 0.3f > createMeteroTime ? 0.3f : createMeteroTime;
        StartCreateMetero();

        if (createMeteroTime <= 0.1f)
        {
            upgradeTimer.Stop();
        }
    }
}
