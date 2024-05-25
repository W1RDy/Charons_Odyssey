using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Canvas _canvas;

    [SerializeField] GoodClickView _goodClickView;
    [SerializeField] BadClickView _badClickView;

    private CustomCamera _customCamera;

    private ClickView _currentClickView;

    public event Action<Vector2> OnGoodClick;

    private PauseToken _pauseToken;

    [Inject]
    private void Construct(CustomCamera camera, IInstantiator instantiator)
    {
        _customCamera = camera;

        var pauseHandler = instantiator.Instantiate<PauseHandler>();
        _pauseToken = pauseHandler.GetPauseToken();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_pauseToken.IsCancellationRequested)
        {
            Debug.Log("Click");
            HandleClick();
        }
    }

    public void HandleClick()
    {
        Ray ray = _customCamera.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 100, 1 << 11);

        if (hits.Length > 0)
        {
            if (_currentClickView != null) _currentClickView.InteraptClick();

            var hit = GetPrioritetHit(hits);

            var isGoodClick = !hit.collider.CompareTag("CancelClick");

            _currentClickView = isGoodClick ? _goodClickView : _badClickView as ClickView;

            _currentClickView.transform.position = GetClickPosition(hit);
            _currentClickView.ShowClick();

            if (isGoodClick) OnGoodClick?.Invoke(GetClickPosition(hit));
        }
    }

    private Vector3 GetClickPosition(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("MapGoalLocation"))
        {
            IGoalMapObject goalObject = hit.collider.gameObject.GetComponent<IGoalMapObject>();
            var goalPosition = goalObject.GetGoalPosition();
            return new Vector3(goalPosition.x, goalPosition.y, _currentClickView.transform.position.z);
        }
        return new Vector3(hit.point.x, hit.point.y, _currentClickView.transform.position.z);
    }

    private RaycastHit2D GetPrioritetHit(RaycastHit2D[] hits)
    {
        RaycastHit2D? cancelClickHit = null;
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("MapGoalLocation")) return hit;
            else if (hit.collider.CompareTag("CancelClick")) cancelClickHit = hit;
        }

        if (cancelClickHit != null) return cancelClickHit.Value;
        return hits[0];
    }
}