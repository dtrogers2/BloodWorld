using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAI
{
    public bool turn(uint me, List<uint> nearCreatures, IGame game, out float actionCost);

}
