using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    //Skrypt odpowiadający za rysowanie linii na zagadce

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

    }

    //Tworzy nowy punkt na linii
    public void CreatePoint(Transform s1)
    {
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, s1.position);
    }

    //Usuwa wszystkie linie
    public void RemoveAllLines()
    {
        lr.positionCount = 0;
    }
}
