using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using GoogleARCore;

public class AugmentedImageVisualizer : MonoBehaviour
{
    [SerializeField] private VideoClip[] videoClips;
    public AugmentedImage image;
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnStop;
    }

    private void OnStop(VideoPlayer source) {
        gameObject.SetActive(false);
    }

    void Update() {
        if (image == null | image.TrackingState != TrackingState.Tracking) {
            return;
        }

        if (!videoPlayer.isPlaying) {
            videoPlayer. clip = videoClips[image.DatabaseIndex];
            videoPlayer.Play();
        }

        transform.localScale = new Vector3(image.ExtentX, image.ExtentZ, 1);

    }

}
