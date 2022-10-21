using UnityEngine;

public class EssenceController : MonoBehaviour
{
    [SerializeField] private int _healthIncreasement;
    [SerializeField] private GameObject _essenceParent;
    [SerializeField] private bool _dontUsePool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            GlobalEventManager.ChangePlayerHealth(_healthIncreasement);
            if (!_dontUsePool)
            {
                transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
                PoolManager.putGameObjectToPool(_essenceParent);
            }
        }
    }
    
}
