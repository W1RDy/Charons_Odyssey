using UnityEngine;

public class TaskReturnToShip : BaseTask
{
    [SerializeField] private ExitToShipTrigger _exitToShipTrigger;

    private void Awake()
    {
        _exitToShipTrigger.TriggerInteracted += FinishTask;
    }

    public override void ActivateTask()
    {
        base.ActivateTask();
        _exitToShipTrigger.ActivateTrigger();
    }

    private void OnDestroy()
    {
        _exitToShipTrigger.TriggerInteracted -= FinishTask;
    }
}
