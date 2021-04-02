using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Video;
using System.Net;
using System;
using System.IO;

public class test_script : MonoBehaviour
{   
    public VideoPlayer vp;
    public int framesTimer;
    public int framesRemaining;
    public int videoIndex;
    public bool isTimerRunning = true;
    public VideoClip[] videoClips;
    public int sceneId = 0;
    
    int getSceneId(){
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://curabit-fyp.herokuapp.com/get-scene");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        SceneInfo info = JsonUtility.FromJson<SceneInfo>(jsonResponse);
        Debug.Log("Received JSON Response for Scene: "+info.scene_id);
        return Int16.Parse(info.scene_id);
    }

    int getFreq(){
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://curabit-fyp.herokuapp.com/get-freq");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        FreqInfo info = JsonUtility.FromJson<FreqInfo>(jsonResponse);
        Debug.Log("Received JSON Response for Frequency: "+info.freq);
        return Int16.Parse(info.freq);
    }

    void Awake(){
        vp = GetComponent<VideoPlayer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        framesTimer = getFreq();
        framesRemaining = framesTimer;
    }

    public void changeClip(int clipId){
        vp.clip = videoClips[clipId];
        vp.Play();
    }
    
    // Update is called once per frame
    void Update()
    {

        if (framesRemaining>0){
            framesRemaining = framesRemaining - 1;
        }
        else
        {
            framesRemaining = framesTimer;
            Debug.Log("Current Scene ID: "+sceneId);
            int newSceneId = getSceneId();
            Debug.Log("New Scene ID: "+newSceneId);
            if (sceneId!=newSceneId){
                Debug.Log("Changing Scene");
                sceneId = newSceneId;
                changeClip(sceneId);
            }
            
        }
        
    }
}

[Serializable]
public class SceneInfo {
  public string scene_id;
}

[Serializable]
public class FreqInfo {
  public string freq;
}