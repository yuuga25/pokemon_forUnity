using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScript_Re : MonoBehaviour
{
    public BattleController_Re battleController;
    public TechniqueScript techniqueScript;
    public ReplaceScript_Re replaceScript;
    public ResultScript resultScript;

    public List<AudioClip> clip_INOUT;

    [HideInInspector]
    public BattleStatus temporary_BattleData;
    [HideInInspector]
    public BattleDataStorage[] battleDataStorages = new BattleDataStorage[2];
    [HideInInspector]
    public bool isDone;
    [HideInInspector]
    public List<string> message_Tech = new List<string>();
    [HideInInspector]
    public int keyPoint;

    public float randomNumber;
    public bool isEnemy;

    public bool isWin = false;
    public bool isLose = false;

    public List<AudioClip> clip_Damage = new List<AudioClip>();

    [Header("リザルト")]
    public GameObject obj_Result;
    public List<Sprite> sprite_Result;
}
