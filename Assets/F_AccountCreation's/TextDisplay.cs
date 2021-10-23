using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public enum TextType
    {
        text1,
        text2,
        text3
    }

    public TextType textType;
    public Text displayText;
    public GameObject nextKey;

    private string[] texts1 = { "ようこそ", "こちらではトレーナーとなる\nあなたの情報を登録します",
                               "まずは、あなたの名前を\n教えてください"};
    private string[] texts2 = { "さんですね。",
                                "次にあなたのパートナーとなる\nポケモンを選んでもらいます",
                                "よく考えてから決めてくださいね"};
    private string[] texts3 = { "これで、トレーナー情報の\n登録が完了しました",
                                "ポケモントレーナーになった\n記念として、",
                                "パートナーの他に３匹の\nポケモンをプレゼントしました！",
                                "後で、確認してみてくださいね",
                                "これ以降の遊び方に関しては\n設定→ヘルプから\n確認してください",
                                "それでは、お楽しみください！"};

    private string[] texts;

    private int textNumber;
    private int textCharNumber;
    private int displayTextSpeed;
    private bool click;
    private bool textStop;

    public GameObject nowObject;
    public GameObject nextObject;

    private void Start()
    {
        switch (textType)
        {
            case TextType.text1:
                texts = texts1;
                break;
            case TextType.text2:
                texts = texts2;
                break;
            case TextType.text3:
                texts = texts3;
                break;
        }
    }

    private void FixedUpdate()
    {
        if(this.gameObject.GetComponent<TextDisplay>().textType == TextType.text2)
        {
            texts[0] = DataLists.playerData.user_Name + "さんですね";
        }

        if(textStop == false) //テキストを表示させるif文
        {
            displayTextSpeed++;
            if(displayTextSpeed % 3 == 0) //x回に一回プログラムを実行するif文
            {
                if(textCharNumber != texts[textNumber].Length) //もしtext[textNumber]の文字列の文字が最後の文字じゃなければ
                {
                    nextKey.SetActive(false);
                    displayText.text = displayText.text + texts[textNumber][textCharNumber]; //displayTextに文字を追加していく
                    textCharNumber++;
                }
                else //もしtext[textNumber]の文字列の文字が最後の文字だったら
                {
                    nextKey.SetActive(true);
                    if(textNumber != texts.Length - 1) //もしtexts[]が最後のセリフじゃないときは
                    {
                        if(click == true) //クリックされた判定
                        {
                            displayText.text = ""; //表示させる文字列を消す
                            textCharNumber = 0; //文字列の番号を最初にする
                            textNumber++; //次のセリフにする
                        }
                    }
                    else //もしtexts[]が最後のセリフになったら
                    {
                        if (click == true) //クリックされた判定
                        {
                            displayText.text = ""; //表示させる文字列を消す
                            textCharNumber = 0; //文字列の番号を最初にする
                            textStop = true;

                            nowObject.SetActive(false);
                            nextObject.SetActive(true);
                        }
                    }
                }

                click = false;
            }
            if (Input.GetMouseButton(0))
            {
                click = true;
            }
        }
    }
}
