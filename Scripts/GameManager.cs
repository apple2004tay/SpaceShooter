using Godot;
using System;

public class GameManager : Node2D
{
    private Label startCountLabel;
    private Label aliveSecLabel;
    private Label gameOverLabel;

    private Player player;
    private MeteroManager meteroManager;
    private EnemyManager enemyManager;

    private Timer startTimer = new Timer();
    private readonly int startCountdown = 3;
    private int remaingStartSec;

    private bool gameStart;
    private bool gameOver;

    private float aliveSec = 0;

    public override void _Ready()
    {
        startCountLabel = GetNode<Label>("Control/StartCountdownLabel");
        aliveSecLabel = GetNode<Label>("Control/AliveSecLabel");
        gameOverLabel = GetNode<Label>("Control/GameOverLabel");

        player = GetNode<Player>("Player");
        player.OnDeadEvent += OnPlayerDead;

        meteroManager = GetNode<MeteroManager>("MeteroManager");

        enemyManager = GetNode<EnemyManager>("EnemyManager");

        remaingStartSec = startCountdown;
        AddChild(startTimer);
        startTimer.WaitTime = 1;
        startTimer.OneShot = false;
        startTimer.Connect("timeout", this, nameof(OnStartTimerTimeout));
        startTimer.Start();
        startCountLabel.Text = remaingStartSec.ToString();
    }

    private void OnStartTimerTimeout()
    {
        remaingStartSec--;

        if (remaingStartSec == 0)
        {
            startCountLabel.Text = "Start";
            StartGame();
        }
        else if (remaingStartSec < 0)
        {
            startCountLabel.Visible = false;
            startTimer.QueueFree();
        }
        else
        {
            startCountLabel.Text = remaingStartSec.ToString();
        }
    }

    private void StartGame()
    {
        gameStart = true;
        player.StartControl();
        meteroManager.StartCreateMetero();
        enemyManager.StartCreateEnemy(player);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (gameStart && !gameOver)
        {
            aliveSec += delta;
            aliveSecLabel.Text = $"Alive Sec: {aliveSec.ToString("f2")}";
        }

        if (gameOver)
        {
            if (Input.IsActionJustPressed("restart"))
            {
                GetTree().ChangeScene("res://Scenes/Game.tscn");
            }

            if (Input.IsActionJustPressed("ui_cancel"))
            {
                GetTree().ChangeScene("res://Scenes/Main.tscn");
            }
        }
    }

    private void OnPlayerDead()
    {
        gameOver = true;
        gameOverLabel.Visible = true;
        if (aliveSec > MainMenu.HeighestSec)
        {
            MainMenu.HeighestSec = aliveSec;
        }
    }
}
