using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class ReorganizationScript : MonoBehaviour
{
    public Menu_PokéController pokéController;
    public ImageData_Pokémon p_ImageDatas;

    [Header("決定音")]
    public AudioClip determination;

    [Header("入れ替えるポケモン")]
    public GameObject Status_Left;

    [Header("選択したポケモン")]
    public GameObject Status_Right;

    [Header("選択するポケモンのリスト")]
    public GameObject content_List;
    public GameObject pokeUnit;

    private int selectPoke_SetNum; //入れ替え先のポケモンのチームナンバー
    private string changePoke_UniqueId; //入れ替えるポケモンのユニークID
    
    public void Change_SetStatus(string uniqueId)
    {
        var selectPoke = DataLists.playerData.pokémonsList.Find(x => x.unique_Id == uniqueId);

        var icon = Status_Right.transform.Find("Pokémon_Unit").gameObject;
        #region タイプカラー設定
        int colorNum = 0;
        switch (selectPoke.userP_Type1)
        {
            case Pokémon_Type.Type.None:
                colorNum = 18;
                break;
            case Pokémon_Type.Type.Normal:
                colorNum = 0;
                break;
            case Pokémon_Type.Type.Fire:
                colorNum = 1;
                break;
            case Pokémon_Type.Type.Water:
                colorNum = 2;
                break;
            case Pokémon_Type.Type.Electric:
                colorNum = 4;
                break;
            case Pokémon_Type.Type.Grass:
                colorNum = 3;
                break;
            case Pokémon_Type.Type.Ice:
                colorNum = 5;
                break;
            case Pokémon_Type.Type.Fighting:
                colorNum = 6;
                break;
            case Pokémon_Type.Type.Poison:
                colorNum = 7;
                break;
            case Pokémon_Type.Type.Ground:
                colorNum = 8;
                break;
            case Pokémon_Type.Type.Flying:
                colorNum = 9;
                break;
            case Pokémon_Type.Type.Psychic:
                colorNum = 10;
                break;
            case Pokémon_Type.Type.Bug:
                colorNum = 11;
                break;
            case Pokémon_Type.Type.Rock:
                colorNum = 12;
                break;
            case Pokémon_Type.Type.Ghost:
                colorNum = 13;
                break;
            case Pokémon_Type.Type.Dragon:
                colorNum = 14;
                break;
            case Pokémon_Type.Type.Dark:
                colorNum = 15;
                break;
            case Pokémon_Type.Type.Steel:
                colorNum = 16;
                break;
            case Pokémon_Type.Type.Fairy:
                colorNum = 17;
                break;
        }
        icon.transform.Find("Image_FirstColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
        switch (selectPoke.userP_Type2)
        {
            case Pokémon_Type.Type.None:
                colorNum = 18;
                break;
            case Pokémon_Type.Type.Normal:
                colorNum = 0;
                break;
            case Pokémon_Type.Type.Fire:
                colorNum = 1;
                break;
            case Pokémon_Type.Type.Water:
                colorNum = 2;
                break;
            case Pokémon_Type.Type.Electric:
                colorNum = 4;
                break;
            case Pokémon_Type.Type.Grass:
                colorNum = 3;
                break;
            case Pokémon_Type.Type.Ice:
                colorNum = 5;
                break;
            case Pokémon_Type.Type.Fighting:
                colorNum = 6;
                break;
            case Pokémon_Type.Type.Poison:
                colorNum = 7;
                break;
            case Pokémon_Type.Type.Ground:
                colorNum = 8;
                break;
            case Pokémon_Type.Type.Flying:
                colorNum = 9;
                break;
            case Pokémon_Type.Type.Psychic:
                colorNum = 10;
                break;
            case Pokémon_Type.Type.Bug:
                colorNum = 11;
                break;
            case Pokémon_Type.Type.Rock:
                colorNum = 12;
                break;
            case Pokémon_Type.Type.Ghost:
                colorNum = 13;
                break;
            case Pokémon_Type.Type.Dragon:
                colorNum = 14;
                break;
            case Pokémon_Type.Type.Dark:
                colorNum = 15;
                break;
            case Pokémon_Type.Type.Steel:
                colorNum = 16;
                break;
            case Pokémon_Type.Type.Fairy:
                colorNum = 17;
                break;
        }
        icon.transform.Find("Image_SecondColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
        #endregion 
        var image = p_ImageDatas.sheet.Find(x => x.p_Id == selectPoke.userP_Id);
        if (selectPoke.isDifferentColors)
        {
            icon.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = image.p_ImageHand_C;
        }
        else
        {
            icon.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = image.p_ImageHand;
        }

        //フレーム
        Color color1 = new Color();
        Color color2 = new Color();
        switch (selectPoke.userP_ELevel)
        {
            case 1:
                color1 = pokéController.Color1_lv1;
                color2 = pokéController.Color2_lv1;
                break;
            case 2:
                color1 = pokéController.Color1_lv2;
                color2 = pokéController.Color2_lv2;
                break;
            case 3:
                color1 = pokéController.Color1_lv3;
                color2 = pokéController.Color2_lv3;
                break;
            case 4:
                color1 = pokéController.Color1_lv4;
                color2 = pokéController.Color2_lv4;
                break;
            case 5:
                color1 = pokéController.Color1_lv5;
                color2 = pokéController.Color2_lv5;
                break;
        }
        var Frame = icon.transform.Find("Frame").gameObject.GetComponent<UICornersGradient>();
        Frame.m_topLeftColor = color1;
        Frame.m_topRightColor = color2;
        Frame.m_bottomRightColor = color1;
        Frame.m_bottomLeftColor = color2;

        //ボール
        int ballId = 0;
        switch (selectPoke.userP_Ball)
        {
            case Pokémon_Ball.Ball.Poké:
                ballId = 0;
                break;
            case Pokémon_Ball.Ball.Great:
                ballId = 1;
                break;
            case Pokémon_Ball.Ball.Ultra:
                ballId = 2;
                break;
            case Pokémon_Ball.Ball.Master:
                ballId = 3;
                break;
        }
        icon.transform.Find("Image_Ball").gameObject.GetComponent<Image>().sprite = pokéController.ballSprite[ballId];

        icon.SetActive(true);


        Status_Right.transform.Find("Text_Name").gameObject.GetComponent<Text>().text = selectPoke.userP_Name;
        Status_Right.transform.Find("Text_Level").gameObject.GetComponent<Text>().text = selectPoke.userP_Level.ToString();
        #region タイプの名前表示
        Status_Right.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = "";
        string typeName = "";
        switch (selectPoke.userP_Type1)
        {
            case Pokémon_Type.Type.None:
                typeName = "";
                break;
            case Pokémon_Type.Type.Normal:
                typeName = "ノーマル";
                break;
            case Pokémon_Type.Type.Fire:
                typeName = "ほのお";
                break;
            case Pokémon_Type.Type.Water:
                typeName = "みず";
                break;
            case Pokémon_Type.Type.Electric:
                typeName = "でんき";
                break;
            case Pokémon_Type.Type.Grass:
                typeName = "くさ";
                break;
            case Pokémon_Type.Type.Ice:
                typeName = "こおり";
                break;
            case Pokémon_Type.Type.Fighting:
                typeName = "かくとう";
                break;
            case Pokémon_Type.Type.Poison:
                typeName = "どく";
                break;
            case Pokémon_Type.Type.Ground:
                typeName = "じめん";
                break;
            case Pokémon_Type.Type.Flying:
                typeName = "ひこう";
                break;
            case Pokémon_Type.Type.Psychic:
                typeName = "エスパー";
                break;
            case Pokémon_Type.Type.Bug:
                typeName = "むし"; 
                break;
            case Pokémon_Type.Type.Rock:
                typeName = "いわ";
                break;
            case Pokémon_Type.Type.Ghost:
                typeName = "ゴースト";
                break;
            case Pokémon_Type.Type.Dragon:
                typeName = "ドラゴン"; 
                break;
            case Pokémon_Type.Type.Dark:
                typeName = "あく";
                break;
            case Pokémon_Type.Type.Steel:
                typeName = "はがね";
                break;
            case Pokémon_Type.Type.Fairy:
                typeName = "フェアリー";
                break;
        }
        Status_Right.transform.Find("Text_Type").gameObject.GetComponent<Text>().text += typeName;
        switch (selectPoke.userP_Type2)
        {
            case Pokémon_Type.Type.None:
                typeName = "";
                break;
            case Pokémon_Type.Type.Normal:
                typeName = "・ノーマル";
                break;
            case Pokémon_Type.Type.Fire:
                typeName = "・ほのお";
                break;
            case Pokémon_Type.Type.Water:
                typeName = "・みず";
                break;
            case Pokémon_Type.Type.Electric:
                typeName = "・でんき";
                break;
            case Pokémon_Type.Type.Grass:
                typeName = "・くさ";
                break;
            case Pokémon_Type.Type.Ice:
                typeName = "・こおり";
                break;
            case Pokémon_Type.Type.Fighting:
                typeName = "・かくとう";
                break;
            case Pokémon_Type.Type.Poison:
                typeName = "・どく";
                break;
            case Pokémon_Type.Type.Ground:
                typeName = "・じめん";
                break;
            case Pokémon_Type.Type.Flying:
                typeName = "・ひこう";
                break;
            case Pokémon_Type.Type.Psychic:
                typeName = "・エスパー";
                break;
            case Pokémon_Type.Type.Bug:
                typeName = "・むし";
                break;
            case Pokémon_Type.Type.Rock:
                typeName = "・いわ";
                break;
            case Pokémon_Type.Type.Ghost:
                typeName = "・ゴースト";
                break;
            case Pokémon_Type.Type.Dragon:
                typeName = "・ドラゴン";
                break;
            case Pokémon_Type.Type.Dark:
                typeName = "・あく";
                break;
            case Pokémon_Type.Type.Steel:
                typeName = "・はがね";
                break;
            case Pokémon_Type.Type.Fairy:
                typeName = "・フェアリー";
                break;
        }
        Status_Right.transform.Find("Text_Type").gameObject.GetComponent<Text>().text += typeName;
        #endregion 
        Status_Right.transform.Find("Text_Characteristic").gameObject.GetComponent<Text>().text = "特性：" + selectPoke.userP_Characteristic;
        Status_Right.transform.Find("Text_Real_Hp").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_Hp.ToString();
        Status_Right.transform.Find("Text_Real_ATK").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_A.ToString();
        Status_Right.transform.Find("Text_Real_DEF").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_B.ToString();
        Status_Right.transform.Find("Text_Real_SAT").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_C.ToString();
        Status_Right.transform.Find("Text_Real_SDE").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_D.ToString();
        Status_Right.transform.Find("Text_Real_SPE").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_S.ToString();
        Status_Right.transform.Find("Text_waza1").gameObject.GetComponent<Text>().text = selectPoke.set_Technique1;
        Status_Right.transform.Find("Text_waza2").gameObject.GetComponent<Text>().text = selectPoke.set_Technique2;
        Status_Right.transform.Find("Text_waza3").gameObject.GetComponent<Text>().text = selectPoke.set_Technique3;
        Status_Right.transform.Find("Text_waza4").gameObject.GetComponent<Text>().text = selectPoke.set_Technique4;
        Status_Right.transform.Find("Image_Gender").gameObject.SetActive(true);
        if (selectPoke.userP_gender == 0)
        {
            Status_Right.transform.Find("Image_Gender").gameObject.GetComponent<Image>().sprite = pokéController.genderImageSprite[0];
        }
        else if (selectPoke.userP_gender == 1)
        {
            Status_Right.transform.Find("Image_Gender").gameObject.GetComponent<Image>().sprite = pokéController.genderImageSprite[1];
        }
        else if (selectPoke.userP_gender == 2)
        {
            Status_Right.transform.Find("Image_Gender").gameObject.SetActive(false);
        }

        changePoke_UniqueId = selectPoke.unique_Id;
    }
    public void Reset_ChangeStatus()
    {
        Status_Right.transform.Find("Pokémon_Unit").gameObject.SetActive(false); Status_Right.transform.Find("Text_Name").gameObject.GetComponent<Text>().text = "ーーーー";
        Status_Right.transform.Find("Text_Level").gameObject.GetComponent<Text>().text = "ーー";
        Status_Right.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = "ーーー・ーーー";
        Status_Right.transform.Find("Text_Characteristic").gameObject.GetComponent<Text>().text = "特性：ーーーー";
        Status_Right.transform.Find("Text_Real_Hp").gameObject.GetComponent<Text>().text = "ーーー";
        Status_Right.transform.Find("Text_Real_ATK").gameObject.GetComponent<Text>().text = "ーーー";
        Status_Right.transform.Find("Text_Real_DEF").gameObject.GetComponent<Text>().text = "ーーー";
        Status_Right.transform.Find("Text_Real_SAT").gameObject.GetComponent<Text>().text = "ーーー";
        Status_Right.transform.Find("Text_Real_SDE").gameObject.GetComponent<Text>().text = "ーーー";
        Status_Right.transform.Find("Text_Real_SPE").gameObject.GetComponent<Text>().text = "ーーー";
        Status_Right.transform.Find("Text_waza1").gameObject.GetComponent<Text>().text = "ーーーーーーーー";
        Status_Right.transform.Find("Text_waza1").gameObject.GetComponent<Text>().text = "ーーーーーーーー";
        Status_Right.transform.Find("Text_waza1").gameObject.GetComponent<Text>().text = "ーーーーーーーー";
        Status_Right.transform.Find("Text_waza1").gameObject.GetComponent<Text>().text = "ーーーーーーーー";
        Status_Right.transform.Find("Image_Gender").gameObject.SetActive(false);
    }
    public void Select_SetStatus(int setNum)
    {
        Reset_ChangeStatus();

        pokéController.loading_Image.SetActive(true);

        var selectPoke = DataLists.playerData.teamDatas.pokémons[setNum];
        var icon = Status_Left.transform.Find("Pokémon_Unit").gameObject;
        #region タイプカラー設定
        int colorNum = 0;
        switch (selectPoke.userP_Type1)
        {
            case Pokémon_Type.Type.None:
                colorNum = 18;
                break;
            case Pokémon_Type.Type.Normal:
                colorNum = 0;
                break;
            case Pokémon_Type.Type.Fire:
                colorNum = 1;
                break;
            case Pokémon_Type.Type.Water:
                colorNum = 2;
                break;
            case Pokémon_Type.Type.Electric:
                colorNum = 4;
                break;
            case Pokémon_Type.Type.Grass:
                colorNum = 3;
                break;
            case Pokémon_Type.Type.Ice:
                colorNum = 5;
                break;
            case Pokémon_Type.Type.Fighting:
                colorNum = 6;
                break;
            case Pokémon_Type.Type.Poison:
                colorNum = 7;
                break;
            case Pokémon_Type.Type.Ground:
                colorNum = 8;
                break;
            case Pokémon_Type.Type.Flying:
                colorNum = 9;
                break;
            case Pokémon_Type.Type.Psychic:
                colorNum = 10;
                break;
            case Pokémon_Type.Type.Bug:
                colorNum = 11;
                break;
            case Pokémon_Type.Type.Rock:
                colorNum = 12;
                break;
            case Pokémon_Type.Type.Ghost:
                colorNum = 13;
                break;
            case Pokémon_Type.Type.Dragon:
                colorNum = 14;
                break;
            case Pokémon_Type.Type.Dark:
                colorNum = 15;
                break;
            case Pokémon_Type.Type.Steel:
                colorNum = 16;
                break;
            case Pokémon_Type.Type.Fairy:
                colorNum = 17;
                break;
        }
        icon.transform.Find("Image_FirstColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
        switch (selectPoke.userP_Type2)
        {
            case Pokémon_Type.Type.None:
                colorNum = 18;
                break;
            case Pokémon_Type.Type.Normal:
                colorNum = 0;
                break;
            case Pokémon_Type.Type.Fire:
                colorNum = 1;
                break;
            case Pokémon_Type.Type.Water:
                colorNum = 2;
                break;
            case Pokémon_Type.Type.Electric:
                colorNum = 4;
                break;
            case Pokémon_Type.Type.Grass:
                colorNum = 3;
                break;
            case Pokémon_Type.Type.Ice:
                colorNum = 5;
                break;
            case Pokémon_Type.Type.Fighting:
                colorNum = 6;
                break;
            case Pokémon_Type.Type.Poison:
                colorNum = 7;
                break;
            case Pokémon_Type.Type.Ground:
                colorNum = 8;
                break;
            case Pokémon_Type.Type.Flying:
                colorNum = 9;
                break;
            case Pokémon_Type.Type.Psychic:
                colorNum = 10;
                break;
            case Pokémon_Type.Type.Bug:
                colorNum = 11;
                break;
            case Pokémon_Type.Type.Rock:
                colorNum = 12;
                break;
            case Pokémon_Type.Type.Ghost:
                colorNum = 13;
                break;
            case Pokémon_Type.Type.Dragon:
                colorNum = 14;
                break;
            case Pokémon_Type.Type.Dark:
                colorNum = 15;
                break;
            case Pokémon_Type.Type.Steel:
                colorNum = 16;
                break;
            case Pokémon_Type.Type.Fairy:
                colorNum = 17;
                break;
        }
        icon.transform.Find("Image_SecondColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
        #endregion 
        var image = p_ImageDatas.sheet.Find(x => x.p_Id == selectPoke.userP_Id);
        if (selectPoke.isDifferentColors)
        {
            icon.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = image.p_ImageHand_C;
        }
        else
        {
            icon.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = image.p_ImageHand;
        }

        //フレーム
        Color color1 = new Color();
        Color color2 = new Color();
        switch (selectPoke.userP_ELevel)
        {
            case 1:
                color1 = pokéController.Color1_lv1;
                color2 = pokéController.Color2_lv1;
                break;
            case 2:
                color1 = pokéController.Color1_lv2;
                color2 = pokéController.Color2_lv2;
                break;
            case 3:
                color1 = pokéController.Color1_lv3;
                color2 = pokéController.Color2_lv3;
                break;
            case 4:
                color1 = pokéController.Color1_lv4;
                color2 = pokéController.Color2_lv4;
                break;
            case 5:
                color1 = pokéController.Color1_lv5;
                color2 = pokéController.Color2_lv5;
                break;
        }
        var Frame = icon.transform.Find("Frame").gameObject.GetComponent<UICornersGradient>();
        Frame.m_topLeftColor = color1;
        Frame.m_topRightColor = color2;
        Frame.m_bottomRightColor = color1;
        Frame.m_bottomLeftColor = color2;

        //ボール
        int ballId = 0;
        switch (selectPoke.userP_Ball)
        {
            case Pokémon_Ball.Ball.Poké:
                ballId = 0;
                break;
            case Pokémon_Ball.Ball.Great:
                ballId = 1;
                break;
            case Pokémon_Ball.Ball.Ultra:
                ballId = 2;
                break;
            case Pokémon_Ball.Ball.Master:
                ballId = 3;
                break;
        }
        icon.transform.Find("Image_Ball").gameObject.GetComponent<Image>().sprite = pokéController.ballSprite[ballId];

        icon.SetActive(true);


        Status_Left.transform.Find("Text_Name").gameObject.GetComponent<Text>().text = selectPoke.userP_Name;
        Status_Left.transform.Find("Text_Level").gameObject.GetComponent<Text>().text = selectPoke.userP_Level.ToString();
        #region タイプの名前表示
        Status_Left.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = "";
        string typeName = "";
        switch (selectPoke.userP_Type1)
        {
            case Pokémon_Type.Type.None:
                typeName = "";
                break;
            case Pokémon_Type.Type.Normal:
                typeName = "ノーマル";
                break;
            case Pokémon_Type.Type.Fire:
                typeName = "ほのお";
                break;
            case Pokémon_Type.Type.Water:
                typeName = "みず";
                break;
            case Pokémon_Type.Type.Electric:
                typeName = "でんき";
                break;
            case Pokémon_Type.Type.Grass:
                typeName = "くさ";
                break;
            case Pokémon_Type.Type.Ice:
                typeName = "こおり";
                break;
            case Pokémon_Type.Type.Fighting:
                typeName = "かくとう";
                break;
            case Pokémon_Type.Type.Poison:
                typeName = "どく";
                break;
            case Pokémon_Type.Type.Ground:
                typeName = "じめん";
                break;
            case Pokémon_Type.Type.Flying:
                typeName = "ひこう";
                break;
            case Pokémon_Type.Type.Psychic:
                typeName = "エスパー";
                break;
            case Pokémon_Type.Type.Bug:
                typeName = "むし";
                break;
            case Pokémon_Type.Type.Rock:
                typeName = "いわ";
                break;
            case Pokémon_Type.Type.Ghost:
                typeName = "ゴースト";
                break;
            case Pokémon_Type.Type.Dragon:
                typeName = "ドラゴン";
                break;
            case Pokémon_Type.Type.Dark:
                typeName = "あく";
                break;
            case Pokémon_Type.Type.Steel:
                typeName = "はがね";
                break;
            case Pokémon_Type.Type.Fairy:
                typeName = "フェアリー";
                break;
        }
        Status_Left.transform.Find("Text_Type").gameObject.GetComponent<Text>().text += typeName;
        switch (selectPoke.userP_Type2)
        {
            case Pokémon_Type.Type.None:
                typeName = "";
                break;
            case Pokémon_Type.Type.Normal:
                typeName = "・ノーマル";
                break;
            case Pokémon_Type.Type.Fire:
                typeName = "・ほのお";
                break;
            case Pokémon_Type.Type.Water:
                typeName = "・みず";
                break;
            case Pokémon_Type.Type.Electric:
                typeName = "・でんき";
                break;
            case Pokémon_Type.Type.Grass:
                typeName = "・くさ";
                break;
            case Pokémon_Type.Type.Ice:
                typeName = "・こおり";
                break;
            case Pokémon_Type.Type.Fighting:
                typeName = "・かくとう";
                break;
            case Pokémon_Type.Type.Poison:
                typeName = "・どく";
                break;
            case Pokémon_Type.Type.Ground:
                typeName = "・じめん";
                break;
            case Pokémon_Type.Type.Flying:
                typeName = "・ひこう";
                break;
            case Pokémon_Type.Type.Psychic:
                typeName = "・エスパー";
                break;
            case Pokémon_Type.Type.Bug:
                typeName = "・むし";
                break;
            case Pokémon_Type.Type.Rock:
                typeName = "・いわ";
                break;
            case Pokémon_Type.Type.Ghost:
                typeName = "・ゴースト";
                break;
            case Pokémon_Type.Type.Dragon:
                typeName = "・ドラゴン";
                break;
            case Pokémon_Type.Type.Dark:
                typeName = "・あく";
                break;
            case Pokémon_Type.Type.Steel:
                typeName = "・はがね";
                break;
            case Pokémon_Type.Type.Fairy:
                typeName = "・フェアリー";
                break;
        }
        Status_Left.transform.Find("Text_Type").gameObject.GetComponent<Text>().text += typeName;
        #endregion 
        Status_Left.transform.Find("Text_Characteristic").gameObject.GetComponent<Text>().text = "特性：" + selectPoke.userP_Characteristic;
        Status_Left.transform.Find("Text_Real_Hp").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_Hp.ToString();
        Status_Left.transform.Find("Text_Real_ATK").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_A.ToString();
        Status_Left.transform.Find("Text_Real_DEF").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_B.ToString();
        Status_Left.transform.Find("Text_Real_SAT").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_C.ToString();
        Status_Left.transform.Find("Text_Real_SDE").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_D.ToString();
        Status_Left.transform.Find("Text_Real_SPE").gameObject.GetComponent<Text>().text = selectPoke.userP_Real_S.ToString();
        Status_Left.transform.Find("Text_waza1").gameObject.GetComponent<Text>().text = selectPoke.set_Technique1;
        Status_Left.transform.Find("Text_waza2").gameObject.GetComponent<Text>().text = selectPoke.set_Technique2;
        Status_Left.transform.Find("Text_waza3").gameObject.GetComponent<Text>().text = selectPoke.set_Technique3;
        Status_Left.transform.Find("Text_waza4").gameObject.GetComponent<Text>().text = selectPoke.set_Technique4;
        Status_Left.transform.Find("Image_Gender").gameObject.SetActive(true);
        if (selectPoke.userP_gender == 0)
        {
            Status_Left.transform.Find("Image_Gender").gameObject.GetComponent<Image>().sprite = pokéController.genderImageSprite[0];
        }
        else if(selectPoke.userP_gender == 1)
        {
            Status_Left.transform.Find("Image_Gender").gameObject.GetComponent<Image>().sprite = pokéController.genderImageSprite[1];
        }
        else if(selectPoke.userP_gender == 2)
        {
            Status_Left.transform.Find("Image_Gender").gameObject.SetActive(false);
        }

        selectPoke_SetNum = setNum;

        var datas = DataLists.playerData.pokémonsList.OrderBy(x => x.userP_Id);

        foreach(Transform n in content_List.transform)
        {
            Destroy(n.gameObject);
        }

        foreach(var i in datas)
        {
            GameObject unit = Instantiate(pokeUnit, content_List.transform);

            //背景色・タイプカラー
            switch (i.userP_Type1)
            {
                case Pokémon_Type.Type.None:
                    colorNum = 18;
                    break;
                case Pokémon_Type.Type.Normal:
                    colorNum = 0;
                    break;
                case Pokémon_Type.Type.Fire:
                    colorNum = 1;
                    break;
                case Pokémon_Type.Type.Water:
                    colorNum = 2;
                    break;
                case Pokémon_Type.Type.Electric:
                    colorNum = 4;
                    break;
                case Pokémon_Type.Type.Grass:
                    colorNum = 3;
                    break;
                case Pokémon_Type.Type.Ice:
                    colorNum = 5;
                    break;
                case Pokémon_Type.Type.Fighting:
                    colorNum = 6;
                    break;
                case Pokémon_Type.Type.Poison:
                    colorNum = 7;
                    break;
                case Pokémon_Type.Type.Ground:
                    colorNum = 8;
                    break;
                case Pokémon_Type.Type.Flying:
                    colorNum = 9;
                    break;
                case Pokémon_Type.Type.Psychic:
                    colorNum = 10;
                    break;
                case Pokémon_Type.Type.Bug:
                    colorNum = 11;
                    break;
                case Pokémon_Type.Type.Rock:
                    colorNum = 12;
                    break;
                case Pokémon_Type.Type.Ghost:
                    colorNum = 13;
                    break;
                case Pokémon_Type.Type.Dragon:
                    colorNum = 14;
                    break;
                case Pokémon_Type.Type.Dark:
                    colorNum = 15;
                    break;
                case Pokémon_Type.Type.Steel:
                    colorNum = 16;
                    break;
                case Pokémon_Type.Type.Fairy:
                    colorNum = 17;
                    break;
            }
            unit.transform.Find("Image_FirstColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
            switch (i.userP_Type2)
            {
                case Pokémon_Type.Type.None:
                    colorNum = 18;
                    break;
                case Pokémon_Type.Type.Normal:
                    colorNum = 0;
                    break;
                case Pokémon_Type.Type.Fire:
                    colorNum = 1;
                    break;
                case Pokémon_Type.Type.Water:
                    colorNum = 2;
                    break;
                case Pokémon_Type.Type.Electric:
                    colorNum = 4;
                    break;
                case Pokémon_Type.Type.Grass:
                    colorNum = 3;
                    break;
                case Pokémon_Type.Type.Ice:
                    colorNum = 5;
                    break;
                case Pokémon_Type.Type.Fighting:
                    colorNum = 6;
                    break;
                case Pokémon_Type.Type.Poison:
                    colorNum = 7;
                    break;
                case Pokémon_Type.Type.Ground:
                    colorNum = 8;
                    break;
                case Pokémon_Type.Type.Flying:
                    colorNum = 9;
                    break;
                case Pokémon_Type.Type.Psychic:
                    colorNum = 10;
                    break;
                case Pokémon_Type.Type.Bug:
                    colorNum = 11;
                    break;
                case Pokémon_Type.Type.Rock:
                    colorNum = 12;
                    break;
                case Pokémon_Type.Type.Ghost:
                    colorNum = 13;
                    break;
                case Pokémon_Type.Type.Dragon:
                    colorNum = 14;
                    break;
                case Pokémon_Type.Type.Dark:
                    colorNum = 15;
                    break;
                case Pokémon_Type.Type.Steel:
                    colorNum = 16;
                    break;
                case Pokémon_Type.Type.Fairy:
                    colorNum = 17;
                    break;
            }
            unit.transform.Find("Image_SecondColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];

            //手持ちポケモン画像
            var imageData_Hand = p_ImageDatas.sheet.Find(x => x.p_Id == i.userP_Id);
            if (i.isDifferentColors)
            {
                unit.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = imageData_Hand.p_ImageHand_C;
            }
            else
            {
                unit.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = imageData_Hand.p_ImageHand;
            }

            //フレーム
            switch (i.userP_ELevel)
            {
                case 1:
                    color1 = pokéController.Color1_lv1;
                    color2 = pokéController.Color2_lv1;
                    break;
                case 2:
                    color1 = pokéController.Color1_lv2;
                    color2 = pokéController.Color2_lv2;
                    break;
                case 3:
                    color1 = pokéController.Color1_lv3;
                    color2 = pokéController.Color2_lv3;
                    break;
                case 4:
                    color1 = pokéController.Color1_lv4;
                    color2 = pokéController.Color2_lv4;
                    break;
                case 5:
                    color1 = pokéController.Color1_lv5;
                    color2 = pokéController.Color2_lv5;
                    break;
            }
            Frame = unit.transform.Find("Frame").gameObject.GetComponent<UICornersGradient>();
            Frame.m_topLeftColor = color1;
            Frame.m_topRightColor = color2;
            Frame.m_bottomRightColor = color1;
            Frame.m_bottomLeftColor = color2;

            //レベル
            unit.transform.Find("Text_Level").gameObject.GetComponent<Text>().text = $"Lv.{i.userP_Level}";

            //ボール
            switch (i.userP_Ball)
            {
                case Pokémon_Ball.Ball.Poké:
                    ballId = 0;
                    break;
                case Pokémon_Ball.Ball.Great:
                    ballId = 1;
                    break;
                case Pokémon_Ball.Ball.Ultra:
                    ballId = 2;
                    break;
                case Pokémon_Ball.Ball.Master:
                    ballId = 3;
                    break;
            }
            unit.transform.Find("Image_Ball").gameObject.GetComponent<Image>().sprite = pokéController.ballSprite[ballId];

            string tmp = i.unique_Id;
            unit.GetComponent<Button>().onClick.AddListener(() => { Change_SetStatus(tmp); });
            unit.GetComponent<Button>().onClick.AddListener(() => { pokéController.audioManager_SE.PlayOneShot(determination); });
        }

        pokéController.loading_Image.SetActive(false);
    }
    public void SetData()
    {
        pokéController.loading_Image.SetActive(true);

        var select = DataLists.playerData.teamDatas.pokémons[selectPoke_SetNum];
        bool isTeam = false;

        for (int i = 0; i < 6; i++)
        {
            if(DataLists.playerData.teamDatas.pokémons[i] == null)
            {
                break;
            }

            if(DataLists.playerData.teamDatas.pokémons[i].unique_Id == changePoke_UniqueId)
            {
                var aru = DataLists.playerData.teamDatas.pokémons[i];
                DataLists.playerData.teamDatas.pokémons[selectPoke_SetNum] = aru;
                DataLists.playerData.teamDatas.pokémons[i] = select;
                isTeam = true;
                break;
            }
        }

        if (isTeam == false)
        {
            var change = DataLists.playerData.pokémonsList.Find(x => x.unique_Id == changePoke_UniqueId);
            select = change;
            DataLists.playerData.teamDatas.pokémons[selectPoke_SetNum] = change;
        }

        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                        {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData) },
                }
            },
            result =>
            {
                pokéController.loading_Image.SetActive(false);
                pokéController.SetOrganization();
                pokéController.Button_Organization();
            },
            error => { Debug.Log(error.GenerateErrorReport()); pokéController.error_Image.SetActive(true); });
    }
}
