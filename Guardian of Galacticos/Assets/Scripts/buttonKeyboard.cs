using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonKeyboard : MonoBehaviour
{
    public KeyCode _key;
    public Button _button;
    public AudioClip buttonSound;


    void Update()
    {
        if(Input.GetKeyDown(_key))
        {
            GetComponent<AudioSource>().PlayOneShot(buttonSound);
            FadeToColor(_button.colors.pressedColor);
            _button.onClick.Invoke();
            _button.interactable = false;
        }
        else if (Input.GetKeyUp(_key))
        {
            FadeToColor(_button.colors.normalColor);
        }
     
    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, _button.colors.fadeDuration, true, true);
    }

}

