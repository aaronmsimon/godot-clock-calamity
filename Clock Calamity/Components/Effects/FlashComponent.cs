using Godot;
using System.Threading.Tasks;

namespace Components.Effects
{
    [GlobalClass]
    public partial class FlashComponent : Node
    {
        // The flash component uses a flash material. I chose to preload this into a constant
        // But you could also export a material instead to allow the component to use a variety of different materials
        // private Resource flashMaterial = ResourceLoader.Load("res://effects/white_flash_material.tres");
        [Export] private Resource flashMaterial;

        // Export the sprite this compononet will be flashing
        [Export] private CanvasItem sprite;

        // Export a duration for the flash
        [Export] private float flashDuration = 0.2f;

        // We need to store the original sprite's material so we can reset it after the flash
        private Material originalSpriteMaterial;

        // Create a timer for the flash component to use
        private Timer timer = new Timer();

        public override void _Ready()
        {
            // We have to add the timer as a child of this component in order to use it
            AddChild(timer);
            
            // Store the original sprite material
            originalSpriteMaterial = sprite.Material;
        }

        // This is the function we can use to activate this component
        public async Task Flash()
        {
            // Set the sprite's material to the flash material
            sprite.Material = (Material)flashMaterial;
            
            // Start the timer (passing in the flash duration)
            timer.Start(flashDuration);
            
            // Wait until the timer times out
            await ToSignal(timer, Timer.SignalName.Timeout);
            
            // Set the sprite's material back to the original material that we stored
            sprite.Material = originalSpriteMaterial;
        }
    }
}
