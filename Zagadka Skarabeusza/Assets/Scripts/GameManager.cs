using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class GameManager : MonoBehaviour
{
    //Główny skrypt zajmujący się logiką gry

    [SerializeField] private int BoardAmount = 4;           //Liczba instancji zagadek, każda zagadka jest otagowana "scarab" + liczba dla skarabeuszy i "board" + liczba dla plansz.
                                                            //INSTANCJE "scarab" i "board" MUSZĄ MIEĆ TAKĄ SAMĄ LICZBE, KAŻDY NUMER TABLICY PONIŻEJ(oprócz "winEffects") ODPOWIADA ZA OSOBNĄ INSTANCJE ZAGADKI
    [SerializeField] private int requiredScarabs = 25;      //liczba skarabeuszy niezbędna do zaliczenia wygranej
    [SerializeField] private LineDrawer[] ld;               //Odniesienie do skryptu "Line Drawer.cs"
    [SerializeField] private ParticleSystem[] winEffects;   //Odniesienie do komponentów "Particle System"

    private bool[] NoMoves;                                 //Boolean odpowiadający za to czy przegraliśmy  
    private int[] winCondition;                             //Integer odpowiadający za to czy wygraliśmy
    private Vector2 screenCenter;                           //Środek ekranu                             
    private ScarabObj[] scarabST;                           //"scarab script temp" przechowuje informacje ostatniego klikniętego skarabeusza

    // Start is called before the first frame update
    void Start()
    {
        SetCenterOfScreen();

        winCondition = new int[BoardAmount];
        scarabST = new ScarabObj[BoardAmount];

        NoMoves = new bool[BoardAmount];
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

    //Funkcja odpowiadająca za wciśnięcia myszy, wybieranie skarabeuszy i resetowanie zagadki
    private void SelectScarab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenCenter.x, screenCenter.y, Input.mousePosition.z));

            if (Physics.Raycast(ray, out hit, 100f))    // Wystrzeliwujemy promień i jeśli coś trafiliśmy
            {
                if (hit.transform != null)
                {                    

                    for(int i = 0; i < BoardAmount; i++)
                    {
                        if((hit.transform.CompareTag("Scarab" + (i + 1)) || hit.transform.CompareTag("Board" + (i + 1))) && NoMoves[i]) // Jeżeli nie masz wolnych ruchów, zresetuj zagadke
                        {
                            ResetBoard(i,hit);
                            break;
                        }
                        else if(hit.transform.CompareTag("Scarab" + (i + 1)))   // Jeżeli kliknąłeś skarabesza 
                        {
                            ChangeScarabSprite(i, hit);
                            if (winCondition[i] == requiredScarabs)    YouWin(i);   //Sprawdza, czy wygrałeś
                            break;
                        }
                    }
                }
            }
        }
    }
    
    //funkcja odpowiadająca za zmiany w skarabeuszach
    private void ChangeScarabSprite(int i, RaycastHit hit)
    {
        if (scarabST[i] == null)    //Jeżeli nie kliknąłeś w skarabeusza
        {
            scarabST[i] = hit.transform.gameObject.GetComponent<ScarabObj>();   //Przypisanie skarabeusza do "scarabST[i]"
            winCondition[i]++;                                                  //dodanie punktu do wygranej
            scarabST[i].ChangeScrabSprite(1);                                   //Zmiana tekstury na "selected scarab sprite"
            ld[i].CreatePoint(scarabST[i].transform);                                   //Dodanie punktu w linii
        }

        if (scarabST[i].gameObject != hit.transform.gameObject && scarabST[i].CheckConnections(hit.transform.gameObject))   //Jeżeli kliknięty skarabeusz nie jest tym co poprzednio i są połączone
        {
            hit.transform.gameObject.GetComponent<ScarabObj>().CheckConnections(scarabST[i].gameObject);    //Funkcja CheckConnections dodatkowo dezaktywuje połączenie między nimi dlatego wywołuję funkcje jeszcze raz
                                                                                                            //Zrobione w ten sposób, ponieważ skrypt musi i tak wykonać te same kroki
            scarabST[i].ChangeScrabSprite(0);                                   //Zmiana tekstury na "active scarab sprite"
            scarabST[i] = hit.transform.gameObject.GetComponent<ScarabObj>();   //Przipisanie nowego skrabeusza
            ld[i].CreatePoint(scarabST[i].transform);                           //Dodanie nowego punktu w linii

            if (!scarabST[i].wasSelected)                                       //Jeżeli skarabeusz nie był wcześniej wciśnięty, dodajemy punkt do wygranej. To jedyny moment w skrypcie, w którym mogę to sprawdzić
            {
                winCondition[i]++;
            }

            scarabST[i].ChangeScrabSprite(1);

            if (scarabST[i].Failure())                                          //Jeżeli nie mamy żadnych wolnych ruchów, przegrywamy
            {
                NoMoves[i] = true;
            }           
        }
    }

    //Reset planszy o id "i" Odbywa się poprzez wciśnięcie LPM
    private void ResetBoard(int i, RaycastHit hit)
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Scarab"+ (i + 1)); //Wyszukuje wszystkie skarabeusze z danej planszy
        for(int j = 0; j < go.Length; j++)
        {
            ScarabObj temp = go[j].GetComponent<ScarabObj>();
            if(temp.wasSelected)                                //Jeśli był wciśnięty;      Trzeba to sprawdzić ze względów optymalizacyjnych(funkcja "ResetConnections()" zawiera także pętle
            temp.ResetConnections();                            //Resetuje wszystkie połączenia
        }

        ld[i].RemoveAllLines();                                 //Usuwa linie
        scarabST[i] = null;
        NoMoves[i] = false;
        winCondition[i] = 0;
    }

    //Wylicza środek ekranu, trzeba użyć w przypadku zmiany rozdzielczości
    private void SetCenterOfScreen()
    {
        screenCenter.x = Camera.main.scaledPixelWidth / 2;
        screenCenter.y = Camera.main.scaledPixelHeight / 2;
    }

    //Wywoływane w przypadku wygranej
    private void YouWin(int i)
    {
        for(int j = 0;j < winEffects.Length; j++)
        {
            winEffects[j].Play();   //Aktywacja prostych efektów specjalnych
        }
        winCondition[i] = 0;
        NoMoves[i] = true;  //Ustawiam na true, żeby można było zresetować plansze żeby zagrać jeszcze raz
    }
}
