using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScreen : IScreen
{
    public string name { get; set; } = "dummy-screen";
    public IScreenMaker screenMaker { get; set; }

    public DummyScreen(IScreenMaker screenMaker)
    {
        this.screenMaker = screenMaker;
    }

    public void draw(ITerm term)
    {
        term.txt(1, 1, "Press a key!", ColorHex.Cyan_Bright, ColorHex.Blue_Dark, " ");
    }

    public void onKey(KeyCode keycode, IStack stack)
    {
        stack.pop();
        stack.push(screenMaker.gameOver());
    }

    public void onKey(KeyCode keycode)
    {
        throw new System.NotImplementedException();
    }

}
