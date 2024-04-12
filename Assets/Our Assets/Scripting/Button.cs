using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    [SerializeField]
    private Button speedUp;
    [SerializeField]
    private Button nextRound;

    [SerializeField]
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        // find game manager
        GameManager g = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
        speedUp.onClick.AddListener(
            g.MayUpgradePlayerSpeed
            );
        nextRound.onClick.AddListener(
            g.NextRoundGame
            );

        if (tmp != null)
        {
            if(g.GetPlayerWon())
            {
                tmp.text = "Player 1 has won!\n\nIncrease Speed: 25 points";
            }
            else
            {
                tmp.text = "Player 2 has won!\n\nIncrease Speed: 25 points";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
