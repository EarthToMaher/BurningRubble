using UnityEngine;

public class Kart : MonoBehaviour, I_Damageable
{
    [SerializeField] private int MAX_HP;
    private int hp;
    private KartMovement kartMovement;
    private RubbleMeter rubbleMeter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        /*if (gameObject.GetComponent<KartMovement>() != null) kartMovement = gameObject.GetComponent<KartMovement>();
        else if (gameObject.GetComponentInChildren<KartMovement>() != null) kartMovement = gameObject.GetComponentInChildren<KartMovement>();
        else kartMovement = gameObject.AddComponent<KartMovement>();*/

        if (gameObject.GetComponent<RubbleMeter>() != null) rubbleMeter = gameObject.GetComponent<RubbleMeter>();
        else if (gameObject.GetComponentInChildren<RubbleMeter>() != null) rubbleMeter = gameObject.GetComponentInChildren<RubbleMeter>();
        else rubbleMeter = gameObject.AddComponent<RubbleMeter>();

        hp = MAX_HP;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) KartDeath();
    }

    private void KartDeath()
    {
        Debug.Log("Oh my god you got a game over what the noob");
    }
}
