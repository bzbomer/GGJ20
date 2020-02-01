﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject brokenRoad;
    public GameObject road;
    public bool isPressed = false;
    public bool isFinished = false;

    [SerializeField, Range(0, 5)] float buildTimeLeft = 2f;
    float oldBuildTimeLeft;

    [SerializeField, Range(0, 5)] float tickTimeLeft = 2f;
    float oldTickTimeLeft;

    [SerializeField, Range(0, 2)] int tickNumber = 1;
    private int currentTickNumber = 0; 

    public MeshRenderer roadSprite;
    public Material[] materials;

    private void Start() {
        RepairRoad();
        roadSprite.material = materials[currentTickNumber];
        oldBuildTimeLeft = buildTimeLeft;
        oldTickTimeLeft = tickTimeLeft;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform == brokenRoad.transform)
                {
                    if(currentTickNumber < tickNumber)
                        UpdateRepairSprite(true);
                    else
                    {
                        isPressed = true;
                        RepairRoad();
                    }
                    
                }
            }
        }

        if(isPressed)
        {
            ReduceBuildTimer();
        }
        ReduceTickTimer();
    }

    private void UpdateRepairSprite(bool isPressed)
    {
        if(isPressed)
            currentTickNumber++;
        else
        {
            if(isFinished)
                BrokeRoad();
            currentTickNumber = 0;
        }
        roadSprite.material = materials[currentTickNumber];
        ReverseTickTimer();
    }

    private void ReduceTickTimer(){
        tickTimeLeft -= Time.deltaTime;
        if(tickTimeLeft <= 0)
        {
            UpdateRepairSprite(false);
            ReverseTickTimer();
        }
    }

    private void ReverseTickTimer(){
        tickTimeLeft = oldTickTimeLeft;
    }

    private void ReduceBuildTimer()
    {
        buildTimeLeft -= Time.deltaTime;
        if (buildTimeLeft < 0)
        {
            isFinished = true;
        }
    }

    private void ReverseBuildTimer()
    {
        buildTimeLeft = oldBuildTimeLeft;
    }

    public void RepairRoad()
    {
        brokenRoad.SetActive(false);
        road.SetActive(true);
    }

    public void BrokeRoad()
    {
        brokenRoad.SetActive(true);
        road.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isFinished && other.tag == "Player")
        {
            print("helal bro");
        }
        else if(!isFinished && other.tag == "Player")
        {
            print("öldün gral");
        }
    }
}