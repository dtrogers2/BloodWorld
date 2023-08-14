using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public interface IScreenMaker
{
    IScreen newGame();
    IScreen gameOver();
    IScreen more(IGame game);
}

public class ScreenMaker : IScreenMaker
{
    public IGame game;
    public IBuild build;
    public ScreenMaker(IBuild build)
    {
        this.build = build;
    }

    public IScreen more(IGame game)
    {
        return new MoreScreen(game, this);
    }

    public IScreen gameOver()
    {
        return new OverScreen(this);
    }

    public IScreen newGame()
    {
        this.game = build.makeGame();
        return new GameScreen(this.game, this);
    }


    public static EventManager run_GFirst(IScreenMaker m, Sprite[] sprites)
    {
        return ScreenStack.run_SScreen(m.newGame(), sprites);
    }

    public static IScreenMaker StockMaker(IBuild build)
    {
        return new ScreenMaker(build);
    }

    public static EventManager Gfirst(IBuild build, Sprite[] sprites)
    {
        return run_GFirst(StockMaker(build), sprites);
    }

}