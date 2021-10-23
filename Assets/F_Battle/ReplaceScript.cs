using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ReplaceScript : MonoBehaviour
{
    public BattleController battleController;

    public TurnScript turnScript;

    private int changePokeId;
    public bool isAttack = false;

    public void EnemyReplace()
    {
        battleController.Control_ModeSelect.SetActive(false);
        battleController.textDisplay.text = "";

        var enemy_Poke_Image = battleController.Pokemon2.transform.Find("Image_Poke_Player2").GetComponent<Image>();
        var poke = BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum];
        var pokeData = battleController.p_ImageDatas.sheet.Where(x => x.p_Id == poke.userP_Id);
        ImageData_Pokémon.GenderType genderType = new ImageData_Pokémon.GenderType();
        foreach (var p in pokeData)
        {
            if (p.genderType == ImageData_Pokémon.GenderType.same)
            {
                genderType = ImageData_Pokémon.GenderType.same;
                break;
            }
            else
            {
                if (poke.userP_gender == 0)
                {
                    genderType = ImageData_Pokémon.GenderType.maleOnly;
                    break;
                }
                else if (poke.userP_gender == 1)
                {
                    genderType = ImageData_Pokémon.GenderType.femeleOnly;
                    break;
                }
            }
        }
        var image = pokeData.Where(x => x.genderType == genderType);
        foreach (var pp in image)
        {
            if (poke.isDifferentColors)
            {
                enemy_Poke_Image.sprite = pp.p_ImageFront_C;
            }
            else
            {
                enemy_Poke_Image.sprite = pp.p_ImageFront;
            }
            break;
        }

        var HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
        var slider_Player = HpBer_Player.transform.Find("Slider").GetComponent<Slider>();
        slider_Player.minValue = Mathf.Floor(poke.userP_Real_Hp / 36.4f * -1);
        slider_Player.maxValue = poke.userP_Real_Hp;
        slider_Player.value = BattleDatas.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp;
        if (slider_Player.value <= slider_Player.maxValue / 5)
        {
            slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
        }
        else if (slider_Player.value <= slider_Player.maxValue / 2)
        {
            slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
        }
        else
        {
            slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
        }

        HpBer_Player.transform.Find("Text_PokeName").GetComponent<Text>().text = poke.userP_Name;

        var genderText = HpBer_Player.Find("Text_Gender").GetComponent<Text>();
        genderText.gameObject.SetActive(true);
        if (poke.userP_gender == 0)
        {
            genderText.text = "♂"; genderText.color = battleController.Color_Gender[0];
        }
        else if (poke.userP_gender == 1)
        {
            genderText.text = "♀"; genderText.color = battleController.Color_Gender[1];
        }
        else if (poke.userP_gender == 2)
        {
            genderText.gameObject.SetActive(false);
        }
        HpBer_Player.Find("Text_Level").GetComponent<Text>().text = poke.userP_Level.ToString();

        var voice_Enemy = battleController.p_VoiceDatas.sheet.Find(x => x.p_Id == poke.userP_Id).voiceData;
        var enemy_PokeName = poke.userP_NickName;

        var enemy_Speed = poke.userP_Real_S;
        if (BattleDatas.enemy_OthersStatus[battleController.enemy_PokeTeamNum].b_item == "こだわりスカーフ")
        {
            enemy_Speed = Mathf.FloorToInt(enemy_Speed * 1.5f);
        }

        var enemy_Chara = poke.userP_Characteristic;

        #region 手持ちポケモン表示（相手のポケモン）
        if (battleController.enemy_PokeTeamNum == 1)
        {
            var image_EnemyPoke = battleController.On_hand_List.transform.Find($"P2_poke2").GetComponent<Image>();
            var data = battleController.p_ImageDatas.sheet.Find(x => x.p_Id == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Id);
            if (BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].isDifferentColors)
            {
                image_EnemyPoke.sprite = data.p_ImageHand_C;
            }
            else
            {
                image_EnemyPoke.sprite = data.p_ImageHand;
            }
        }
        else if (battleController.enemy_PokeTeamNum == 2)
        {
            var image_EnemyPoke = battleController.On_hand_List.transform.Find($"P2_poke3").GetComponent<Image>();
            var data = battleController.p_ImageDatas.sheet.Find(x => x.p_Id == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Id);
            if (BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].isDifferentColors)
            {
                image_EnemyPoke.sprite = data.p_ImageHand_C;
            }
            else
            {
                image_EnemyPoke.sprite = data.p_ImageHand;
            }
        }
        #endregion

        StartCoroutine(inPokemon());

        IEnumerator inPokemon()
        {
            battleController.Control_ModeSelect.SetActive(false);
            yield return new WaitForSeconds(1);
            battleController.enemy_Anim.SetBool("p_In", true);
            battleController.Audio_SE.PlayOneShot(turnScript.inOut_Audio[0]);
            yield return new WaitForSeconds(0.25f);
            battleController.textDisplay.text = $"{enemy_PokeName}があらわれた！";
            yield return new WaitForSeconds(0.75f);
            battleController.Audio_Voice.PlayOneShot(voice_Enemy);
            yield return new WaitForSeconds(2);

            var p1_Image_Characteristic = battleController.Pokemon2.transform.Find("Image_Characteristic");

            var player2 = BattleDatas.user_PokemonData[battleController.player_PokeTeamNum];
            var player1 = BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum];

            if (player1.userP_Characteristic == "きけんよち")
            {
                bool isFear = false;
                for (var i = 0; i < 4; i++)
                {
                    string technique = "";
                    switch (i)
                    {
                        case 0:
                            technique = player2.set_Technique1;
                            break;
                        case 1:
                            technique = player2.set_Technique2;
                            break;
                        case 2:
                            technique = player2.set_Technique3;
                            break;
                        case 3:
                            technique = player2.set_Technique4;
                            break;
                    }

                    var techData = DataLists.titleData_Technique.Find(x => x.t_Name == technique);

                    var type = battleController.c_Data.sheet.Find(x => x.typeName == techData.t_Type);
                    if (!type.invalid.Contains(player1.userP_Type1) && !type.invalid.Contains(player1.userP_Type2))
                    {
                        if (type.twice.Contains(player1.userP_Type1) || type.twice.Contains(player1.userP_Type2))
                        {
                            isFear = true;
                        }
                    }
                }

                p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_NickName + "の";
                p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                battleController.enemy_Anim.SetBool("c_In", true);
                yield return new WaitForSeconds(1.5f);
                battleController.enemy_Anim.SetBool("c_In", false);
                if (isFear)
                {
                    battleController.textDisplay.text = $"{player1.userP_NickName}は みぶるいした！";
                    yield return new WaitForSeconds(2);
                }
            }
            else if (player1.userP_Characteristic == "テラボルテージ")
            {
                p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_NickName + "の";
                p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                battleController.enemy_Anim.SetBool("c_In", true);
                yield return new WaitForSeconds(2f);
                battleController.enemy_Anim.SetBool("c_In", false);
            }

            if (BattleDatas.enemy_Fields.stealthRock == true)
            {
                var type = battleController.c_Data.sheet.Find(x => x.type == Pokémon_Type.Type.Rock);
                int damage = player1.userP_Real_Hp / 8;
                if (type.twice.Contains(player1.userP_Type1))
                {
                    damage = damage * 2;
                }
                else if (type.half.Contains(player1.userP_Type1))
                {
                    damage = damage / 2;
                }
                if (type.twice.Contains(player1.userP_Type2))
                {
                    damage = damage * 2;
                }
                else if (type.half.Contains(player1.userP_Type2))
                {
                    damage = damage / 2;
                }
                if (damage <= 0) { damage = 1; }
                BattleDatas.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp -= Mathf.FloorToInt(damage);
                battleController.textDisplay.text = $"{player1.userP_NickName}に\nとがった岩が　食いこんだ！";
                yield return new WaitForSeconds(1.2f);
                battleController.textDisplay.text = "";
            }

            battleController.textDisplay.text = $"行動を選択してください";
            battleController.Control_ModeSelect.SetActive(true);
        }
    }

    public void Confirmation(int num)
    {
        battleController.Control_ReplaceSelect.transform.Find("Poke1").gameObject.SetActive(false);
        battleController.Control_ReplaceSelect.transform.Find("Poke2").gameObject.SetActive(false);
        battleController.Control_ReplaceSelect.transform.Find("ConfirmationScreen").gameObject.SetActive(true);
        battleController.Control_ReplaceSelect.transform.Find("Button_Back").gameObject.SetActive(false);

        isAttack = false;
        changePokeId = num;
        if(battleController.player_PokeTeamNum != 255)
        {
            isAttack = true;
        }
        battleController.textDisplay.text = "ポケモンを入れ替えますか？";
    }

    public void Cancel()
    {
        battleController.Control_ReplaceSelect.transform.Find("Poke1").gameObject.SetActive(true);
        battleController.Control_ReplaceSelect.transform.Find("Poke2").gameObject.SetActive(true);
        battleController.Control_ReplaceSelect.transform.Find("ConfirmationScreen").gameObject.SetActive(false);

        if(battleController.player_PokeTeamNum != 255)
        {
            battleController.Control_ReplaceSelect.transform.Find("Button_Back").gameObject.SetActive(true);
        }
        battleController.textDisplay.text = "行動を選択してください";
    }

    public void OutPoke()
    {
        battleController.Control_ReplaceSelect.transform.Find("ConfirmationScreen").gameObject.SetActive(false);
        battleController.textDisplay.text = "";
        StartCoroutine(move());

        IEnumerator move()
        {
            battleController.player_Anim.SetBool("p_In", false);
            battleController.Audio_SE.PlayOneShot(turnScript.inOut_Audio[1]);
            yield return new WaitForSeconds(0.5f);
            PlayerReplace();
        }
    }

    public void PlayerReplace()
    {
        battleController.player_PokeTeamNum = changePokeId;

        int player_Speed = 0;
        int enemy_Speed = 0;
        string player_Chara = "";
        string enemy_Chara = "";

        #region プレイヤーポケモンの表示・UI
        var player_Poke_Image = battleController.Pokemon1.transform.Find("Image_Poke_Player1").GetComponent<Image>();
        var poke = BattleDatas.user_PokemonData[battleController.player_PokeTeamNum];
        var pokeData = battleController.p_ImageDatas.sheet.Where(x => x.p_Id == poke.userP_Id);
        ImageData_Pokémon.GenderType genderType = new ImageData_Pokémon.GenderType();
        foreach (var p in pokeData)
        {
            if (p.genderType == ImageData_Pokémon.GenderType.same)
            {
                genderType = ImageData_Pokémon.GenderType.same;
                break;
            }
            else
            {
                if (poke.userP_gender == 0)
                {
                    genderType = ImageData_Pokémon.GenderType.maleOnly;
                    break;
                }
                else if (poke.userP_gender == 1)
                {
                    genderType = ImageData_Pokémon.GenderType.femeleOnly;
                    break;
                }
            }
        }
        var image = pokeData.Where(x => x.genderType == genderType);
        foreach (var pp in image)
        {
            if (poke.isDifferentColors)
            {
                player_Poke_Image.sprite = pp.p_ImageBack_C;
            }
            else
            {
                player_Poke_Image.sprite = pp.p_ImageBack;
            }
            break;
        }

        var HpBer_Player = battleController.Pokemon1.transform.Find("HPBer_Player1");
        var slider_Player = HpBer_Player.transform.Find("Slider").GetComponent<Slider>();
        slider_Player.minValue = Mathf.Floor(poke.userP_Real_Hp / 36.4f * -1);
        slider_Player.maxValue = poke.userP_Real_Hp;
        slider_Player.value = BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].hp;
        if (slider_Player.value <= slider_Player.maxValue / 5)
        {
            slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
        }
        else if (slider_Player.value <= slider_Player.maxValue / 2)
        {
            slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
        }
        else
        {
            slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
        }

        HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
        if (BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].condition != BattleEnum.condition.none)
        {
            var condition = HpBer_Player.transform.Find("condition");
            condition.gameObject.SetActive(true);
            Color conditionColor = new Color();
            string conditionName = "";
            switch (BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].condition)
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

        HpBer_Player.transform.Find("Text_PokeName").GetComponent<Text>().text = poke.userP_NickName;
        HpBer_Player.transform.Find("Text_HPValue").GetComponent<Text>().text = $"{BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].hp}/{poke.userP_Real_Hp}";

        var genderText = HpBer_Player.Find("Text_Gender").GetComponent<Text>();
        genderText.gameObject.SetActive(true);
        if (poke.userP_gender == 0)
        {
            genderText.text = "♂"; genderText.color = battleController.Color_Gender[0];
        }
        else if (poke.userP_gender == 1)
        {
            genderText.text = "♀"; genderText.color = battleController.Color_Gender[1];
        }
        else if (poke.userP_gender == 2)
        {
            genderText.gameObject.SetActive(false);
        }
        HpBer_Player.Find("Text_Level").GetComponent<Text>().text = poke.userP_Level.ToString();

        var voice_Player = battleController.p_VoiceDatas.sheet.Find(x => x.p_Id == poke.userP_Id).voiceData;
        battleController.player_PokeName = poke.userP_NickName;

        player_Speed = poke.userP_Real_S;
        if (BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].b_item == "こだわりスカーフ")
        {
            player_Speed = Mathf.FloorToInt(player_Speed * 1.5f);
        }
        player_Chara = poke.userP_Characteristic;

        #endregion
        if(BattleDatas.enemy_BattleStatus.participationTurn == 0)
        {
            #region エネミーポケモンの表示・UI
            var enemy_Poke_Image = battleController.Pokemon2.transform.Find("Image_Poke_Player2").GetComponent<Image>();
            poke = BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum];
            pokeData = battleController.p_ImageDatas.sheet.Where(x => x.p_Id == poke.userP_Id);
            foreach (var p in pokeData)
            {
                if (p.genderType == ImageData_Pokémon.GenderType.same)
                {
                    genderType = ImageData_Pokémon.GenderType.same;
                    break;
                }
                else
                {
                    if (poke.userP_gender == 0)
                    {
                        genderType = ImageData_Pokémon.GenderType.maleOnly;
                        break;
                    }
                    else if (poke.userP_gender == 1)
                    {
                        genderType = ImageData_Pokémon.GenderType.femeleOnly;
                        break;
                    }
                }
            }
            image = pokeData.Where(x => x.genderType == genderType);
            foreach (var pp in image)
            {
                if (poke.isDifferentColors)
                {
                    enemy_Poke_Image.sprite = pp.p_ImageFront_C;
                }
                else
                {
                    enemy_Poke_Image.sprite = pp.p_ImageFront;
                }
                break;
            }

            HpBer_Player = battleController.Pokemon2.transform.Find("HPBer_Player2");
            slider_Player = HpBer_Player.transform.Find("Slider").GetComponent<Slider>();
            slider_Player.minValue = Mathf.Floor(poke.userP_Real_Hp / 36.4f * -1);
            slider_Player.maxValue = poke.userP_Real_Hp;
            slider_Player.value = BattleDatas.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp;
            if (slider_Player.value <= slider_Player.maxValue / 5)
            {
                slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (slider_Player.value <= slider_Player.maxValue / 2)
            {
                slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                slider_Player.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }

            HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
            if (BattleDatas.enemy_OthersStatus[battleController.player_PokeTeamNum].condition != BattleEnum.condition.none)
            {
                var condition = HpBer_Player.transform.Find("condition");
                condition.gameObject.SetActive(true);
                Color conditionColor = new Color();
                string conditionName = "";
                switch (BattleDatas.enemy_OthersStatus[battleController.player_PokeTeamNum].condition)
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

            HpBer_Player.transform.Find("Text_PokeName").GetComponent<Text>().text = poke.userP_Name;

            genderText = HpBer_Player.Find("Text_Gender").GetComponent<Text>();
            genderText.gameObject.SetActive(true);
            if (poke.userP_gender == 0)
            {
                genderText.text = "♂"; genderText.color = battleController.Color_Gender[0];
            }
            else if (poke.userP_gender == 1)
            {
                genderText.text = "♀"; genderText.color = battleController.Color_Gender[1];
            }
            else if (poke.userP_gender == 2)
            {
                genderText.gameObject.SetActive(false);
            }
            HpBer_Player.Find("Text_Level").GetComponent<Text>().text = poke.userP_Level.ToString();

            battleController.enemy_PokeName = poke.userP_NickName;

            enemy_Speed = poke.userP_Real_S;
            if (BattleDatas.enemy_OthersStatus[battleController.enemy_PokeTeamNum].b_item == "こだわりスカーフ")
            {
                enemy_Speed = Mathf.FloorToInt(enemy_Speed * 1.5f);
            }

            enemy_Chara = poke.userP_Characteristic;

            #endregion
        }

        var voice_Enemy = battleController.p_VoiceDatas.sheet.Find(x => x.p_Id == poke.userP_Id).voiceData;

        #region 手持ちポケモン表示（相手のポケモン）
        if(battleController.enemy_PokeTeamNum == 1)
        {
            var image_EnemyPoke = battleController.On_hand_List.transform.Find($"P2_poke2").GetComponent<Image>();
            var data = battleController.p_ImageDatas.sheet.Find(x => x.p_Id == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Id);
            if (BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].isDifferentColors)
            {
                image_EnemyPoke.sprite = data.p_ImageHand_C;
            }
            else
            {
                image_EnemyPoke.sprite = data.p_ImageHand;
            }
        }
        else if (battleController.enemy_PokeTeamNum == 2)
        {
            var image_EnemyPoke = battleController.On_hand_List.transform.Find($"P2_poke3").GetComponent<Image>();
            var data = battleController.p_ImageDatas.sheet.Find(x => x.p_Id == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].userP_Id);
            if (BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum].isDifferentColors)
            {
                image_EnemyPoke.sprite = data.p_ImageHand_C;
            }
            else
            {
                image_EnemyPoke.sprite = data.p_ImageHand;
            }
        }
        #endregion

        StartCoroutine(inPokemon());

        IEnumerator inPokemon()
        {
            if(BattleDatas.enemy_BattleStatus.participationTurn == 0)
            {
                battleController.enemy_Anim.SetBool("p_In", true);
                battleController.Audio_SE.PlayOneShot(turnScript.inOut_Audio[0]);
                yield return new WaitForSeconds(0.25f);
                battleController.textDisplay.text = $"{battleController.enemy_PokeName}があらわれた！";
                yield return new WaitForSeconds(0.75f);
                battleController.Audio_Voice.PlayOneShot(voice_Enemy);
                yield return new WaitForSeconds(3);
                battleController.player_Anim.SetBool("p_In", true);
                battleController.Audio_SE.PlayOneShot(turnScript.inOut_Audio[0]);
                battleController.textDisplay.text = $"行け！{battleController.player_PokeName}！";
                yield return new WaitForSeconds(1);
                battleController.Audio_Voice.PlayOneShot(voice_Player);
                yield return new WaitForSeconds(2);

                var p1_Image_Characteristic = battleController.Pokemon1.transform.Find("Image_Characteristic");
                var p2_Image_Characteristic = battleController.Pokemon2.transform.Find("Image_Characteristic");

                var player1 = BattleDatas.user_PokemonData[battleController.player_PokeTeamNum];
                var player2 = BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum];

                var anim1 = battleController.player_Anim;
                var anim2 = battleController.enemy_Anim;

                if (player_Speed < enemy_Speed)
                {
                    (player1, player2) = (player2, player1);
                    (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                    (anim1, anim2) = (anim2, anim1);
                }
                else if (player_Speed == enemy_Speed)
                {
                    int r = Random.Range(0, 100);
                    if (r >= 50)
                    {
                        (player1, player2) = (player2, player1);
                        (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                        (anim1, anim2) = (anim2, anim1);
                    }
                }

                for (var c = 0; c < 2; c++)
                {
                    if (c == 1)
                    {
                        (player1, player2) = (player2, player1);
                        (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                        (anim1, anim2) = (anim2, anim1);
                    }

                    if (player1.userP_Characteristic == "きけんよち")
                    {
                        bool isFear = false;
                        for (var i = 0; i < 4; i++)
                        {
                            string technique = "";
                            switch (i)
                            {
                                case 0:
                                    technique = player2.set_Technique1;
                                    break;
                                case 1:
                                    technique = player2.set_Technique2;
                                    break;
                                case 2:
                                    technique = player2.set_Technique3;
                                    break;
                                case 3:
                                    technique = player2.set_Technique4;
                                    break;
                            }

                            var techData = DataLists.titleData_Technique.Find(x => x.t_Name == technique);

                            var type = battleController.c_Data.sheet.Find(x => x.typeName == techData.t_Type);
                            if (!type.invalid.Contains(player1.userP_Type1) && !type.invalid.Contains(player1.userP_Type2))
                            {
                                if (type.twice.Contains(player1.userP_Type1) || type.twice.Contains(player1.userP_Type2))
                                {
                                    isFear = true;
                                }
                            }
                        }

                        p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_NickName + "の";
                        p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                        if (isFear)
                        {
                            anim1.SetBool("c_In", true);
                            yield return new WaitForSeconds(1.5f);
                            anim1.SetBool("c_In", false);
                            battleController.textDisplay.text = $"{player1.userP_NickName}は みぶるいした！";
                            yield return new WaitForSeconds(2);
                        }
                        else
                        {
                            yield return new WaitForSeconds(1.5f);
                        }
                    }
                    else if (player1.userP_Characteristic == "テラボルテージ")
                    {
                        p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_NickName + "の";
                        p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                        anim1.SetBool("c_In", true);
                        yield return new WaitForSeconds(2f);
                        anim1.SetBool("c_In", false);
                    }

                    if (player1 == BattleDatas.user_PokemonData[battleController.player_PokeTeamNum])
                    {
                        if (BattleDatas.user_Fields.stealthRock == true)
                        {
                            var type = battleController.c_Data.sheet.Find(x => x.type == Pokémon_Type.Type.Rock);
                            int damage = player1.userP_Real_Hp / 8;
                            if (type.twice.Contains(player1.userP_Type1))
                            {
                                damage = damage * 2;
                            }
                            else if (type.half.Contains(player1.userP_Type1))
                            {
                                damage = damage / 2;
                            }
                            if (type.twice.Contains(player1.userP_Type2))
                            {
                                damage = damage * 2;
                            }
                            else if (type.half.Contains(player1.userP_Type2))
                            {
                                damage = damage / 2;
                            }
                            if (damage <= 0) { damage = 1; }
                            BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].hp -= Mathf.FloorToInt(damage);
                        }
                    }
                    else if (player1 == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum])
                    {
                        if (BattleDatas.enemy_Fields.stealthRock == true)
                        {
                            var type = battleController.c_Data.sheet.Find(x => x.type == Pokémon_Type.Type.Rock);
                            int damage = player1.userP_Real_Hp / 8;
                            if (type.twice.Contains(player1.userP_Type1))
                            {
                                damage = damage * 2;
                            }
                            else if (type.half.Contains(player1.userP_Type1))
                            {
                                damage = damage / 2;
                            }
                            if (type.twice.Contains(player1.userP_Type2))
                            {
                                damage = damage * 2;
                            }
                            else if (type.half.Contains(player1.userP_Type2))
                            {
                                damage = damage / 2;
                            }
                            if (damage <= 0) { damage = 1; }
                            BattleDatas.enemy_OthersStatus[battleController.enemy_PokeTeamNum].hp -= Mathf.FloorToInt(damage);
                        }
                    }
                }
            }
            else
            {
                battleController.player_Anim.SetBool("p_In", true);
                battleController.Audio_SE.PlayOneShot(turnScript.inOut_Audio[0]);
                battleController.textDisplay.text = $"行け！{battleController.player_PokeName}！";
                yield return new WaitForSeconds(1);
                battleController.Audio_Voice.PlayOneShot(voice_Player);
                yield return new WaitForSeconds(2);

                var p1_Image_Characteristic = battleController.Pokemon1.transform.Find("Image_Characteristic");
                var p2_Image_Characteristic = battleController.Pokemon2.transform.Find("Image_Characteristic");

                var player1 = BattleDatas.user_PokemonData[battleController.player_PokeTeamNum];
                var player2 = BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum];

                var anim1 = battleController.player_Anim;

                if (player1.userP_Characteristic == "きけんよち")
                {
                    bool isFear = false;
                    for (var i = 0; i < 4; i++)
                    {
                        string technique = "";
                        switch (i)
                        {
                            case 0:
                                technique = player2.set_Technique1;
                                break;
                            case 1:
                                technique = player2.set_Technique2;
                                break;
                            case 2:
                                technique = player2.set_Technique3;
                                break;
                            case 3:
                                technique = player2.set_Technique4;
                                break;
                        }

                        var techData = DataLists.titleData_Technique.Find(x => x.t_Name == technique);

                        var type = battleController.c_Data.sheet.Find(x => x.typeName == techData.t_Type);
                        if (!type.invalid.Contains(player1.userP_Type1) && !type.invalid.Contains(player1.userP_Type2))
                        {
                            if (type.twice.Contains(player1.userP_Type1) || type.twice.Contains(player1.userP_Type2))
                            {
                                isFear = true;
                            }
                        }
                    }

                    p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_NickName + "の";
                    p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                    if (isFear)
                    {
                        anim1.SetBool("c_In", true);
                        yield return new WaitForSeconds(1.5f);
                        anim1.SetBool("c_In", false);
                        battleController.textDisplay.text = $"{player1.userP_NickName}は みぶるいした！";
                        yield return new WaitForSeconds(2);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                else if (player1.userP_Characteristic == "テラボルテージ")
                {
                    p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_NickName + "の";
                    p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                    anim1.SetBool("c_In", true);
                    yield return new WaitForSeconds(2f);
                    anim1.SetBool("c_In", false);
                }

                if(BattleDatas.user_Fields.stealthRock == true)
                {
                    var type = battleController.c_Data.sheet.Find(x => x.type == Pokémon_Type.Type.Rock);
                    int damage = player1.userP_Real_Hp / 8;
                    if (type.twice.Contains(player1.userP_Type1))
                    {
                        damage = damage * 2;
                    }
                    else if (type.half.Contains(player1.userP_Type1))
                    {
                        damage = damage / 2;
                    }
                    if (type.twice.Contains(player1.userP_Type2))
                    {
                        damage = damage * 2;
                    }
                    else if (type.half.Contains(player1.userP_Type2))
                    {
                        damage = damage / 2;
                    }
                    if(damage <= 0) { damage = 1; }
                    BattleDatas.user_OthersStatus[battleController.player_PokeTeamNum].hp -= Mathf.FloorToInt(damage);
                    battleController.textDisplay.text = $"{player1.userP_NickName}に\nとがった岩が　食いこんだ！";
                    yield return new WaitForSeconds(1.2f);
                    battleController.textDisplay.text = "";
                }
            }

            if(isAttack==false)
            {
                battleController.textDisplay.text = $"行動を選択してください";
                battleController.Control_ModeSelect.SetActive(true);
                battleController.turnScript.SetBattleZone();
            }
            else
            {
                turnScript.enemyAttack();
            }
        }
    }
}
