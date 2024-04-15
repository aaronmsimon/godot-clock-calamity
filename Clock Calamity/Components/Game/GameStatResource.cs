using Godot;

namespace Components.Game
{
    [GlobalClass]
    public partial class GameStatResource : Resource
    {
        [ExportCategory("Game Stat Resource")]
        [Export] public string StatName { get; set; }
        [Export] private float _statValue;

        [Signal] public delegate void StatChangedEventHandler();

        public float StatValue
        {
            get
            {
                return _statValue;
            }
            set
            {
                _statValue = value;
                EmitSignal(SignalName.StatChanged);
            }
        }
    }
}
