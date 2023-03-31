using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceRespawningScript : MonoBehaviour
{
    //[SerializeField] private MeshCollider respawn_mesh;

    [SerializeField] GameObject[] Minerals_prefab;
    //[SerializeField] GameObject[] Minerals_destroy_prefab;
    [SerializeField] GameObject[] Trees_prefab;


    [SerializeField] MeshFilter mesh_filt;


    //private float mesh_radius;


    // Start is called before the first frame update
    void Start()
    {


        int n = 20;

        for (int i = 0; i < n; i++)
        {
            respawn_ressource();
        }


        //Vector3 rayOrigin = ((Random.onUnitSphere * r) + transform.position); // put the ray randomly around the transform

        //RaycastHit hitPoint;
        //Physics.Raycast(rayOrigin, newPointOnMesh - rayOrigin, out hitPoint, 100f)


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    public void respawn_ressource()
    {

        Vector3 spawn_point = get_point_on_mesh();

        //Debug.DrawRay(spawn_point, Vector3.up, Color.red, 3f);
        
        if (Random.Range(0, 2) == 0)
        {
            Instantiate(Trees_prefab[Random.Range(0, Trees_prefab.Length)], spawn_point, Quaternion.Euler(0, Random.Range(0, 360), 0), TempleScript.instance.transform.parent);
        }
        else
        {
            Instantiate(Minerals_prefab[Random.Range(0, Minerals_prefab.Length)], spawn_point, Quaternion.Euler(0, Random.Range(0, 360), 0), TempleScript.instance.transform.parent);
        }
        
    }



    private Vector3 get_point_on_mesh()
    {

        Vector3[] meshPoints = mesh_filt.mesh.vertices;
        int[] tris = mesh_filt.mesh.triangles;
        int triStart = Random.Range(0, tris.Length / 3) * 3; // get first index of each triangle

        float a = Random.value;
        float b = Random.value;

        if (a + b >= 1)
        { // reflect back if > 1
            a = 1 - a;
            b = 1 - b;
        }

        Vector3 newPointOnMesh = meshPoints[tris[triStart]] + (a * (meshPoints[tris[triStart + 1]] - meshPoints[tris[triStart]])) + (b * (meshPoints[tris[triStart + 2]] - meshPoints[tris[triStart]])); // apply formula to get new random point inside triangle

        newPointOnMesh = transform.TransformPoint(newPointOnMesh); // convert back to worldspace

        return newPointOnMesh;

        //Debug.DrawRay(newPointOnMesh, Vector3.up,Color.red,3f);

    }



}
