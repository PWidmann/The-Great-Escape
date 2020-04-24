using UnityEngine;
using UnityEngine.UI;

public class ButtonElement : MonoBehaviour
{
    Button thisButton;
    Text thisButtonText;
    bool isSelectedButton;

    public bool IsSelectedButton { get => isSelectedButton; set => isSelectedButton = value; }

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButtonText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (isSelectedButton)
            ButtonColorTurnSelected();
        else
            ButtonColorTurnUnselected();
    }

    void ButtonColorTurnSelected()
    {
        ColorBlock colors = thisButton.colors;
        colors.normalColor = new Color32(114, 114, 114, 255);

        thisButton.colors = colors;

        thisButtonText.color = Color.white;
    }

    public void ButtonColorTurnUnselected()
    {
        ColorBlock colors = thisButton.colors;
        colors.normalColor = Color.white;

        thisButton.colors = colors;

        thisButtonText.color = Color.black;
    }
}
