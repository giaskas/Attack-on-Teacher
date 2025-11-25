using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    public Light fireLight;
    public float minIntensity = 2f;
    public float maxIntensity = 4f;
    public float speed = 5f;

    float targetIntensity;

    void Start()
    {
        if (fireLight == null)
            fireLight = GetComponent<Light>();

        targetIntensity = fireLight.intensity;
    }

    void Update()
    {
        // Cambia el objetivo de intensidad de forma aleatoria
        if (Mathf.Abs(fireLight.intensity - targetIntensity) < 0.05f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        // Interpola suavemente hacia el nuevo valor
        fireLight.intensity = Mathf.Lerp(
            fireLight.intensity,
            targetIntensity,
            Time.deltaTime * speed
        );
    }
}
