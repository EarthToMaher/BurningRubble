using UnityEngine;
using System.Collections.Generic;

public class ReenableManager : MonoBehaviour
{
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
    [Tooltip("How often the reenable check runs in seconds")]
    [SerializeField] private float checkRate = 3f;
    [Tooltip("The maximum # of items it will respawn. Anything less than 1 will respawn all")]
    [SerializeField] private int maxRespawnAmount = 5;
    [Tooltip("The minimum amount of time an object must wait before being respawned")]
    [SerializeField] private float minWaitTime = 5f;
    private readonly List<Respawnable> pending = new();

    private float nextCheckTime;

    private void Start()
    {
        InvokeRepeating("ReenableClock", 0, checkRate);
    }

    public void ReenableClock()
    {
        if (pending.Count == 0) return;
        if (Time.time < nextCheckTime) return;

        // Re-enable all that are ready
        float now = Time.time;
        int amountEnabled = 1; //Set to 1 so that way I can just do a greater than check instead of greater than or equal to
        for (int i = pending.Count - 1; i >= 0; i--)
        {
            if (pending[i].eligibleTime <= now)
            {
                if (pending[i].obj) pending[i].obj.RepairMe();
                pending.RemoveAt(i);
                if (maxRespawnAmount < 1) continue;
                amountEnabled++;
                if (amountEnabled > maxRespawnAmount) break;
            }
        }

        // Schedule next check (find earliest remaining time)
        nextCheckTime = float.MaxValue;
        for (int i = 0; i < pending.Count; i++)
        {
            if (pending[i].eligibleTime < nextCheckTime)
                nextCheckTime = pending[i].eligibleTime;
        }
    }

    public void AddToBatch(DestructibleBlock obj)
    {
        pending.Add(new Respawnable(obj, Time.time + minWaitTime));
        if (pending.Count == 1 || Time.time + minWaitTime < nextCheckTime)
            nextCheckTime = Time.time + minWaitTime;
    }

    [ContextMenu("DestroyAll")]
    public void DestroyAll()
    {
        DestructibleBlock[] blocks = FindObjectsByType<DestructibleBlock>(FindObjectsSortMode.None);
        foreach (DestructibleBlock block in blocks) block.DestroyMe(this.gameObject,this.gameObject);
    }
}
