using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDeath : MonoBehaviour
{
    /*[SerializeField] ObjectPlsHelp objectPlsHelp;
    public float maxHealth = 100f;
    public float regenRate = 5f;
    public bool isDead = false;
    [Header("Damage/regen")]
    public float damageGracePeriod = 1.0f;
    private float lastDamageTime = -999f;
    private float prevHealth = -1f;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (objectPlsHelp == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                objectPlsHelp = player.GetComponent<ObjectPlsHelp>();
            }
        }
        if (objectPlsHelp != null)
        {
            prevHealth = objectPlsHelp.playerHealth;
        }
        objectPlsHelp.playerHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectPlsHelp == null) return;
        objectPlsHelp.playerHealth = Mathf.Clamp(objectPlsHelp.playerHealth, 0f, maxHealth);
        if (!isDead && objectPlsHelp.playerHealth <= 0f)
        {
            isDead = true;
            objectPlsHelp.canMove = false;
            objectPlsHelp.canThrow = false;
            StartCoroutine(FadeOut());
        }
        if (prevHealth < 0f) prevHealth = objectPlsHelp.playerHealth;
        if (objectPlsHelp.playerHealth < prevHealth)
        {
            lastDamageTime = Time.time;
        }
        if (!isDead && objectPlsHelp.playerHealth < maxHealth && Time.time - lastDamageTime > damageGracePeriod)
        {
            objectPlsHelp.playerHealth += regenRate * Time.deltaTime;
            objectPlsHelp.playerHealth = Mathf.Min(objectPlsHelp.playerHealth, maxHealth);
        }

        prevHealth = objectPlsHelp.playerHealth;
    }
    IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
        Object.FindFirstObjectByType<respawnManager>().RespawnPlayer();
    }*/
}
