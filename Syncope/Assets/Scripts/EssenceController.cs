using UnityEngine;

public class EssenceController : MonoBehaviour
{
    [SerializeField] private int healthIncreasement = 2;
    [SerializeField] private GameObject essenceParent;
    [SerializeField] private bool dontUsePool = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            GlobalEventManager.ChangePlayerHealth(healthIncreasement);
            if (!dontUsePool)
            {
                transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
                PoolManager.putGameObjectToPool(essenceParent);
            }
        }
    }
    
}
