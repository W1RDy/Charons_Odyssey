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

    [Inject]
    private void Construct(CustomCamera camera)
    {
        _customCamera = camera;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Click");
        HandleClick();
    }

    public void HandleClick()
    {
        Ray ray = _customCamera.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, 1 << 5);

        if (hit.collider != null)
        {
            if (_currentClickView != null) _currentClickView.InteraptClick();

            var isGoodClick = !hit.collider.CompareTag("CancelClick");

            _currentClickView = isGoodClick ? _goodClickView : _badClickView as ClickView;
            
            _currentClickView.transform.position = new Vector3(hit.point.x, hit.point.y, _currentClickView.transform.position.z);
            _currentClickView.ShowClick();

            if (isGoodClick) OnGoodClick?.Invoke(hit.point);
        }
    }
}