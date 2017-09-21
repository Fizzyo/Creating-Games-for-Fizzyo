using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public GameObject CoinParticlesPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ScoreManager.Instance.GetCoin();
        GameObject particles = Instantiate(CoinParticlesPrefab);
        particles.transform.position = this.transform.position;
        Destroy(particles, 1f);
        Destroy(this.gameObject);
    }
}
