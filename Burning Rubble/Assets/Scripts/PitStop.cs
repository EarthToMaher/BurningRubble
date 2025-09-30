using System.Collections;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    [SerializeField] private int healRate;

    public IEnumerator HealObject(Kart kart)
    {
        kart.kartMovement.enabled = false;
        yield return new WaitForSeconds(1);
        while (!kart.Heal(healRate)) yield return new WaitForSeconds(1);
        kart.kartMovement.enabled = true;
    }
}
