using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_SettingsController : MonoBehaviour
{
    public GameObject SettingsMenu;

    [Header("オーディオ関連")]
    public GameObject VolumeSlider_Master;
    public GameObject VolumeSlider_BGM;
    public GameObject VolumeSlider_SE;
    public GameObject VolumeSlider_Voice;

    [Header("ヘルプ")]
    public GameObject helpMenu;
    public GameObject helpMenu_Unit;
    public GameObject parentObj;
    public GameObject help_Content;
    public AudioClip Decision;

    public Menu_PokéController pokéController;

    public void SetSettings()
    {
        SettingsMenu.SetActive(true);

        VolumeSlider_Master.GetComponent<Slider>().value = AudioController.Master_Volume;
        VolumeSlider_Master.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_Master.GetComponent<Slider>().value * 100).ToString();

        VolumeSlider_BGM.GetComponent<Slider>().value = AudioController.BGM_Volume;
        VolumeSlider_BGM.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_BGM.GetComponent<Slider>().value * 1000).ToString();

        VolumeSlider_SE.GetComponent<Slider>().value = AudioController.SE_Volume;
        VolumeSlider_SE.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_SE.GetComponent<Slider>().value * 1000).ToString();

        VolumeSlider_Voice.GetComponent<Slider>().value = AudioController.Voice_Volume;
        VolumeSlider_Voice.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_Voice.GetComponent<Slider>().value * 1000).ToString();

        helpMenu.SetActive(false);
        help_Content.SetActive(false);
    }
    public void ChangeVolume(string volumeType)
    {
        if(volumeType == "Master")
        {
            AudioController.Master_Volume = VolumeSlider_Master.GetComponent<Slider>().value;
            VolumeSlider_Master.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_Master.GetComponent<Slider>().value * 100).ToString();
        }
        else if(volumeType == "BGM")
        {
            AudioController.BGM_Volume = VolumeSlider_BGM.GetComponent<Slider>().value;
            VolumeSlider_BGM.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_BGM.GetComponent<Slider>().value * 1000).ToString();
        }
        else if(volumeType == "SE")
        {
            AudioController.SE_Volume = VolumeSlider_SE.GetComponent<Slider>().value;
            VolumeSlider_SE.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_SE.GetComponent<Slider>().value * 1000).ToString();
        }
        else if(volumeType == "Voice")
        {
            AudioController.Voice_Volume = VolumeSlider_Voice.GetComponent<Slider>().value;
            VolumeSlider_Voice.transform.Find("Text_Value").GetComponent<Text>().text = Mathf.FloorToInt(VolumeSlider_Voice.GetComponent<Slider>().value * 1000).ToString();
        }
    }
    public void SaveVolumeData()
    {
        PlayerPrefs.SetFloat("Volume_Master", AudioController.Master_Volume);
        PlayerPrefs.SetFloat("Volume_BGM", AudioController.BGM_Volume);
        PlayerPrefs.SetFloat("Volume_SE", AudioController.SE_Volume);
        PlayerPrefs.SetFloat("Volume_Voice", AudioController.Voice_Volume);
    }

    public void SetHelpMenu()
    {
        foreach(Transform u in parentObj.transform)
        {
            Destroy(u.gameObject);
        }

        foreach(var i in DataLists.titleData_helpDatas)
        {
            GameObject unit = Instantiate(helpMenu_Unit, parentObj.transform);

            unit.transform.Find("Text_ItemName").GetComponent<Text>().text = i.title;
            var tmp = i.contents;
            var tit = i.title;
            unit.GetComponent<Button>().onClick.AddListener(() => SetContents(tmp, tit));
            unit.GetComponent<Button>().onClick.AddListener(() => pokéController.audioManager_SE.PlayOneShot(Decision));
        }
    }

    public void SetContents(string contents, string title)
    {
        help_Content.transform.Find("Text_Contents").GetComponent<Text>().text = contents;
        help_Content.transform.Find("Text_Title").GetComponent<Text>().text = title;
        helpMenu.SetActive(false);
        help_Content.SetActive(true);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
