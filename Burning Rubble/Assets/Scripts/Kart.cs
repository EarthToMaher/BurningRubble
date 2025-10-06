using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
public class Kart : MonoBehaviour, I_Damageable
{
    //This is a change to try to make it appear in Richard's branch
    [SerializeField] private int MAX_HP;
    [SerializeField] private int hp;
    public KartMovement kartMovement;
    private RubbleMeter rubbleMeter;
    [SerializeField] private Image hpImage;
    [SerializeField] private TextMeshProUGUI hpText;
    private bool invincible = false;
    [SerializeField] private float rubbleBoostIntensity;

    private InputAction rubbleAction;
    private InputAction restart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        transform.position = new Vector3(transform.position.x, 2.0001f, transform.position.z);
        if (gameObject.GetComponent<KartMovement>() != null) kartMovement = gameObject.GetComponent<KartMovement>();
        else if (gameObject.GetComponentInChildren<KartMovement>() != null) kartMovement = gameObject.GetComponentInChildren<KartMovement>();
        else kartMovement = gameObject.AddComponent<KartMovement>();

        if (gameObject.GetComponent<RubbleMeter>() != null) rubbleMeter = gameObject.GetComponent<RubbleMeter>();
        else if (gameObject.GetComponentInChildren<RubbleMeter>() != null) rubbleMeter = gameObject.GetComponentInChildren<RubbleMeter>();
        else rubbleMeter = gameObject.AddComponent<RubbleMeter>();

        hp = MAX_HP;

        rubbleAction = InputSystem.actions.FindAction("Rubble");
        restart = InputSystem.actions.FindAction("Reset");
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 2.0001f, transform.position.z);
        if (rubbleAction.WasPerformedThisFrame()) RubbleBoost();
        if (restart.WasPerformedThisFrame()) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TakeDamage(int dmg)
    {
        if (invincible) return;
        hp -= dmg;
        UpdateUI();
        if (hp <= 0) KartDeath();
    }

    public bool Heal(int healAmt)
    {
        hp = Mathf.Clamp(hp + healAmt, 0, MAX_HP);
        UpdateUI();
        if (hp < MAX_HP) return false;
        return true;
    }

    public void StopKart() { kartMovement.ResetVelocity(); }

    private void KartDeath()
    {
        invincible = true;
        kartMovement.ResetVelocity();
        GameObject _lapManager = GameObject.Find("LapManager");
        CheckpointDetection _checkDetect = this.GetComponent<CheckpointDetection>();
        Vector3 _respawnPoint = _lapManager.GetComponent<LapManager>().SetCheckpointPos(_checkDetect._currCheckpoint);
        this.transform.position = _respawnPoint;
        StartCoroutine(HealUponDeath());
    }

    private IEnumerator HealUponDeath()
    {
        yield return new WaitForEndOfFrame();
        hp = MAX_HP;
        invincible = false;
        UpdateUI();
    }

    public bool NeedsHealing()
    {
        if (hp < MAX_HP) return true;
        return false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (hp <= 0) return;
        PitStop pitStop = collision.gameObject.GetComponent<PitStop>();
        if (pitStop != null) StartCoroutine(pitStop.HealObject(this));
    }

    public void UpdateUI()
    {
        hpText.text = "Health: " + hp;
        hpImage.fillAmount = hp * 1.0f / MAX_HP * 1.0f;
    }

    public void LateUpdate()
    {
        if (hp <= 0)
        {
            Debug.Log("Kart death");
            KartDeath();
        }
    }

    public void RubbleBoost()
    {
        if (rubbleMeter.CanPerformRubbleAction() && kartMovement.CanMove())
        {
            StartCoroutine(BecomeInvincible(2f));
            StartCoroutine(kartMovement.RubbleBoost(rubbleBoostIntensity));
            rubbleMeter.UseRubble();
        }
    }

    public IEnumerator BecomeInvincible(float seconds)
    {
        if (invincible) StopCoroutine("BecomeInvincible");
        invincible = true;
        yield return new WaitForSeconds(seconds);
        invincible = false;
    }
}
