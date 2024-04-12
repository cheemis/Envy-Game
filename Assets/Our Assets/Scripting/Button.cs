using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    [SerializeField]
    private Button speedUp;
    [SerializeField]
    private Button nextRound;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
