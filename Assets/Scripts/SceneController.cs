using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public int gameState = 0;

    public int bulletChainLevel;
    public bool bulletChainStatus;
    public bool bulletSlowStatus;
    public bool bulletForkStatus;

    public int playerAggro = 1;
    public float inGameTime;
    public Text countdownTimer;
    public Text mainText;
    public Text carCount;
    public Text moneyCount;
    public Button pauseButton;
    public Text pauseText;
    public Text endText;
    public Text endNumber;
    public Text homeText;
    public Button muteButton;
    Button home;
   


    public int gameStage;
    public float spawnCount;
    public int money = 0;
    public int mute;

    int saveKill;
    int highScore;
    float surviveTime;

    public bool pause = false;
    public int killCount = 0;

    public bool gameOver = false;
    float overTime = 0;


    public int carID;
    public GameObject[] cars;
    public bool[] carsUnlockedOrNot;
    int unlockedOrNot;

    public GameObject currentCar;
    public GameObject chosenCar;

    int preferredCar;
    bool doAlready = false;

    public string[] carDescription;
    public Text carDes_text;

    
    private void Awake()
    {
       
        SCR_Pool.Flush();
        unlockedOrNot = PlayerPrefs.GetInt("Unlocked");
        if (unlockedOrNot == 0)
        {
            for (int i = 1; i <= cars.Length - 1; i++)
            {
                carsUnlockedOrNot[i] = false;
            }
        } else if (unlockedOrNot == 1)
        {
            for (int i = 0; i<= cars.Length - 1; i++)
            {
                carsUnlockedOrNot[i] = true;
            }
        }
        preferredCar = PlayerPrefs.GetInt("PreferredCar");



        chosenCar = SCR_Pool.GetFreeObject(cars[preferredCar]);
        currentCar = chosenCar;
        chosenCar.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        saveKill = PlayerPrefs.GetInt("KillCount", 0);
        mute = PlayerPrefs.GetInt("MuteOrNot", 0);
        surviveTime = PlayerPrefs.GetInt("SurviveTime", 0);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == 0)
        {
            ChangeCar();
        } else if (gameState == 1 && doAlready == false)
        {
            FindObjectOfType<AudioManager>().Play("Theme 2");
            FindObjectOfType<AudioManager>().Play("Police Siren");
            doAlready = true;
        }
        PauseButton();
        TimeCountDown();
        GameProgression();
        carCount.text = "CARS " + killCount.ToString();
        moneyCount.text = "$" + money.ToString();
        if (pause == true)
        {
            pauseText.text = " PAUSED ";
            
           // muteButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
        } else if (pause == false)
        {
            pauseText.text = " ";
            
           // muteButton.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }


        if (money >= PlayerPrefs.GetInt("HighScore",0))
        {
            moneyCount.color = new Color(255, 15, 15);
            PlayerPrefs.SetInt("HighScore", money);
           
        }
        if (killCount >= PlayerPrefs.GetInt("KillCount",0))
        {
            carCount.color = new Color(255, 15, 15);
            PlayerPrefs.SetInt("KillCount", killCount);
           
        }
      
        if (gameOver == true)
        {
            overTime += Time.deltaTime;
            if (overTime >= 1f)
            {
                endText.text = "BUSTED!";
                endNumber.text = "$" + money.ToString() + " Highscore: " + PlayerPrefs.GetInt("HighScore", 0);
                mainText.text = " ";
                if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene(1);
                }
            }
           
        }

        
    }

    void TimeCountDown ()
    {
        inGameTime += Time.deltaTime;
        string minutes = Mathf.Floor(inGameTime / 60).ToString("00");
        string seconds = (inGameTime % 60).ToString("00");
       
        countdownTimer.text = string.Format("{0}:{1}", minutes, seconds);
        if (inGameTime > PlayerPrefs.GetInt("SurviveTime"))
        {
            surviveTime = inGameTime;
        }
    }
    void GameProgression()
    {
        spawnCount += Time.deltaTime;
        if (spawnCount >= 10)
        {
            gameStage += 1;
          
            spawnCount = 0f;
        }
    }
    void PauseButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = true;
        }
        if (Pause.pausing == true)
        {
            pause = true;
        }
        if (Pause.pausing == false)
        {
            pause = false;
        }
    }
    void ChangeCar ()
    {
        if (gameState == 0)
        {
            if (currentCar.activeSelf == false)
            {
                chosenCar = SCR_Pool.GetFreeObject(cars[carID]);
                currentCar = chosenCar;

                chosenCar.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                
                if (carsUnlockedOrNot[carID] == false)
                {
                    carDes_text.text = "Locked";
                    GameObject.Find("Change GameState").GetComponent<Image>().color = Color.gray;
                    GameObject.Find("Change GameState").GetComponent<Button>().interactable = false;
                }
                else if (carsUnlockedOrNot[carID] == true)
                {
                    carDes_text.text = "";
                    GameObject.Find("Change GameState").GetComponent<Image>().color = Color.white;
                    GameObject.Find("Change GameState").GetComponent<Button>().interactable = true;
                }
            }
           
        }
      
    }


    //Unlock car system

    void UnlockCar()
    {

    }
}
