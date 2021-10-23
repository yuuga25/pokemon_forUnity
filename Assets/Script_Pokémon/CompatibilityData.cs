using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObject/c_Data")]
public class CompatibilityData : ScriptableObject
{
    public List<Compatibility> sheet;

    [Serializable]
    public class Compatibility
    {
        public string typeName;
        public Pokémon_Type.Type type;
        public List<Pokémon_Type.Type> twice;
        public List<Pokémon_Type.Type> half;
        public List<Pokémon_Type.Type> invalid;
    }
}
