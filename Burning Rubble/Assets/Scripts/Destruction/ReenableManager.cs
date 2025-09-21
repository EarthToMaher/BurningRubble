using UnityEngine;
using System.Collections.Generic;
/*
Written By: Matthew Maher
Last Updated: 9/21/2025
ChatGPT was used to discuss ways of optimizing
*/
public class ReenableManager : MonoBehaviour
{
    //Private class, containing our DestructibleBlock and the earliest time it can respawn
    private class Respawnable
    {
        public DestructibleBlock obj;
        public float eligibleTime;
        public Respawnable(DestructibleBlock o, float t)
        {
            obj = o;
            eligibleTime = t;
        }
    }

    [Header("Optimization Settings")]
    [Tooltip("How often the reenable check runs in seconds")]
    [SerializeField] private float checkRate = 3f;
    [Tooltip("The maximum # of items it will respawn. Anything less than 1 will respawn all")]
    [SerializeField] private int maxRespawnAmount = 5;
    [Tooltip("The minimum amount of time an object must wait before being respawned")]
    [SerializeField] private float minWaitTime = 5f;

    //Instance variables, handled dynamically
    private readonly List<Respawnable> pending = new();
    private float nextCheckTime;

    //Starts our ReenableClock with our specified parameters
    private void Start()
    {
        InvokeRepeating("ReenableClock", 0, checkRate);
    }

    /// <summary>
    /// Function to check if anything needs to be reenabled
    /// </summary>
    public void ReenableClock()
    {
        if (pending.Count == 0) return; //Stop running if the list is empty
        if (Time.time < nextCheckTime) return; //Stop running if there is nothing to be reenabled

        // Re-enable all that are ready
        float now = Time.time;
        int amountEnabled = 1; //Set to 1 so that way I can just do a greater than check instead of greater than or equal to
        for (int i = pending.Count - 1; i >= 0; i--)
        {
            if (pending[i].eligibleTime <= now)
            {
                if (pending[i].obj) pending[i].obj.RepairMe();
                pending.RemoveAt(i);
                if (maxRespawnAmount < 1) continue; //If our maxRespawnAmount is less than 1, reenable as many as needed
                amountEnabled++;
                if (amountEnabled > maxRespawnAmount) break;
            }
            else break; //Since these are all waiting the same time, if we reach one in the list that hasn't reached its wait time yet we can break
        }

        if (pending.Count == 0) nextCheckTime = float.MaxValue; //If there is nothing in the list, sets our nextCheck to be as late as possible
        else nextCheckTime = pending[0].eligibleTime; //Updates our next check time

        /*
        // Schedule next check (find earliest remaining time) might be worth removing, as this does make this run as O(n^2)
        nextCheckTime = float.MaxValue;
        for (int i = 0; i < pending.Count; i++)
        {
            if (pending[i].eligibleTime < nextCheckTime)
                nextCheckTime = pending[i].eligibleTime;
        }*/
    }

    /// <summary>
    /// Adds a new Respawnable to the list with the specified DestructibleBlock object
    /// </summary>
    /// <param name="obj">DestructibleBlock that got destroyed</param>
    public void AddToBatch(DestructibleBlock obj)
    {
        pending.Add(new Respawnable(obj, Time.time + minWaitTime)); //Adds the Respawnable with obj and the time it can reenable
        if (pending.Count == 1 || Time.time + minWaitTime < nextCheckTime)
            nextCheckTime = Time.time + minWaitTime; //Updates the next check time relative to this object if its the only object or if its reenable time is less than the next check time
    }

    //Debug function that destroys all DestructibleBlocks in the scene
    [ContextMenu("DestroyAll")]
    public void DestroyAll()
    {
        DestructibleBlock[] blocks = FindObjectsByType<DestructibleBlock>(FindObjectsSortMode.None);
        foreach (DestructibleBlock block in blocks) block.DestroyMe(this.gameObject, this.gameObject);
    }
}
