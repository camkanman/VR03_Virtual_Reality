using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;

    [Tooltip("Durasi animasi transisi (dari full ke empty)")]
    public float fillDuration = 0.5f;

    private float targetHealth;
    private Coroutine healthChangeCoroutine;

    void Start()
    {
        targetHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void Attack()
    {
        targetHealth -= 10f;
        if (targetHealth < 0f) targetHealth = 0f;
        StartHealthChange(targetHealth);
    }

    public void Heal()
    {
        targetHealth += 10f;
        if (targetHealth > maxHealth) targetHealth = maxHealth;
        StartHealthChange(targetHealth);
    }

    void StartHealthChange(float newHealth)
    {
        // Jika masih ada coroutine animasi lama, hentikan dulu
        if (healthChangeCoroutine != null)
        {
            StopCoroutine(healthChangeCoroutine);
        }

        // Mulai coroutine baru
        healthChangeCoroutine = StartCoroutine(AnimateHealthChange(newHealth));
    }

    IEnumerator AnimateHealthChange(float newHealth)
    {
        // Mulai dari posisi slider saat ini (bukan currentHealth terpisah)
        float startValue = healthSlider.value;
        float elapsed = 0f;

        while (elapsed < fillDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fillDuration);

            // Lerp dari startValue ke newHealth
            healthSlider.value = Mathf.Lerp(startValue, newHealth, t);
            yield return null; 
        }

        // Pastikan persis di nilai akhir
        healthSlider.value = newHealth;
    }
}
