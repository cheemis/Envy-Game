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
    private float playerSpeed = 10;

    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text enemyText;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    // Check player money and upgrade fee for upgrade speed
    public void MayUpgradePlayerSpeed()
    {
        if (playerMoney >= currentUpgradeSpeedFee)
        {
            playerMoney -= currentUpgradeSpeedFee;
            moneyText.text = "My Money: " + playerMoney;
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

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public void AddPlayerMoney(int addMoney)
    {
        playerMoney += addMoney;
        moneyText.text = "My Money: " + playerMoney;
    }

    public void AddEnemyMoney(int addMoney)
    {
        enemyMoney += addMoney;
        enemyText.text = "Enemy Money: " + enemyMoney;
    }

    public void FinishCurrentGame()
    {
        // swith to the upgrading scene
        SceneManager.LoadScene(1);
    }

    public void NextRoundGame()
    {
        playerMoney = 0;
        enemyMoney = 0;
        SceneManager.LoadScene(0);
    }



}
