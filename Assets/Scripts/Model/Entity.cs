using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public Vector3Int position { get; set; }
    public string name { get; }
    //public Element element { get; }
    //public State state { get; }
    //public Element stains { get; }
    // Use-volume, not literal volume. i.e. a sword has low game volume because we care about its surface area and cutting power than the whole thing.
    //public float volume { get; }
    // Mass correlates with forced movement and damage when force is applied
    //public float mass { get; }
    // Damp decreases force applied to it, scaling with volume.
    //public float damp { get; }
    // Damp decreases force applied to it, inversely with volume
    //public float deflect { get; }
    public char glyph { get; }
    public string color { get; }
}
