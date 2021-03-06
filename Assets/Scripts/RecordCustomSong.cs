﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;

public class RecordCustomSong : MonoBehaviour
{
    // MAT
    private MatInputController Mat = new MatInputController();
    public GameObject[] tiles;

    // CSV
    private string csvName = "test.csv";
    public string song = "TheBigEmptySong";

    // Moves
    List<CustomMove> moves = new List<CustomMove>();

    // UI Input    
    public SteamVR_Action_Boolean RecordTrigger;
    public SteamVR_Action_Boolean StopRecording;
    public SteamVR_Input_Sources handType;
    private bool TriggerOnOff = false;

    // Timings
    private float prevTime = 0f;
    private float currTime = 0f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Menu").
        
        // Start Mat & Timer
        Mat.Start();
        timer = 0f;
        // INPUT
        RecordTrigger.AddOnStateDownListener(TriggerDown, handType);
        RecordTrigger.AddOnStateUpListener(TriggerUp, handType);
        StopRecording.AddOnStateDownListener(StopTrigger, handType);
    }

    // Update is called once per frame
    void Update()
    {
        // Update Mat & Timer
        Mat.Update();
        ShowFeet();
        timer += Time.deltaTime;
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {        
        if (TriggerOnOff)
            TriggerOnOff = false;
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {        
        if (!TriggerOnOff)
        {
            TriggerOnOff = true;
            //  Add new move
            currTime = timer;            
            moves.Add(new CustomMove(prevTime, currTime, LookAtMove()));
            prevTime = currTime;                      
        }           
    }

    public void StopTrigger(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        WriteCsv();
    }

    private string LookAtMove()
    {
        string move = "";

        List<bool> FloorTiles = new List<bool>();

        FloorTiles.Add(Mat.LeftForward);
        FloorTiles.Add(Mat.Forward);
        FloorTiles.Add(Mat.RightForward);
        FloorTiles.Add(Mat.Left);
        FloorTiles.Add(Mat.Center);
        FloorTiles.Add(Mat.Right);
        FloorTiles.Add(Mat.LeftBackward);
        FloorTiles.Add(Mat.Backward);
        FloorTiles.Add(Mat.RightBackward);

        foreach (var state in FloorTiles)
        {
            if (state == true)
                move += "1";
            else
                move += "0";
        }

        return move;
    }

    private void ShowFeet() //visually show where your feet are
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetActive(false);
        }
        if (Mat.LeftForward) tiles[0].SetActive(true);
        if (Mat.Forward) tiles[1].SetActive(true);
        if (Mat.RightForward) tiles[2].SetActive(true);
        if (Mat.Left) tiles[3].SetActive(true);
        if (Mat.Center) tiles[4].SetActive(true);
        if (Mat.Right) tiles[5].SetActive(true);
        if (Mat.LeftBackward) tiles[6].SetActive(true);
        if (Mat.Backward) tiles[7].SetActive(true);
        if (Mat.RightBackward) tiles[8].SetActive(true);

        Debug.Log("MAT=" + Mat.Forward + " = " + tiles[1].active.ToString());
    }

    private void WriteCsv()
    {        
        csvName = song;

        StreamWriter writer = new StreamWriter(@"songScripts\" + csvName + ".csv");

        writer.WriteLine(song + ";;"); //First line = songname

        foreach (var move in moves)
        {
            writer.WriteLine(move.beginTime + ";" + move.endTime + ";" + move.move); // moves
        }

        writer.Close();

        Mat.CloseConnection();

        Destroy(this);
    }
}
