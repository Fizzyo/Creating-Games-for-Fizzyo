using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float JumpForce = 1;
    public float PlayerSpeed = 20f;
    public float Gravity = 9.8f;
    private float maxJumpCount = 2;
    private float availableJumpCount = 2;

    private Rigidbody2D thisRigidbody;

    private AudioSource JumpEffect;

    // Use this for initialization
    void Start()
    {
        JumpEffect = this.GetComponent<AudioSource>();
        thisRigidbody = this.GetComponent<Rigidbody2D>();
        thisRigidbody.gravityScale = Gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.Instance.currentStage == ScoreManager.GameStage.LevelPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        //Vector2 newPosition = new Vector2(this.transform.position.x * PlayerSpeed * Time.deltaTime, this.transform.position.y);
        //thisRigidbody.MovePosition(newPosition);
        if (ScoreManager.Instance.currentStage == ScoreManager.GameStage.LevelPlaying)
        {
            float levelProgress = (float)ScoreManager.Instance.CurrentLevel.GoodBreathCount / (float)ScoreManager.Instance.CurrentLevel.GoodBreathMax;
            PlayerSpeed = Mathf.Lerp(ScoreManager.Instance.CurrentLevel.MinPlayerSpeed, ScoreManager.Instance.CurrentLevel.MaxPlayerSpeed, levelProgress);

            thisRigidbody.velocity = new Vector2(PlayerSpeed, thisRigidbody.velocity.y);
        }
        else
        {
            // stop the player if the level is not playing
            thisRigidbody.velocity = Vector2.zero;
            thisRigidbody.angularVelocity = 0f;
        }
    }

    void Jump()
    {
        if (availableJumpCount > 0)
        {
            availableJumpCount--;

            JumpEffect.Stop();
            JumpEffect.Play();
            thisRigidbody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
        }
    }

    public void Land()
    {
        if (thisRigidbody.velocity.y < .2f)
        {
            availableJumpCount = maxJumpCount;
        }
    }
}
