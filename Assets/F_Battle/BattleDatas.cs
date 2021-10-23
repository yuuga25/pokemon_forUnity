using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDatas
{
    public static UserData_Pokémon[] user_PokemonData = new UserData_Pokémon[3];
    public static UserData_Pokémon[] enemy_PokemonData = new UserData_Pokémon[3];
    public static BattleStatus user_BattleStatus;
    public static BattleStatus enemy_BattleStatus;

    public static OthersStatus[] user_OthersStatus = new OthersStatus[3];
    public static OthersStatus[] enemy_OthersStatus = new OthersStatus[3];

    public static IndividualFields user_Fields = new IndividualFields();
    public static IndividualFields enemy_Fields = new IndividualFields();

    public static CommonField commonField = new CommonField();
}

//バトル場に出ているポケモンのステータス
public class BattleStatus
{
    public int AscendingRank_Atk;   //上昇ランク（攻撃）
    public int AscendingRank_Def;   //上昇ランク（防御）
    public int AscendingRank_Sat;   //上昇ランク（特攻）
    public int AscendingRank_Sde;   //上昇ランク（特防）
    public int AscendingRank_Spe;   //上昇ランク（素早さ）
    public int AscendingRank_Hit;   //上昇ランク（命中）
    public int AscendingRank_Avo;   //上昇ランク（回避）

    public int veryPoisonousTurn;   //猛毒ターン
    public int protectTrun = 0;         //守る・こらえるターン
    public int participationTurn;   //場に出ているターン

    public int sleepinessTurn;      //眠気ターン

    public int reverseScaleTurn;    //げきりんターン

    public bool protect = false;            //守る
    public bool frightened = false;         //ひるみ

    public int discerningID = 0;        //こだわり状態の技のID
}

//個別のポケモンのステータス
public class OthersStatus
{
    public int hp;      //変動するHP

    public int pp_Technique1;       //技1のPP
    public int pp_Technique2;       //技2のPP
    public int pp_Technique3;       //技3のPP
    public int pp_Technique4;       //技4のPP

    //状態f
    public BattleEnum.condition condition;  //状態異常

    public int sleepTurn;           //眠りターン
    public int iceTurn;             //氷ターン

    public string b_item;           //持ち物
}

public class IndividualFields
{
    public bool stealthRock;
}

public class CommonField
{
    public bool trickRoom;
    public int trickRoomTurn;
}

public class BattleEnum
{
    public enum condition
    {
        none,               //なし
        paralysis,          //麻痺
        ice,                //こおり
        burn,               //やけど
        poison,             //どく
        veryPoisonous,      //猛毒
        sleep,              //眠り
        dying               //瀕死
    }
}