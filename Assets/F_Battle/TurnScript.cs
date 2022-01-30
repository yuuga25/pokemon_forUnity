using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnScript : MonoBehaviour
{
    public BattleController battleController;
    public TechniqueScript techniqueScript;
    public ReplaceScript replaceScript;
    public ResultScript resultScript;

    public List<AudioClip> inOut_Audio;

    [HideInInspector]
    public BattleStatus temporary_BattleData;
    [HideInInspector]
    public BattleDataStorage p1_Storage;
    [HideInInspector]
    public BattleDataStorage p2_Storage;
    [HideInInspector]
    public bool isDone;
    [HideInInspector]
    public List<string> message_Tech = new List<string>();
    [HideInInspector]
    public int KeyPoint;
    public float randomNumber;
    public bool isEnemy;

    public bool isWin = false;
    public bool isLose = false;

    public List<AudioClip> AudioClip_Damage = new List<AudioClip>();

    [Header("リザルト画面")]
    public GameObject result_Obj;
    public List<Sprite> result_Sprite;

    private void Start()
    {
        result_Obj.SetActive(false);
    }
    private void Update()
    {
        if (battleController.player_Anim.GetBool("p_In") == false || battleController.enemy_Anim.GetBool("p_In") == false)
        {
            battleController.Control_ModeSelect.SetActive(false);
        }
    }

    public void SetBattleZone()
    {
        battleController.Control_ModeSelect.SetActive(false);
        battleController.Control_ReplaceSelect.SetActive(false);
        battleController.Control_Situation.SetActive(false);
        battleController.Control_TechniqueSelect.SetActive(false);

        BattleDatas_Default.user_BattleStatus.participationTurn++;
        BattleDatas_Default.enemy_BattleStatus.participationTurn++;

        if (isWin || isLose)
        {
            var stage = DataLists.titleData_StageData.Find(x => x.StageId == Menu_BattleController.StageId);

            var result_Image = result_Obj.transform.Find("result_Image").GetComponent<Image>();

            var GD = 500;
            var BP = 25;

            if (isWin)
            {
                result_Image.sprite = result_Sprite[0];
            }
            else if (isLose)
            {
                result_Image.sprite = result_Sprite[1];

                GD = 250;
                BP = 12;
            }

            resultScript.AddMoney(GD, BP);

            var result_Message = result_Obj.transform.Find("Text_Message").GetComponent<Text>();
            result_Message.text = $"おこづかい\n{DataLists.player_Money} 円　→　{DataLists.player_Money + GD} 円\n\nBP\n{DataLists.player_BattlePoint} BP　→　{DataLists.player_BattlePoint + BP} BP";

            result_Obj.SetActive(true);
        }
        else
        {
            #region 交代機能
            if (battleController.player_PokeTeamNum == 255)
            {
                print("自分入れ替え");

                battleController.Control_ModeSelect.SetActive(false);
                battleController.Control_ReplaceSelect.SetActive(true);
                battleController.ReplaceDisplay();
                battleController.Control_ReplaceSelect.transform.Find("Button_Back").gameObject.SetActive(false);
                battleController.textDisplay.text = "交代先を選んでください";

                BattleDatas_Default.user_BattleStatus = new BattleStatus();
            }

            if (battleController.enemy_PokeTeamNum == 255)
            {
                print("相手入れ替え");

                if (BattleDatas_Default.enemy_OthersStatus[1].hp <= 0)
                {
                    if (BattleDatas_Default.enemy_OthersStatus[2].hp <= 0)
                    {
                        //勝利判定
                        print("勝った");
                    }
                    else
                    {
                        battleController.enemy_PokeTeamNum = 2;
                    }
                }
                else
                {
                    battleController.enemy_PokeTeamNum = 1;
                }

                BattleDatas_Default.enemy_BattleStatus = new BattleStatus();

                if (battleController.player_PokeTeamNum != 255)
                {
                    replaceScript.EnemyReplace();
                }
            }
            #endregion

            if (battleController.player_Anim.GetBool("p_In") == true && battleController.enemy_Anim.GetBool("p_In") == true)
            {
                battleController.Control_ModeSelect.SetActive(true);
                battleController.textDisplay.text = $"行動を選択してください";
            }
        }
    }

    public void Damage(int damage, bool isEnemy, string techniqueName)
    {
        var techData = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName);
        var c_Data = battleController.c_Data.sheet.Find(x => x.typeName == techData.t_Type);
        if (damage <= 0)
        {
            damage = 1;
        }

        if (isEnemy)
        {
            if(BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp - damage <= 0)
            {
                damage = BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp;
            }

            if(BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp > 0)
            {
                StartCoroutine(enemy_Damage());
            }

            IEnumerator enemy_Damage()
            {
                BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp -= damage;
                float time = 1.2f / damage;

                int magnification = 100;
                if (c_Data.twice.Contains(BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Type1))
                {
                    magnification = magnification * 2;
                }
                if (c_Data.half.Contains(BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Type1))
                {
                    magnification = magnification / 2;
                }
                if (c_Data.twice.Contains(BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Type2))
                {
                    magnification = magnification * 2;
                }
                if (c_Data.half.Contains(BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Type2))
                {
                    magnification = magnification / 2;
                }

                if(magnification > 100)
                {
                    battleController.Audio_SE.PlayOneShot(AudioClip_Damage[1]);
                }
                else if(magnification < 100)
                {
                    battleController.Audio_SE.PlayOneShot(AudioClip_Damage[2]);
                }
                else
                {
                    battleController.Audio_SE.PlayOneShot(AudioClip_Damage[0]);
                }

                while (true)
                {
                    battleController.enemy_HpSlider.value--;
                    if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 5)
                    {
                        battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
                    }
                    else if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 2)
                    {
                        battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
                    }
                    else
                    {
                        battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
                    }
                    yield return new WaitForSeconds(time);
                    if (battleController.enemy_HpSlider.value <= BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp)
                    {
                        #region いのちのたま
                        if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Life_Orb")
                        {
                            damage = Mathf.FloorToInt(BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Real_Hp / 10);
                            if (damage < 0) damage = 1;

                            BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp -= damage;
                            time = 1.2f / damage;
                            while (true)
                            {
                                battleController.player_HpSlider.value--;
                                if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
                                {
                                    battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
                                }
                                else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
                                {
                                    battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
                                }
                                else
                                {
                                    battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
                                }
                                battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                                yield return new WaitForSeconds(time);
                                if (battleController.player_HpSlider.value <= BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp)
                                {
                                    battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は\n命が　少し削られた！";
                                    yield return new WaitForSeconds(1.2f);
                                    battleController.textDisplay.text = "";

                                    battleController.player_HpSlider.value = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp;
                                    battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";

                                    if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp == 0)
                                    {
                                        BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].condition = BattleEnum.condition.dying;
                                        battleController.player_PokeTeamNum = 255;
                                    }
                                    break;
                                }
                            }
                        }
                        #endregion
                        #region かいがらのすず
                        if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Shell_Bell")
                        {
                            int heal = Mathf.FloorToInt(damage / 8);

                            BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp += heal;
                            if(BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp > BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp)
                            {
                                BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp = BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp;
                            }

                            time = 1.2f / heal;
                            while (true)
                            {
                                battleController.player_HpSlider.value++;
                                if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
                                {
                                    battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
                                }
                                else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
                                {
                                    battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
                                }
                                else
                                {
                                    battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
                                }
                                battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                                yield return new WaitForSeconds(time);
                                if (battleController.player_HpSlider.value >= BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp)
                                {
                                    battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は\nすずの音色で　癒された！";
                                    yield return new WaitForSeconds(1.2f);
                                    battleController.textDisplay.text = "";

                                    battleController.player_HpSlider.value = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp;
                                    battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                                    break;
                                }
                            }
                        }
                        #endregion
                        yield return new WaitForSeconds(0.5f);
                        if (BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp == 0)
                        {
                            BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].condition = BattleEnum.condition.dying;
                            battleController.enemy_PokeTeamNum = 255;
                            var slider = battleController.Pokemon2.transform.Find("HPBer_Player2").Find("Slider").GetComponent<Slider>();
                            slider.value = slider.minValue;
                        }

                        isDone = true;
                        break;
                    }
                }
            }
        }
        else
        {
            if(BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp > 0)
            {
                StartCoroutine(player_Damage());
            }

            IEnumerator player_Damage()
            {
                bool tasuki = false;
                if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp - damage <= 0)
                {
                    damage = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp;

                    if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Focus_Sash" && battleController.player_HpSlider.value == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp)
                    {
                        damage = damage - 1;
                        tasuki = true;
                    }
                }

                int magnification = 100;
                if (c_Data.twice.Contains(BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Type1))
                {
                    magnification = magnification * 2;
                }
                if (c_Data.half.Contains(BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Type1))
                {
                    magnification = magnification / 2;
                }
                if (c_Data.twice.Contains(BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Type2))
                {
                    magnification = magnification * 2;
                }
                if (c_Data.half.Contains(BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Type2))
                {
                    magnification = magnification / 2;
                }

                if (magnification > 100)
                {
                    battleController.Audio_SE.PlayOneShot(AudioClip_Damage[1]);
                }
                else if (magnification < 100)
                {
                    battleController.Audio_SE.PlayOneShot(AudioClip_Damage[2]);
                }
                else
                {
                    battleController.Audio_SE.PlayOneShot(AudioClip_Damage[0]);
                }

                BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp -= damage;
                float time = 1.2f / damage;
                while (true)
                {
                    battleController.player_HpSlider.value--;
                    if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
                    {
                        battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
                    }
                    else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
                    {
                        battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
                    }
                    else
                    {
                        battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
                    }
                    battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                    yield return new WaitForSeconds(time);
                    if(battleController.player_HpSlider.value <= BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp)
                    {
                        yield return new WaitForSeconds(0.5f);

                        battleController.player_HpSlider.value = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp;
                        battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";

                        if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp <= 0)
                        {
                            BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].condition = BattleEnum.condition.dying;
                            battleController.player_PokeTeamNum = 255; 
                            var slider = battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Slider").GetComponent<Slider>();
                            slider.value = slider.minValue;
                        }
                        else
                        {

                            #region きあいのタスキ
                            if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Focus_Sash" && tasuki == true)
                            {
                                battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は\nきあいのタスキで　もちこたえた！";
                                BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item = null;
                                yield return new WaitForSeconds(1.2f);
                                battleController.textDisplay.text = "";
                            }
                            #endregion

                            Technique_Pokémon tech = new Technique_Pokémon();
                            if(techniqueName != "")
                            {
                                tech = DataLists.titleData_Technique.Find(x => x.t_Name == techniqueName);
                            }
                            #region じゃくてんほけん
                            if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Weakness_Policy")
                            {
                                Pokémon_Type.Type techType = new Pokémon_Type.Type();
                                switch (tech.t_Type)
                                {
                                    case "ノーマル":
                                        techType = Pokémon_Type.Type.Normal;
                                        break;
                                    case "ほのお":
                                        techType = Pokémon_Type.Type.Fire;
                                        break;
                                    case "みず":
                                        techType = Pokémon_Type.Type.Water;
                                        break;
                                    case "でんき":
                                        techType = Pokémon_Type.Type.Electric;
                                        break;
                                    case "くさ":
                                        techType = Pokémon_Type.Type.Grass;
                                        break;
                                    case "こおり":
                                        techType = Pokémon_Type.Type.Ice;
                                        break;
                                    case "かくとう":
                                        techType = Pokémon_Type.Type.Fighting;
                                        break;
                                    case "どく":
                                        techType = Pokémon_Type.Type.Poison;
                                        break;
                                    case "じめん":
                                        techType = Pokémon_Type.Type.Ground;
                                        break;
                                    case "ひこう":
                                        techType = Pokémon_Type.Type.Flying;
                                        break;
                                    case "エスパー":
                                        techType = Pokémon_Type.Type.Psychic;
                                        break;
                                    case "むし":
                                        techType = Pokémon_Type.Type.Bug;
                                        break;
                                    case "いわ":
                                        techType = Pokémon_Type.Type.Rock;
                                        break;
                                    case "ゴースト":
                                        techType = Pokémon_Type.Type.Ghost;
                                        break;
                                    case "ドラゴン":
                                        techType = Pokémon_Type.Type.Dragon;
                                        break;
                                    case "あく":
                                        techType = Pokémon_Type.Type.Dark;
                                        break;
                                    case "はがね":
                                        techType = Pokémon_Type.Type.Steel;
                                        break;
                                    case "フェアリー":
                                        techType = Pokémon_Type.Type.Fairy;
                                        break;
                                }
                                var type = battleController.c_Data.sheet.Find(x => x.type == techType);
                                if (type.twice.Contains(BattleDatas_Default.user_PokemonData[battleController.enemy_PokeTeamNum].userP_Type1) && BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Weakness_Policy")
                                {
                                    BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item = null;
                                    battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は じゃくてんほけんで\n攻撃が　ぐーんと　上がった！";
                                    BattleDatas_Default.user_BattleStatus.AscendingRank_Atk += 2;
                                    Mathf.Clamp(BattleDatas_Default.user_BattleStatus.AscendingRank_Atk, -6, 6);
                                    yield return new WaitForSeconds(1.2f);
                                    battleController.textDisplay.text = "";
                                    yield return new WaitForSeconds(1);
                                    battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は じゃくてんほけんで\n特攻が　ぐーんと　上がった！";
                                    BattleDatas_Default.user_BattleStatus.AscendingRank_Sat += 2;
                                    Mathf.Clamp(BattleDatas_Default.user_BattleStatus.AscendingRank_Sat, -6, 6);
                                    yield return new WaitForSeconds(1.2f);
                                    battleController.textDisplay.text = "";
                                }
                                if(BattleDatas_Default.user_PokemonData[battleController.enemy_PokeTeamNum].userP_Type2 != Pokémon_Type.Type.None)
                                {
                                    type = battleController.c_Data.sheet.Find(x => x.type == techType);
                                    if (type.twice.Contains(BattleDatas_Default.user_PokemonData[battleController.enemy_PokeTeamNum].userP_Type2) && BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Weakness_Policy")
                                    {
                                        BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item = null;
                                        battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は じゃくてんほけんで\n攻撃が　ぐーんと　上がった！";
                                        BattleDatas_Default.user_BattleStatus.AscendingRank_Atk += 2;
                                        Mathf.Clamp(BattleDatas_Default.user_BattleStatus.AscendingRank_Atk, -6, 6);
                                        yield return new WaitForSeconds(1.2f);
                                        battleController.textDisplay.text = "";
                                        yield return new WaitForSeconds(1);
                                        battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は じゃくてんほけんで\n特攻が　ぐーんと　上がった！";
                                        BattleDatas_Default.user_BattleStatus.AscendingRank_Sat += 2;
                                        Mathf.Clamp(BattleDatas_Default.user_BattleStatus.AscendingRank_Sat, -6, 6);
                                        yield return new WaitForSeconds(1.2f);
                                        battleController.textDisplay.text = "";
                                    }
                                }
                            }
                            #endregion

                            #region オボンのみ
                            if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Sitrus_Berry" && BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp <= BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp / 2)
                            {
                                battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は　オボンのみを　食べた！\n体力が　少し　回復した！";
                                BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item = null;
                                yield return new WaitForSeconds(1.2f);
                                battleController.textDisplay.text = "";
                                BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp += Mathf.FloorToInt(BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp / 4);
                                while (true)
                                {
                                    battleController.player_HpSlider.value++;
                                    if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
                                    {
                                        battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
                                    }
                                    else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
                                    {
                                        battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
                                    }
                                    else
                                    {
                                        battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
                                    }
                                    battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                                    yield return new WaitForSeconds(time);
                                    if (battleController.player_HpSlider.value >= BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp)
                                    {
                                        yield return new WaitForSeconds(0.5f);

                                        battleController.player_HpSlider.value = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].hp;
                                        battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region ゴツゴツメット
                            if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "Rocky_Helmet" && tech.t_DirectAttack == true && BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp > 0)
                            {
                                battleController.textDisplay.text = $"{BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_NickName}は\nゴツゴツメットで　傷ついた！";
                                yield return new WaitForSeconds(1.2f);
                                battleController.textDisplay.text = "";

                                damage = Mathf.FloorToInt(BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Real_Hp / 6);
                                if (damage < 0) damage = 1;

                                BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp -= damage;
                                time = 1.2f / damage;

                                while (true)
                                {
                                    battleController.enemy_HpSlider.value--;
                                    if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 5)
                                    {
                                        battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
                                    }
                                    else if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 2)
                                    {
                                        battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
                                    }
                                    else
                                    {
                                        battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
                                    }
                                    yield return new WaitForSeconds(time);
                                    if (battleController.enemy_HpSlider.value <= BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp)
                                    {
                                        yield return new WaitForSeconds(0.5f);
                                        if (BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp == 0)
                                        {
                                            BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].condition = BattleEnum.condition.dying;
                                            battleController.enemy_PokeTeamNum = 255;
                                            var slider = battleController.Pokemon2.transform.Find("HPBer_Player2").Find("Slider").GetComponent<Slider>();
                                            slider.value = slider.minValue;
                                        }
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                        isDone = true;
                        break;
                    }
                }
            }
        }
    }

    public void UseTechnique(int techniqueId)
    {
        #region 定義
        var player_Poke = BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum];
        var player_Poke_Other = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum];
        var enemy_Poke = BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum];
        var enemy_Poke_Other = BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum];
        var text_Display = battleController.textDisplay;
        #endregion

        #region プレイヤーのポケモンの技取得
        string player_TechName = "";
        Technique_Pokémon player_Technique = new Technique_Pokémon();
        int player_PP = 0;
        switch (techniqueId)
        {
            case 1:
                player_TechName = player_Poke.set_Technique1;
                player_PP = player_Poke_Other.pp_Technique1;
                break;
            case 2:
                player_TechName = player_Poke.set_Technique2;
                player_PP = player_Poke_Other.pp_Technique2;
                break;
            case 3:
                player_TechName = player_Poke.set_Technique3;
                player_PP = player_Poke_Other.pp_Technique3;
                break;
            case 4:
                player_TechName = player_Poke.set_Technique4;
                player_PP = player_Poke_Other.pp_Technique4;
                break;
        }
        if(player_PP <= 0)
        {
            text_Display.text = "PPがたりません";
            return;
        }
        if (player_Poke_Other.b_item == "Choice_Scarf" || player_Poke_Other.b_item == "Choice_Band" || player_Poke_Other.b_item == "Choice_Specs")
        {
            if(BattleDatas_Default.user_BattleStatus.discerningID == 0)
            {
                BattleDatas_Default.user_BattleStatus.discerningID = techniqueId;
            }
            else if (BattleDatas_Default.user_BattleStatus.discerningID == techniqueId)
            {

            }
            else
            {
                text_Display.text = "こだわっているため、他の技を選択できません";
                return;
            }
        }
        if(player_Poke_Other.b_item == "Assault_Vest")
        {
            if(player_Technique.t_Classification == "Change")
            {
                text_Display.text = "とつげきチョッキを　持っているため\n攻撃技しか　選択できません";
                return;
            }
        }
        player_Technique = DataLists.titleData_Technique.Find(x => x.t_Name == player_TechName);
        #endregion

        #region 相手のポケモンの技決定
        string enemy_TechName = "";
        Technique_Pokémon enemy_Technique = new Technique_Pokémon();
        int r = Random.Range(0,100);
        if (r < 30)
        {
            enemy_TechName = enemy_Poke.set_Technique1;
        }
        else if (30 <= r && r < 60)
        {
            enemy_TechName = enemy_Poke.set_Technique2;
        }
        else if(60 <= r && r < 80)
        {
            enemy_TechName = enemy_Poke.set_Technique3;
        }
        else if(80 <= r)
        {
            enemy_TechName = enemy_Poke.set_Technique4;
        }
        enemy_Technique = DataLists.titleData_Technique.Find(x => x.t_Name == enemy_TechName);
        #endregion

        text_Display.text = "";

        var p1 = player_Poke;
        var p1_Other = player_Poke_Other;
        var p2 = enemy_Poke;
        var p2_Other = enemy_Poke_Other;

        int p1_Speed = player_Poke.userP_Real_S;
        int p2_Speed = enemy_Poke.userP_Real_S;

        int p1_Priority = player_Technique.t_Priority;
        int p2_Priority = enemy_Technique.t_Priority;

        var p1_Tech = player_Technique;
        var p2_Tech = enemy_Technique;

        p1_Storage = new BattleDataStorage();
        p2_Storage = new BattleDataStorage();

        #region 素早さ・優先度計算
        if (player_Poke_Other.b_item == "Quick_Claw")
        {
            int r1 = Random.Range(1, 101);
            if(r1 <= 20)
            {
                p1_Priority += 1;
            }
        }

        if(player_Poke.userP_Characteristic == "いたずらごころ" && p1_Tech.t_Classification == "Change")
        {
            p1_Priority++;
        }
        
        if(enemy_Poke.userP_Characteristic == "いたずらごころ" && p2_Tech.t_Classification == "Change")
        {
            p2_Priority++;
        }

        switch (BattleDatas_Default.user_BattleStatus.AscendingRank_Spe)
        {
            case -6:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (25 / 100));
                break;
            case -5:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (28 / 100));
                break;
            case -4:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (33 / 100));
                break;
            case -3:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (40 / 100));
                break;
            case -2:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (50 / 100));
                break;
            case -1:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (66 / 100));
                break;
            case 0:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (100 / 100));
                break;
            case 1:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (150 / 100));
                break;
            case 2:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (200 / 100));
                break;
            case 3:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (250 / 100));
                break;
            case 4:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (300 / 100));
                break;
            case 5:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (350 / 100));
                break;
            case 6:
                p1_Speed = Mathf.FloorToInt(p1_Speed * (400 / 100));
                break;
        }
        if (player_Poke_Other.b_item == "Choice_Scarf")
        {
            p1_Speed = Mathf.FloorToInt(p1_Speed * 1.5f);
        }
        switch (BattleDatas_Default.enemy_BattleStatus.AscendingRank_Spe)
        {
            case -6:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (25 / 100));
                break;
            case -5:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (28 / 100));
                break;
            case -4:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (33 / 100));
                break;
            case -3:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (40 / 100));
                break;
            case -2:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (50 / 100));
                break;
            case -1:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (66 / 100));
                break;
            case 0:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (100 / 100));
                break;
            case 1:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (150 / 100));
                break;
            case 2:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (200 / 100));
                break;
            case 3:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (250 / 100));
                break;
            case 4:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (300 / 100));
                break;
            case 5:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (350 / 100));
                break;
            case 6:
                p2_Speed = Mathf.FloorToInt(p2_Speed * (400 / 100));
                break;
        }

        if(player_Poke_Other.condition == BattleEnum.condition.paralysis)
        {
            p1_Speed = Mathf.FloorToInt(p1_Speed / 2);
        }
        if(enemy_Poke_Other.condition == BattleEnum.condition.paralysis)
        {
            p2_Speed = Mathf.FloorToInt(p2_Speed / 2);
        }
        #endregion

        #region ストレージの格納する
        p1_Storage.userData_Pokémon = p1;
        p1_Storage.othersStatus = p1_Other;
        p1_Storage.speed = p1_Speed;
        p1_Storage.priority = p1_Priority;
        p1_Storage.technique_Pokémon = p1_Tech;
        p1_Storage.battleStatus = BattleDatas_Default.user_BattleStatus;
        p1_Storage.individualFields = BattleDatas_Default.user_Fields;

        p2_Storage.userData_Pokémon = p2;
        p2_Storage.othersStatus = p2_Other;
        p2_Storage.speed = p2_Speed;
        p2_Storage.priority = p2_Priority;
        p2_Storage.technique_Pokémon = p2_Tech;
        p2_Storage.battleStatus = BattleDatas_Default.enemy_BattleStatus;
        p2_Storage.individualFields = BattleDatas_Default.enemy_Fields;
        #endregion

        #region 技発動順番計算
        if (!BattleDatas_Default.commonField.trickRoom)
        {
            if (p1_Priority < p2_Priority)
            {
                (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
            }
            else if(p1_Priority == p2_Priority)
            {
                if (p1_Speed < p2_Speed)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }
                else if (p1_Speed == p2_Speed)
                {
                    int r2 = Random.Range(1, 101);
                    if (r2 < 51)
                    {
                        (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                    }
                }
            }
        }
        else if(BattleDatas_Default.commonField.trickRoom)
        {
            if (p1_Priority > p2_Priority)
            {
                (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
            }
            else if(p1_Priority == p2_Priority)
            {
                if (p1_Speed > p2_Speed)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }
                else if (p1_Speed == p2_Speed)
                {
                    int r2 = Random.Range(1, 101);
                    if (r2 < 51)
                    {
                        (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                    }
                }
            }
        }
        #endregion
        
        print($"{p1_Storage.userData_Pokémon.userP_Name}/{p2_Storage.userData_Pokémon.userP_Name}");

        battleController.Control_TechniqueSelect.SetActive(false);
        player_PP--;

        StartCoroutine(battle());

        IEnumerator battle()
        {
            #region プレイヤー・エネミー攻撃処理
            for (int i = 0; i < 2; i++)
            {

                temporary_BattleData = new BattleStatus();

                if(battleController.player_PokeTeamNum == 255 || battleController.enemy_PokeTeamNum == 255)
                {
                    break;
                }

                if(i == 1)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }

                if(p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                {
                    isEnemy = true;
                }
                else
                {
                    isEnemy = false;
                }

                print(isEnemy);

                //急所計算
                KeyPoint = 0;
                if(p1_Storage.technique_Pokémon.t_Name == "ストーンエッジ")
                {
                     KeyPoint = Random.Range(0, 8);
                }
                else KeyPoint = Random.Range(0, 24);

                //乱数計算
                randomNumber = Random.Range(85, 101);
                randomNumber = randomNumber / 100;

                //タイプチェック
                bool isT = false;
                var c_Data = battleController.c_Data.sheet.Find(x => x.typeName == p1_Storage.technique_Pokémon.t_Type);

                if (c_Data.invalid.Contains(p2_Storage.userData_Pokémon.userP_Type1)) isT = true;
                if (c_Data.invalid.Contains(p2_Storage.userData_Pokémon.userP_Type2)) isT = true;

                yield return new WaitForSeconds(0.5f);

                int random_Hit = Random.Range(1, 101);

                print($"命中率{random_Hit}");

                if (p1_Storage.othersStatus.sleepTurn > 0)
                {
                    p1_Storage.othersStatus.sleepTurn--;
                    if(p1_Storage.othersStatus.sleepTurn <= 0)
                    {
                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　目を覚ました！";
                        p1_Storage.othersStatus.condition = BattleEnum.condition.none;
                        yield return new WaitForSeconds(1.2f);
                    }
                }

                if(p1_Storage.othersStatus.iceTurn > 0)
                {
                    p1_Storage.othersStatus.iceTurn--;
                }

                text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}の\n{p1_Storage.technique_Pokémon.t_Name}！";
                yield return new WaitForSeconds(1.2f);

                if (p1_Storage.othersStatus.iceTurn > 0)
                {
                    text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　凍っている！";
                    yield return new WaitForSeconds(1.2f);
                }
                else if(p1_Storage.othersStatus.sleepTurn > 0 && p1_Storage.othersStatus.condition == BattleEnum.condition.sleep)
                {
                    text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　眠っている！";
                    yield return new WaitForSeconds(1.2f);
                }
                else if (p1_Storage.battleStatus.frightened) //ひるみ
                {
                    text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　ひるんで　動けない！";
                    yield return new WaitForSeconds(1.2f);
                    p1_Storage.battleStatus.frightened = false;
                }
                else if(p1_Storage.technique_Pokémon.t_Hit >= random_Hit) //命中
                {
                    if (p2_Storage.battleStatus.protect) //まもる
                    {
                        text_Display.text = $"{p2_Storage.userData_Pokémon.userP_NickName}は　攻撃を　防いだ！";
                        yield return new WaitForSeconds(1.2f);
                        p1_Storage.battleStatus.protect = false;
                        p1_Storage.battleStatus.protectTrun = 1;
                    }
                    else if (isT) //こうかなし
                    {
                        text_Display.text = $"{p2_Storage.userData_Pokémon.userP_NickName}には\n効果がない　ようだ";
                        yield return new WaitForSeconds(1.2f);
                    }
                    else
                    {
                        isDone = false;
                        techniqueScript.actions[p1_Storage.technique_Pokémon.t_Name]();
                        yield return new WaitUntil(() => isDone);
                        for (var m = 0; m < message_Tech.Count; m++)
                        {
                            text_Display.text = message_Tech[m];
                            yield return new WaitForSeconds(1.3f);
                        }

                        message_Tech.Clear();
                    }

                }
                else //命中しなかった
                {
                    text_Display.text = $"{p2_Storage.userData_Pokémon.userP_NickName}には　あたらなかった";
                    yield return new WaitForSeconds(1.2f);
                }
                text_Display.text = "";

                if(p2_Storage.othersStatus.hp != 0)
                {
                    if (p2_Storage.battleStatus.frightened == true && p2_Storage.userData_Pokémon.userP_Characteristic == "せいしんりょく")
                    {
                        p2_Storage.battleStatus.frightened = false;
                    }
                }

                #region 状態異常表示
                var HpBer_Player = battleController.Pokemon1.transform.Find("HPBer_Player1");
                HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                if (p2_Storage.othersStatus.hp != 0)
                {
                    if (player_Poke_Other.condition != BattleEnum.condition.none)
                    {
                        var condition = HpBer_Player.transform.Find("condition");
                        condition.gameObject.SetActive(true);
                        Color conditionColor = new Color();
                        string conditionName = "";
                        switch (player_Poke_Other.condition)
                        {
                            case BattleEnum.condition.paralysis:
                                conditionColor = battleController.Color_Condition[0];
                                conditionName = "まひ";
                                break;
                            case BattleEnum.condition.ice:
                                conditionColor = battleController.Color_Condition[1];
                                conditionName = "こおり";
                                break;
                            case BattleEnum.condition.burn:
                                conditionColor = battleController.Color_Condition[2];
                                conditionName = "やけど";
                                break;
                            case BattleEnum.condition.poison:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.veryPoisonous:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.sleep:
                                conditionColor = battleController.Color_Condition[4];
                                conditionName = "ねむり";
                                break;
                            case BattleEnum.condition.dying:
                                conditionColor = battleController.Color_Condition[5];
                                conditionName = "ひんし";
                                break;
                        }
                        condition.gameObject.GetComponent<Image>().color = conditionColor;
                        condition.Find("Text").GetComponent<Text>().text = conditionName;
                    }
                }

                HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
                HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                if(p2_Storage.othersStatus.hp != 0)
                {
                    if (enemy_Poke_Other.condition != BattleEnum.condition.none)
                    {
                        var condition = HpBer_Player.transform.Find("condition");
                        condition.gameObject.SetActive(true);
                        Color conditionColor = new Color();
                        string conditionName = "";
                        switch (enemy_Poke_Other.condition)
                        {
                            case BattleEnum.condition.paralysis:
                                conditionColor = battleController.Color_Condition[0];
                                conditionName = "まひ";
                                break;
                            case BattleEnum.condition.ice:
                                conditionColor = battleController.Color_Condition[1];
                                conditionName = "こおり";
                                break;
                            case BattleEnum.condition.burn:
                                conditionColor = battleController.Color_Condition[2];
                                conditionName = "やけど";
                                break;
                            case BattleEnum.condition.poison:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.veryPoisonous:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.sleep:
                                conditionColor = battleController.Color_Condition[4];
                                conditionName = "ねむり";
                                break;
                            case BattleEnum.condition.dying:
                                conditionColor = battleController.Color_Condition[5];
                                conditionName = "ひんし";
                                break;
                        }
                        condition.gameObject.GetComponent<Image>().color = conditionColor;
                        condition.Find("Text").GetComponent<Text>().text = conditionName;
                    }
                }
                #endregion

                if(p2_Storage.othersStatus.hp != 0)
                {
                    if (p2_Storage.othersStatus.b_item == "Lum_Berry" && p2_Storage.othersStatus.condition != BattleEnum.condition.none)
                    {
                        if (p2_Storage.othersStatus.condition != BattleEnum.condition.dying)
                        {
                            battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は\nラムのみを　食べた！";
                            p2_Storage.othersStatus.condition = BattleEnum.condition.none;
                            yield return new WaitForSeconds(1.2f);
                            battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}の\n状態異常が　回復した！";
                            #region 状態異常表示
                            HpBer_Player = battleController.Pokemon1.transform.Find("HPBer_Player1");
                            HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                            if (player_Poke_Other.condition != BattleEnum.condition.none)
                            {
                                var condition = HpBer_Player.transform.Find("condition");
                                condition.gameObject.SetActive(true);
                                Color conditionColor = new Color();
                                string conditionName = "";
                                switch (player_Poke_Other.condition)
                                {
                                    case BattleEnum.condition.paralysis:
                                        conditionColor = battleController.Color_Condition[0];
                                        conditionName = "まひ";
                                        break;
                                    case BattleEnum.condition.ice:
                                        conditionColor = battleController.Color_Condition[1];
                                        conditionName = "こおり";
                                        break;
                                    case BattleEnum.condition.burn:
                                        conditionColor = battleController.Color_Condition[2];
                                        conditionName = "やけど";
                                        break;
                                    case BattleEnum.condition.poison:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.veryPoisonous:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.sleep:
                                        conditionColor = battleController.Color_Condition[4];
                                        conditionName = "ねむり";
                                        break;
                                    case BattleEnum.condition.dying:
                                        conditionColor = battleController.Color_Condition[5];
                                        conditionName = "ひんし";
                                        break;
                                }
                                condition.gameObject.GetComponent<Image>().color = conditionColor;
                                condition.Find("Text").GetComponent<Text>().text = conditionName;
                            }

                            HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
                            HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                            if (enemy_Poke_Other.condition != BattleEnum.condition.none)
                            {
                                var condition = HpBer_Player.transform.Find("condition");
                                condition.gameObject.SetActive(true);
                                Color conditionColor = new Color();
                                string conditionName = "";
                                switch (enemy_Poke_Other.condition)
                                {
                                    case BattleEnum.condition.paralysis:
                                        conditionColor = battleController.Color_Condition[0];
                                        conditionName = "まひ";
                                        break;
                                    case BattleEnum.condition.ice:
                                        conditionColor = battleController.Color_Condition[1];
                                        conditionName = "こおり";
                                        break;
                                    case BattleEnum.condition.burn:
                                        conditionColor = battleController.Color_Condition[2];
                                        conditionName = "やけど";
                                        break;
                                    case BattleEnum.condition.poison:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.veryPoisonous:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.sleep:
                                        conditionColor = battleController.Color_Condition[4];
                                        conditionName = "ねむり";
                                        break;
                                    case BattleEnum.condition.dying:
                                        conditionColor = battleController.Color_Condition[5];
                                        conditionName = "ひんし";
                                        break;
                                }
                                condition.gameObject.GetComponent<Image>().color = conditionColor;
                                condition.Find("Text").GetComponent<Text>().text = conditionName;
                            }
                            #endregion
                            yield return new WaitForSeconds(1.2f);
                            battleController.textDisplay.text = "";
                        }
                    }
                }
            }
            #endregion


            (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);

            #region たべのこし
            if(p1_Other.b_item == "Leftovers" && p1_Storage.othersStatus.hp > 0)
            {
                int heal = Mathf.FloorToInt(p1.userP_Real_Hp / 16);
                if(heal + p1_Other.hp >= p1.userP_Real_Hp)
                {
                    p1_Other.hp = p1.userP_Real_Hp;
                }
                else
                {
                    p1_Other.hp += heal;
                }

                battleController.player_HpSlider.value = p1_Other.hp;
                battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　たべのこしで\n体力が　少し回復した！";
                yield return new WaitForSeconds(1.2f);
                text_Display.text = "";

            }
            #endregion

            //状態異常　両方
            for(var c = 0; c < 2; c++)
            {
                if(c == 1)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }

                if (p1_Storage.othersStatus.hp > 0)
                {
                    if(p1_Storage.othersStatus.condition == BattleEnum.condition.poison)
                    {
                        int damage = Mathf.FloorToInt(p1_Storage.userData_Pokémon.userP_Real_Hp * 0.125f);
                        if(p1_Storage.othersStatus.hp - damage < 0)
                        {
                            damage = p1_Storage.othersStatus.hp;
                        }

                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　どくで\nダメージを　うけた！";
                        p1_Storage.othersStatus.hp -= damage;
                        if(p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                        {
                            battleController.player_HpSlider.value = p1_Storage.othersStatus.hp;
                            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                        }
                        else if(p1_Storage.userData_Pokémon == BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                        {
                            battleController.enemy_HpSlider.value = p1_Storage.othersStatus.hp;
                        }
                        yield return new WaitForSeconds(1.2f);
                        text_Display.text = "";
                        if (p1_Other.hp <= 0)
                        {
                            battleController.player_PokeTeamNum = 255;
                        }
                        else if (p2_Other.hp <= 2)
                        {
                            battleController.enemy_PokeTeamNum = 255;
                        }

                    }
                    else if(p1_Storage.othersStatus.condition == BattleEnum.condition.veryPoisonous)
                    {
                        if(p1_Storage.battleStatus.veryPoisonousTurn < 15)
                        {
                            p1_Storage.battleStatus.veryPoisonousTurn++;
                        }

                        int damage = Mathf.FloorToInt(p1_Storage.userData_Pokémon.userP_Real_Hp * (p1_Storage.battleStatus.veryPoisonousTurn * 0.125f));
                        if (p1_Storage.othersStatus.hp - damage < 0)
                        {
                            damage = p1_Storage.othersStatus.hp;
                        }

                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　どくで\nダメージを　うけた！";
                        p1_Storage.othersStatus.hp -= damage;
                        if (p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                        {
                            battleController.player_HpSlider.value = p1_Storage.othersStatus.hp;
                            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                        }
                        else if (p1_Storage.userData_Pokémon == BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                        {
                            battleController.enemy_HpSlider.value = p1_Storage.othersStatus.hp;
                        }
                        yield return new WaitForSeconds(1.2f);
                        text_Display.text = "";
                        if (p1_Other.hp <= 0)
                        {
                            battleController.player_PokeTeamNum = 255;
                        }
                        else if (p2_Other.hp <= 2)
                        {
                            battleController.enemy_PokeTeamNum = 255;
                        }
                    }
                    else if(p1_Storage.othersStatus.condition == BattleEnum.condition.burn)
                    {
                        int damage = Mathf.FloorToInt(p1_Storage.userData_Pokémon.userP_Real_Hp * 0.125f);
                        if (p1_Storage.othersStatus.hp - damage < 0)
                        {
                            damage = p1_Storage.othersStatus.hp;
                        }

                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　やけどで\nダメージを　うけた！";
                        p1_Storage.othersStatus.hp -= damage;
                        if (p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                        {
                            battleController.player_HpSlider.value = p1_Storage.othersStatus.hp;
                            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                        }
                        else if (p1_Storage.userData_Pokémon == BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                        {
                            battleController.enemy_HpSlider.value = p1_Storage.othersStatus.hp;
                        }
                        yield return new WaitForSeconds(1.2f);
                        text_Display.text = "";
                        if (p1_Other.hp <= 0)
                        {
                            battleController.player_PokeTeamNum = 255;
                        }
                        else if(p2_Other.hp <= 2)
                        {
                            battleController.enemy_PokeTeamNum = 255;
                        }
                    }
                }
            }

            (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
            //ねむけ　両方
            for (var c = 0; c < 2; c++)
            {
                if (c == 1)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }

                if (p1_Storage.othersStatus.hp > 0)
                {
                    if(p1_Storage.battleStatus.sleepinessTurn > 0)
                    {
                        p1_Storage.battleStatus.sleepinessTurn--;
                        if(p1_Storage.battleStatus.sleepinessTurn <= 0)
                        {
                            int sleep = Random.Range(2, 5);
                            p1_Storage.othersStatus.condition = BattleEnum.condition.sleep;
                            p1_Storage.othersStatus.sleepTurn = sleep;

                            text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　眠ってしまった！";
                            yield return new WaitForSeconds(1.2f);
                        }
                    }
                }


                if (p1_Storage.battleStatus.protectTrun == 1 && !p1_Storage.battleStatus.protect)
                {
                    p1_Storage.battleStatus.protectTrun = 0;
                }
                if (p2_Storage.battleStatus.protectTrun == 1 && !p2_Storage.battleStatus.protect)
                {
                    p2_Storage.battleStatus.protectTrun = 0;
                }

                p1_Storage.battleStatus.frightened = false;
                p1_Storage.battleStatus.protect = false;
                p2_Storage.battleStatus.frightened = false;
                p2_Storage.battleStatus.protect = false;
            }

            if(BattleDatas_Default.commonField.trickRoomTurn > 0)
            {
                BattleDatas_Default.commonField.trickRoomTurn--;
                if(BattleDatas_Default.commonField.trickRoomTurn <= 0)
                {
                    text_Display.text = "ゆがんだ時空が　元に　戻った！";
                    BattleDatas_Default.commonField.trickRoom = false;
                }
            }

            if (p1_Other.hp <= 0)
            {
                battleController.player_PokeTeamNum = 255;
                text_Display.text = $"{p1.userP_NickName}は　倒れた";
                yield return new WaitForSeconds(1.2f);
            }

            if (p2_Other.hp <= 0)
            {
                battleController.enemy_PokeTeamNum = 255;
                text_Display.text = $"{p2.userP_NickName}は　倒れた";
                yield return new WaitForSeconds(1.2f);
            }

            yield return new WaitForSeconds(1);
            print("ターン終了");
            TurnEnd();
        }
    }

    public void TurnEnd()
    {
        if(battleController.player_PokeTeamNum == 255)
        {
            battleController.player_Anim.SetBool("p_In", false);
            battleController.Audio_SE.PlayOneShot(inOut_Audio[1]);
        }
        if(battleController.enemy_PokeTeamNum == 255)
        {
            battleController.enemy_Anim.SetBool("p_In", false);
            battleController.Audio_SE.PlayOneShot(inOut_Audio[1]);
        }


        if (BattleDatas_Default.enemy_OthersStatus[0].hp <= 0 && BattleDatas_Default.enemy_OthersStatus[1].hp <= 0 && BattleDatas_Default.enemy_OthersStatus[2].hp <= 0)
        {
            if (!isLose && !isWin)
            {
                isWin = true;
            }
        }

        if (BattleDatas_Default.user_OthersStatus[0].hp <= 0 && BattleDatas_Default.user_OthersStatus[1].hp <= 0 && BattleDatas_Default.user_OthersStatus[2].hp <= 0)
        {
            if (!isWin && !isLose)
            {
                isLose = true;
            }
        }

        #region 状態異常表示
        var HpBer_Player = battleController.Pokemon1.transform.Find("HPBer_Player1");
        HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
        if(battleController.player_PokeTeamNum != 255)
        {
            if (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].condition != BattleEnum.condition.none)
            {
                var condition = HpBer_Player.transform.Find("condition");
                condition.gameObject.SetActive(true);
                Color conditionColor = new Color();
                string conditionName = "";
                switch (BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum].condition)
                {
                    case BattleEnum.condition.paralysis:
                        conditionColor = battleController.Color_Condition[0];
                        conditionName = "まひ";
                        break;
                    case BattleEnum.condition.ice:
                        conditionColor = battleController.Color_Condition[1];
                        conditionName = "こおり";
                        break;
                    case BattleEnum.condition.burn:
                        conditionColor = battleController.Color_Condition[2];
                        conditionName = "やけど";
                        break;
                    case BattleEnum.condition.poison:
                        conditionColor = battleController.Color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.veryPoisonous:
                        conditionColor = battleController.Color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.sleep:
                        conditionColor = battleController.Color_Condition[4];
                        conditionName = "ねむり";
                        break;
                    case BattleEnum.condition.dying:
                        conditionColor = battleController.Color_Condition[5];
                        conditionName = "ひんし";
                        break;
                }
                condition.gameObject.GetComponent<Image>().color = conditionColor;
                condition.Find("Text").GetComponent<Text>().text = conditionName;
            }
        }

        HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
        HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
        if (battleController.enemy_PokeTeamNum != 255) 
        {
            if (BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].condition != BattleEnum.condition.none)
            {
                var condition = HpBer_Player.transform.Find("condition");
                condition.gameObject.SetActive(true);
                Color conditionColor = new Color();
                string conditionName = "";
                switch (BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum].condition)
                {
                    case BattleEnum.condition.paralysis:
                        conditionColor = battleController.Color_Condition[0];
                        conditionName = "まひ";
                        break;
                    case BattleEnum.condition.ice:
                        conditionColor = battleController.Color_Condition[1];
                        conditionName = "こおり";
                        break;
                    case BattleEnum.condition.burn:
                        conditionColor = battleController.Color_Condition[2];
                        conditionName = "やけど";
                        break;
                    case BattleEnum.condition.poison:
                        conditionColor = battleController.Color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.veryPoisonous:
                        conditionColor = battleController.Color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.sleep:
                        conditionColor = battleController.Color_Condition[4];
                        conditionName = "ねむり";
                        break;
                    case BattleEnum.condition.dying:
                        conditionColor = battleController.Color_Condition[5];
                        conditionName = "ひんし";
                        break;
                }
                condition.gameObject.GetComponent<Image>().color = conditionColor;
                condition.Find("Text").GetComponent<Text>().text = conditionName;
            }
        }
        #endregion

        StartCoroutine(NextTurn());

        IEnumerator NextTurn()
        {
            yield return new WaitForSeconds(1.2f);
            SetBattleZone();
        }
    }

    public void enemyAttack()
    {
        #region 定義
        var player_Poke = BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum];
        var player_Poke_Other = BattleDatas_Default.user_OthersStatus[battleController.player_PokeTeamNum];
        var enemy_Poke = BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum];
        var enemy_Poke_Other = BattleDatas_Default.enemy_OthersStatus[battleController.enemy_PokeTeamNum];
        var text_Display = battleController.textDisplay;
        #endregion

        #region 相手のポケモンの技決定
        string enemy_TechName = "";
        Technique_Pokémon enemy_Technique = new Technique_Pokémon();
        int r = Random.Range(0, 100);
        if (r < 30)
        {
            enemy_TechName = enemy_Poke.set_Technique1;
        }
        else if (30 <= r && r < 60)
        {
            enemy_TechName = enemy_Poke.set_Technique2;
        }
        else if (60 <= r && r < 80)
        {
            enemy_TechName = enemy_Poke.set_Technique3;
        }
        else if (80 <= r)
        {
            enemy_TechName = enemy_Poke.set_Technique4;
        }
        enemy_Technique = DataLists.titleData_Technique.Find(x => x.t_Name == enemy_TechName);
        #endregion

        text_Display.text = "";

        var p1 = player_Poke;
        var p1_Other = player_Poke_Other;
        var p2 = enemy_Poke;
        var p2_Other = enemy_Poke_Other;

        int p1_Speed = player_Poke.userP_Real_S;
        int p2_Speed = enemy_Poke.userP_Real_S;

        int p1_Priority = 0;
        int p2_Priority = enemy_Technique.t_Priority;

        Technique_Pokémon p1_Tech = new Technique_Pokémon();
        var p2_Tech = enemy_Technique;

        p1_Storage = new BattleDataStorage();
        p2_Storage = new BattleDataStorage();

        #region ストレージの格納する
        p1_Storage.userData_Pokémon = p1;
        p1_Storage.othersStatus = p1_Other;
        p1_Storage.speed = p1_Speed;
        p1_Storage.priority = p1_Priority;
        p1_Storage.technique_Pokémon = p1_Tech;
        p1_Storage.battleStatus = BattleDatas_Default.user_BattleStatus;
        p1_Storage.individualFields = BattleDatas_Default.user_Fields;

        p2_Storage.userData_Pokémon = p2;
        p2_Storage.othersStatus = p2_Other;
        p2_Storage.speed = p2_Speed;
        p2_Storage.priority = p2_Priority;
        p2_Storage.technique_Pokémon = p2_Tech;
        p2_Storage.battleStatus = BattleDatas_Default.enemy_BattleStatus;
        p2_Storage.individualFields = BattleDatas_Default.enemy_Fields;
        #endregion        

        battleController.Control_TechniqueSelect.SetActive(false);

        (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);

        StartCoroutine(battle());

        IEnumerator battle()
        {
            #region エネミー攻撃処理
            for (int i = 0; i < 1; i++)
            {

                temporary_BattleData = new BattleStatus();

                if (battleController.player_PokeTeamNum == 255 || battleController.enemy_PokeTeamNum == 255)
                {
                    break;
                }

                if (p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                {
                    isEnemy = true;
                }
                else
                {
                    isEnemy = false;
                }

                //急所計算
                KeyPoint = 0;
                if (p1_Storage.technique_Pokémon.t_Name == "ストーンエッジ")
                {
                    KeyPoint = Random.Range(0, 8);
                }
                else KeyPoint = Random.Range(0, 24);

                //乱数計算
                randomNumber = Random.Range(85, 101);
                randomNumber = randomNumber / 100;

                //タイプチェック
                bool isT = false;
                var c_Data = battleController.c_Data.sheet.Find(x => x.typeName == p1_Storage.technique_Pokémon.t_Type);

                if (c_Data.invalid.Contains(p2_Storage.userData_Pokémon.userP_Type1)) isT = true;
                if (c_Data.invalid.Contains(p2_Storage.userData_Pokémon.userP_Type2)) isT = true;

                yield return new WaitForSeconds(0.5f);

                int random_Hit = Random.Range(1, 101);

                if (p1_Storage.othersStatus.sleepTurn > 0)
                {
                    p1_Storage.othersStatus.sleepTurn--;
                    if (p1_Storage.othersStatus.sleepTurn <= 0)
                    {
                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　目を覚ました！";
                        p1_Storage.othersStatus.condition = BattleEnum.condition.none;
                        yield return new WaitForSeconds(1.2f);
                    }
                }

                if (p1_Storage.othersStatus.iceTurn > 0)
                {
                    p1_Storage.othersStatus.iceTurn--;
                }

                text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}の\n{p1_Storage.technique_Pokémon.t_Name}！";
                yield return new WaitForSeconds(1.2f);

                if (p1_Storage.othersStatus.iceTurn > 0)
                {
                    text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　凍っている！";
                    yield return new WaitForSeconds(1.2f);
                }
                else if (p1_Storage.othersStatus.sleepTurn > 0 && p1_Storage.othersStatus.condition == BattleEnum.condition.sleep)
                {
                    text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　眠っている！";
                    yield return new WaitForSeconds(1.2f);
                }
                else if (p1_Storage.battleStatus.frightened) //ひるみ
                {
                    text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　ひるんで　動けない！";
                    yield return new WaitForSeconds(1.2f);
                    p1_Storage.battleStatus.frightened = false;
                }
                else if (p1_Storage.technique_Pokémon.t_Hit > random_Hit) //命中
                {

                    if (p1_Storage.battleStatus.protect) //まもる
                    {
                        text_Display.text = $"{p2_Storage.userData_Pokémon.userP_NickName}は　攻撃を　防いだ！";
                        yield return new WaitForSeconds(1.2f);
                        p1_Storage.battleStatus.protect = false;
                    }
                    
                    else if (isT) //こうかなし
                    {
                        text_Display.text = $"{p2_Storage.userData_Pokémon.userP_NickName}には\n効果がない　ようだ";
                        yield return new WaitForSeconds(1.2f);
                    }
                    else
                    {
                        print("技発動");
                        isDone = false;
                        techniqueScript.actions[p1_Storage.technique_Pokémon.t_Name]();
                        yield return new WaitUntil(() => isDone);
                        for (var m = 0; m < message_Tech.Count; m++)
                        {
                            text_Display.text = message_Tech[m];
                            yield return new WaitForSeconds(1.3f);
                        }

                        message_Tech.Clear();
                        print("技終了");
                    }

                }
                else //命中しなかった
                {
                    text_Display.text = $"{p2_Storage.userData_Pokémon.userP_NickName}には　あたらなかった";
                    yield return new WaitForSeconds(1.2f);
                }
                text_Display.text = "";

                if (p2_Storage.othersStatus.hp != 0)
                {
                    if (p2_Storage.battleStatus.frightened == true && p2_Storage.userData_Pokémon.userP_Characteristic == "せいしんりょく")
                    {
                        p2_Storage.battleStatus.frightened = false;
                    }
                }


                #region 状態異常表示
                var HpBer_Player = battleController.Pokemon1.transform.Find("HPBer_Player1");
                HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                if (p2_Storage.othersStatus.hp != 0)
                {
                    if (player_Poke_Other.condition != BattleEnum.condition.none)
                    {
                        var condition = HpBer_Player.transform.Find("condition");
                        condition.gameObject.SetActive(true);
                        Color conditionColor = new Color();
                        string conditionName = "";
                        switch (player_Poke_Other.condition)
                        {
                            case BattleEnum.condition.paralysis:
                                conditionColor = battleController.Color_Condition[0];
                                conditionName = "まひ";
                                break;
                            case BattleEnum.condition.ice:
                                conditionColor = battleController.Color_Condition[1];
                                conditionName = "こおり";
                                break;
                            case BattleEnum.condition.burn:
                                conditionColor = battleController.Color_Condition[2];
                                conditionName = "やけど";
                                break;
                            case BattleEnum.condition.poison:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.veryPoisonous:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.sleep:
                                conditionColor = battleController.Color_Condition[4];
                                conditionName = "ねむり";
                                break;
                            case BattleEnum.condition.dying:
                                conditionColor = battleController.Color_Condition[5];
                                conditionName = "ひんし";
                                break;
                        }
                        condition.gameObject.GetComponent<Image>().color = conditionColor;
                        condition.Find("Text").GetComponent<Text>().text = conditionName;
                    }
                }

                HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
                HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                if (p2_Storage.othersStatus.hp != 0)
                {
                    if (enemy_Poke_Other.condition != BattleEnum.condition.none)
                    {
                        var condition = HpBer_Player.transform.Find("condition");
                        condition.gameObject.SetActive(true);
                        Color conditionColor = new Color();
                        string conditionName = "";
                        switch (enemy_Poke_Other.condition)
                        {
                            case BattleEnum.condition.paralysis:
                                conditionColor = battleController.Color_Condition[0];
                                conditionName = "まひ";
                                break;
                            case BattleEnum.condition.ice:
                                conditionColor = battleController.Color_Condition[1];
                                conditionName = "こおり";
                                break;
                            case BattleEnum.condition.burn:
                                conditionColor = battleController.Color_Condition[2];
                                conditionName = "やけど";
                                break;
                            case BattleEnum.condition.poison:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.veryPoisonous:
                                conditionColor = battleController.Color_Condition[3];
                                conditionName = "どく";
                                break;
                            case BattleEnum.condition.sleep:
                                conditionColor = battleController.Color_Condition[4];
                                conditionName = "ねむり";
                                break;
                            case BattleEnum.condition.dying:
                                conditionColor = battleController.Color_Condition[5];
                                conditionName = "ひんし";
                                break;
                        }
                        condition.gameObject.GetComponent<Image>().color = conditionColor;
                        condition.Find("Text").GetComponent<Text>().text = conditionName;
                    }
                }
                #endregion

                if (p2_Storage.othersStatus.hp != 0)
                {
                    if (p2_Storage.othersStatus.b_item == "Lum_Berry" && p2_Storage.othersStatus.condition != BattleEnum.condition.none)
                    {
                        if (p2_Storage.othersStatus.condition != BattleEnum.condition.dying)
                        {
                            battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}は\nラムのみを　食べた！";
                            p2_Storage.othersStatus.condition = BattleEnum.condition.none;
                            yield return new WaitForSeconds(1.2f);
                            battleController.textDisplay.text = $"{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_NickName}の\n状態異常が　回復した！";
                            #region 状態異常表示
                            HpBer_Player = battleController.Pokemon1.transform.Find("HPBer_Player1");
                            HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                            if (player_Poke_Other.condition != BattleEnum.condition.none)
                            {
                                var condition = HpBer_Player.transform.Find("condition");
                                condition.gameObject.SetActive(true);
                                Color conditionColor = new Color();
                                string conditionName = "";
                                switch (player_Poke_Other.condition)
                                {
                                    case BattleEnum.condition.paralysis:
                                        conditionColor = battleController.Color_Condition[0];
                                        conditionName = "まひ";
                                        break;
                                    case BattleEnum.condition.ice:
                                        conditionColor = battleController.Color_Condition[1];
                                        conditionName = "こおり";
                                        break;
                                    case BattleEnum.condition.burn:
                                        conditionColor = battleController.Color_Condition[2];
                                        conditionName = "やけど";
                                        break;
                                    case BattleEnum.condition.poison:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.veryPoisonous:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.sleep:
                                        conditionColor = battleController.Color_Condition[4];
                                        conditionName = "ねむり";
                                        break;
                                    case BattleEnum.condition.dying:
                                        conditionColor = battleController.Color_Condition[5];
                                        conditionName = "ひんし";
                                        break;
                                }
                                condition.gameObject.GetComponent<Image>().color = conditionColor;
                                condition.Find("Text").GetComponent<Text>().text = conditionName;
                            }

                            HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
                            HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
                            if (enemy_Poke_Other.condition != BattleEnum.condition.none)
                            {
                                var condition = HpBer_Player.transform.Find("condition");
                                condition.gameObject.SetActive(true);
                                Color conditionColor = new Color();
                                string conditionName = "";
                                switch (enemy_Poke_Other.condition)
                                {
                                    case BattleEnum.condition.paralysis:
                                        conditionColor = battleController.Color_Condition[0];
                                        conditionName = "まひ";
                                        break;
                                    case BattleEnum.condition.ice:
                                        conditionColor = battleController.Color_Condition[1];
                                        conditionName = "こおり";
                                        break;
                                    case BattleEnum.condition.burn:
                                        conditionColor = battleController.Color_Condition[2];
                                        conditionName = "やけど";
                                        break;
                                    case BattleEnum.condition.poison:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.veryPoisonous:
                                        conditionColor = battleController.Color_Condition[3];
                                        conditionName = "どく";
                                        break;
                                    case BattleEnum.condition.sleep:
                                        conditionColor = battleController.Color_Condition[4];
                                        conditionName = "ねむり";
                                        break;
                                    case BattleEnum.condition.dying:
                                        conditionColor = battleController.Color_Condition[5];
                                        conditionName = "ひんし";
                                        break;
                                }
                                condition.gameObject.GetComponent<Image>().color = conditionColor;
                                condition.Find("Text").GetComponent<Text>().text = conditionName;
                            }
                            #endregion
                            yield return new WaitForSeconds(1.2f);
                            battleController.textDisplay.text = "";
                        }
                    }
                }
            }
            #endregion

            #region たべのこし
            if (p1_Other.b_item == "Leftovers" && p1_Storage.othersStatus.hp > 0)
            {
                int heal = Mathf.FloorToInt(p1.userP_Real_Hp / 16);
                if (heal + p1_Other.hp >= p1.userP_Real_Hp)
                {
                    p1_Other.hp = p1.userP_Real_Hp;
                }
                else
                {
                    p1_Other.hp += heal;
                }

                battleController.player_HpSlider.value = p1_Other.hp;
                battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　たべのこしで\n体力が　少し回復した！";
                yield return new WaitForSeconds(1.2f);
                text_Display.text = "";

            }
            #endregion

            //状態異常　両方
            for (var c = 0; c < 2; c++)
            {
                if (c == 1)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }

                print(p1_Storage.userData_Pokémon.userP_Name);

                if (p1_Storage.othersStatus.hp > 0)
                {
                    if (p1_Storage.othersStatus.condition == BattleEnum.condition.poison)
                    {
                        int damage = Mathf.FloorToInt(p1_Storage.userData_Pokémon.userP_Real_Hp * 0.125f);
                        if (p1_Storage.othersStatus.hp - damage < 0)
                        {
                            damage = p1_Storage.othersStatus.hp;
                        }

                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　どくで\nダメージを　うけた！";
                        p1_Storage.othersStatus.hp -= damage;
                        if (p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                        {
                            battleController.player_HpSlider.value = p1_Storage.othersStatus.hp;
                            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                        }
                        else if (p1_Storage.userData_Pokémon == BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                        {
                            battleController.enemy_HpSlider.value = p1_Storage.othersStatus.hp;
                        }
                        yield return new WaitForSeconds(1.2f);
                        text_Display.text = "";
                        if (p1_Other.hp <= 0)
                        {
                            battleController.player_PokeTeamNum = 255;
                        }
                        else if (p2_Other.hp <= 2)
                        {
                            battleController.enemy_PokeTeamNum = 255;
                        }

                    }
                    else if (p1_Storage.othersStatus.condition == BattleEnum.condition.veryPoisonous)
                    {
                        if (p1_Storage.battleStatus.veryPoisonousTurn < 15)
                        {
                            p1_Storage.battleStatus.veryPoisonousTurn++;
                        }

                        int damage = Mathf.FloorToInt(p1_Storage.userData_Pokémon.userP_Real_Hp * (p1_Storage.battleStatus.veryPoisonousTurn * 0.125f));
                        if (p1_Storage.othersStatus.hp - damage < 0)
                        {
                            damage = p1_Storage.othersStatus.hp;
                        }

                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　どくで\nダメージを　うけた！";
                        p1_Storage.othersStatus.hp -= damage;
                        if (p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                        {
                            battleController.player_HpSlider.value = p1_Storage.othersStatus.hp;
                            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                        }
                        else if (p1_Storage.userData_Pokémon == BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                        {
                            battleController.enemy_HpSlider.value = p1_Storage.othersStatus.hp;
                        }
                        yield return new WaitForSeconds(1.2f);
                        text_Display.text = "";
                        if (p1_Other.hp <= 0)
                        {
                            battleController.player_PokeTeamNum = 255;
                        }
                        else if (p2_Other.hp <= 2)
                        {
                            battleController.enemy_PokeTeamNum = 255;
                        }
                    }
                    else if (p1_Storage.othersStatus.condition == BattleEnum.condition.burn)
                    {
                        int damage = Mathf.FloorToInt(p1_Storage.userData_Pokémon.userP_Real_Hp * 0.125f);
                        if (p1_Storage.othersStatus.hp - damage < 0)
                        {
                            damage = p1_Storage.othersStatus.hp;
                        }

                        text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　やけどで\nダメージを　うけた！";
                        p1_Storage.othersStatus.hp -= damage;
                        if (p1_Storage.userData_Pokémon == BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum])
                        {
                            battleController.player_HpSlider.value = p1_Storage.othersStatus.hp;
                            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas_Default.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
                        }
                        else if (p1_Storage.userData_Pokémon == BattleDatas_Default.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                        {
                            battleController.enemy_HpSlider.value = p1_Storage.othersStatus.hp;
                        }
                        yield return new WaitForSeconds(1.2f);
                        text_Display.text = "";
                        if (p1_Other.hp <= 0)
                        {
                            battleController.player_PokeTeamNum = 255;
                        }
                        else if (p2_Other.hp <= 2)
                        {
                            battleController.enemy_PokeTeamNum = 255;
                        }
                    }
                }
            }

            if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }

            if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 5)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 2)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }

            (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
            //ねむけ　両方
            for (var c = 0; c < 2; c++)
            {
                if (c == 1)
                {
                    (p1_Storage, p2_Storage) = (p2_Storage, p1_Storage);
                }

                if (p1_Storage.othersStatus.hp > 0)
                {
                    if (p1_Storage.battleStatus.sleepinessTurn > 0)
                    {
                        p1_Storage.battleStatus.sleepinessTurn--;
                        if (p1_Storage.battleStatus.sleepinessTurn <= 0)
                        {
                            int sleep = Random.Range(2, 5);
                            p1_Storage.othersStatus.condition = BattleEnum.condition.sleep;
                            p1_Storage.othersStatus.sleepTurn = sleep;

                            text_Display.text = $"{p1_Storage.userData_Pokémon.userP_NickName}は　眠ってしまった！";
                            yield return new WaitForSeconds(1.2f);
                        }
                    }
                }

                if (p1_Storage.battleStatus.protectTrun == 1 && !p1_Storage.battleStatus.protect)
                {
                    p1_Storage.battleStatus.protectTrun = 0;
                }
                if (p2_Storage.battleStatus.protectTrun == 1 && !p2_Storage.battleStatus.protect)
                {
                    p2_Storage.battleStatus.protectTrun = 0;
                }

                p1_Storage.battleStatus.frightened = false;
                p1_Storage.battleStatus.protect = false;
                p2_Storage.battleStatus.frightened = false;
                p2_Storage.battleStatus.protect = false;
            }

            if (BattleDatas_Default.commonField.trickRoomTurn > 0)
            {
                BattleDatas_Default.commonField.trickRoomTurn--;
                if (BattleDatas_Default.commonField.trickRoomTurn <= 0)
                {
                    text_Display.text = "ゆがんだ時空が　元に　戻った！";
                    BattleDatas_Default.commonField.trickRoom = false;
                }
            }

            if (p1_Other.hp <= 0)
            {
                battleController.player_PokeTeamNum = 255;
                text_Display.text = $"{p1.userP_NickName}は　倒れた";
                yield return new WaitForSeconds(1.2f);
            }

            if (p2_Other.hp <= 2)
            {
                battleController.enemy_PokeTeamNum = 255;
                text_Display.text = $"{p2.userP_NickName}は　倒れた";
                yield return new WaitForSeconds(1.2f);
            }

            yield return new WaitForSeconds(1);
            print("ターン終了");
            TurnEnd();
        }
    }
}
