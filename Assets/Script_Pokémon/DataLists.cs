using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab.ClientModels;

public class DataLists
{
    public static List<SaveData_Pokémon> titleData_Pokémon;
    public static PlayerData playerData = new PlayerData();
    //チュートリアルが終わったかどうか
    public static string isTutorialCompleted;

    public static List<Remember_Pokémon> titleData_Remember;
    public static List<Technique_Pokémon> titleData_Technique;

    public static List<ItemInstance> playerData_Inventry;
    public static int player_Money;
    public static int player_BattlePoint;

    public static List<CatalogItem> catalogData;

    public static List<HelpData> titleData_helpDatas;
    public static List<StageData> titleData_StageData;
    public static List<CharacteristicData> titleData_CharacteristicDatas;
}

public class PlayerData
{
    /*
    プレイヤーの名前
    ランク
    所持ポケモン
    チームリスト
     */

    public string user_Name;
    public int user_Rank;
    public List<int> picList = new List<int>();

    public List<UserData_Pokémon> pokémonsList = new List<UserData_Pokémon>();
    public TeamData_Pokémon teamDatas = new TeamData_Pokémon();
}
public class TeamData_Pokémon
{
    public UserData_Pokémon[] pokémons = new UserData_Pokémon[6];

    public string b_item1;
    public string b_item2;
    public string b_item3;
    public string b_item4;
    public string b_item5;
    public string b_item6;
}

public class UserData_Pokémon
{
    public bool isFavorite { get; set; }//true = お気に入り : false = なし
    public string unique_Id { get; set; }//ユニークID
    public int userP_Id { get; set; }//Id
    public string userP_Name { get; set; } //名前
    public string userP_NickName { get; set; } //ニックネーム
    public Pokémon_Ball.Ball userP_Ball { get; set; } //入っているボール
    public int userP_gender { get; set; } //性別: 0 = オス♂ : 1 = メス♀ : 2 = 性別なし
    public Pokémon_Type.Type userP_Type1 { get; set; } //タイプ１
    public Pokémon_Type.Type userP_Type2 { get; set; } //タイプ２
    public int userP_Level { get; set; } //レベル
    public int userP_Personality { get; set; } //性格
    /*Valueに対応する性格一覧 
    
        1  = さみしがり
        2  = いじっぱり
        3  = やんちゃ
        4  = ゆうかん
        5  = ずぶとい
        6  = わんぱく
        7  = のうてんき
        8  = のんき
        9  = ひかえめ
        10 = おっとり
        11 = うっかりや
        12 = れいせい
        13 = おだやか
        14 = おとなしい
        15 = しんちょう
        16 = なまいき
        17 = おくびょう
        18 = せっかち
        19 = ようき
        20 = むじゃき
        21 = てれや
        22 = がんばりや
        23 = すなお
        24 = きまぐれ
        25 = まじめ

     */
    public int userP_ExperiencePoint { get; set; } //所持経験値
    public int userP_UntilLevelUp { get; set; } //次のレベルまで
    public string userP_Characteristic { get; set; } //特性
    public bool isDreamCharacteristic { get; set; } = false; //夢特性かどうか
    public bool isDifferentColors { get; set; } //true = 色違い : false = 通常
    public int userP_CurrentHp { get; set; } //現在のHp
    public int userP_ELevel { get; set; }//強化レベル

    //個体値
    public int userP_Individual_Hp { get; set; }
    public int userP_Individual_A { get; set; }
    public int userP_Individual_B { get; set; }
    public int userP_Individual_C { get; set; }
    public int userP_Individual_D { get; set; }
    public int userP_Individual_S { get; set; }

    //努力値
    public int userP_Effort_Hp { get; set; }
    public int userP_Effort_A { get; set; }
    public int userP_Effort_B { get; set; }
    public int userP_Effort_C { get; set; }
    public int userP_Effort_D { get; set; }
    public int userP_Effort_S { get; set; }

    //実数値
    public int userP_Real_Hp { get; set; }
    public int userP_Real_A { get; set; }
    public int userP_Real_B { get; set; }
    public int userP_Real_C { get; set; }
    public int userP_Real_D { get; set; }
    public int userP_Real_S { get; set; }

    //技
    public string set_Technique1 { get; set; }
    public string set_Technique2 { get; set; }
    public string set_Technique3 { get; set; }
    public string set_Technique4 { get; set; }
    public int set_TechniqueID1 { get; set; }
    public int set_TechniqueID2 { get; set; }
    public int set_TechniqueID3 { get; set; }
    public int set_TechniqueID4 { get; set; }
}
public class Pokémon_Type
{
    public enum Type
    {
        None,
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Steel,
        Fairy
    }
}
public class Pokémon_Ball
{
    public enum Ball
    {
        Poké,
        Great,
        Ultra,
        Master
    }
}

//ポケモンのタイトルデータ
public class SaveData_Pokémon
{
    public string p_Name { get; set; }//名前
    public int p_Id { get; set; }//図鑑番号
    public string p_Type1 { get; set; }//タイプ１
    public string p_Type2 { get; set; }//タイプ２
    public string p_Classification { get; set; }//分類
    public string p_Explanation { get; set; }//図鑑説明
    public float p_Height { get; set; }//高さ
    public float p_Weight { get; set; }//重さ
    public int race_H { get; set; }//種族値_HP
    public int race_A { get; set; }//種族値_攻撃
    public int race_B { get; set; }//種族値_防御
    public int race_C { get; set; }//種族値_特攻
    public int race_D { get; set; }//種族値_特防
    public int race_S { get; set; }//種族値_素早さ
    public int p_ExperienceType { get; set; }//経験値タイプ
    public string p_Characteristic { get; set; }//特性
    public string p_Characteristic_Dream { get; set; }//隠れ特性
    public float p_MaleProbability { get; set; }//オスの確率
    public int p_EvolutionDN { get; set; }//レベル進化の場合の進化先ID
    public bool p_GenderType { get; set; }//姿が性別ごとに違いがあるか
}

public class CustomData
{
    public string icon;
}

public class HelpData
{
    public string title;
    public string contents;
}

public class StageData
{
    public int StageId;
    public string s_Name;
    public int p1_Id;
    public int p2_Id;
    public int p3_Id;
    public int p4_Id;
    public int p5_Id;
    public int p6_Id;
    public int s_Reward_GD;
    public int s_Reward_BP;
}

public class CharacteristicData
{
    public string c_Name;
    public string c_Explanation;
}