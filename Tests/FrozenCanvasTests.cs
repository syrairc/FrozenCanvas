using EmberHUD;
using EmberHUD.Testing;

namespace FrozenCanvas;

public sealed class FrozenCanvasTests
{
    [Test]
    public void TheSettingsPageDraws()
    {
        Check.That(TestFrame.Vertices(new Settings().Draw, inWindow: true) > 0, "settings page draws");
    }

    [Test]
    public void RenderDrawsNothing()
    {
        TestGame.Set(GameState.Empty);
        Check.Equal(0, TestFrame.Vertices(new Main().Render));
    }
}
