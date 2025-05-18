using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BallFollower : MonoBehaviour
{
    public Transform target;
    
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void Update()
    {
        transform.DOMove(target.position + offset, 0.2f);
    }
}
