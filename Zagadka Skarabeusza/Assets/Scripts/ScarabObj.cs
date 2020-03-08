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

    [HideInInspector]public bool wasSelected = false;

    private List<bool> activatedObjects = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < connectedObjects.Count; i++)
        activatedObjects.Add(true);
    }

    // targetSprite - 0(activeScarabSprite), 1(selectedScarabSprite), 2(inactiveScarabSprite)
    public void ChangeScrabSprite(int targetSprite)
    {
        switch (targetSprite)
        {
            case 0:
                spriteobj.sprite = activeScarabSprite;
                wasSelected = true;
                break;

            case 1:
                spriteobj.sprite = selectedScarabSprite;
                wasSelected = true;
                break;

            case 2:
                spriteobj.sprite = inactiveScarabSprite;
                wasSelected = false;
                break;

            default: Debug.LogError("Wrong Number! targetSprite - 0(activeScarabSprite), 1(selectedScarabSprite), 2(inactiveScarabSprite)");
                break;
        }
    }

    public bool CheckConnections(GameObject target)
    {
        for(int i = 0; i < connectedObjects.Count; i++)
        {
            if(target == connectedObjects[i].gameObject && activatedObjects[i])
            {
                activatedObjects[i] = false;
                return true;
            }
        }
        return false;
    }

    public void ResetConnections()
    {
        for(int i = 0; i < activatedObjects.Count; i++)
        {
            activatedObjects[i] = true;
            ChangeScrabSprite(2);
        }
    }

    public bool Failure()
    {
        int gg = 0;
        for (int i = 0; i < activatedObjects.Count; i++)
        {
            if (!activatedObjects[i])
            {
                gg++;
            }
        }
        if (gg == activatedObjects.Count)
        {
            return true;
        }

            return false;
    }
}
