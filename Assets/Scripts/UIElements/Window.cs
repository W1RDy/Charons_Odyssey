using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private WindowType type;

    public WindowType Type => type;

    public virtual void ActivateWindow()
    {
        gameObject.SetActive(true);
    }

    public virtual void DeactivateWindow() 
    {
        gameObject.SetActive(false);
    }
}
