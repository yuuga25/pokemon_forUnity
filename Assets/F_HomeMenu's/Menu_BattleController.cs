using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Menu_BattleController : MonoBehaviour
{
    public static int StageId;

    private List<int> teamMenberList = new List<int>();

    public GameObject Modeselect;
    public GameObject StageSelect;
    public GameObject Confirmationscreen;

    public Menu_PokéController pokéController;

    [Header("ステージ選択")]
    public GameObject Stage_Unit;
    public GameObject parentObj;

    public AudioClip Decision;

    [Header("確認画面")]
    public Text StageDataDisplayText;

    public GameObject[] pokeObj;
    public GameObject Button;
    public Font textFont;
    public Font valueFont;

    public void SetModeSelect()
    {
        Modeselect.SetActive(true);
        StageSelect.SetActive(false);
        Confirmationscreen.SetActive(false);

        SetStage();
    }

    public void SetStage()
    {
        Modeselect.SetActive(false);
        StageSelect.SetActive(true);
        Confirmationscreen.SetActive(false);

        foreach (Transform u in parentObj.transform)
        {
            Destroy(u.gameObject);
        }

        foreach(var stage in DataLists.titleData_StageData)
        {
            GameObject unit = Instantiate(Stage_Unit, parentObj.transform);

            unit.transform.Find("Text_StageName").GetComponent<Text>().text = stage.s_Name;

            var image = pokéController.imageData.sheet.Find(x => x.p_Id == stage.p6_Id);
            unit.transform.Find("poke1").GetComponent<Image>().sprite = image.p_ImageFront;
            image = pokéController.imageData.sheet.Find(x => x.p_Id == stage.p5_Id);
            unit.transform.Find("poke2").GetComponent<Image>().sprite = image.p_ImageFront;
            image = pokéController.imageData.sheet.Find(x => x.p_Id == stage.p4_Id);
            unit.transform.Find("poke3").GetComponent<Image>().sprite = image.p_ImageFront;
            image = pokéController.imageData.sheet.Find(x => x.p_Id == stage.p3_Id);
            unit.transform.Find("poke4").GetComponent<Image>().sprite = image.p_ImageFront;
            image = pokéController.imageData.sheet.Find(x => x.p_Id == stage.p2_Id);
            unit.transform.Find("poke5").GetComponent<Image>().sprite = image.p_ImageFront;
            image = pokéController.imageData.sheet.Find(x => x.p_Id == stage.p1_Id);
            unit.transform.Find("poke6").GetComponent<Image>().sprite = image.p_ImageFront;

            var tmp = stage.StageId;
            unit.GetComponent<Button>().onClick.AddListener(() => SelectStage(tmp));
            unit.GetComponent<Button>().onClick.AddListener(() => pokéController.audioManager_SE.PlayOneShot(Decision));
        }
    }

    public void SelectStage(int stageId)
    {
        StageId = stageId;
        Modeselect.SetActive(false);
        StageSelect.SetActive(false);
        Confirmationscreen.SetActive(true);

        Button.SetActive(false);

        teamMenberList = new List<int>();

        var stageName = DataLists.titleData_StageData.Find(x => x.StageId == StageId).s_Name;
        StageDataDisplayText.text = $"Stage：　{stageName}";

        for(int i = 0; i < 6; i++)
        {
            if(DataLists.playerData.teamDatas.pokémons[i] == null)
            {
                pokeObj[i].SetActive(false);
            }
            else
            {
                pokeObj[i].SetActive(true);

                var poke = DataLists.playerData.teamDatas.pokémons[i];

                pokeObj[i].transform.Find("Text_NickName").GetComponent<Text>().text = poke.userP_NickName;
                pokeObj[i].transform.Find("Text_Lv").GetComponent<Text>().text = "Lv."+poke.userP_Level.ToString();

                var image = pokéController.imageData.sheet.Find(x => x.p_Id == poke.userP_Id);

                if (poke.isDifferentColors)
                {
                    pokeObj[i].transform.Find("Image_poke").GetComponent<Image>().sprite = image.p_ImageHand_C;
                }
                else
                {
                    pokeObj[i].transform.Find("Image_poke").GetComponent<Image>().sprite = image.p_ImageHand;
                }

                var image_Belongings = pokeObj[i].transform.Find("Image_Belongings").GetComponent<Image>();

                var imageData = pokéController.imageData_Belongings.sheet;
                image_Belongings.enabled = true;
                switch (i)
                {
                    case 0:
                        if (DataLists.playerData.teamDatas.b_item1 != null)
                        {
                            image_Belongings.sprite = imageData.Find(x => x.imageId == DataLists.playerData.teamDatas.b_item1).item_Image;
                        }
                        else image_Belongings.enabled = false;
                        break;
                    case 1:
                        if(DataLists.playerData.teamDatas.b_item2 != null)
                        {
                            image_Belongings.sprite = imageData.Find(x => x.imageId == DataLists.playerData.teamDatas.b_item2).item_Image;
                        }
                        else image_Belongings.enabled = false;
                        break;
                    case 2:
                        if (DataLists.playerData.teamDatas.b_item3 != null)
                        {
                            image_Belongings.sprite = imageData.Find(x => x.imageId == DataLists.playerData.teamDatas.b_item3).item_Image;
                        }
                        else image_Belongings.enabled = false;
                        break;
                    case 3:
                        if (DataLists.playerData.teamDatas.b_item4 != null)
                        {
                            image_Belongings.sprite = imageData.Find(x => x.imageId == DataLists.playerData.teamDatas.b_item4).item_Image;
                        }
                        else image_Belongings.enabled = false;
                        break;
                    case 4:
                        if (DataLists.playerData.teamDatas.b_item5 != null)
                        {
                            image_Belongings.sprite = imageData.Find(x => x.imageId == DataLists.playerData.teamDatas.b_item5).item_Image;
                        }
                        else image_Belongings.enabled = false;
                        break;
                    case 5:
                        if (DataLists.playerData.teamDatas.b_item6 != null)
                        {
                            image_Belongings.sprite = imageData.Find(x => x.imageId == DataLists.playerData.teamDatas.b_item6).item_Image;
                        }
                        else image_Belongings.enabled = false;
                        break;
                }

                var text = pokeObj[i].transform.Find("Election").Find("Text").GetComponent<Text>();

                text.text = "選択";
                text.font = textFont;
                text.fontSize = 60;
            }
        }
    }

    public void SelectTeam(int unitNum)
    {
        if (teamMenberList.Contains(unitNum))
        {
            var text = pokeObj[unitNum - 1].transform.Find("Election").Find("Text").GetComponent<Text>();
            
            teamMenberList.Remove(unitNum);

            for (int u = 0; u < 6; u++)
            {
                if (!teamMenberList.Contains(u + 1))
                {
                    text = pokeObj[u].transform.Find("Election").Find("Text").GetComponent<Text>();

                    text.text = "選択";
                    text.font = textFont;
                    text.fontSize = 60;
                }
            }

            for (int i = 0; i < teamMenberList.Count; i++)
            {
                text = pokeObj[teamMenberList[i]-1].transform.Find("Election").Find("Text").GetComponent<Text>();
                text.text = $"{teamMenberList.IndexOf(teamMenberList[i])+1}";
                text.font = valueFont;
                text.fontSize = 95;
            }

            Button.SetActive(false);
        }
        else
        {
            if(teamMenberList.Count < 3)
            {
                teamMenberList.Add(unitNum);

                var text = pokeObj[unitNum - 1].transform.Find("Election").Find("Text").GetComponent<Text>();

                text.text = $"{teamMenberList.IndexOf(unitNum) + 1}";
                text.font = valueFont;
                text.fontSize = 95;

                if (teamMenberList.Count >= 3)
                {
                    for (int u = 0; u < 6; u++)
                    {
                        if (!teamMenberList.Contains(u + 1))
                        {
                            text = pokeObj[u].transform.Find("Election").Find("Text").GetComponent<Text>();

                            text.text = "×";
                            text.font = valueFont;
                            text.fontSize = 95;
                        }
                    }

                    Button.SetActive(true);
                }
            }
        }
    }

    public void StartGame()
    {
        pokéController.loading_Image.SetActive(true);

        int maxlevel = 1;       //プレイヤーのパーティーポケモンたちの中で最大レベル
        int maxELevel = 1;      //ELevel版

        //自ポケモンの登録
        for(var i = 0; i < 3; i++)
        {
            UserData_Pokémon battlePokemon = new UserData_Pokémon();
            battlePokemon = DataLists.playerData.teamDatas.pokémons[teamMenberList[i] - 1];
            BattleDatas_Default.user_PokemonData[i] = battlePokemon;

            OthersStatus othersStatus = new OthersStatus();
            othersStatus.hp = battlePokemon.userP_Real_Hp;
            othersStatus.condition = BattleEnum.condition.none;
            othersStatus.iceTurn = 0;
            othersStatus.sleepTurn = 0;

            switch (teamMenberList[i] - 1)
            {
                case 0:
                    othersStatus.b_item = DataLists.playerData.teamDatas.b_item1;
                    break;
                case 1:
                    othersStatus.b_item = DataLists.playerData.teamDatas.b_item2;
                    break;
                case 2:
                    othersStatus.b_item = DataLists.playerData.teamDatas.b_item3;
                    break;
                case 3:
                    othersStatus.b_item = DataLists.playerData.teamDatas.b_item4;
                    break;
                case 4:
                    othersStatus.b_item = DataLists.playerData.teamDatas.b_item5;
                    break;
                case 5:
                    othersStatus.b_item = DataLists.playerData.teamDatas.b_item6;
                    break;
            }

            for(var t = 0; t < 4; t++)
            {
                string techniqueName = "";
                int pp = 0;
                switch (t)
                {
                    case 0:
                        techniqueName = battlePokemon.set_Technique1;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique1 = pp;
                        break;
                    case 1:
                        techniqueName = battlePokemon.set_Technique2;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique2 = pp;
                        break;
                    case 2:
                        techniqueName = battlePokemon.set_Technique3;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique3 = pp;
                        break;
                    case 3:
                        techniqueName = battlePokemon.set_Technique4;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique4 = pp;
                        break;
                }
            }

            BattleDatas_Default.user_OthersStatus[i] = othersStatus;
        }

        //最大レベル代入
        foreach(var i in DataLists.playerData.teamDatas.pokémons)
        {
            if(i == null)
            {
                break;
            }

            if(maxlevel < i.userP_Level)
            {
                maxlevel = i.userP_Level;
            }

            if (maxELevel < i.userP_ELevel)
            {
                maxELevel = i.userP_ELevel;
            }
        }

        //敵ポケモンの登録
        for(var i = 0; i < 3; i++)
        {
            var stageData = DataLists.titleData_StageData.Find(x => x.StageId == StageId);
            #region //選出決定
            int r = 0;
            switch (i)
            {
                case 0:
                    r = Random.Range(0, 3);
                    break;
                case 1:
                    r = Random.Range(3, 5);
                    break;
                case 2:
                    r = 5;
                    break;
            }
            UserData_Pokémon enemy_PokeData = new UserData_Pokémon();
            switch (r)
            {
                case 0:
                    enemy_PokeData.userP_Id = stageData.p1_Id;
                    break;
                case 1:
                    enemy_PokeData.userP_Id = stageData.p2_Id;
                    break;
                case 2:
                    enemy_PokeData.userP_Id = stageData.p3_Id;
                    break;
                case 3:
                    enemy_PokeData.userP_Id = stageData.p4_Id;
                    break;
                case 4:
                    enemy_PokeData.userP_Id = stageData.p5_Id;
                    break;
                case 5:
                    enemy_PokeData.userP_Id = stageData.p6_Id;
                    break;
            }
            #endregion

            var pokeData = DataLists.titleData_Pokémon.Find(x => x.p_Id == enemy_PokeData.userP_Id);

            enemy_PokeData.unique_Id = "ALUFA";
            enemy_PokeData.userP_Name = pokeData.p_Name;
            enemy_PokeData.userP_NickName = "相手の　"+pokeData.p_Name;

            enemy_PokeData.userP_Ball = Pokémon_Ball.Ball.Poké;

            var r2 = Random.Range(1.0f, 100.0f);
            {
                if (255 == pokeData.p_MaleProbability)
                {
                    enemy_PokeData.userP_gender = 2;
                }
                else if (r2 <= pokeData.p_MaleProbability)
                {
                    enemy_PokeData.userP_gender = 0;
                }
                else if (r2 > pokeData.p_MaleProbability)
                {
                    enemy_PokeData.userP_gender = 1;
                }
            }
            //タイプ
            {
                switch (pokeData.p_Type1)//タイプ1
                {
                    #region
                    case "ノーマル":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Normal;
                        break;
                    case "ほのお":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Fire;
                        break;
                    case "みず":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Water;
                        break;
                    case "でんき":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Electric;
                        break;
                    case "くさ":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Grass;
                        break;
                    case "こおり":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Ice;
                        break;
                    case "かくとう":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Fighting;
                        break;
                    case "どく":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Poison;
                        break;
                    case "じめん":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Ground;
                        break;
                    case "ひこう":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Flying;
                        break;
                    case "エスパー":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Psychic;
                        break;
                    case "むし":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Bug;
                        break;
                    case "いわ":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Rock;
                        break;
                    case "ゴースト":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Ghost;
                        break;
                    case "ドラゴン":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Dragon;
                        break;
                    case "あく":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Dark;
                        break;
                    case "はがね":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Steel;
                        break;
                    case "フェアリー":
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Fairy;
                        break;
                    case "":
                    case null:
                        Debug.LogError("タイプが設定されていません");
                        break;
                    default:
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Normal;
                        break;
                        #endregion
                }
                switch (pokeData.p_Type2)//タイプ2
                {
                    #region
                    case "ノーマル":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Normal;
                        break;
                    case "ほのお":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Fire;
                        break;
                    case "みず":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Water;
                        break;
                    case "でんき":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Electric;
                        break;
                    case "くさ":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Grass;
                        break;
                    case "こおり":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Ice;
                        break;
                    case "かくとう":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Fighting;
                        break;
                    case "どく":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Poison;
                        break;
                    case "じめん":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Ground;
                        break;
                    case "ひこう":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Flying;
                        break;
                    case "エスパー":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Psychic;
                        break;
                    case "むし":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Bug;
                        break;
                    case "いわ":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Rock;
                        break;
                    case "ゴースト":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Ghost;
                        break;
                    case "ドラゴン":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Dragon;
                        break;
                    case "あく":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Dark;
                        break;
                    case "はがね":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Steel;
                        break;
                    case "フェアリー":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.Fairy;
                        break;
                    case "":
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.None;
                        break;
                    case null:
                        enemy_PokeData.userP_Type2 = Pokémon_Type.Type.None;
                        break;
                    default:
                        enemy_PokeData.userP_Type1 = Pokémon_Type.Type.Normal;
                        break;
                        #endregion
                }
            }
            enemy_PokeData.userP_Level = maxlevel;
            //特性
            enemy_PokeData.userP_Characteristic = pokeData.p_Characteristic;
            //個体値
            {
                enemy_PokeData.userP_ELevel = maxELevel;

                int e = 5;

                switch (enemy_PokeData.userP_ELevel)
                {
                    case 1:
                        e = 5;
                        enemy_PokeData.userP_Individual_Hp = e;
                        enemy_PokeData.userP_Individual_A = e;
                        enemy_PokeData.userP_Individual_B = e;
                        enemy_PokeData.userP_Individual_C = e;
                        enemy_PokeData.userP_Individual_D = e;
                        enemy_PokeData.userP_Individual_S = e;
                        break;
                    case 2:
                        e = 10;
                        enemy_PokeData.userP_Individual_Hp = e;
                        enemy_PokeData.userP_Individual_A = e;
                        enemy_PokeData.userP_Individual_B = e;
                        enemy_PokeData.userP_Individual_C = e;
                        enemy_PokeData.userP_Individual_D = e;
                        enemy_PokeData.userP_Individual_S = e;
                        break;
                    case 3:
                        e = 18;
                        enemy_PokeData.userP_Individual_Hp = e;
                        enemy_PokeData.userP_Individual_A = e;
                        enemy_PokeData.userP_Individual_B = e;
                        enemy_PokeData.userP_Individual_C = e;
                        enemy_PokeData.userP_Individual_D = e;
                        enemy_PokeData.userP_Individual_S = e;
                        break;
                    case 4:
                        e = 25;
                        enemy_PokeData.userP_Individual_Hp = e;
                        enemy_PokeData.userP_Individual_A = e;
                        enemy_PokeData.userP_Individual_B = e;
                        enemy_PokeData.userP_Individual_C = e;
                        enemy_PokeData.userP_Individual_D = e;
                        enemy_PokeData.userP_Individual_S = e;
                        break;
                    case 5:
                        e = 31;
                        enemy_PokeData.userP_Individual_Hp = e;
                        enemy_PokeData.userP_Individual_A = e;
                        enemy_PokeData.userP_Individual_B = e;
                        enemy_PokeData.userP_Individual_C = e;
                        enemy_PokeData.userP_Individual_D = e;
                        enemy_PokeData.userP_Individual_S = e;
                        break;
                }
            }
            //努力値
            {
                var hp = enemy_PokeData.userP_Effort_Hp;
                var atk = enemy_PokeData.userP_Effort_A;
                var def = enemy_PokeData.userP_Effort_B;
                var sat = enemy_PokeData.userP_Effort_C;
                var sde = enemy_PokeData.userP_Effort_D;
                var spe = enemy_PokeData.userP_Effort_S;

                int h = pokeData.race_H;
                int a = pokeData.race_A;
                int b = pokeData.race_B;
                int c = pokeData.race_C;
                int d = pokeData.race_D;
                int s = pokeData.race_S;

                #region 努力値配分
                //A
                if (a >= h && a >= b && a >= c && a >= d && a >= s)
                {
                    atk = 255;

                    if (s > h)
                    {
                        spe = 255;

                        if (h > b && h > d)
                        {
                            hp = 4;
                        }
                        else if (b > d)
                        {
                            def = 4;
                        }
                        else
                        {
                            sde = 4;
                        }
                    }
                    else
                    {
                        hp = 252;

                        if (b > d)
                        {
                            def = 4;
                        }
                        else
                        {
                            sde = 4;
                        }
                    }
                }
                //C
                else if (c >= h && c >= a && c >= b && c >= d && c >= s)
                {
                    sat = 252;

                    if (s > h)
                    {
                        spe = 252;

                        if (h > b && h > d) hp = 4;
                        else if (b > d) def = 4;
                        else sde = 4;
                    }
                    else
                    {
                        hp = 252;

                        if (b > d) def = 4;
                        else sde = 4;
                    }
                }
                //S
                else if (s >= h && s >= a && s >= b && s >= c && s >= d)
                {
                    spe = 252;

                    if (h > c)
                    {
                        hp = 252;

                        if (c > b && c > d) sat = 4;
                        else if (b > d) def = 4;
                        else sde = 4;
                    }
                    else
                    {
                        sat = 252;
                        if (h > b && h > d) hp = 4;
                        else if (b > d) def = 4;
                        else sde = 4;
                    }
                }
                //HP
                else if (h >= a && h >= b && h >= c && h >= d && h >= s)
                {
                    hp = 252;

                    if (a > s && a > b && a > d)
                    {
                        atk = 252;
                        def = 4;
                    }
                    else if (b > d)
                    {
                        def = 252;
                        sde = 4;
                    }
                    else
                    {
                        sde = 252;
                        def = 4;
                    }
                }
                //B
                else if (b >= h && b >= a && b >= c && b >= d && b >= s)
                {
                    def = 252;
                    hp = 252;
                    sde = 4;
                }
                //D
                else if (d >= h && d >= a && d >= b && d >= c && d >= s)
                {
                    sde = 252;
                    hp = 252;
                    def = 4;
                }
                #endregion
            }
            //実数値
            {
                //HP
                enemy_PokeData.userP_Real_Hp = Mathf.FloorToInt((pokeData.race_H * 2 + enemy_PokeData.userP_Individual_Hp + enemy_PokeData.userP_Effort_Hp / 4) * enemy_PokeData.userP_Level / 100 + enemy_PokeData.userP_Level + 10);
                enemy_PokeData.userP_CurrentHp = enemy_PokeData.userP_Real_Hp;
                //攻撃
                {
                    var poke_a = Mathf.FloorToInt((pokeData.race_A * 2 + enemy_PokeData.userP_Individual_A + enemy_PokeData.userP_Effort_A / 4) * enemy_PokeData.userP_Level / 100 + 5);
                    if (enemy_PokeData.userP_Personality == 1 || enemy_PokeData.userP_Personality == 2 || enemy_PokeData.userP_Personality == 3 || enemy_PokeData.userP_Personality == 4)
                    {
                        poke_a = Mathf.FloorToInt(poke_a * 1.1f);
                    }
                    else if (enemy_PokeData.userP_Personality == 5 || enemy_PokeData.userP_Personality == 9 || enemy_PokeData.userP_Personality == 13 || enemy_PokeData.userP_Personality == 17)
                    {
                        poke_a = Mathf.FloorToInt(poke_a * 0.9f);
                    }
                    enemy_PokeData.userP_Real_A = Mathf.FloorToInt(poke_a);
                }
                //防御
                {
                    var poke_b = Mathf.FloorToInt((pokeData.race_B * 2 + enemy_PokeData.userP_Individual_B + enemy_PokeData.userP_Effort_B / 4) * enemy_PokeData.userP_Level / 100 + 5);
                    if (enemy_PokeData.userP_Personality == 5 || enemy_PokeData.userP_Personality == 6 || enemy_PokeData.userP_Personality == 7 || enemy_PokeData.userP_Personality == 8)
                    {
                        poke_b = Mathf.FloorToInt(poke_b * 1.1f);
                    }
                    else if (enemy_PokeData.userP_Personality == 1 || enemy_PokeData.userP_Personality == 10 || enemy_PokeData.userP_Personality == 14 || enemy_PokeData.userP_Personality == 18)
                    {
                        poke_b = Mathf.FloorToInt(poke_b * 0.9f);
                    }
                    enemy_PokeData.userP_Real_B = Mathf.FloorToInt(poke_b);
                }
                //特攻
                {
                    var poke_c = Mathf.FloorToInt((pokeData.race_C * 2 + enemy_PokeData.userP_Individual_C + enemy_PokeData.userP_Effort_C / 4) * enemy_PokeData.userP_Level / 100 + 5);
                    if (enemy_PokeData.userP_Personality == 9 || enemy_PokeData.userP_Personality == 10 || enemy_PokeData.userP_Personality == 11 || enemy_PokeData.userP_Personality == 12)
                    {
                        poke_c = Mathf.FloorToInt(poke_c * 1.1f);
                    }
                    else if (enemy_PokeData.userP_Personality == 2 || enemy_PokeData.userP_Personality == 6 || enemy_PokeData.userP_Personality == 15 || enemy_PokeData.userP_Personality == 19)
                    {
                        poke_c = Mathf.FloorToInt(poke_c * 0.9f);
                    }
                    enemy_PokeData.userP_Real_C = Mathf.FloorToInt(poke_c);
                }
                //特防
                {
                    var poke_d = Mathf.FloorToInt((pokeData.race_D * 2 + enemy_PokeData.userP_Individual_D + enemy_PokeData.userP_Effort_D / 4) * enemy_PokeData.userP_Level / 100 + 5);
                    if (enemy_PokeData.userP_Personality == 13 || enemy_PokeData.userP_Personality == 14 || enemy_PokeData.userP_Personality == 15 || enemy_PokeData.userP_Personality == 16)
                    {
                        poke_d = Mathf.FloorToInt(poke_d * 1.1f);
                    }
                    else if (enemy_PokeData.userP_Personality == 3 || enemy_PokeData.userP_Personality == 7 || enemy_PokeData.userP_Personality == 11 || enemy_PokeData.userP_Personality == 20)
                    {
                        poke_d = Mathf.FloorToInt(poke_d * 0.9f);
                    }
                    enemy_PokeData.userP_Real_D = Mathf.FloorToInt(poke_d);
                }
                //素早さ
                {
                    var poke_s = Mathf.FloorToInt((pokeData.race_S * 2 + enemy_PokeData.userP_Individual_S + enemy_PokeData.userP_Effort_S / 4) * enemy_PokeData.userP_Level / 100 + 5);
                    if (enemy_PokeData.userP_Personality == 17 || enemy_PokeData.userP_Personality == 18 || enemy_PokeData.userP_Personality == 19 || enemy_PokeData.userP_Personality == 20)
                    {
                        poke_s = Mathf.FloorToInt(poke_s * 1.1f);
                    }
                    else if (enemy_PokeData.userP_Personality == 4 || enemy_PokeData.userP_Personality == 8 || enemy_PokeData.userP_Personality == 12 || enemy_PokeData.userP_Personality == 16)
                    {
                        poke_s = Mathf.FloorToInt(poke_s * 0.9f);
                    }
                    enemy_PokeData.userP_Real_S = Mathf.FloorToInt(poke_s);
                }
            }
            var rememberData = DataLists.titleData_Remember.Find(x => x.p_T_Id == pokeData.p_Id);
            #region 技
            for (var ii = 0; ii < 4; ii++)
            {
                string techniqueName = "";
                int techniqueID = 0;
                int r3 = 0;
                switch (ii)
                {
                    case 0:
                        r3 = Random.Range(0, 2);
                        break;
                    case 1:
                        r3 = Random.Range(2, 4);
                        break;
                    case 2:
                        r3 = Random.Range(4, 7);
                        break;
                    case 3:
                        r3 = Random.Range(7, 10);
                        break;
                }
                switch (r3)
                {
                    case 0:
                        techniqueName = rememberData.Technique1;
                        techniqueID = rememberData.TechniqueID1;
                        break;
                    case 1:
                        techniqueName = rememberData.Technique2;
                        techniqueID = rememberData.TechniqueID2;
                        break;
                    case 2:
                        techniqueName = rememberData.Technique3;
                        techniqueID = rememberData.TechniqueID3;
                        break;
                    case 3:
                        techniqueName = rememberData.Technique4;
                        techniqueID = rememberData.TechniqueID4;
                        break;
                    case 4:
                        techniqueName = rememberData.Technique5;
                        techniqueID = rememberData.TechniqueID5;
                        break;
                    case 5:
                        techniqueName = rememberData.Technique6;
                        techniqueID = rememberData.TechniqueID6;
                        break;
                    case 6:
                        techniqueName = rememberData.Technique7;
                        techniqueID = rememberData.TechniqueID7;
                        break;
                    case 7:
                        techniqueName = rememberData.Technique8;
                        techniqueID = rememberData.TechniqueID8;
                        break;
                    case 8:
                        techniqueName = rememberData.Technique9;
                        techniqueID = rememberData.TechniqueID9;
                        break;
                    case 9:
                        techniqueName = rememberData.Technique10;
                        techniqueID = rememberData.TechniqueID10;
                        break;
                }
                switch (ii)
                {
                    case 0:
                        enemy_PokeData.set_Technique1 = techniqueName;
                        enemy_PokeData.set_TechniqueID1 = techniqueID;
                        break;
                    case 1:
                        enemy_PokeData.set_Technique2 = techniqueName;
                        enemy_PokeData.set_TechniqueID2 = techniqueID;
                        break;
                    case 2:
                        enemy_PokeData.set_Technique3 = techniqueName;
                        enemy_PokeData.set_TechniqueID3 = techniqueID;
                        break;
                    case 3:
                        enemy_PokeData.set_Technique4 = techniqueName;
                        enemy_PokeData.set_TechniqueID4 = techniqueID;
                        break;
                }
            }
            #endregion
            BattleDatas_Default.enemy_PokemonData[i] = enemy_PokeData;

            OthersStatus othersStatus = new OthersStatus();
            othersStatus.hp = enemy_PokeData.userP_Real_Hp;
            othersStatus.condition = BattleEnum.condition.none;
            othersStatus.iceTurn = 0;
            othersStatus.sleepTurn = 0;

            for (var t = 0; t < 4; t++)
            {
                string techniqueName = "";
                int pp = 0;
                switch (t)
                {
                    case 0:
                        techniqueName = enemy_PokeData.set_Technique1;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique1 = pp;
                        break;
                    case 1:
                        techniqueName = enemy_PokeData.set_Technique2;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique2 = pp;
                        break;
                    case 2:
                        techniqueName = enemy_PokeData.set_Technique3;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique3 = pp;
                        break;
                    case 3:
                        techniqueName = enemy_PokeData.set_Technique4;
                        pp = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName).t_PP;
                        othersStatus.pp_Technique4 = pp;
                        break;
                }
            }

            BattleDatas_Default.enemy_OthersStatus[i] = othersStatus;
        }

        StartCoroutine(loadScene());

        IEnumerator loadScene()
        {
            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("BattleScene");
            async.allowSceneActivation = false;

            yield return new WaitForSeconds(1);
            async.allowSceneActivation = true;
        }
    }
}
