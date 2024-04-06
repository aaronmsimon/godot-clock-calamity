using Godot;

public partial class Bullet : Node2D
{
    [Export] public float speed { get; set; } = 100f;

    private VisibleOnScreenNotifier2D visible;

    public override void _Ready()
    {
        visible = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

        visible.ScreenExited += QueueFree;
    }

    public override void _Process(double delta)
    {
        Vector2 direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
        Translate(direction * speed * (float)delta);
    }
}
