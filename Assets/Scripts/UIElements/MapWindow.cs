using UnityEngine;

public class MapWindow : Window
{
    public override void ActivateWindow()
    {
        transform.localPosition = new Vector3(0, 0, 1);
    }

    public override void DeactivateWindow()
    {
        transform.localPosition = new Vector3(0, 1000, 1);
    }
}
