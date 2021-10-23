using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ButtonExtention))]
public class ButtonAction : MonoBehaviour
{
    public Menu_PokéController menu_PokéController;

    public int Technique_ID;
    private void Start()
    {
        var button = GetComponent<ButtonExtention>();
        button.onClick.AddListener(() => Debug.Log("Click!!"));
        //button.onLongPress.AddListener(() => menu_PokéController.TechniqueChange_Set(Technique_ID));
    }
}