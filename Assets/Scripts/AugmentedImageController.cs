using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;


public class AugmentedImageController : MonoBehaviour
{
    [SerializeField] private AugmentedImageVisualizer augmented_image_visualizer;

    private readonly Dictionary<int, AugmentedImageVisualizer> visualizers = 
        new Dictionary<int, AugmentedImageVisualizer>();

    private readonly List<AugmentedImage> images = new List<AugmentedImage>();

    private void Update() {
        if (Session.Status != SessionStatus.Tracking) {
            return;
        }

        Session.GetTrackables(images, TrackableQueryFilter.Updated);
        VisualizeTrackables();
    }

    private void VisualizeTrackables() {
        foreach (var image in images) {
            var visualizer = GetVisualizer(image);

            if (image.TrackingState == TrackingState.Tracking && visualizer == null) {
                AddVisualizer(image);
            }

            else if (image.TrackingState == TrackingState.Stopped && visualizer != null) {
                RemoveVisualizer(image, visualizer);
            }
        }
    }

    private void RemoveVisualizer(AugmentedImage image, AugmentedImageVisualizer visualizer) {
        visualizers.Remove(image.DatabaseIndex);
        Destroy(visualizer.gameObject);
    }
    
    private void AddVisualizer(AugmentedImage image) {
        var anchor = image.CreateAnchor(image.CenterPose);
        var visualizer = Instantiate(augmented_image_visualizer, anchor.transform);
        visualizer.image = image;
        visualizers.Add(image.DatabaseIndex, visualizer);
    }

    private AugmentedImageVisualizer GetVisualizer(AugmentedImage image) {
        AugmentedImageVisualizer visualizer;
        visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
        return visualizer;
    }
}
