using Godot;

[GlobalClass]
public partial class PlayerResource : Resource
{
    [Export] public int StartingHealth { get; private set; }

    public bool IsAlive;

    private int currentHealth;

    [Signal] public delegate void HealthChangedEventHandler();
    [Signal] public delegate void DieEventHandler();

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            EmitSignal(SignalName.HealthChanged);
            if (currentHealth <= 0)
            {
                IsAlive = false;
                EmitSignal(SignalName.Die);
            }
        }
    }
}
