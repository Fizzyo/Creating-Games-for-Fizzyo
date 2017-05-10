using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public List<BackgroundLayer> BackgroundLayers;

    // Use this for initialization
    void Start()
    {
        BackgroundLayers = new List<BackgroundLayer>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            BackgroundLayers.Add(new BackgroundLayer(this.transform.GetChild(i)));
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        foreach (BackgroundLayer layer in BackgroundLayers)
        {
            Vector3 newPosition = Vector3.Lerp(layer.StartPosition, cameraPosition, layer.ParallaxMultiplier);
            newPosition.z = layer.StartPosition.z;

            layer.LayerTransform.position = newPosition;
        }
    }
}

public class BackgroundLayer
{
    public Transform LayerTransform;
    public Vector3 StartPosition;
    public float ParallaxMultiplier;

    public BackgroundLayer(Transform layerTransform)
    {
        LayerTransform = layerTransform;
        ParallaxMultiplier = LayerTransform.localPosition.z * .1f;
        StartPosition = LayerTransform.position;
    }
}
