using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStack
{
    void pop();
    void push(IScreen screen);
    IScreen cur();
}
