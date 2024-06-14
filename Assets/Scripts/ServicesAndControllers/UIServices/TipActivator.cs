public class TipActivator
{
    private Tip _tip;
    public TipActivator(Tip tip)
    {
        _tip = tip;
    }

    public void ActivateTip(TipType tipType)
    {
        if (_tip) _tip.ActivateTip(GetTipText(tipType));
    }

    public void DeactivateTip()
    {
        if (_tip) _tip.DeactivateTip();
    }

    public string GetTipText(TipType tipType)
    {
        switch(tipType)
        {
            case TipType.ExitToStation:
                return "Чтобы сойти на станцию нажмите <E>";
            case TipType.OpenMap:
                return "Чтобы открыть карту нажмите <E>";
            case TipType.ExitToShip:
                return "Чтобы вернуться на корабль нажмите <E>";
        }
        throw new System.ArgumentNullException("Tip with type " + tipType + " doesn't exist!");
    }
}

public enum TipType
{
    ExitToStation,
    OpenMap,
    ExitToShip
}