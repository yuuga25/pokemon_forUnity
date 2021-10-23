using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idCreate : MonoBehaviour
{
    private const string PASSWORD_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    void Start()
    {
        var sb = new System.Text.StringBuilder(12);
        var r = new System.Random();

        for(int i = 0; i < 12; i++)
        {
            int pos = r.Next(PASSWORD_CHARS.Length);
            char c = PASSWORD_CHARS[pos];
            sb.Append(c);
        }

        print(sb);
    }
}
