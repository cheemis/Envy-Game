using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    private int playerMoney = 0;
    [SerializeField]
    private int enemyMoney = 0;

    [SerializeField]
    private int currentUpgradeSpeedFee = 10;
    [SerializeField]
    private float currentSpeedUpgradeValue = 5;
    [SerializeField]
    private float currentKnockBackUpgradeValue = .5f;

    [SerializeField]
    private float playerSpeed = 10;
    [SerializeField]
    private float playerKnockBack = -2;

    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text enemyText;

    [SerializeField]
    private bool playerWon = false;

    private bool gameStart = false;

    //sound effect variables
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip countDownSFX;
    [SerializeField]
    private AudioClip countDownEndSFX;
    [SerializeField]
    private AudioClip confirmSFX;

    //string variables
    [SerializeField]
    private string playerTextString = "P1 Score: ";
    [SerializeField]
    private string enemyTextString = "P2 Score: ";



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if(audioSource == null) audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameStart = false;

        // if the current scene is main scene, lock for 3 sec and then play.
        if (scene.buildIndex == 1)
        {
            //start to coutndown
            StartCoroutine(TimerCoroutine());
        }

    }

    private IEnumerator TimerCoroutine()
    {
        Text countdownText = GameObject.FindGameObjectWithTag("CountDownText").GetComponent<Text>();
        Canvas countdown = GameObject.FindGameObjectWithTag("CountDown").GetComponent<Canvas>();
        countdown.enabled = true;
        for (int secondsPassed = 0; secondsPassed <= 3; secondsPassed++)
        {
            //set the text
            int showText = 3 - secondsPassed;
            if (showText != 0)
            {
                countdownText.text = showText.ToString();
                PlaySFX(countDownSFX);
            }
            else
            {
                countdownText.text = "Go!";
                PlaySFX(countDownEndSFX);
            }
            
            yield return new WaitForSeconds(1);
            
        }

        // After 3 seconds, set the boolean to true
        gameStart = true;
        countdown.enabled = false;
    }

    public bool IsGameStart()
    {
        return gameStart;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    static public GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlaySFX(AudioClip clip)
    {
        if(clip !=null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Check player money and upgrade fee for upgrade speed
    public void MayUpgradePlayerSpeed()
    {
        if (playerMoney >= currentUpgradeSpeedFee)
        {
            playerMoney -= currentUpgradeSpeedFee;
            moneyText.text = playerTextString + playerMoney;
            //FlatPlayerController player = GameObject.FindWithTag("Player").GetComponent<FlatPlayerController>();
            //player.AddPlayerSpeed(currentSpeedUpgradeValue);
            playerSpeed += currentSpeedUpgradeValue;

            // may change both current Speed fee and Current speed change value

        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    // Check player money and upgrade fee for upgrade speed
    public void MayUpgradePlayerKnockBack()
    {
        if (playerMoney >= currentUpgradeSpeedFee)
        {
            playerMoney -= currentUpgradeSpeedFee;
            moneyText.text = playerTextString + playerMoney;
            playerKnockBack += currentKnockBackUpgradeValue;
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public float GetPlayerKnockBack()
    {
        return playerKnockBack;
    }

    public void AddPlayerMoney(int addMoney)
    {
        playerMoney += addMoney;
        moneyText.text = playerTextString + playerMoney;
    }

    public void AddEnemyMoney(int addMoney)
    {
        enemyMoney += addMoney;
        enemyText.text = enemyTextString + enemyMoney;
    }

    public void FinishCurrentGame()
    {
        playerWon = playerMoney > enemyMoney ? true : false;
        // swith to the upgrading scene

        GameObject winnerScreen = GameObject.FindGameObjectWithTag("Winner Screen");
        GameObject gameScreen = GameObject.FindGameObjectWithTag("Game Screen");

        //if a winner screen exists
        if ( winnerScreen != null && gameScreen != null)
        {
            gameScreen.SetActive(false);

            //enemy won
            winnerScreen.transform.GetChild(winnerScreen.transform.childCount - 1).gameObject.SetActive(playerWon);
            
            //player won
            winnerScreen.transform.GetChild(winnerScreen.transform.childCount - 2).gameObject.SetActive(!playerWon);
        }
        //else, just go to game screen
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void NextRoundGame()
    {
        playerMoney = 0;
        moneyText.text = playerTextString + playerMoney;

        enemyMoney = 0;
        enemyText.text = enemyTextString + enemyMoney;

        SceneManager.LoadScene(1);
    }

    public bool GetPlayerWon()
    {
        return playerWon;
    }



}
