using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoidListHandler : MonoBehaviour {
    
    public GameObject boidTemplate;
    public List<GameObject> boids;

	// Use this for initialization
	void Start () {
        //boids = new GameObject[maxBoids];
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            var pos = Camera.main.ScreenToWorldPoint(mousePos);
            addBoid(pos.x, pos.y);
        }

        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Casts the ray and get the first game object hit
            if(Physics.Raycast(ray, out hit))
            {
                deleteBoid(hit.collider.gameObject);
            }
        }
	}

    public List<GameObject> getBoids()
    {
        return boids;
    }

    void addBoid(float x, float y)
    {
        var newBoid = (GameObject)Object.Instantiate(boidTemplate, new Vector3(x, y, 0), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        boids.Add(newBoid);
    }

    void deleteBoid(GameObject boid)
    {
        boids.Remove(boid);
        Destroy(boid);
    }
}
