//By Beyioku Daniel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Fade
{
    public class FadeObjectBlockingView : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private Transform target;

        [SerializeField]
        private Camera camera;

        [SerializeField]
        [Range(0, 1f)]
        private float fadedAlpha = 0.5f;

        [SerializeField]
        [Range(0, 1f)]
        float initialAlpha = 1.0f;

        [SerializeField]
        private bool retainShadows = true;

        [SerializeField]
        private Vector3 targetPositionOffset = Vector3.up;

        [SerializeField]
        private float fadeSpeed = 1f;

        [Header("Read Only Data")]
        [SerializeField]
        private List<GameObject> ObjectsBlockingView = new List<GameObject>();
        private Dictionary<GameObject, Coroutine> RunningCoroutine = new Dictionary<GameObject, Coroutine>();

        private RaycastHit[] Hits = new RaycastHit[5];

        void OnEnable()
        {
            StartCoroutine(CheckForObjects());
        }

        private IEnumerator CheckForObjects()
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
            
            while (true)
            {
                int hits = Physics.RaycastNonAlloc(camera.transform.position,
                    (target.transform.position + targetPositionOffset - camera.transform.position).normalized,
                    Hits,
                    Vector3.Distance(camera.transform.position, target.transform.position + targetPositionOffset),
                    layerMask);

                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        GameObject fadeObject = GetFadingObjectFromHit(Hits[i]);

                        if (fadeObject != null && !ObjectsBlockingView.Contains(fadeObject))
                        {
                            if (RunningCoroutine.ContainsKey(fadeObject))
                            {
                                if (RunningCoroutine[fadeObject] != null)
                                {
                                    StopCoroutine(RunningCoroutine[fadeObject]);
                                }

                                RunningCoroutine.Remove(fadeObject);
                            }

                            Debug.Log(1);
                            RunningCoroutine.Add(fadeObject, StartCoroutine(FadeObjectsOut(fadeObject)));
                            ObjectsBlockingView.Add(fadeObject);
                        }
                    }
                }

                FadeObjectsNoLongerBeingHit();
                ClearHits();

                yield return null;
            }
        }

        private void FadeObjectsNoLongerBeingHit() 
        { 
            List<GameObject> objectsToRemove = new List<GameObject>(ObjectsBlockingView.Count);

            foreach (GameObject fadeObject in ObjectsBlockingView)
            {
                bool objectIsBeingHit = false;

                for (int i = 0; i < Hits.Length; i++)
                {
                    GameObject hitFadeObjects = GetFadingObjectFromHit(Hits[i]);
                    if(hitFadeObjects != null && fadeObject == hitFadeObjects)
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }

                if(!objectIsBeingHit)
                {
                    if (RunningCoroutine.ContainsKey(fadeObject))
                    {
                        if (RunningCoroutine[fadeObject] != null)
                        {
                            StopCoroutine(RunningCoroutine[fadeObject]);
                        }
                        RunningCoroutine.Remove(fadeObject);
                    }

                    RunningCoroutine.Add(fadeObject, StartCoroutine(FadeObjectsIn(fadeObject)));
                    objectsToRemove.Add(fadeObject);
                }
            }

            foreach(GameObject fadeObject in objectsToRemove) 
            {
                ObjectsBlockingView.Remove(fadeObject);
            }
        }

        private List<Material> GetMaterials(ref GameObject fadeObject)
        {
            List<Renderer> renderers = new List<Renderer>();
            List<Material> materials = new List<Material>();

            renderers.AddRange(fadeObject.GetComponentsInChildren<Renderer>());
            foreach (Renderer renderer in renderers)
            {
                materials.Add(renderer.material);
            }

            return materials;
        }

        private IEnumerator FadeObjectsOut(GameObject fadeObject)
        {
            List<Material> materials = GetMaterials(ref fadeObject);

            foreach (Material material in materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcColor);
                material.SetInt("_ZWrite", 0);
                material.SetInt("_Surface", 1);

                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", retainShadows);

                material.SetOverrideTag("RenderType", "Transparent");

                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            float time = 0;

            while (materials[0].color.a > fadedAlpha) 
            {
                foreach (Material material in materials)
                {
                    if (material.HasProperty("_BaseColor"))
                    {
                        material.color = new Color(
                            material.color.r,
                            material.color.g,
                            material.color.b,
                            Mathf.Lerp(initialAlpha, fadedAlpha, time)
                        );
                    }
                }

                time += fadeSpeed * Time.deltaTime;
                yield return null;
            }

            if (RunningCoroutine.ContainsKey(fadeObject))
            {
                StopCoroutine(RunningCoroutine[fadeObject]);
                RunningCoroutine.Remove(fadeObject);
            }
        }

        private IEnumerator FadeObjectsIn(GameObject fadeObject)
        {
            List<Material> materials = GetMaterials(ref fadeObject);

            float time = 0;

            while (materials[0].color.a < fadedAlpha)
            {
                foreach (Material material in materials)
                {
                    if (material.HasProperty("_BaseColor"))
                    {
                        material.color = new Color(
                            material.color.r,
                            material.color.g,
                            material.color.b,
                            Mathf.Lerp(fadedAlpha, initialAlpha, time)
                        );
                    }
                }

                time += fadeSpeed * Time.deltaTime;
                yield return null;
            }
            foreach (Material material in materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.SetInt("_Surface", 0);

                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

                material.SetShaderPassEnabled("DepthOnly", true);
                material.SetShaderPassEnabled("SHADOWCASTER", true);

                material.SetOverrideTag("RenderType", "Opaque");

                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }


            if (RunningCoroutine.ContainsKey(fadeObject))
            {
                StopCoroutine(RunningCoroutine[fadeObject]);
                RunningCoroutine.Remove(fadeObject);
            }
        }

        public void ClearHits()
        {
            System.Array.Clear(Hits, 0, Hits.Length);
        }

        private GameObject GetFadingObjectFromHit(RaycastHit hit)
        {
            return hit.collider != null ? hit.collider.gameObject : null;
        }

        public void FadeInAllObjects()
        {
            foreach (GameObject fadeObject in ObjectsBlockingView)
            {
                if (RunningCoroutine.ContainsKey(fadeObject))
                {
                    if (RunningCoroutine[fadeObject] != null)
                    {
                        StopCoroutine(RunningCoroutine[fadeObject]);
                    }
                    RunningCoroutine.Remove(fadeObject);
                }

                RunningCoroutine.Add(fadeObject, StartCoroutine(FadeObjectsIn(fadeObject)));
            }
        }

        private void Update()
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        }
    }
}
