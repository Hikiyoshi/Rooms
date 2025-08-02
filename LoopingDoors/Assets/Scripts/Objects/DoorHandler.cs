using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameManager.Instance.NextStage();
        }
    }
}
