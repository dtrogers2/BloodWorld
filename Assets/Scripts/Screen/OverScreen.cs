using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverScreen : IScreen
{
    public string name { get; set; } = "gameover";
    public IScreenMaker screenMaker { get; set; }
    public OverScreen(IScreenMaker m)
    {
        this.screenMaker = m;
    }

    public void draw(ITerm term)
    {
        term.txt(1, 1, "You died!", ColorHex.Red_Bright, ColorHex.Black, "");
    }

    public void onKey(KeyCode keycode, IStack stack)
    {
        stack.pop();
        stack.push(screenMaker.newGame());
    }

    public void onKey(KeyCode keycode)
    {
        throw new System.NotImplementedException();
    }
}
