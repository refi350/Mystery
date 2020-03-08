using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    private LineRenderer lr;

    private Vector3 vCenter;
    private float vAngle;
    private float vLength;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

    }
    public void CreatePoint(Transform s1)
    {
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, s1.position);
    }

    public void RemoveAllLines()
    {
        lr.positionCount = 0;
    }
}
