using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingEffect : MonoBehaviour
{
    private float[] screenEdges;                        // edges of the screen in world coordinates [left, right, bottom, top]
    [SerializeField] private float edgePad;

    void Start()
    {
        screenEdges = GameManager.Instance.screenEdges;
    }

    void Update()
    {
        ScreenWrapper();
    }

    //
    void ScreenWrapper()
    {
        if (transform.position.x < (screenEdges[0] - edgePad))
        {
            transform.position = new Vector3(screenEdges[1], transform.position.y, transform.position.z);
        }
        else if (transform.position.x > (screenEdges[1] + edgePad))
        {
            transform.position = new Vector3(screenEdges[0], transform.position.y, transform.position.z);
        }

        if (transform.position.y < (screenEdges[2] - edgePad))
        {
            transform.position = new Vector3(transform.position.x, screenEdges[3], transform.position.z);
        }
        else if (transform.position.y > (screenEdges[3] + edgePad))
        {
            transform.position = new Vector3(transform.position.x, screenEdges[2], transform.position.z);
        }
    }

}
