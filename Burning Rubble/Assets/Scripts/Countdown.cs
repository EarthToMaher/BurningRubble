using UnityEngine;
using System.Collections;
using TMPro;

public class Countdown : MonoBehaviour
{
    private bool isActive;
    private float intensity;
    private float count;
    private bool hasBoosted;
    private bool isCounting;
    [SerializeField] private KartMovement move;
    [SerializeField] private TextMeshProUGUI countText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = true;
        hasBoosted = false;
        isCounting = false;
        count = 3;

        //-1 value used to determine whether intensity has been set already
        intensity = -1;

        StartCoroutine(WaitToStart());
    }

    private void Update()
    {
        if(isCounting)
        {
            if (count > 0)
            {
                count -= Time.deltaTime;
            }

            if (move.GetAccelerateValue() != 0)
            {
                if (count > 2) { intensity = 0; }
                else if (count >= 1.7f && intensity == -1) { intensity = 2; }
                else if (count >= 1.4f && intensity == -1) { intensity = 1; }
                else if (count > 1 && intensity == -1) { intensity = 0.5f; }
                else if (count > 0 && intensity == -1) { intensity = 0; }
            }
            else if (count > 0 && intensity != -1) { intensity = -1; }

            if (count <= 0)
            {
                isActive = false;
                if (!hasBoosted)
                {
                    if(intensity > 0) 
                    {
                        Debug.Log("Calling coroutine...");
                        StartCoroutine(move.StartBoost(intensity));
                        Debug.Log("Boost with intensity: " + intensity); 
                    }
                    hasBoosted = true;
                }
            }

            if (count.ToString("F0").Equals("0"))
            {
                countText.SetText("GO!");
                //StartCoroutine(RemoveCountdown());
            }
            else { countText.SetText(count.ToString("F0")); }


            //DEBUG: show countdown with 1 decimal place
            //Debug.Log("Count: " + count.ToString("F1"));
        }
    }

    public bool GetActive()
    {
        return isActive;
    }

    private IEnumerator RemoveCountdown()
    {
        yield return new WaitForSeconds(0.7f);
        this.gameObject.SetActive(false);
    }
    
    private IEnumerator WaitToStart()
    {
        //waiting for kart to be on the ground
        yield return new WaitForSeconds(3f);
        isCounting = true;
    }
}
