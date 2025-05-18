using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankController : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float xLeft, xRight;

    private float yLeft, yRight;
    
    public float plankMoveSpeed;

    public Transform ball;

    public float gravity;

    private float speedX;
    private float coordinationX;

    public float bounceRatio;
    
    private void Start()
    {
        yLeft = 0f;
        yRight = 0f;

        speedX = 0f;
        coordinationX = 0f;
    }

    private void Update()
    {
        UpdateSpeed();
        
        UpdateBall();
        UpdatePlank();

        if (Input.GetKey(KeyCode.W))
        {
            yLeft += plankMoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            yLeft -= plankMoveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.I))
        {
            yRight += plankMoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            yRight -= plankMoveSpeed * Time.deltaTime;
        }
        
        
    }

    private void UpdateSpeed()
    {
        float yDiff = yRight - yLeft;
        float plankLength = new Vector2(xLeft - xRight, yLeft - yRight).magnitude;

        float cosA = yDiff / plankLength;
        float sinA = (xRight - xLeft) / plankLength;

        float acceleration = gravity * cosA * sinA;

        speedX -= acceleration * Time.deltaTime;
        
        
        // bounce
        if ((coordinationX <= xLeft && speedX < 0f) 
            || (coordinationX >= xRight && speedX > 0f))
        {
            speedX = -speedX * bounceRatio;
        }
        
    }
    
    private void UpdatePlank()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(xLeft, yLeft, 0f));
        lineRenderer.SetPosition(1, new Vector3(xRight, yRight, 0f));
    }

    private void UpdateBall()
    {
        
        coordinationX += speedX * Time.deltaTime;
        
        ball.transform.position = GetBallWorldPos();
    }

    private Vector3 GetBallWorldPos()
    {
        var midPos = new Vector2((xLeft + xRight) / 2, (yLeft + yRight) / 2);

        var rightNormVec = new Vector2(xRight - xLeft, yRight - yLeft).normalized;
        
        float plankLength = new Vector2(xLeft - xRight, yLeft - yRight).magnitude;
        float sinA = (xRight - xLeft) / plankLength;

        var coordinationAxis = coordinationX / sinA;
        var pos2d = midPos + rightNormVec * coordinationAxis;

        return new Vector3(pos2d.x, pos2d.y, 0f);
    }

}
