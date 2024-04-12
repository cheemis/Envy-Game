using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    [SerializeField]
    private Button thisB;
    // Start is called before the first frame update
    void Start()
    {
        // find game manager
        GameManager g = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
        thisB.onClick.AddListener(
            g.MayUpgradePlayerSpeed
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
