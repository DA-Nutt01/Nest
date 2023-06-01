using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth; 
    private MeshRenderer meshRenderer;
    [Tooltip("The original material color on this gameobject")]
    private Color baseMaterial;

    private void Awake()
    {
        currentHealth = maxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) baseMaterial = meshRenderer.material.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //StopCoroutine(DamageRenderer());
        StartCoroutine(DamageRenderer());
        if (currentHealth <= 0) Die();
    }

    // Makes the renderer appear red for a tenth of a second when it gets damaged.
    private IEnumerator DamageRenderer() 
    {
        if (meshRenderer != null) meshRenderer.material.color = Color.red;     // Turn the renderer on this gameobject red
        yield return new WaitForSeconds(0.1f);                                   // Wait a tenth of a second
        if (meshRenderer != null) meshRenderer.material.color = baseMaterial; // Return the renderer color back to normal
        // Add Sound effect here
    }

    private void Die()
    {
        // Handle the death logic here
        // For example, destroy the game object or disable it
        Debug.Log($"{gameObject.name} has Died");
        DestroyImmediate(gameObject);
    }
}
