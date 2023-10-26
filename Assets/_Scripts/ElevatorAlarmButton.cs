using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ElevatorAlarmButton : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip soundClip; 
    private Button button; 
    private bool isPressing = false;
    private float pressStartTime;

    private void Start(){
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned!");
            return;
        }
     
        button = GetComponent<Button>();
        
        if (button != null)
        {
            button.onClick.AddListener(OnButtonDown);
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }
    }

    private void OnButtonDown()
    {
        isPressing = true;
        pressStartTime = Time.time;

        if (soundClip != null)
        {
            audioSource.clip = soundClip;
            audioSource.loop = false; 
            audioSource.Stop(); 
            audioSource.Play();
        }
    }

    
    
}