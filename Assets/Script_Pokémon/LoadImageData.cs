using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LoadImageData : MonoBehaviour
{
    public ImageData_Pokémon imageData;
    public Image image;
    public int pokeNum;

    private void Start()
    {

    }

    public void SetImageFront(UserData_Pokémon pokeData, Image pokeImage_Front)
    {
        var pokeId = pokeData.userP_Id;
        var isMismatch = LoadData.ChangeImageID.Contains(pokeId);
        var setImage = imageData.sheet.Where(x => x.p_Id == pokeId);
        if (isMismatch)
        {
            if(pokeData.userP_gender == 0)//オス
            {
                setImage = imageData.sheet.Where(x => x.genderType == ImageData_Pokémon.GenderType.maleOnly);
                
            }
            else if(pokeData.userP_gender == 1)//メス
            {
                setImage = imageData.sheet.Where(x => x.genderType == ImageData_Pokémon.GenderType.femeleOnly);
            }
        }
        else if (!isMismatch)
        {
            setImage = imageData.sheet.Where(x => x.genderType == ImageData_Pokémon.GenderType.same);
        }
        
        if (pokeData.isDifferentColors)
        {
            foreach (var i in setImage) { pokeImage_Front.sprite = i.p_ImageFront; }
        }
        else if (!pokeData.isDifferentColors)
        {
            foreach (var i in setImage) { pokeImage_Front.sprite = i.p_ImageFront_C; }
        }
    }

    public void SetImageBack(UserData_Pokémon pokeData, Image pokeImage_Back)
    {
        var pokeId = pokeData.userP_Id;
        var isMismatch = LoadData.ChangeImageID.Contains(pokeId);
        var setImage = imageData.sheet.Where(x => x.p_Id == pokeId);
        if (isMismatch)
        {
            if (pokeData.userP_gender == 0)//オス
            {
                setImage = imageData.sheet.Where(x => x.genderType == ImageData_Pokémon.GenderType.maleOnly);

            }
            else if (pokeData.userP_gender == 1)//メス
            {
                setImage = imageData.sheet.Where(x => x.genderType == ImageData_Pokémon.GenderType.femeleOnly);
            }
        }
        else if (!isMismatch)
        {
            setImage = imageData.sheet.Where(x => x.genderType == ImageData_Pokémon.GenderType.same);
        }

        if (pokeData.isDifferentColors)
        {
            foreach (var i in setImage) { pokeImage_Back.sprite = i.p_ImageBack; }
        }
        else if (!pokeData.isDifferentColors)
        {
            foreach (var i in setImage) { pokeImage_Back.sprite = i.p_ImageBack_C; }
        }
    }

    public void SetImageHand(UserData_Pokémon pokeData, Image pokeImage_Hand)
    {
        var pokeId = pokeData.userP_Id;
        var setImage = imageData.sheet.Where(x => x.p_Id == pokeId);
        if (pokeData.isDifferentColors)
        {
            foreach (var i in setImage) { pokeImage_Hand.sprite = i.p_ImageHand; }
        }
        else if (!pokeData.isDifferentColors)
        {
            foreach (var i in setImage) { pokeImage_Hand.sprite = i.p_ImageHand_C; }
        }
    }
}
