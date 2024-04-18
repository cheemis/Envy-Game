using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSwitch : MonoBehaviour
{
    


    public void LoadBuyScreen()
    {
        SceneManager.LoadSceneAsync(2);
    }




}
