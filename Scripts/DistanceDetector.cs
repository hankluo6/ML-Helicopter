using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDetector : MonoBehaviour
{
    public float[] distance = new float[5];

    private float timer = 0;
    private float scanTime = 0.5f;
    private float maxDistance = 100f;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            distance[i] = Mathf.Infinity;
        }
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        // Left, Right, Top, Back
        Vector3[] direction = new Vector3[5] { -transform.right, transform.forward, -transform.forward, -transform.up, transform.up };
        Debug.DrawLine(transform.position, transform.position + direction[2] * 100, Color.red);
        if (timer >= scanTime)
        {
            for (int i = 0; i < 5; ++i)
            {
                Ray ray = new Ray(transform.position, direction[i] * 100);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxDistance, ~LayerMask.GetMask("Aircraft")))
                {
                    distance[i] = Mathf.Round(hit.distance);
                }
                else
                {
                    distance[i] = Mathf.Infinity;
                }
            }
            timer = 0;
        }
    }
}
