using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class LoadData : MonoBehaviour
{
    public static List<SaveData_Pokémon> saveData;
    public static List<UserData_Pokémon> userData_s;

    //性別ごとに姿違いが存在するポケモン
    public static int[] ChangeImageID = { 3, 25, 322, 323, 443, 444, 445 };

    [SerializeField] private string value_Id;

    [SerializeField] private Text text;
    private int pokeNo = 0;
    private void Start()
    {
        Screen.SetResolution(576, 1024, false, 60);
    }
    public void GetTitleData_P()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (result.Data.ContainsKey(value_Id))
                {
                    var dataMaster = PlayFabSimpleJson.DeserializeObject<List<SaveData_Pokémon>>(result.Data[value_Id]);
                    saveData = new List<SaveData_Pokémon>();
                    print(dataMaster.Count);
                    print(saveData.Count);
                    for(var i = 0; i < dataMaster.Count; i++)
                    {
                        saveData.Add(dataMaster[i]);
                    }
                    print("タイトルデータのロード成功");
                }
            },
            error => { Debug.Log(error.GenerateErrorReport()); });
    }

    public void LoadSaveData(int charaNum)
    {
        text.text = $"ポケモンのId:{saveData[charaNum].p_Id}" +
            $"\nポケモンの名前:{saveData[charaNum].p_Name}" +
            $"\nタイプ１:{saveData[charaNum].p_Type1}" +
            $"\nタイプ２:{saveData[charaNum].p_Type2}" +
            $"\n分類;{saveData[charaNum].p_Classification}ポケモン" +
            $"\n高さ:{saveData[charaNum].p_Height}" +
            $"\n重さ:{saveData[charaNum].p_Weight}" +
            $"\nポケモンのHp:{saveData[charaNum].race_H}" +
            $"\nポケモンのA:{saveData[charaNum].race_A}" +
            $"\nポケモンのB:{saveData[charaNum].race_B}" +
            $"\nポケモンのC:{saveData[charaNum].race_C}" +
            $"\nポケモンのD:{saveData[charaNum].race_D}" +
            $"\nポケモンのS:{saveData[charaNum].race_S}" +
            $"\n経験値タイプ:{saveData[charaNum].p_ExperienceType}万タイプ" +
            $"\n特性:{saveData[charaNum].p_Characteristic}" +
            $"\n隠れ特性:{saveData[charaNum].p_Characteristic_Dream}" +
            $"\n♂確率:{saveData[charaNum].p_MaleProbability}" +
            $"\n♀確率:{100 - saveData[charaNum].p_MaleProbability}" +
            $"\nレベル進化の場合の進化先ID:{saveData[charaNum].p_EvolutionDN}";
        pokeNo = charaNum;
    }

    public void SetUserData()
    {
        for(var charaNum = 0; charaNum < saveData.Count; charaNum++)
        {
            print($"ポケモンのId:{saveData[charaNum].p_Id}" +
            $"\nポケモンの名前:{saveData[charaNum].p_Name}" +
            $"\nポケモンのHp:{saveData[charaNum].race_H}" +
            $"\nポケモンのA:{saveData[charaNum].race_A}" +
            $"\nポケモンのB:{saveData[charaNum].race_B}" +
            $"\nポケモンのC:{saveData[charaNum].race_C}" +
            $"\nポケモンのD:{saveData[charaNum].race_D}" +
            $"\nポケモンのS:{saveData[charaNum].race_S}");
        }
        var pokeData = saveData;
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    { "poke_UserData", PlayFabSimpleJson.SerializeObject(pokeData) }
                }
            },
        result => { Debug.Log("プレイヤーデータの登録成功"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }

    //ポケモンステータス計算
    //ポケモンをボックスに生成する際に使う
    public void SetStatus(int pokeNum)
    {
        pokeNum = pokeNo;

        var pokemon = new UserData_Pokémon();

        pokemon.userP_Id = saveData[pokeNum].p_Id;

        //名前
        pokemon.userP_Name = saveData[pokeNum].p_Name;

        //ニックネーム
        pokemon.userP_NickName = saveData[pokeNum].p_Name;

        //性別
        var r1 = Random.Range(1.0f, 100.0f);
        {
            if(255 == saveData[pokeNum].p_MaleProbability)
            {
                pokemon.userP_gender = 2;
            }
            else if(r1 <= saveData[pokeNum].p_MaleProbability)//オス
            {
                pokemon.userP_gender = 0;
            }
            else if(r1 > saveData[pokeNum].p_MaleProbability)//メス
            {
                pokemon.userP_gender = 1;
            }
        }

        //タイプ ※詳細はUserData_Pokémonクラス
        {
            switch (saveData[pokeNum].p_Type1)
            {
                case "ノーマル":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Normal;
                    break;
                case "ほのお":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Fire;
                    break;
                case "みず":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Water;
                    break;
                case "でんき":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Electric;
                    break;
                case "くさ":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Grass;
                    break;
                case "こおり":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Ice;
                    break;
                case "かくとう":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Fighting;
                    break;
                case "どく":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Poison;
                    break;
                case "じめん":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Ground;
                    break;
                case "ひこう":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Flying;
                    break;
                case "エスパー":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Psychic;
                    break;
                case "むし":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Bug;
                    break;
                case "いわ":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Rock;
                    break;
                case "ゴースト":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Ghost;
                    break;
                case "ドラゴン":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Dragon;
                    break;
                case "あく":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Dark;
                    break;
                case "はがね":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Steel;
                    break;
                case "フェアリー":
                    pokemon.userP_Type1 = Pokémon_Type.Type.Fairy;
                    break;
                case "":
                    Debug.LogError("タイプが設定されていません");
                    break;
                default:
                    pokemon.userP_Type1 = Pokémon_Type.Type.Normal;
                    break;
            }
            switch (saveData[pokeNum].p_Type2)
            {
                case "ノーマル":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Normal;
                    break;
                case "ほのお":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Fire;
                    break;
                case "みず":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Water;
                    break;
                case "でんき":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Electric;
                    break;
                case "くさ":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Grass;
                    break;
                case "こおり":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Ice;
                    break;
                case "かくとう":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Fighting;
                    break;
                case "どく":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Poison;
                    break;
                case "じめん":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Ground;
                    break;
                case "ひこう":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Flying;
                    break;
                case "エスパー":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Psychic;
                    break;
                case "むし":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Bug;
                    break;
                case "いわ":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Rock;
                    break;
                case "ゴースト":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Ghost;
                    break;
                case "ドラゴン":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Dragon;
                    break;
                case "あく":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Dark;
                    break;
                case "はがね":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Steel;
                    break;
                case "フェアリー":
                    pokemon.userP_Type2 = Pokémon_Type.Type.Fairy;
                    break;
                case "":
                    pokemon.userP_Type2 = Pokémon_Type.Type.None;
                    break;
                default:
                    pokemon.userP_Type1 = Pokémon_Type.Type.Normal;
                    break;
            }
        }

        //レベル(初期値 0)
        pokemon.userP_Level = 50;
        pokemon.userP_Level = Mathf.Clamp(pokemon.userP_Level, 1, 100);

        //性格 ※詳細はUserData_Pokémonクラス
        {
            var r2 = Random.Range(1, 26);
            {
                pokemon.userP_Personality = r2;
            }

        }


        //所持経験値(初期値 0)
        if(pokemon.userP_Level == 0)
        {
            pokemon.userP_ExperiencePoint = 0;
        }
        else
        {
            switch (saveData[pokeNum].p_ExperienceType)
            {
                case 60:
                    if (2 <= pokemon.userP_Level && pokemon.userP_Level <= 50)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * (100 - pokemon.userP_Level) / 50);
                    }
                    else if (50 <= pokemon.userP_Level && pokemon.userP_Level <= 68)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * (150 - pokemon.userP_Level) / 100);
                    }
                    else if (68 <= pokemon.userP_Level && pokemon.userP_Level <= 98)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * Mathf.FloorToInt(637 - 10 * pokemon.userP_Level / 3) / 500);
                    }
                    else if (98 <= pokemon.userP_Level && pokemon.userP_Level <= 100)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * (160 - pokemon.userP_Level) / 100);
                    }
                    break;
                case 80:
                    pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * 0.8f);
                    break;
                case 100:
                    pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3));
                    break;
                case 105:
                    pokemon.userP_ExperiencePoint = Mathf.FloorToInt(1.2f * Mathf.Pow(pokemon.userP_Level, 3) - (15 * Mathf.Pow(pokemon.userP_Level, 2)) + (100 * pokemon.userP_Level - 140));
                    break;
                case 125:
                    pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * 1.25f);
                    break;
                case 164:
                    if (2 <= pokemon.userP_Level && pokemon.userP_Level <= 15)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * (24 + Mathf.FloorToInt((pokemon.userP_Level + 1) / 3)) / 50);
                    }
                    else if (15 <= pokemon.userP_Level && pokemon.userP_Level <= 36)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * (14 + pokemon.userP_Level) / 50);
                    }
                    else if (36 <= pokemon.userP_Level && pokemon.userP_Level <= 100)
                    {
                        pokemon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level, 3) * (32 + Mathf.FloorToInt(pokemon.userP_Level / 2)) / 50);
                    }
                    break;
            }
        }
        print(pokemon.userP_ExperiencePoint);
        
        //次のレベルまで
        {
            switch (saveData[pokeNum].p_ExperienceType)
            {
                case 60:
                    if (2 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 50)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * (100 - (pokemon.userP_Level + 1)) / 50);
                    }
                    else if (50 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 68)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * (150 - (pokemon.userP_Level + 1)) / 100);
                    }
                    else if (68 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 98)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * Mathf.FloorToInt(637 - 10 * (pokemon.userP_Level + 1) / 3) / 500);
                    }
                    else if (98 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 100)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * (160 - (pokemon.userP_Level + 1)) / 100);
                    }
                    break;
                case 80:
                    pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * 0.8f);
                    break;
                case 100:
                    pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3));
                    break;
                case 105:
                    pokemon.userP_UntilLevelUp = Mathf.FloorToInt(1.2f * Mathf.Pow(pokemon.userP_Level + 1, 3) - (15 * Mathf.Pow(pokemon.userP_Level + 1, 2)) + (100 * (pokemon.userP_Level + 1) - 140));
                    print(pokemon.userP_UntilLevelUp);
                    break;
                case 125:
                    pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * 1.25f);

                    break;
                case 164:
                    if (2 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 15)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * (24 + Mathf.FloorToInt(((pokemon.userP_Level + 1) + 1) / 3)) / 50);
                    }
                    else if (15 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 36)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * (14 + (pokemon.userP_Level + 1)) / 50);
                    }
                    else if (36 <= pokemon.userP_Level + 1 && pokemon.userP_Level + 1 <= 100)
                    {
                        pokemon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokemon.userP_Level + 1, 3) * (32 + Mathf.FloorToInt((pokemon.userP_Level + 1) / 2)) / 50);
                    }
                    break;
            }
        }


        /*特性
        {
            if(saveData[pokeNum].p_Characteristic2 != "")
            {
                var r3 = Random.Range(1, 101);
                if(r3 <= 50)
                {
                    pokemon.userP_Characteristic = saveData[pokeNum].p_Characteristic1;
                }
                else if(r3 > 50)
                {
                    pokemon.userP_Characteristic = saveData[pokeNum].p_Characteristic2;
                }
            }
            else
            {
                pokemon.userP_Characteristic = saveData[pokeNum].p_Characteristic1;
            }
        }
        */


        //色違い確率
        {
            var r4 = Random.Range(1, 4097);
            if(r4 == 666)
            {
                pokemon.isDifferentColors = true;
            }
            else
            {
                pokemon.isDifferentColors = false;
            }
        }

        /*
        //個体値
        {
            pokemon.userP_Individual_Hp = 31;
            pokemon.userP_Individual_A = 31;
            pokemon.userP_Individual_B = 31;
            pokemon.userP_Individual_C = 31;
            pokemon.userP_Individual_D = 31;
            pokemon.userP_Individual_S = 31;

            for(var i = 0; i < 3; i++)
            {
                var r5 = Random.Range(1, 7);
                switch (r5)
                {
                    case 1:
                        pokemon.userP_Individual_Hp = Random.Range(0, 32);
                        break;
                    case 2:
                        pokemon.userP_Individual_A = Random.Range(0, 32);
                        break;
                    case 3:
                        pokemon.userP_Individual_B = Random.Range(0, 32);
                        break;
                    case 4:
                        pokemon.userP_Individual_C = Random.Range(0, 32);
                        break;
                    case 5:
                        pokemon.userP_Individual_D = Random.Range(0, 32);
                        break;
                    case 6:
                        pokemon.userP_Individual_S = Random.Range(0, 32);
                        break;
                }
            }
        }
        */

        //努力値
        {
            pokemon.userP_Effort_Hp = 0;
            pokemon.userP_Effort_A = 0;
            pokemon.userP_Effort_B = 0;
            pokemon.userP_Effort_C = 0;
            pokemon.userP_Effort_D = 0;
            pokemon.userP_Effort_S = 0;
        }

        //実数値
        {
            //Hp
            pokemon.userP_Real_Hp = Mathf.FloorToInt((saveData[pokeNum].race_H * 2 + pokemon.userP_Individual_Hp + pokemon.userP_Effort_Hp / 4) * pokemon.userP_Level / 100 + pokemon.userP_Level + 10);
            pokemon.userP_CurrentHp = pokemon.userP_Real_Hp;
            //攻撃
            {
                var poke_a = Mathf.FloorToInt((saveData[pokeNum].race_A * 2 + pokemon.userP_Individual_A + pokemon.userP_Effort_A / 4) * pokemon.userP_Level / 100 + 5);
                if (pokemon.userP_Personality == 1 || pokemon.userP_Personality == 2 || pokemon.userP_Personality == 3 || pokemon.userP_Personality == 4)
                {
                    poke_a = Mathf.FloorToInt(poke_a * 1.1f);
                }
                else if (pokemon.userP_Personality == 5 || pokemon.userP_Personality == 9 || pokemon.userP_Personality == 13 || pokemon.userP_Personality == 17)
                {
                    poke_a = Mathf.FloorToInt(poke_a * 0.9f);
                }
                pokemon.userP_Real_A = Mathf.FloorToInt(poke_a);
            }
            //防御
            {
                var poke_b = Mathf.FloorToInt((saveData[pokeNum].race_B * 2 + pokemon.userP_Individual_B + pokemon.userP_Effort_B / 4) * pokemon.userP_Level / 100 + 5);
                if (pokemon.userP_Personality == 5 || pokemon.userP_Personality == 6 || pokemon.userP_Personality == 7 || pokemon.userP_Personality == 8)
                {
                    poke_b = Mathf.FloorToInt(poke_b * 1.1f);
                }
                else if (pokemon.userP_Personality == 1 || pokemon.userP_Personality == 10 || pokemon.userP_Personality == 14 || pokemon.userP_Personality == 18)
                {
                    poke_b = Mathf.FloorToInt(poke_b * 0.9f);
                }
                pokemon.userP_Real_B = Mathf.FloorToInt(poke_b);
            }
            //特攻
            {
                var poke_c = Mathf.FloorToInt((saveData[pokeNum].race_C * 2 + pokemon.userP_Individual_C + pokemon.userP_Effort_C / 4) * pokemon.userP_Level / 100 + 5);
                if (pokemon.userP_Personality == 9 || pokemon.userP_Personality == 10 || pokemon.userP_Personality == 11 || pokemon.userP_Personality == 12)
                {
                    poke_c = Mathf.FloorToInt(poke_c * 1.1f);
                }
                else if (pokemon.userP_Personality == 2 || pokemon.userP_Personality == 6 || pokemon.userP_Personality == 15 || pokemon.userP_Personality == 19)
                {
                    poke_c = Mathf.FloorToInt(poke_c * 0.9f);
                }
                pokemon.userP_Real_C = Mathf.FloorToInt(poke_c);
            }
            //特防
            {
                var poke_d = Mathf.FloorToInt((saveData[pokeNum].race_D * 2 + pokemon.userP_Individual_D + pokemon.userP_Effort_D / 4) * pokemon.userP_Level / 100 + 5);
                if (pokemon.userP_Personality == 13 || pokemon.userP_Personality == 14 || pokemon.userP_Personality == 15 || pokemon.userP_Personality == 16)
                {
                    poke_d = Mathf.FloorToInt(poke_d * 1.1f);
                }
                else if (pokemon.userP_Personality == 3 || pokemon.userP_Personality == 7 || pokemon.userP_Personality == 11 || pokemon.userP_Personality == 20)
                {
                    poke_d = Mathf.FloorToInt(poke_d * 0.9f);
                }
                pokemon.userP_Real_D = Mathf.FloorToInt(poke_d);
            }
            //素早さ
            {
                var poke_s = Mathf.FloorToInt((saveData[pokeNum].race_S * 2 + pokemon.userP_Individual_S + pokemon.userP_Effort_S / 4) * pokemon.userP_Level / 100 + 5);
                if (pokemon.userP_Personality == 17 || pokemon.userP_Personality == 18 || pokemon.userP_Personality == 19 || pokemon.userP_Personality == 20)
                {
                    poke_s = Mathf.FloorToInt(poke_s * 1.1f);
                }
                else if (pokemon.userP_Personality == 4 || pokemon.userP_Personality == 8 || pokemon.userP_Personality == 12 || pokemon.userP_Personality == 16)
                {
                    poke_s = Mathf.FloorToInt(poke_s * 0.9f);
                }
                pokemon.userP_Real_S = Mathf.FloorToInt(poke_s);
            }
        }

        userData_s.Add(pokemon);

        LoadStatus(0);
    }

    public void LoadStatus(int pokeNum)
    {
        text.text=($"名前：{userData_s[pokeNum].userP_Name}" +
            $"\n実数値HP：{userData_s[pokeNum].userP_Real_Hp}" +
            $"\n実数値A：{userData_s[pokeNum].userP_Real_A}" +
            $"\n実数値B：{userData_s[pokeNum].userP_Real_B}" +
            $"\n実数値C：{userData_s[pokeNum].userP_Real_C}" +
            $"\n実数値D：{userData_s[pokeNum].userP_Real_D}" +
            $"\n実数値S：{userData_s[pokeNum].userP_Real_S}" +
            $"\n次のレベルまで：{userData_s[pokeNum].userP_UntilLevelUp}" +
            $"\n特性：{userData_s[pokeNum].userP_Characteristic}");
    }
}