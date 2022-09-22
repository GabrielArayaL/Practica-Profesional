using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerManager : MonoBehaviour
{

    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    private VideoPlayer _videoPlayer;
    private bool isImageTrackable;


    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;

    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs data)
    {
        foreach (var trackedImage in data.added)
        {
            _videoPlayer = trackedImage.GetComponentInChildren<VideoPlayer>();
            _videoPlayer.Play();
        }
        
        foreach (var trackedImage in data.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                if (!isImageTrackable)
                {
                    isImageTrackable = true;
                    _videoPlayer.gameObject.SetActive(true);
                    _videoPlayer.Play();
                }
            }
            else if (trackedImage.trackingState == TrackingState.Limited)
            {
                if (isImageTrackable)
                {
                    isImageTrackable = false;
                    _videoPlayer.gameObject.SetActive(false);
                    _videoPlayer.Pause();
                }
            }
        }    
    }
}
