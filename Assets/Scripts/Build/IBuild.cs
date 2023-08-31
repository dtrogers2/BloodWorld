using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public interface IBuild
{
    public IGame makeGame();
    public IRegion makeLevel(IGame game, Vector3Int regionPos, bool addMobs = false);
}

public interface IBuild1 : IBuild
{
    public Creature makePlayer();
}