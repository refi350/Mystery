using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarabObj : MonoBehaviour
{
    [SerializeField]private Sprite activeScarabSprite;
    [SerializeField]private Sprite inactiveScarabSprite;
    [SerializeField]private Sprite goldenScarabSprite;

    private bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetScarabPos()
    {
        return transform.position;
    }

    // targetSprite - 0(inactiveScarabSprite), 1(activeScarabSprite), 2(goldenScarabSprite)
    private void ChangeScrabSprite(int targetSprite)
    {

    }
}
