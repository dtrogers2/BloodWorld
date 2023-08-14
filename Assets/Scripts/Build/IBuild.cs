using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public interface IBuild
{
    public IGame makeGame();
    public IRegion makeLevel(Rng rng, Vector3Int worldPos);
    public IRegion makeMap(Rng rng, Vector3Int worldPos);
}

public interface IBuild1 : IBuild
{
    public Creature makePlayer();
}