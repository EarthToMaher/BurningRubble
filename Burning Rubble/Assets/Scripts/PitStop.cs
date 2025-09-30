using System.Collections;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    //This is a change to try to make it appear in Richard's branch
    [SerializeField] private int healRate;

    public IEnumerator HealObject(Kart kart)
    {
        kart.kartMovement.enabled = false;
        yield return new WaitForSeconds(1);
        while (!kart.Heal(healRate)) yield return new WaitForSeconds(1);
        kart.kartMovement.enabled = true;
    }
}
