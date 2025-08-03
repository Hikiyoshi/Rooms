using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpikeHandler : MonoBehaviour
{
    [SerializeField] private float spikeSpeed = 1f;
    [SerializeField] private bool isFacingRight;

    private bool startMove = false;
    private int direction = 1;

    private void Start()
    {
        direction = isFacingRight ? -1 : 1;

        StageManager.Instance.OnStartStage += GameManager_OnStartStage;
        StageManager.Instance.OnEndMove += GameManager_OnEndMove;
    }

    private void FixedUpdate()
    {
        if (!startMove)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        transform.position =
                Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z + direction * 4),
                    spikeSpeed * Time.deltaTime);
    }

    public void StartMove()
    {
        startMove = true;
    }

    private void GameManager_OnStartStage()
    {
        StartMove();
    }

    private void GameManager_OnEndMove()
    {
        gameObject.GetComponent<SpikeHandler>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            GameManager.Instance.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            GameManager.Instance.GameOver();
        }

        if (other.TryGetComponent(out SpikeHandler spikeHandler))
        {
            gameObject.GetComponent<SpikeHandler>().enabled = false;
            spikeHandler.enabled = false;
            
            StageManager.Instance.EndMoveStage2();
        }
    }
}
