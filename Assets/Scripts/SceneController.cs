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
    public bool bulletPierceStatus;
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
    Button home;



    public int gameStage;
    public float spawnCount;
    public int money = 0;
    int highScore;

    public bool pause = false;
    public int killCount = 0;

    public bool gameOver = false;
    float overTime = 0;


    public int carID;
    public GameObject[] cars;
    public GameObject currentCar;
    public GameObject chosenCar;
    private void Awake()
    {
       
        SCR_Pool.Flush();
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        home = homeText.GetComponent<Button>();
        home.enabled = false;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == 0)
        {
            ChangeCar();
        }
        PauseButton();
        TimeCountDown();
        GameProgression();
        carCount.text = "CARS " + killCount.ToString();
        moneyCount.text = "$" + money.ToString();
        if (pause == true)
        {
            pauseText.text = " PAUSED ";
            homeText.text = " HOME ";
            home.enabled = true;
            Time.timeScale = 0f;
        } else if (pause == false)
        {
            pauseText.text = " ";
            homeText.text = " ";
            home.enabled = false;
            Time.timeScale = 1f;
        }
        if (money > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", money);
            moneyCount.color = new Color(255, 15, 15);
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
        if ( pauseButton.GetComponent<Pause>().pausing == true)
        {
            pause = true;
        }
        if (pauseButton.GetComponent<Pause>().pausing == false)
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
            }
           
        }
    }
}
