using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarabObj : MonoBehaviour
{
    //Skrypt odpowiadający za pojedyńczą instancje skarabeusza

    [SerializeField]private Sprite activeScarabSprite;                                  //Tekstura aktywnego skarabeusza
    [SerializeField]private Sprite selectedScarabSprite;                                //Tekstura wybranego skarabeusza
    [SerializeField]private Sprite inactiveScarabSprite;                                //Tekstura nieaktywnego skarabeusza
                                                                                        //Mogłem to zrobić w pojedyńczej tablicy, ale chciałem żeby to było wyróżnione podczas wprowadzania
    [SerializeField] private SpriteRenderer spriteobj;                                  //Odniesienie do sprite renderera

    [SerializeField] private List<ScarabObj> connectedObjects = new List<ScarabObj>();  //Lista aktywnych połączeń, do wprowadzenia manualnego
                                                                                        
    [HideInInspector]public bool wasSelected = false;                                   //Czy było wcześniej wciśnięte

    private List<bool> activatedObjects = new List<bool>();                             //Lista zawierająca dostepne połączenia z innymi skarabeuszami

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < connectedObjects.Count; i++)                                 //Ustawia wszystkie "activatedObjects" na true
        activatedObjects.Add(true);
    }

    //Skrypt odpowiadający za zmiane sprite skarabeusza i oznaczenie, że był wciśnięty dla "winCondition" w skrypcie GameManager.cs
    //targetSprite - 0(activeScarabSprite), 1(selectedScarabSprite), 2(inactiveScarabSprite)
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

    //Sprawdzenie, czy skarabeusz posiada połączenie z podanym skarabeuszem i dezaktywacja połączenia
    public bool CheckConnections(GameObject target)
    {
        for(int i = 0; i < connectedObjects.Count; i++)
        {
            if(target == connectedObjects[i].gameObject && activatedObjects[i]) //Jeśli skarabeusz posiada połączenie i nie było wcześniej aktywowane
            {
                activatedObjects[i] = false;    //dezaktywuje połączenie
                return true;
            }
        }
        return false;
    }

    //Resetuje połączenia
    public void ResetConnections()
    {
        for(int i = 0; i < activatedObjects.Count; i++)
        {
            activatedObjects[i] = true;
            ChangeScrabSprite(2);
        }
    }

    //Sprawdza, czy zabrakło możliwych ścieżek
    public bool Failure()
    {
        int gg = 0; //przelicznik zablokowanych ścieżek
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
