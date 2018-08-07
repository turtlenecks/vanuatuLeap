﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class readLanguageData : MonoBehaviour
{

    public string filename = "languageData.json";
    public GameObject languageDome;
    public GameObject languageBoundsCube;
 
    public int scaleX = 1000;
    public int scaleY = 2000;

    private languageData[] mylanguageData;

    // Use this for initialization
    void Start()
    {
        loadData();
    }

    private void loadData()
    {
        //file path
        string dataFilePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(dataFilePath))
        {
            string dataAsJson = File.ReadAllText(dataFilePath);
            languageData[] loadedData = JsonHelper.FromJson<languageData>(dataAsJson);


            for (int i = 0; i < loadedData.Length; i++)
            {
                // dome
                float[] thisXY = helpers.getXYPos(loadedData[i].latitude, loadedData[i].longitude, scaleX, scaleY);
                GameObject thisDome = Instantiate(languageDome, new Vector3(thisXY[0], 0, thisXY[1]), Quaternion.Euler(90, 0, 0));

                // audio
                if (loadedData[i].audiofile != null)
                {
                    AudioSource thesource = thisDome.GetComponent<AudioSource>();
                    thesource.clip = Resources.Load<AudioClip>("languageAudio/" + loadedData[i].audiofile);
                    thesource.Play();
                }
                // limits
                float[] NWLimits = helpers.getXYPos(loadedData[i].northlimit, loadedData[i].westlimit, scaleX, scaleY);
                float[] SELimits = helpers.getXYPos(loadedData[i].southlimit, loadedData[i].eastlimit, scaleX, scaleY);

                float theWidth = SELimits[0] - NWLimits[0]; // south - north
                float theHeight = SELimits[1] - NWLimits[1]; // east - west

                GameObject limitsCube = Instantiate(languageBoundsCube, new Vector3(thisXY[0], 0, thisXY[1]), Quaternion.Euler(0, 0, 0));
                limitsCube.transform.localScale = new Vector3(theWidth, loadedData[i].numberofspeakers/1000+0.01f, theHeight);
            }

        }
    }
}






