using Godot;
using Components.Pathfinding;
using Components.Game;
using Components.Weapons;
using System.Threading.Tasks;
using System.ComponentModel;
using Components.Effects;

namespace CC.Enemies
{
    [GlobalClass]
    public partial class Enemy : Node2D, IDamageable
    {
        [Export] private CharacterResource characterResource;

        [ExportGroup("A* Pathfinding Resources")]
        [Export] public AStarGrid2DComponent aStarGrid2DComponent;
        [Export] private Waypoints2DResource waypoints2DResource;
        [Export] private GridResource gridResource;

        [ExportGroup("Movement")]
        [Export] private float speed;

        [ExportGroup("Firing")]
        [Export] private Marker2D muzzle;
        [Export] private Node2D target;

        [ExportGroup("Scoring")]
        [Export] private float scoreTiming = 5f;
        [Export] private float baseScore = 1000f;
        [Export] private int minimumScore = 100;

        private FollowWaypoints2DComponent followWaypoints2DComponent;
        private Vector2 offset;
        private WeaponComponent weaponComponent;
        private Timer shotTimer = new Timer();
        private bool isFiring = false;
        private GameStatComponent shotsHitStatComponent;
        private GameStatComponent enemiesKilledStatComponent;
        private GameStatComponent scoreStatComponent;
        private Timer scoreTimer = new Timer();
        private int health;
        private AudioStreamPlayer2D deathAudio = new AudioStreamPlayer2D();
        private FlashComponent flashComponent;

        public override void _Ready()
        {
            if (aStarGrid2DComponent == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs the AStarGrid2DComponent set.");
                return;
            }
            if (waypoints2DResource == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs the Waypoints2DResource set.");
                return;
            }

            followWaypoints2DComponent = GetNode<FollowWaypoints2DComponent>("FollowWaypoints2DComponent");
            weaponComponent = GetNode<WeaponComponent>("WeaponComponent");
            shotsHitStatComponent = GetNode<GameStatComponent>("ShotsHitStatComponent");
            enemiesKilledStatComponent = GetNode<GameStatComponent>("EnemiesKilledStatComponent");
            scoreStatComponent = GetNode<GameStatComponent>("ScoreStatComponent");
            flashComponent = GetNode<FlashComponent>("FlashComponent");

            followWaypoints2DComponent.AStarGrid2DComponent = aStarGrid2DComponent;
            followWaypoints2DComponent.Waypoints2DResource = waypoints2DResource;

            followWaypoints2DComponent.EndOfPath += OnEndOfPath;
            followWaypoints2DComponent.EndOfWaypoints += OnEndOfWaypoints;

            offset = aStarGrid2DComponent.tileMap.Position;
            health = characterResource.health;

            AddChild(shotTimer);
            AddChild(scoreTimer);
            Node parent = GetTree().CurrentScene;
            parent.CallDeferred("add_child", deathAudio);

            if (characterResource.deathSFX != null)
            {
                deathAudio.Stream = characterResource.deathSFX;
            }
            deathAudio.Finished += () => QueueFree();

            scoreTimer.Start(scoreTiming);

            if (weaponComponent.weaponResource == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs a WeaponResource.");
                return;
            }
            if (muzzle == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs a Muzzle Marker2D.");
                return;
            }
            if (target == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs a target.");
                return;
            }

            if (speed == 0)
            {
                GD.PrintErr("Speed is set to zero. Is this right?");
            }
        }

        public override void _Process(double delta)
        {
            if (aStarGrid2DComponent == null || waypoints2DResource == null) return;

            Pathfinding((float)delta);
        }

        public async void TakeDamage(int damage)
        {
            health -= damage;
            shotsHitStatComponent.UpdateStatAddAmount(1);
            await flashComponent.Flash();

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            gridResource.Data[followWaypoints2DComponent.CurrentCell.X, followWaypoints2DComponent.CurrentCell.Y] = null;
            enemiesKilledStatComponent.UpdateStatAddAmount(1);
            scoreStatComponent.UpdateStatAddAmount((int)Mathf.Max((float)scoreTimer.TimeLeft / scoreTiming * baseScore, minimumScore));

            if (characterResource.Splat != null)
            {
                Node2D instance = (Node2D)characterResource.Splat.Instantiate();
                instance.GlobalPosition = GlobalPosition;
                Node parent = GetTree().CurrentScene;
                parent.AddChild(instance);
            }

            ProcessMode = ProcessModeEnum.Disabled;
            Visible = false;

            deathAudio.Play();
        }

        private void Pathfinding(float delta)
        {
            if (followWaypoints2DComponent.GetNextCell() == null) return;

            Vector2I targetCell = (Vector2I)followWaypoints2DComponent.GetNextCell();
            Vector2 targetPos = followWaypoints2DComponent.GetCellPosition(targetCell) + offset;

            float targetX = Mathf.MoveToward(GlobalPosition.X, targetPos.X, speed * delta);
            float targetY = Mathf.MoveToward(GlobalPosition.Y, targetPos.Y, speed * delta);

            if (!isFiring)
            {
                if (gridResource.Data[targetCell.X, targetCell.Y] == null || gridResource.Data[targetCell.X, targetCell.Y] == this)
                {
                    Rotate(GetAngleTo(targetPos));
                    GlobalPosition = new Vector2(targetX, targetY);
                }
                else
                {
                    OnEndOfWaypoints();
                }
            }

            if (GlobalPosition == targetPos)
            {
                gridResource.Data[followWaypoints2DComponent.CurrentCell.X, followWaypoints2DComponent.CurrentCell.Y] = null;
                followWaypoints2DComponent.NextCell();
                gridResource.Data[followWaypoints2DComponent.CurrentCell.X, followWaypoints2DComponent.CurrentCell.Y] = this;
            }
        }

        private float Aim(Node2D _target)
        {
            return GetAngleTo(_target.GlobalPosition);
        }

        private async Task Fire(float shots, float timeBetweenShots)
        {
            if (muzzle == null || target == null) return;

            isFiring = true;

            for (int i = 0; i < shots; i++)
            {
                if (!IsInstanceValid(target)) return;
                
                Rotate(Aim(target));
                weaponComponent.Fire();

                shotTimer.Start(timeBetweenShots);
                await ToSignal(shotTimer, Timer.SignalName.Timeout);
            }

            isFiring = false;
        }

        private async void OnEndOfPath()
        {
            await Fire(characterResource.shots, characterResource.timeBetweenShots);
        }

        private async void OnEndOfWaypoints()
        {
            await Fire(Mathf.Inf, characterResource.timeBetweenShots);
        }
    }
}
