using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private WindowType type;

    public WindowType Type => type;

    public void ActivateWindow()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateWindow() 
    {
        gameObject.SetActive(false);
    }
}
