using UnityEngine;
using Zenject;

public class TaskKillEnemies : BaseTask
{
    private EnemiesCounter _enemiesCounter;

    [Inject]
    private void Construct(EnemiesCounter enemiesCounter)
    {
        _enemiesCounter = enemiesCounter;
        _enemiesCounter.OnCounterUpdated += CheckTaskCondition;
    }

    private void CheckTaskCondition()
    {
        if (_enemiesCounter.GetEnemiesCount() == 0)
        {
            _enemiesCounter.OnCounterUpdated -= CheckTaskCondition;
            FinishTask();
        }
    }

    private void OnDestroy()
    {
        _enemiesCounter.OnCounterUpdated -= CheckTaskCondition;
    }
}