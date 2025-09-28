using UnityEngine;
using TMPro;
using System;
public class RaceTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI raceTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (raceTime == null) raceTime = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int minutes = Mathf.FloorToInt(Time.time) / 60;
        int seconds = Mathf.FloorToInt(Time.time)%60;
        int milliseconds = Mathf.FloorToInt((Time.time-Mathf.FloorToInt(Time.time)) * Mathf.Pow(10f,3));
        String timeString = String.Format("{0}:{1}.{2}", minutes, seconds, milliseconds);
        raceTime.text = timeString;
    }
}
