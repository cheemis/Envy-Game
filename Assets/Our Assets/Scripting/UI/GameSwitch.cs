using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSwitch : MonoBehaviour
{
    public void LoadBuyScreen()
    {
        Debug.Log("here");
        if(GameManager.Instance != null)
        {
            GameManager.Instance.LoadUpgradeScene();
        }
        else
        {
            Debug.Log("LOADED INCORRECTLY");
            SceneManager.LoadSceneAsync(2);
        }
    }




}
