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

    [SerializeField] private float minWaitTime = 5f;
    private readonly List<Respawnable> pending = new();

    private float nextCheckTime;

    void Update()
    {
        if (pending.Count == 0) return;
        if (Time.time < nextCheckTime) return;

        // Re-enable all that are ready
        float now = Time.time;
        for (int i = pending.Count - 1; i >= 0; i--)
        {
            if (pending[i].eligibleTime <= now)
            {
                if (pending[i].obj) pending[i].obj.RepairMe();
                pending.RemoveAt(i);
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
}
