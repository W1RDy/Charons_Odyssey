using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class WayView : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ChangeWayView();
    }

    public void CalculateAndShowWay(NavMeshAgent agent, NavMeshPath path, Vector2 destination)
    {
        if (_agent == null)
        {
            _agent = agent;
        }

        Debug.Log(destination);
        _agent.CalculatePath(destination, path);

        ShowWay(path.corners);
    }

    public void ShowWay(Vector3[] points)
    {
        Debug.Log(points.Length);
        _lineRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            _lineRenderer.SetPosition(points.Length - 1 - i, points[i]);
        }
    }

    public void ClearView()
    {
        _lineRenderer.positionCount = 0;
        _lineRenderer.SetPositions(new Vector3[0]);
    }

    public void ChangeWayView()
    {
        if (_agent != null && _lineRenderer.positionCount > 0)
        {
            var lastPointIndex = _lineRenderer.positionCount - 1;

            _lineRenderer.SetPosition(lastPointIndex, _agent.transform.position);

            if (lastPointIndex - 1 > 0 && Vector2.Distance(_agent.transform.position, _lineRenderer.GetPosition(lastPointIndex - 1)) < 0.6f)
            {
                _lineRenderer.positionCount--;
            }
        }
    }
}
