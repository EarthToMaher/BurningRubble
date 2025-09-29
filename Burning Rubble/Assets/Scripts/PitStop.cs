using System.Collections;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    [SerializeField] private int healRate;

    public IEnumerator HealObject(Kart kart)
    {
        yield return new WaitForSeconds(1);
        while (!kart.Heal(healRate)) yield return new WaitForSeconds(1);
    }
}
