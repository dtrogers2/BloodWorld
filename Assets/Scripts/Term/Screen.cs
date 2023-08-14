using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen
{
    public string name { get; set; }
    public void draw(ITerm term);
    public void onKey(KeyCode keycode, IStack stack);

    public void onKey(KeyCode keycode);
}

public class RawTestScreen : IScreen
{
    public string key = "-";
    public string name { get; set; } = "test2";
    public void onKey(KeyCode e)
    {
        this.key = $"?:{e}";
    }

    public void onKey(KeyCode e, IStack stack)
    {
        this.key = $"?:{e}";
    }

    public void draw(ITerm term)
    {
        TestTerm.test2(term, this.key);
    }
}