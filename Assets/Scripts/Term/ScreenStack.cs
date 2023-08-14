using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenStack : IStack, IScreen
{
    public string name { get; set; } = "screenstack";
    public Stack<IScreen> s = new Stack<IScreen>();


    public void pop()
    {
        this.s.Pop();
    }
    public void push(IScreen screen)
    {
        this.s.Push(screen);
    }
    public IScreen cur()
    {
        return this.s.Peek();
    }

    public void draw(ITerm term)
    {
        IScreen screen = this.cur();
        if (screen != null) screen.draw(term);
    }

    public void onKey(KeyCode keycode, IStack stack)
    {
        IScreen screen = this.cur();
        if (screen != null) screen.onKey(keycode, stack);
    }

    public void onKey(KeyCode keycode)
    {
        IScreen screen = this.cur();
        if (screen != null) screen.onKey(keycode, this);
    }

    public static EventManager run_SScreen(IScreen screen, Sprite[] sprites)
    {
        ScreenStack stack = new ScreenStack();
        stack.push(screen);
        return EventManager.runScreen(stack, sprites);
    }

}
