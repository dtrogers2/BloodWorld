using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICmd
{
    public bool exc();
    public bool turn(out float actionCost);
    public bool raw();
    public Creature me { get; }
    public IGame game { get; }
    public ICmd setDir(Vector2Int dir);
}
