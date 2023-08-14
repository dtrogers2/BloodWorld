using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EventManager
{
    public ITerm term;
    public IScreen screen;
    public EventManager(ITerm term, IScreen screen)
    {
        this.term = term;
        this.screen = screen;
        this.screen.draw(this.term);
        
    }

    public void onKey(KeyCode keyCode)
    {
        this.screen.onKey(keyCode);
        this.screen.draw(this.term);
    }

    public static EventManager runScreen(IScreen screen, Sprite[] sprites)
    {
        return new EventManager(GTerm.StockTerm(sprites), screen);
    }
}
