/*
Script for timing the race
Written By: Matthew Maher
Last Modified: 9/28/2025
*/

using UnityEngine;
using TMPro;
using System;
public class RaceTimer : MonoBehaviour
{
    [Tooltip("The text element we want to modify")]
    [SerializeField] private TextMeshProUGUI raceTime;

    //Bool to check if the race is complete. Hidden in inspector as it shouldn't be modified from there
    [HideInInspector] public bool raceComplete = false;
    private float time = 0;

    void Start()
    {
        
        //If we do not have a race timer, try to get one on this object
        if (raceTime == null) raceTime = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //Don't update timer if the race is complete
        if (raceComplete) return;


        //Get the number of minutes, seconds, and milliseconds that the scene has been running
        int minutes = Mathf.FloorToInt(time) / 60;
        int seconds = Mathf.FloorToInt(time)%60;
        int milliseconds = Mathf.FloorToInt((time-Mathf.FloorToInt(time)) * Mathf.Pow(10f,3));

        //Format our time as a string and change the text
        String timeString = String.Format("{0}:{1}.{2}", minutes, seconds, milliseconds);
        raceTime.text = timeString;
    }
}
