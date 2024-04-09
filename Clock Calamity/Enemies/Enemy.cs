using Godot;
using System.Threading.Tasks;
using Components.Pathfinding;

namespace CC.Characters
{
    public partial class Enemy : Node2D
    {
        [Export] private PackedScene projectile;
        [Export] private float projectileSpeed = 500f;
        [Export] private float projectileDamage = 1;

        // Resources
        public AStarGrid2DComponent astarGrid2DComponent { private get; set; }
        public WaypointsResource pathResource { private get; set; }
        public NPCResource npcResource { private get; set; }
        
        public PlayerController player { get; private set; }

        // Pathfinding
        private FollowWaypointsComponent followWaypointsComponent;

        // Aiming info
        private Node2D anchor;
        private Marker2D muzzle;
        private Timer timer = new Timer();
        private bool firing = false;

        // Signals
        [Signal] public delegate void DamagedEventHandler();
        [Signal] public delegate void DiedEventHandler();

        public override void _Ready()
        {
            player = GetNode<PlayerController>("../../Player");
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");

            followWaypointsComponent = GetNode<FollowWaypointsComponent>("FollowWaypointsComponent");
            followWaypointsComponent.astarGrid2DComponent = astarGrid2DComponent;
            followWaypointsComponent.pathResource = pathResource;
            followWaypointsComponent.npcResource = npcResource;

            AddChild(timer);
        }

        public override void _Process(double delta)
        {
        }

        public async Task Fire()
        {
            Node2D projectileInstance = (Node2D)projectile.Instantiate();
            projectileInstance.Set("speed", projectileSpeed);
            projectileInstance.Set("damage", projectileDamage);
            projectileInstance.GlobalPosition = muzzle.GlobalPosition;
            projectileInstance.Rotation = Rotation;
            Node parent = GetParent();
            parent.AddChild(projectileInstance);

            timer.Start(npcResource.timeBetweenShots);
            await ToSignal(timer, Timer.SignalName.Timeout);
        }

        public void TakeDamage(int damage)
        {
            npcResource.health -= damage;
            EmitSignal(SignalName.Damaged);

            if (npcResource.health <= 0)
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
