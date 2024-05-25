using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{
    [SerializeField] private SpriteMask _mask;
    [SerializeField] private SpriteRenderer _firstPlane;

    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpaceBordersChanger _bordersChanger;

    private bool _isOpen;
    private bool _isColliderTrigger;

    private void Awake()
    {
        _isColliderTrigger = _collider.isTrigger;
    }

    public void OpenSpace()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            _mask.gameObject.SetActive(true);
            if (_firstPlane) _firstPlane.enabled = false;

            MakeObjAvailable();
            _collider.isTrigger = true;
            if (_bordersChanger != null) _bordersChanger.ActivateBorders();
        }
    }

    public void CloseSpace()
    {
        if (_isOpen)
        {
            _isOpen = false;
            _mask.gameObject.SetActive(false);
            if (_firstPlane) _firstPlane.enabled = true;

            MakeObjAvailable();
            _collider.isTrigger = _isColliderTrigger;
            if (_bordersChanger != null) _bordersChanger.DeactivateBorders();
        }
    }

    private void MakeObjAvailable()
    {
        var availableObjs = Physics2D.OverlapBoxAll(new Vector2(_collider.transform.position.x, _collider.transform.position.y) + _collider.offset, _collider.bounds.size, 0, 1 << 6 | 1 << 0);
        if (availableObjs.Length > 0)
        {
            foreach (var obj in availableObjs)
            {
                var availableObj = obj.GetComponent<IAvailable>();
                if (availableObj == null) availableObj = obj.GetComponentInParent<IAvailable>();
                if (availableObj != null) availableObj.ChangeAvailable(_isOpen);
            }
        }
    }

    public bool IsOpen() => _isOpen;
}
