using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndCollision : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ScoreManager.Instance.EndLevel();
        }
    }
}
