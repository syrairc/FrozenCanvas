using EmberHUD;
using EmberHUD.Ui;

namespace FrozenCanvas;

public sealed class Settings : PluginSettings
{
    public bool Enabled = true;

    public override void Draw()
    {
        Setting.Checkbox("Enabled", ref Enabled);
    }
}
