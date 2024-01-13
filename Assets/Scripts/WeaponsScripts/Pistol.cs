using UnityEngine;

public class Pistol : Guns // оставить пистолет MonoBehaviour, от остальных классов оружий избавиться
{
    public void DisablePatron()
    {
        PatronsCount--;
    }

    public void AddPatron()
    {
        PatronsCount++;
    }
}
