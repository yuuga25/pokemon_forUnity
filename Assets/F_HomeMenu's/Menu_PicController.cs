using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class Menu_PicController : MonoBehaviour
{
    public GameObject Scrollber;
    public GameObject Detail;

    public GameObject pokeUnit_PB;
    public GameObject parentObj;

    public Menu_PokéController pokéController;

    public AudioClip decision;

    public AudioSource Audio_Voice;
    public VoiceData_Pokémon voiceData;

    public ImageData_Pokémon imageData;

    public void SetPic()
    {
        Scrollber.SetActive(true);
        Detail.SetActive(false);

        foreach(Transform u in parentObj.transform)
        {
            Destroy(u.gameObject);
        }

        var data = DataLists.titleData_Pokémon.OrderBy(x => x.p_Id);
        foreach(var d in data)
        {
            GameObject unit = Instantiate(pokeUnit_PB, parentObj.transform);

            unit.transform.Find("Text_Name").gameObject.GetComponent<Text>().text = d.p_Name;

            int colorNum = 0;
            #region 背景色・タイプカラー
            switch (d.p_Type1)//タイプ1
            {
                case "ノーマル":
                    colorNum = 0;
                    break;
                case "ほのお":
                    colorNum = 1;
                    break;
                case "みず":
                    colorNum = 2;
                    break;
                case "でんき":
                    colorNum = 4;
                    break;
                case "くさ":
                    colorNum = 3;
                    break;
                case "こおり":
                    colorNum = 5;
                    break;
                case "かくとう":
                    colorNum = 6;
                    break;
                case "どく":
                    colorNum = 7;
                    break;
                case "じめん":
                    colorNum = 8;
                    break;
                case "ひこう":
                    colorNum = 9;
                    break;
                case "エスパー":
                    colorNum = 10;
                    break;
                case "むし":
                    colorNum = 11;
                    break;
                case "いわ":
                    colorNum = 12;
                    break;
                case "ゴースト":
                    colorNum = 13;
                    break;
                case "ドラゴン":
                    colorNum = 14;
                    break;
                case "あく":
                    colorNum = 15;
                    break;
                case "はがね":
                    colorNum = 16;
                    break;
                case "フェアリー":
                    colorNum = 17;
                    break;
                case "":
                    colorNum = 18;
                    break;
                case null:
                    Debug.LogError("タイプが設定されていません");
                    break;
                default:
                    break;
            }
            unit.transform.Find("Image_FirstColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
            switch (d.p_Type2)//タイプ2
            {
                case "ノーマル":
                    colorNum = 0;
                    break;
                case "ほのお":
                    colorNum = 1;
                    break;
                case "みず":
                    colorNum = 2;
                    break;
                case "でんき":
                    colorNum = 4;
                    break;
                case "くさ":
                    colorNum = 3;
                    break;
                case "こおり":
                    colorNum = 5;
                    break;
                case "かくとう":
                    colorNum = 6;
                    break;
                case "どく":
                    colorNum = 7;
                    break;
                case "じめん":
                    colorNum = 8;
                    break;
                case "ひこう":
                    colorNum = 9;
                    break;
                case "エスパー":
                    colorNum = 10;
                    break;
                case "むし":
                    colorNum = 11;
                    break;
                case "いわ":
                    colorNum = 12;
                    break;
                case "ゴースト":
                    colorNum = 13;
                    break;
                case "ドラゴン":
                    colorNum = 14;
                    break;
                case "あく":
                    colorNum = 15;
                    break;
                case "はがね":
                    colorNum = 16;
                    break;
                case "フェアリー":
                    colorNum = 17;
                    break;
                case "":
                case null:
                    colorNum = 18;
                    break;
                default:
                    break;
            }
            unit.transform.Find("Image_SecondColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
            #endregion

            //手持ちポケモン画像
            var imageData_Hand = imageData.sheet.Find(x => x.p_Id == d.p_Id);
            unit.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = imageData_Hand.p_ImageHand;

            var p = DataLists.playerData.pokémonsList.Find(x => x.userP_Id == d.p_Id);
            if(p != null)
            {
                DataLists.playerData.picList.Add(d.p_Id);
            }

            if(DataLists.playerData.picList.Any(x => x == d.p_Id) == true)
            {
                unit.transform.Find("Image_Filter").gameObject.SetActive(false);
            }

            var id = d.p_Id;
            unit.GetComponent<Button>().onClick.AddListener(() => SetDetail(id));
            unit.GetComponent<Button>().onClick.AddListener(() => pokéController.audioManager_SE.PlayOneShot(decision));
        }
        DataLists.playerData.picList.Distinct();
        SetUserData();
    }

    private void SetUserData()
    {
        pokéController.loading_Image.SetActive(true);

        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData)}
                }
            }, result =>
            {
                pokéController.loading_Image.SetActive(false);
            }
            , error => { Debug.Log(error.GenerateErrorReport()); });
    }

    public void SetDetail(int id)
    {
        Scrollber.SetActive(false);
        Detail.SetActive(true);

        var detailThis = Detail.transform.Find("Image");
        var button_Right = Detail.transform.Find("Button_Right");
        var button_Left = Detail.transform.Find("Button_Left");

        var thisPoke = DataLists.titleData_Pokémon.Find(x => x.p_Id == id);
        var thisPoke_index = DataLists.titleData_Pokémon.IndexOf(thisPoke);

        detailThis.transform.Find("Text_Name").GetComponent<Text>().text = thisPoke.p_Name;
        detailThis.transform.Find("Text_Number").GetComponent<Text>().text = "No."+thisPoke.p_Id.ToString("000");
        detailThis.transform.Find("Text_Type").GetComponent<Text>().text = thisPoke.p_Type1;
        if (thisPoke.p_Type2 != "")
        {
            detailThis.transform.Find("Text_Type").GetComponent<Text>().text += "・" + thisPoke.p_Type2;
        }

        detailThis.transform.Find("Text_Value").GetComponent<Text>().text = $"{thisPoke.p_Height}m\n{thisPoke.p_Weight}kg";
        detailThis.transform.Find("Text_Classification").GetComponent<Text>().text = $"{thisPoke.p_Classification}ポケモン";
        detailThis.transform.Find("Text_Explanation").GetComponent<Text>().text = $"{thisPoke.p_Explanation}";

        var image = pokéController.imageData.sheet.Find(x => x.p_Id == thisPoke.p_Id);
        detailThis.transform.Find("Image_poke").GetComponent<Image>().sprite = image.p_ImageFront;
        detailThis.transform.Find("Image_Hand").Find("Image_poke_hand").GetComponent<Image>().sprite = image.p_ImageHand;

        var first = DataLists.titleData_Pokémon.First();
        var last = DataLists.titleData_Pokémon.Last();

        var afterPoke = DataLists.titleData_Pokémon[thisPoke_index];
        if (thisPoke.p_Id == first.p_Id)
        {
            afterPoke = last;
        }
        else
        {
            afterPoke = DataLists.titleData_Pokémon[thisPoke_index - 1];
        }
        var afterPoke_Image = pokéController.imageData.sheet.Find(x => x.p_Id == afterPoke.p_Id);
        button_Left.transform.Find("Text").GetComponent<Text>().text = "〈　" + afterPoke.p_Name;
        button_Left.transform.Find("Image").GetComponent<Image>().sprite = afterPoke_Image.p_ImageHand;

        var nextPoke = DataLists.titleData_Pokémon[thisPoke_index];
        if(thisPoke.p_Id == last.p_Id)
        {
            nextPoke = first;
        }
        else
        {
            nextPoke = DataLists.titleData_Pokémon[thisPoke_index + 1];
        }
        var nextPoke_Image = pokéController.imageData.sheet.Find(x => x.p_Id == nextPoke.p_Id);
        button_Right.transform.Find("Text").GetComponent<Text>().text = nextPoke.p_Name + "　〉";
        button_Right.transform.Find("Image").GetComponent<Image>().sprite = nextPoke_Image.p_ImageHand;

        afterId = afterPoke.p_Id;
        nextId = nextPoke.p_Id;

        var voice = voiceData.sheet.Find(x => x.p_Id == thisPoke.p_Id);
        Audio_Voice.PlayOneShot(voice.voiceData);
    }

    private int afterId;
    private int nextId;

    public void changeDetail(bool isNext)
    {
        if (isNext) SetDetail(nextId);
        else SetDetail(afterId);
    }
}
