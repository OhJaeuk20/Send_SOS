using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 20f;
    public float upwardThreshold = 0.8f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                // Calculate the dot product of the normal of the pad and the upward vector
                float dotProduct = Vector3.Dot(transform.up, Vector3.up);
                if (dotProduct > upwardThreshold)
                {
                    playerMove.BounceFromPad(bounceForce);
                }
            }
        }
    }
}
