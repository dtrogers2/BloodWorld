using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : BaseScreen
{
    public string name { get; } = "game";
    public GameScreen(IGame game, IScreenMaker maker) : base(game, maker) {}

    public override void onKey(KeyCode keyCode, IStack stack)
    {
        playerKey(keyCode, stack);
    }

    public void playerKey(KeyCode keyCode, IStack stack)
    {
        if (game.log != null) { game.log.clearQueue(); }
        if (playerTurn(keyCode, stack, out float actionCost))
        {
            game.player.actionCost += actionCost;
            game.time += actionCost;
            npcTurns(stack, actionCost);
        }
    }

    public bool playerTurn(KeyCode keyCode, IStack stack, out float actionCost)
    {
        ParseCommand parser = new ParseCommand(game, maker);
        return parser.parseKeycodeTurn(keyCode, stack, out actionCost);
    }

}
