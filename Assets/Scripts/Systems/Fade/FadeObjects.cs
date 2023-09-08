using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjects : MonoBehaviour
{
    public List<Renderer> renderers = new List<Renderer>();
    public Vector3 position;
    public List<Material> materials = new List<Material>();

    [HideInInspector]
    public float InitialAlpha;

    private void Awake()
    {
        position = transform.position;
        
        if(renderers.Count == 0)
        {
            renderers.AddRange(GetComponentsInChildren<Renderer>());
        }

        foreach(Renderer renderer in renderers)
        {
            materials.Add(renderer.material);
        }

        InitialAlpha = materials[0].color.a;
    }
}
