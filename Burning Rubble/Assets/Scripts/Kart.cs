using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Kart : MonoBehaviour, I_Damageable
{
    //This is a change to try to make it appear in Richard's branch
    [SerializeField] private int MAX_HP;
    private int hp;
    public KartMovement kartMovement;
    private RubbleMeter rubbleMeter;
    [SerializeField] private Image hpImage;
    [SerializeField] private TextMeshProUGUI hpText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (gameObject.GetComponent<KartMovement>() != null) kartMovement = gameObject.GetComponent<KartMovement>();
        else if (gameObject.GetComponentInChildren<KartMovement>() != null) kartMovement = gameObject.GetComponentInChildren<KartMovement>();
        else kartMovement = gameObject.AddComponent<KartMovement>();

        if (gameObject.GetComponent<RubbleMeter>() != null) rubbleMeter = gameObject.GetComponent<RubbleMeter>();
        else if (gameObject.GetComponentInChildren<RubbleMeter>() != null) rubbleMeter = gameObject.GetComponentInChildren<RubbleMeter>();
        else rubbleMeter = gameObject.AddComponent<RubbleMeter>();

        hp = MAX_HP;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) KartDeath();
        UpdateUI();
    }

    public bool Heal(int healAmt)
    {
        hp = Mathf.Clamp(hp + healAmt, 0, MAX_HP);
        UpdateUI();
        if (hp < MAX_HP) return false;
        return true;
    }

    private void KartDeath()
    {
        Debug.Log("Oh my god you got a game over what the noob");
    }

    void OnTriggerEnter(Collider collision)
    {
        PitStop pitStop = collision.gameObject.GetComponent<PitStop>();
        if (pitStop != null) StartCoroutine(pitStop.HealObject(this));
    }

    public void UpdateUI()
    {
        hpText.text = "Health: " + hp;
        hpImage.fillAmount = hp*1.0f / MAX_HP*1.0f;
    }
}
