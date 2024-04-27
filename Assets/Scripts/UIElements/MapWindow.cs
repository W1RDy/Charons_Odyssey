using Cysharp.Threading.Tasks;
using NavMeshPlus.Components;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class MapWindow : Window
{
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private Camera camera;

    private MapShip _mapShip;
    private MapWayMovementController _wayMovementController;

    [Inject]
    private void Construct(MapShip mapShip, MapWayMovementController mapWayMovementController)
    {
        _mapShip = mapShip;
        _wayMovementController = mapWayMovementController;
    }

    public override void ActivateWindow()
    {
        _wayMovementController.InteruptMovement();

        var destination = _wayMovementController.GetDestination();
        if (destination != null) destination = transform.InverseTransformPoint(destination.Value);
        TranslateMapObject(transform, -1000);
        //TranslateMapObject(_mapShip.transform, 0);

        Action action = () =>
        {
            if (destination != null)
            {
                destination = TranslateMapPos(destination.Value, 0);
                _wayMovementController.TryActivateWayMovement(destination.Value);
            }
        };

        StartCoroutine(WaitWhileBuild(() => _surface.UpdateNavMesh(_surface.navMeshData), action));
    }

    private IEnumerator WaitWhileBuild(Action action, Action action1)
    {
        yield return null;
        action.Invoke();
        yield return new WaitForSeconds(0.02f);
        action1.Invoke();
    }

    public override void DeactivateWindow()
    {
        _wayMovementController.InteruptMovement();

        var destination = _wayMovementController.GetDestination();
        if (destination != null) destination = transform.InverseTransformPoint(destination.Value);
        TranslateMapObject(transform, 1000);
        //TranslateMapObject(_mapShip.transform, 0);

        Action action = () =>
        {
            if (destination != null)
            {
                Debug.Log(destination.Value);
                destination = TranslateMapPos(destination.Value, 0);
                Debug.Log(destination.Value);
                _wayMovementController.TryActivateWayMovement(destination.Value);
            }
        };

        StartCoroutine(WaitWhileBuild(() => _surface.UpdateNavMesh(_surface.navMeshData), action));
    }

    private void TranslateMapObject(Transform transform, float translateDistance)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + translateDistance, 0);
        Debug.Log(transform.localPosition.y);
    }

    private Vector2 TranslateMapPos(Vector2 pos, float translateDistance)
    {
        return transform.TransformPoint(new Vector2(pos.x, pos.y + translateDistance));
    }
}
