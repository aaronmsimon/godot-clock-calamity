using Components.Game;
using Godot;

public partial class GameOver : Node
{
    [ExportCategory("Game Over")]

    [ExportGroup("Resources")]
    [Export] private GameStatResource scoreResource;
    [Export] private GameStatResource shotsFiredResource;
    [Export] private GameStatResource shotsHitResource;
    [Export] private GameStatResource enemiesKilledResource;

    private Label scoreLabel;
    private Label shotsFiredLabel;
    private Label shotsHitLabel;
    private Label accuracyLabel;
    private Label enemiesKilledLabel;

    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("%ScoreValueLabel");
        shotsFiredLabel = GetNode<Label>("%ShotsFiredValueLabel");
        shotsHitLabel = GetNode<Label>("%ShotsHitValueLabel");
        accuracyLabel = GetNode<Label>("%AccuracyValueLabel");
        enemiesKilledLabel = GetNode<Label>("%EnemiesKilledValueLabel");

        scoreLabel.Text = $"{scoreResource.StatValue:n0}";
        shotsFiredLabel.Text = shotsFiredResource.StatValue.ToString();
        shotsHitLabel.Text = shotsHitResource.StatValue.ToString();
        if (shotsFiredResource.StatValue > 0)
        {
            accuracyLabel.Text = $"{Mathf.Round(shotsHitResource.StatValue / shotsFiredResource.StatValue * 100)}%";
        }
        else
        {
            accuracyLabel.Text = "0%";
        }
        enemiesKilledLabel.Text = enemiesKilledResource.StatValue.ToString();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            scoreResource.StatValue = 0;
            shotsFiredResource.StatValue = 0;
            shotsHitResource.StatValue = 0;
            enemiesKilledResource.StatValue = 0;

            GetTree().ChangeSceneToFile("res://Levels/Level01/level_01.tscn");
        }
    }
}
