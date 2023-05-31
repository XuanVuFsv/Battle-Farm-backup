using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int lineSegments = 60;
    [SerializeField, Min(1)]
    private float timeOfTheFlight = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTrajectoryLine(Vector3 starPoint, Vector3 starVelocity)
    {
        float timeStep = timeOfTheFlight / lineSegments;

        Vector3[] lineRendererPoints = CalculateTracjectoryLine(starPoint, starVelocity, timeStep);

        lineRenderer.positionCount = lineSegments;
        lineRenderer.SetPositions(lineRendererPoints);
    }

    private Vector3[] CalculateTracjectoryLine(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];

        lineRendererPoints[0] = startPoint;

        for (int i = 0; i < lineSegments; i++)
        {
            float timeOffset = timeStep * i;

            Vector3 progressBeforeFravity = startPoint * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPosition = startPoint + progressBeforeFravity - gravityOffset;

            Debug.Log(progressBeforeFravity + " " + gravityOffset + " " + newPosition);
        }

        return lineRendererPoints;
    }
}
