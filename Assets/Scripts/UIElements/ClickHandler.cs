using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _clickView;
    private CustomCamera _customCamera;

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
            var cube = Instantiate(_clickView, hit.point, Quaternion.identity, _canvas.transform);
            if (hit.collider.CompareTag("CancelClick"))
            {
                Debug.Log("Click cancelled");
                return;
            }
            Debug.Log("Click on " + hit.collider.gameObject.name);
        }
    }
}