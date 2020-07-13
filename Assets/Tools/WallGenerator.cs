using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{

    //PUBLIC INSPECTOR VARIABLES
    [SerializeField]
    public WallGenerator go_next_fence_post;
    public Vector3 FencePostSize = new Vector3(.2f, 2f, .2f);
    public Vector3 FencePostCenter = new Vector3(0f, .5f, .2f);

    //PRIVATE VARIABLES

    private Vector3 v3_distance;
    private Vector3 v3_nextend1;
    private Vector3 v3_nextend2;
    private Vector3 v3_nexthalflength;
    private Vector3 v3_thisend1;
    private Vector3 v3_thisend2;
    private Vector3 v3_thishalflength;


    //MONOBEHAVIOUR FUNCTIONS

    void Awake()
    {
        //is there another fence post?
        if (go_next_fence_post != null)
        {

            //get Vector3's that are half the length of the fence pole heights, with the same rotations
            v3_thishalflength = transform.TransformVector(Vector3.up * FencePostSize.y / 2);
            v3_nexthalflength = go_next_fence_post.transform.TransformVector(Vector3.up * go_next_fence_post.FencePostSize.y / 2);

            //get the Vector3 that's the distance direction between these two fence posts
            v3_distance = go_next_fence_post.FencePostCenterWorldSpace - FencePostCenterWorldSpace;
        }
    }

    void Start()
    {
        //connect it to another fence post?
        if (go_next_fence_post != null)
        {
            //normalize the scale of this object and set its rotation to nothin'
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            v3_nexthalflength = transform.InverseTransformVector(v3_nexthalflength);
            v3_distance = transform.InverseTransformVector(v3_distance);

            //create the mesh filter
            if (GetComponent<MeshFilter>() == null)
            {
                gameObject.AddComponent<MeshFilter>();
            }
            Mesh ms_mesh = GetComponent<MeshFilter>().mesh;

            //create the 4 vertices
            ms_mesh.Clear();
            ms_mesh.vertices = new Vector3[] {
                FencePostCenter + v3_thishalflength,
                FencePostCenter - v3_thishalflength,
                FencePostCenter + v3_distance + v3_nexthalflength,
                FencePostCenter + v3_distance - v3_nexthalflength };

            //create 4 triangles between the 2 fence posts, to cover the space between them thoroughly.  Create each triangle in 2 ways (like 0,1,2 and 0,2,1) so they're impenetrable from both sides
            ms_mesh.triangles = new int[] { 0, 1, 2, 0, 2, 1, 0, 1, 3, 1, 3, 1, 0, 2, 3, 0, 3, 2, 1, 2, 3, 1, 3, 2 };

            //remove all colliders
            Destroy(GetComponent<BoxCollider>());
            Destroy(GetComponent<SphereCollider>());
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<MeshCollider>());
            Destroy(GetComponent<WheelCollider>());

            //create a mesh collider in this shape
            gameObject.AddComponent<MeshCollider>();
            GetComponent<MeshCollider>().sharedMesh = ms_mesh;

            //make sure these doen't interfere with OnMouseOver
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        }
    }

    public Vector3 FencePostCenterWorldSpace => transform.TransformPoint(FencePostCenter);

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(FencePostCenterWorldSpace, FencePostSize);

        Gizmos.color = Color.yellow;
        if(go_next_fence_post != null)
            Gizmos.DrawLine(FencePostCenterWorldSpace, this.go_next_fence_post.FencePostCenterWorldSpace);
    }
}