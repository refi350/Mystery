using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int BoardAmount = 4;
    [SerializeField] private int requiredScarabs = 25;
    [SerializeField] private LineDrawer[] ld;
    [SerializeField] private ParticleSystem[] winEffects;

    private float screenCenterX;
    private float screenCenterY;

    private bool[] NoMoves;
    private int[] winCondition;
    private ScarabObj[] scarabST;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        SetCenterOfScreen();

        NoMoves = new bool[BoardAmount];
        scarabST = new ScarabObj[BoardAmount];
        winCondition = new int[winEffects.Length];

        for(int i = 0; i < BoardAmount; i++)
        {
            NoMoves.SetValue(false, i);
        }
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

                    for(int i = 0; i < BoardAmount; i++)
                    {
                        if((hit.transform.CompareTag("Scarab" + (i + 1)) || hit.transform.CompareTag("Board" + (i + 1))) && NoMoves[i])
                        {
                            ResetBoard(i,hit);
                            break;
                        }
                        else if(hit.transform.CompareTag("Scarab" + (i + 1)))
                        {
                            ChangeScarabSprite(i, hit);
                            if (winCondition[i] == requiredScarabs)    YouWin(i);
                            break;
                        }
                    }
                }
            }
        }
    }
    
    private void ChangeScarabSprite(int i, RaycastHit hit)
    {
        if (scarabST[i] == null)
        {
            scarabST[i] = hit.transform.gameObject.GetComponent<ScarabObj>();
            winCondition[i]++;
            scarabST[i].ChangeScrabSprite(1);
            ld[i].CreatePoint(hit.transform);
        }

        if (scarabST[i].gameObject != hit.transform.gameObject && scarabST[i].CheckConnections(hit.transform.gameObject))
        {
            ld[i].CreatePoint(hit.transform);
            hit.transform.gameObject.GetComponent<ScarabObj>().CheckConnections(scarabST[i].gameObject);
            scarabST[i].ChangeScrabSprite(0);

            scarabST[i] = hit.transform.gameObject.GetComponent<ScarabObj>();
            if (!scarabST[i].wasSelected)
            {
                winCondition[i]++;
            }
            scarabST[i].ChangeScrabSprite(1);
            if (scarabST[i].Failure())
            {
                NoMoves[i] = true;
            }           
        }
    }

    private void ResetBoard(int i, RaycastHit hit)
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Scarab"+ (i + 1));
        for(int j = 0; j < go.Length; j++)
        {
            ScarabObj temp = go[j].GetComponent<ScarabObj>();
            if(temp.wasSelected)
            temp.ResetConnections();
        }
        ld[i].RemoveAllLines();
        scarabST[i] = null;
        NoMoves[i] = false;
        winCondition[i] = 0;
    }

    private void SetCenterOfScreen()
    {
        screenCenterX = Camera.main.scaledPixelWidth / 2;
        screenCenterY = Camera.main.scaledPixelHeight / 2;
    }

    private void YouWin(int i)
    {
        for(int j = 0;j < winEffects.Length; j++)
        {
            winEffects[j].Play();
        }
        winCondition[i] = 0;
        NoMoves[i] = true;
    }
}
