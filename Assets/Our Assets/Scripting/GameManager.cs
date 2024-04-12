using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    private int playerMoney = 0;

    [SerializeField]
    private int currentUpgradeSpeedFee = 10;
    [SerializeField]
    private float currentSpeedUpgradeValue = 5;

    [SerializeField]
    private float playerSpeed = 10;



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
        Debug.Log(playerMoney);
    }

    // Check player money and upgrade fee for upgrade speed
    public void MayUpgradePlayerSpeed()
    {
        if (playerMoney >= currentUpgradeSpeedFee)
        {
            playerMoney -= currentUpgradeSpeedFee;
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
    }




}
