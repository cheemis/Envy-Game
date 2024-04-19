using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Button speedUpButton;
    [SerializeField]
    private Button knockBackButton;
    [SerializeField]
    private Button nextRoundButton;

    //[SerializeField]
    //private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        // find game manager
        GameManager g = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();

        speedUpButton.onClick.AddListener(
            g.MayUpgradePlayerSpeed
            );

        knockBackButton.onClick.AddListener(
            g.MayUpgradePlayerKnockBack
            );

        nextRoundButton.onClick.AddListener(
            g.NextRoundGame
            );
    }
}
