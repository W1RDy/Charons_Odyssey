using Cysharp.Threading.Tasks;
using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

public class MapWindow : Window
{
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Canvas _mapCanvas;

    private MapShip _mapShip;
    private MapWayMovementController _wayMovementController;

    private CancellationToken _token;
    private PauseTokenSource _pauseTokenSource;

    [Inject]
    private void Construct(MapShip mapShip, MapWayMovementController mapWayMovementController)
    {
        _mapShip = mapShip;
        _wayMovementController = mapWayMovementController;

        _token = this.GetCancellationTokenOnDestroy();
    }

    public override void ActivateWindow()
    {
        _wayMovementController.InteruptMovement();

        var destination = _wayMovementController.GetDestination();
        if (destination != null) destination = transform.InverseTransformPoint(destination.Value);

        transform.SetParent(_canvas.transform);
        transform.localPosition = Vector2.zero;
        //TranslateMapObject(_mapShip.transform, 0);

        Action action = () =>
        {
            if (destination != null)
            {
                destination = TranslateMapPos(destination.Value);
                _wayMovementController.TryActivateWayMovement(destination.Value);
            }
        };

        WaitWhileBuild(() => _surface.UpdateNavMesh(_surface.navMeshData), action);
    }

    private async void WaitWhileBuild(Action action, Action action1)
    {
        await UniTask.Yield();
        action.Invoke();
        await Delayer.DelayWithPause(0.02f, _token, _pauseTokenSource.Token);
        if (_token.IsCancellationRequested) return;
        action1.Invoke();
    }

    public override void DeactivateWindow()
    {
        _wayMovementController.InteruptMovement();

        var destination = _wayMovementController.GetDestination();
        if (destination != null) destination = transform.InverseTransformPoint(destination.Value);

        transform.SetParent(_mapCanvas.transform);
        transform.localPosition = Vector2.zero;
        //TranslateMapObject(_mapShip.transform, 0);

        Action action = () =>
        {
            if (destination != null)
            {
                destination = TranslateMapPos(destination.Value);
                _wayMovementController.TryActivateWayMovement(destination.Value);
            }
        };

        WaitWhileBuild(() => _surface.UpdateNavMesh(_surface.navMeshData), action);
    }

    private Vector2 TranslateMapPos(Vector2 pos)
    {
        return transform.TransformPoint(new Vector2(pos.x, pos.y));
    }
}
