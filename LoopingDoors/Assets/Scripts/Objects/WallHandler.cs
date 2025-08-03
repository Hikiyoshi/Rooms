using System;
using System.Collections;
using UnityEngine;

public class WallHandler : MonoBehaviour
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

    private void GameManager_OnStartStage()
    {
        StartMove();
    }

    private void GameManager_OnEndMove()
    {
        gameObject.GetComponent<WallHandler>().enabled = false;
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
}
