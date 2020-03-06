using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float screenCenterX;
    private float screenCenterY;

    private GameObject[] scarabGOT = new GameObject[4];
    private ScarabObj[] scarabST = new ScarabObj[4];
    // Start is called before the first frame update
    void Start()
    {
        screenCenterX = Camera.main.scaledPixelWidth / 2;
        screenCenterY = Camera.main.scaledPixelHeight / 2;
    }

    // Update is called once per frame
    void Update()
    {
        SelectScarab();
    }

    private void SelectScarab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenCenterX, screenCenterY, Input.mousePosition.z));

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        if(hit.transform.CompareTag("Scarab" + (i + 1)))
                        {
                            ChangeScarabSprite(i, hit);
                            break;
                        }
                    }
                }
            }
        }
    }
    
    private void ChangeScarabSprite(int i, RaycastHit hit)
    {
        if (scarabGOT[i] == null)
        {
            scarabGOT[i] = hit.transform.gameObject;
            scarabST[i] = scarabGOT[i].GetComponent<ScarabObj>();
            scarabST[i].ChangeScrabSprite(1);
        }

        if (scarabGOT[i] != hit.transform.gameObject && scarabST[i].CheckConnections(hit.transform.gameObject))
        {
            hit.transform.gameObject.GetComponent<ScarabObj>().CheckConnections(scarabGOT[i]);
            scarabST[i].ChangeScrabSprite(0);
            scarabGOT[i] = hit.transform.gameObject;
            scarabST[i] = scarabGOT[i].GetComponent<ScarabObj>();
            scarabST[i].ChangeScrabSprite(1);
            if (scarabST[i].Failure())
            {

            }
        }
    }
}
