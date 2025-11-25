using UnityEngine;

public class Utility_DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float timeUntilDestroy  = 5;

    private void Awake()
    {
        Destroy  (gameObject, timeUntilDestroy);
    }
}
