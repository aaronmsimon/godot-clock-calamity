using Godot;

namespace CC.Characters
{
    public partial class Enemy : Node2D
    {
        [Export] public int health { get; private set; } = 3;

        private Area2D collider;

        [Signal] public delegate void DamagedEventHandler();
        [Signal] public delegate void DiedEventHandler();

        public override void _Ready()
        {
            collider = GetNode<Area2D>("Area2D");
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            EmitSignal(SignalName.Damaged);

            if (health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            EmitSignal(SignalName.Died);
            QueueFree();
        }
    }
}
