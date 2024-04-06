using Godot;
using CC.Characters;

namespace CC.Shooting
{
    public partial class Bullet : Node2D
    {
        public float speed { get; set; } = 100f;
        public int damage { get; set; } = 1;

        private VisibleOnScreenNotifier2D visible;
        private Area2D collider;

        public override void _Ready()
        {
            visible = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
            collider = GetNode<Area2D>("Area2D");

            visible.ScreenExited += QueueFree;
            collider.AreaEntered += OnAreaEntered;
        }

        public override void _Process(double delta)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
            Translate(direction * speed * (float)delta);
        }

        private void OnAreaEntered(Area2D area)
        {
            Enemy enemy = (Enemy)area.GetParent();
            enemy.TakeDamage(damage);
            QueueFree();
        }
    }
}
