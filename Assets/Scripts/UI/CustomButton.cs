using System;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private Event customEvent; 
    
    public void OnButtonClicked()
    {
        ButtonClickEventArgs e = new ButtonClickEventArgs();
        var onButtonClick = ButtonClick;
        onButtonClick?.Invoke(this, e);
    }

    public event EventHandler<ButtonClickEventArgs> ButtonClick;
}

public class ButtonClickEventArgs : EventArgs { }