using Godot;

namespace Components.Projectiles2D
{
    [GlobalClass]
    public partial class FireProjectileComponent: Node2D
    {
        [ExportCategory("Fire Projectile Component")]
        [Export] private PackedScene projectile;
        [Export] private float speed;
        [Export] private float damage;

        public override void _Ready()
        {
            if (projectile == null)
            {
                GD.PrintErr($"The FireProjectileComponent { this.Name } requires a projectile to spawn. Please add one to node { this.GetPath() }");
                return;
            }
            if (speed == 0)
            {
                GD.PrintErr($"The FireProjectileComponent { this.Name } has a speed of 0. Was that intentional?");
                return;
            }
            if (damage == 0)
            {
                GD.PrintErr($"The FireProjectileComponent { this.Name } has a damage of 0. Was that intentional?");
                return;
            }
        }

        public void Fire(Vector2 pos, float rot)
        {
            if (projectile == null) return;

            // Create instance
            Node2D instance = (Node2D)projectile.Instantiate();
            // Set position & rotation
            instance.Position = pos;
            instance.Rotation = rot;
            // Set projectile info
            instance.Set("speed", speed);
            instance.Set("damage", damage);
            // Get parent to set child to
            Node parent = GetTree().CurrentScene;
            // Add to scene as a child of this component
            parent.AddChild(instance);
        }
    }
}