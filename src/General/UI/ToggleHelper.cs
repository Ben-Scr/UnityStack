using UnityEngine;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour
{
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Image img;
    public bool isOn = false;

    public void Toggle()
    {
        isOn = !isOn;

        if (img != null)
        {
            img.sprite = isOn ? onSprite : offSprite;
        }
    }
}
