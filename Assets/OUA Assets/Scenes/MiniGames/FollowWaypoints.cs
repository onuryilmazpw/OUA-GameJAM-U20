using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    public float hiz = 5f;
    public List<GameObject> waypoints;

    private int currentWP = 0;

    void Start()
    {
        int uzunluk = GameObject.Find("Waypoints").transform.childCount;
        for (int i = 0; i < uzunluk; i++)
        {
            waypoints.Add(GameObject.Find("Waypoints").transform.GetChild(i).gameObject);
        }
        //Her seferinde waypoinlerin sırasını değiştiriyoruz
        waypoints = waypoints.OrderBy(a => System.Guid.NewGuid()).ToList();
    }

    void Update()
    {
        if (Vector2.Distance(this.transform.position, waypoints[currentWP].transform.position) < 0.1f)
        {
            currentWP = (currentWP + 1) % waypoints.Count;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWP].transform.position, hiz * Time.deltaTime);
    }
}
