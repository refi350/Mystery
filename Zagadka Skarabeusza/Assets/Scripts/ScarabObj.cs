using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarabObj : MonoBehaviour
{
    [SerializeField]private Sprite activeScarabSprite;
    [SerializeField]private Sprite selectedScarabSprite;
    [SerializeField]private Sprite inactiveScarabSprite;

    [SerializeField] private SpriteRenderer spriteobj;

    [SerializeField] private List<ScarabObj> connectedObjects = new List<ScarabObj>();

    private bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // targetSprite - 0(activeScarabSprite), 1(selectedScarabSprite), 2(inactiveScarabSprite)
    public void ChangeScrabSprite(int targetSprite)
    {
        switch (targetSprite)
        {
            case 0:
                spriteobj.sprite = activeScarabSprite;
                isSelected = false;
                break;
            case 1:
                spriteobj.sprite = selectedScarabSprite;
                isSelected = true;
                break;
            case 2:
                spriteobj.sprite = inactiveScarabSprite;
                isSelected = false;
                break;
         default: Debug.LogError("Wrong Number! targetSprite - 0(activeScarabSprite), 1(selectedScarabSprite), 2(inactiveScarabSprite)");
                break;
        }
    }

    /*private void OnMouseExit()
    {
        if (isSelected)
        {
            ChangeScrabSprite(0);
            Debug.Log("Click");
            for(int i = 0;i < connectedObjects.Count; i++)
            {
                Debug.Log(connectedObjects[i]);
                connectedObjects[i].ChangeScrabSprite(1);
            }
            
        }
    } */

    public bool CheckConnections(GameObject target)
    {
        for(int i = 0; i < connectedObjects.Count; i++)
        {
            if(target == connectedObjects[i].gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
