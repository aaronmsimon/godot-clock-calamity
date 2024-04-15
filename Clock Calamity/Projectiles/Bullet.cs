using Godot;
using CC.Enemies;

namespace CC.Shooting
{
    public partial class Bullet : Node2D
    {
        private float speed = 100f;
        private int damage = 1;

        private VisibleOnScreenNotifier2D visible;
        private Area2D collider;

        public override void _Ready()
        {
            visible = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
            collider = GetNode<Area2D>("Area2D");

            visible.ScreenExited += QueueFree;
            collider.AreaEntered += OnAreaEntered;

            collider.BodyEntered += OnBodyEntered;
        }

        public override void _Process(double delta)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
            Translate(direction * speed * (float)delta);
        }

        private void OnAreaEntered(Area2D area)
        {
            Node parent = area.GetParent();
            if (parent.GetType() == typeof(Enemy))
            {
                Enemy enemy = (Enemy)parent;
                enemy.TakeDamage(damage);
            }
            QueueFree();
        }

        private void OnBodyEntered(Node2D body)
        {
            QueueFree();
        }
    }
}
