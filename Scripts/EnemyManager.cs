using Godot;
using System;

public class EnemyManager : Node2D
{
    [Export]
    private PackedScene[] enemys;
    [Export]
    private PackedScene powerUp;

    private Timer createEnemyTimer = new Timer();
    private float createEnemyTime = 2.0f;

    private Player player;

    private int enemyLevel;

    public override void _Ready()
    {
        AddChild(createEnemyTimer);
        createEnemyTimer.OneShot = false;
        createEnemyTimer.Connect("timeout", this, nameof(CreateEnemy));
    }

    public void StartCreateEnemy(Player player)
    {
        // createEnemyTimer.WaitTime = createEnemyTime;
        // createEnemyTimer.Start();
        this.player = player;
        CreateEnemy(enemyLevel);
    }

    private void CreateEnemy(int level)
    {
        var enemy = enemys[0].Instance<Enemy>();
        enemy.Position = new Vector2((int)GD.RandRange(50, 670), -100);
        enemy.Player = player;
        enemy.Level = level;
        enemy.OnDeadEvent += OnEnemyDead;
        AddChild(enemy);
    }

    private async void OnEnemyDead(Vector2 posision)
    {
        CreatePowerUp(posision);

        await ToSignal(GetTree().CreateTimer((int)GD.RandRange(1, 3)), "timeout");
        enemyLevel++;
        CreateEnemy(enemyLevel);
    }

    private void CreatePowerUp(Vector2 posision)
    {
        var item = powerUp.Instance<PowerUp>();
        item.Position = posision;
        //AddChild(item);
        CallDeferred("add_child", item);
    }
}
