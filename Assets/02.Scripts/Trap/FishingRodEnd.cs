using UnityEngine;

public class FishingRodEnd : MonoBehaviour
{
    private FishingRod fishingRod;

    void Start()
    {
        fishingRod = GetComponentInParent<FishingRod>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fishingRod.CatchPlayer(other.gameObject);
        }
    }
}
