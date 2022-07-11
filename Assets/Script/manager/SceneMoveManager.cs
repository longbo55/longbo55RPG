using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMoveManager : MonoBehaviour
{
    public void ChangeScene(int i)
    {
        if (i != -1)
        {
            SceneManager.LoadScene(i);
        }
    }
}
