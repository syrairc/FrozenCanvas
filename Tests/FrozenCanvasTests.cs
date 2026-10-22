using System.Linq;
using EmberHUD;
using EmberHUD.Testing;
using EmberHUD.Ui;

namespace FrozenCanvas;

public sealed class FrozenCanvasTests
{
    [Test]
    public void TickAsksForFrozenCanvas()
    {
        TestGame.Set(GameState.Empty);
        Check.That(TestGame.FrozenCanvasAskedDuring(new Main().Tick));
    }

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

    [Test]
    public void RenderDrawsNothingWithoutState()
    {
        TestGame.Set(new GameState { FrozenCanvas = null });
        Check.Equal(0, TestFrame.Vertices(new Main().Render));
    }

    static FrozenCanvasState Battle() => new(FrozenCanvasPhase.Battle, 3,
        [new FrozenCanvasPlot(0, 0, 0, FrozenCanvasPlotKind.Free, 0, 0, true, false), new FrozenCanvasPlot(7, 1, 1, FrozenCanvasPlotKind.Ruins, 5, 2, false, true)],
        11, 36, 12, 6, 6, [new FrozenCanvasReward(100, 2)])
    {
        Objective = new FrozenCanvasObjective("Kill 10 monsters", 4, 42.5f),
    };

    [Test]
    public void RowsListThePhaseAndCounters()
    {
        var rows = Main.Rows(Battle(), 560, TextStyle.Hud());
        Check.That(rows.Any(r => r.Left == "Phase" && r.Right.Contains("Battle")), "phase");
        Check.That(rows.Any(r => r.Left.Contains("Cells") && r.Right.Contains("11")), "cells");
        Check.That(rows.Any(r => r.Right.Contains("6/6")), "challenges");
        Check.That(rows.Any(r => r.Left.Contains("Servants") && r.Right == "1"), "servants");
        Check.That(rows.Any(r => r.Left.Contains("Player") && r.Right.Contains('7')), "player slot");
        Check.That(rows.Any(r => r.Left.Contains("Kill 10 monsters")), "objective");
    }

    [Test]
    public void RowsWithoutDrawSkipTheDrawBlock()
    {
        var rows = Main.Rows(Battle(), 560, TextStyle.Hud());
        Check.That(!rows.Any(r => r.Left.StartsWith("Screen")), "no draw rows");
    }

    [Test]
    public void RowsWithDrawListTheDrawBlock()
    {
        var draw = new FrozenCanvasDraw(FrozenCanvasScreen.Placing, 3, 12, 4, 1, 2, 0, 3, 0, true, "Place 3", "", [], [new FrozenCanvasPaper(1, "Plain", "", 10, false, true)], 4);
        var rows = Main.Rows(Battle() with { Phase = FrozenCanvasPhase.Draw, Draw = draw }, 560, TextStyle.Hud());
        Check.That(rows.Any(r => r.Left == "Screen" && r.Right == "Placing"), "screen");
        Check.That(rows.Any(r => r.Left.Contains("Placed") && r.Right == "3 / 12"), "placed");
    }
}
