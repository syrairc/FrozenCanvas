using EmberHUD;

namespace FrozenCanvas;

public sealed class Main : Plugin<Settings>
{
    public override void Render()
    {
        if (!Settings.Enabled) return;
    }
}
