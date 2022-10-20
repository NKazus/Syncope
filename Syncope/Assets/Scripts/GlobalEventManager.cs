//fixed
using UnityEngine.Events;

public class GlobalEventManager
{

    public static UnityEvent<int> HealthChangeEvent = new UnityEvent<int>();
    public static UnityEvent<bool> HealthAffectingEvent = new UnityEvent<bool>();
    public static UnityEvent RestartSceneEvent = new UnityEvent();

    public static void ChangePlayerHealth(int healthAmount)
    {
        HealthChangeEvent.Invoke(healthAmount);
    }

    public static void AffectPlayerHealth(bool healthCoefficient)
    {
        HealthAffectingEvent.Invoke(healthCoefficient);
    }

    public static void RestartScene()
    {
        RestartSceneEvent.Invoke();
    }

}
