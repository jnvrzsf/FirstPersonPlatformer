using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFromCamera : MonoBehaviour
{
    private Camera cam;
    private Vector3 pos;
    [HideInInspector] public bool hitSomething;
    public RaycastHit hitInfo;

    void Start()
    {
        cam = Camera.main;
        pos = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);
    }

    private void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(pos);
        hitSomething = Physics.Raycast(ray, out hitInfo);
    }
}
