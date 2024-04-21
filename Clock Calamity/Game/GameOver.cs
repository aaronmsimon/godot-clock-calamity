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
        accuracyLabel.Text = $"{Mathf.Round(shotsHitResource.StatValue / shotsFiredResource.StatValue * 100)}%";
        enemiesKilledLabel.Text = enemiesKilledResource.StatValue.ToString();
    }   
}
