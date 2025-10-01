using System.Collections;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    //This is a change to try to make it appear in Richard's branch
    [SerializeField] private int healRate;

    public IEnumerator HealObject(Kart kart)
    {
        if (!kart.NeedsHealing()) yield return new WaitForEndOfFrame();
        else
        {
            kart.kartMovement.enabled = false;
            kart.StopKart();
            yield return new WaitForSeconds(1);
            while (!kart.Heal(healRate)) yield return new WaitForSeconds(1);
            kart.kartMovement.enabled = true;
        }
    }
}
