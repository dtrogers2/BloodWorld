using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAI
{
    public bool turn(Creature me, Creature tgt, IGame game, out float actionCost);

}
