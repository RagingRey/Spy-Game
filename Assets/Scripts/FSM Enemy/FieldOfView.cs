//Credits: Beyioku Daniel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM_Enemy
{
    public class FieldOfView : MonoBehaviour
    {
        #region VARIABLES
        public float viewRadius;
        [Range(0,360)]
        public float viewAngle;

        public LayerMask playerMask;
        public LayerMask obstacleMask;

        [HideInInspector]
        public List<Transform> visiblePlayer = new List<Transform>();

        public MeshFilter viewMeshFilter;
        Mesh viewMesh;
        public float meshResolution;
        public int edgeResolveIterations;
        public float edgeDstThreshold;
	
        public bool playerDetected;
        #endregion

        #region GETTERS & SETTERS
        public bool PLAYER_DETECTED{
            get{ return playerDetected; }
        }
        #endregion
   
        #region EXECUTION
        void Start()
        {
            viewMesh = new Mesh
            {
                name = "View Mesh"
            };

            viewMeshFilter.mesh = viewMesh;
        }

        void Update()
        {
            FindVisibleTarget();
            playerDetected = visiblePlayer.Count != 0;
        }
        #endregion

        void LateUpdate()
        {
            DrawFieldOfView ();
        }

        void DrawFieldOfView() 
        {
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3> ();
            ViewCastInfo oldViewCast = new ViewCastInfo ();
            for (int i = 0; i <= stepCount; i++) 
            {
                float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast (angle);

                if (i > 0) 
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs (oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                    if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) 
                    {
                        EdgeInfo edge = FindEdge (oldViewCast, newViewCast);
                        if (edge.pointA != Vector3.zero) 
                        {
                            viewPoints.Add (edge.pointA);
                        }
                        if (edge.pointB != Vector3.zero) 
                        {
                            viewPoints.Add (edge.pointB);
                        }
                    }

                }
                viewPoints.Add (newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount-2) * 3];

            vertices [0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++) 
            {
                vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]);

                if (i < vertexCount - 2) 
                {
                    triangles [i * 3] = 0;
                    triangles [i * 3 + 1] = i + 1;
                    triangles [i * 3 + 2] = i + 2;
                }
            }

            viewMesh.Clear ();
		
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals ();
        }


        void FindVisibleTarget()
        {
            Collider[] playerInRadius = Physics.OverlapSphere (transform.position, viewRadius, playerMask);

            if (playerInRadius.Length > 0)
            {
                foreach (var player in playerInRadius)
                {
                    if (player.gameObject.CompareTag("Player"))
                    {
                        visiblePlayer.Clear();

                        Transform playerTransform = player.transform;
                        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;

                        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2 && !player.gameObject.GetComponent<PlayerMovement>().isHiding)
                        {
                            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                            if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                            {
                                visiblePlayer.Add(playerTransform);
                            }
                            else
                            {
                                visiblePlayer.Clear();
                            }
                        }
                    }
                }
            }
            else
            {
                visiblePlayer.Clear();
            }
        }

        EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeResolveIterations; i++) {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast (angle);

                bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                } else {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo (minPoint, maxPoint);
        }

	

        public struct ViewCastInfo {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }

        ViewCastInfo ViewCast(float globalAngle) {
            Vector3 dir = DirFromAngle (globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast (transform.position, dir, out hit, viewRadius, obstacleMask)) {
                return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
            } else {
                return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);
            }
        }

        public struct EdgeInfo {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
                pointA = _pointA;
                pointB = _pointB;
            }
        }



        public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
        {
            if(!angleIsGlobal)
            {
                angleInDegree += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
        }
    }
}
