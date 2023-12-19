using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{
    [SerializeField] private SpriteMask _mask;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _firstPlane;
    private bool _isOpen;

    public void OpenSpace()
    {
        _isOpen = true;
        _mask.gameObject.SetActive(true);
        _firstPlane.sortingLayerName = "Background"; 
        MakeObjAvailable();
        _collider.enabled = false;
    }

    public void CloseSpace()
    {
        _isOpen = false;
        _mask.gameObject.SetActive(false);
        MakeObjAvailable();
        _collider.enabled = true;
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
