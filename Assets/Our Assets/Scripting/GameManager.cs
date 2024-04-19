using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


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
    private float InitialPlayerSpeed = 10;
    [SerializeField]
    private float InitialPlayerKnockBack = -2;

    private float playerSpeed = 10;
    private float playerKnockBack = -2;

    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI enemyText;


    private GameObject statsObjects;
    private TextMeshProUGUI statsText;


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

    // CoverImage and Scrolling text after player wins
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI scrollingText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if(audioSource == null) audioSource = GetComponent<AudioSource>();

            //set text
            moneyText.text = playerTextString + playerMoney;
            enemyText.text = enemyTextString + enemyMoney;

            //set stats text objects
            statsObjects = transform.GetChild(0).GetChild(1).gameObject; //hard coded
            if (statsObjects != null)
            {
                statsText = statsObjects.GetComponentInChildren<TextMeshProUGUI>();
                statsObjects.SetActive(false);
            }

            //set default values
            playerSpeed = InitialPlayerSpeed;
            playerKnockBack = InitialPlayerKnockBack;


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
        TextMeshProUGUI countdownText = GameObject.FindGameObjectWithTag("CountDownText").GetComponent<TextMeshProUGUI>();
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
        // Debug winning 
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerMoney += enemyMoney + 1;
            playerWon = true;
            FinishCurrentGame();
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if(clip !=null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void SetStatsText(bool statsAwake)
    {
        if(statsObjects != null)
        {
            statsObjects.SetActive(statsAwake);

            if (statsAwake)
            {
                statsText.text = " -- Stats --\nSpeed: " + playerSpeed + "\nStrength: " + playerKnockBack;
            }
        }

    }

    // Check player money and upgrade fee for upgrade speed
    public void MayUpgradePlayerSpeed()
    {
        if (playerMoney >= currentUpgradeSpeedFee)
        {
            playerMoney -= currentUpgradeSpeedFee;
            moneyText.text = playerTextString + playerMoney;
            playerSpeed += currentSpeedUpgradeValue;

            SetStatsText(true);
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

            SetStatsText(true);
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
            if (playerWon)
            {
                StartCoroutine(PlayWinEffects());
            }
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

        //disable stats objects
        SetStatsText(false);
    }

    public void LoadUpgradeScene()
    {
        SceneManager.LoadScene(2);

        //enable stats objects
        SetStatsText(true);
    }

    public bool GetPlayerWon()
    {
        return playerWon;
    }

    // Helper functions for fading and text scrolling effects
    
    IEnumerator FadeScreen(Color startColor, Color endColor, float duration)
    {
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, counter / duration);
            yield return null;
        }
    }
    
    IEnumerator ScrollText(float duration, float scrollSpeed)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            scrollingText.rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator PlayWinEffects()
    {
        yield return new WaitForSeconds(2);

        // Fade to white and then to black
        yield return StartCoroutine(FadeScreen(Color.clear, Color.white, 1.5f));
        yield return new WaitForSeconds(0.5f); // A brief pause in white
        yield return StartCoroutine(FadeScreen(Color.white, Color.black, 2.5f));

        // Show and scroll the winner text
        yield return StartCoroutine(ScrollText(52, 25));
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

}
