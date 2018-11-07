using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoidController : MonoBehaviour {

    public float speedConst = 10f;
    public List<GameObject> boids;
    public float rotationSpeed = 0.5f;
    
    private Vector3 vector;
    private float repulsionRange = 1;
    private float detectionRange = 3;
    private float boundaryDetection = 1;
    private float minX, maxX, minY, maxY;

    // Use this for initialization
    void Start () {
        var speed = -(transform.up);
        vector = new Vector3(speed.x, speed.y, speed.z);
        boids = FindObjectOfType<BoidListHandler>().getBoids();

        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        minX = -horzExtent;
        maxX = horzExtent;
        minY = -vertExtent;
        maxY = vertExtent;

    }
	
	// Update is called once per frame
	void Update () {
        setVector();
        checkForBoundary();
        move();
	}

    void checkForBoundary()
    {
        var differenceMin = Mathf.Abs(minX - gameObject.transform.position.x);
        var differenceMax = Mathf.Abs(maxX - gameObject.transform.position.x);

        if (Mathf.Abs(differenceMin) < boundaryDetection || Mathf.Abs(differenceMax) < boundaryDetection)
        {
            vector.x *= -1;
        }
        differenceMin = Mathf.Abs(minY - gameObject.transform.position.y);
        differenceMax = Mathf.Abs(maxY - gameObject.transform.position.y);

        if (Mathf.Abs(differenceMin) < boundaryDetection || Mathf.Abs(differenceMax) < boundaryDetection)
        {
            vector.y *= -1;
        }

    }

    void setVector()
    {
        Vector3 rForce = repulsiveForce();
        Vector3 fForce = flockingForce();
        Vector3 dForce = directionalForce();
        //Vector3 forwardSpeed = new Vector3(transform.up.x * 0.05f, transform.up.y * 0.05f, 0);

        vector = vector + rForce + fForce + dForce;
    }

    Vector3 repulsiveForce()
    {
        Vector3 rForce = new Vector3(0,0,0);
        foreach(var boid in boids)
        {
            var difference = boid.transform.position - gameObject.transform.position;
            var distance = getDistance(difference.x, difference.y);
            if(Mathf.Abs(distance) < repulsionRange)
            {
                rForce = rForce - (boid.transform.position - transform.position);
            }
        }
        return rForce;
    }

    float getDistance(float x, float y)
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    Vector3 flockingForce()
    {
        Vector3 fForce = new Vector3(0, 0, 0);
        var counter = 0;
        foreach (var boid in boids)
        {
            var difference = boid.transform.position - gameObject.transform.position;
            var distance = getDistance(difference.x, difference.y);
            if (boid.gameObject != gameObject && Mathf.Abs(distance) < detectionRange) 
            {
                fForce += boid.transform.position;
                counter++;
            }
        }
        if(counter == 0)
        {
            return fForce;
        }
        fForce = fForce / counter;
        return (fForce - transform.position) / 100;
    }
    
    Vector3 directionalForce()
    {
        Vector3 dForce = new Vector3(0, 0, 0);
        var counter = 0;
        foreach (var boid in boids)
        {
            var difference = boid.transform.position - gameObject.transform.position;
            var distance = getDistance(difference.x, difference.y);
            if (boid.gameObject != gameObject && Mathf.Abs(distance) < detectionRange)
            {
                BoidController c = boid.GetComponent<BoidController>();
                dForce += c.getVector();
                counter++;
            }
        }
        if(counter == 0)
        {
            return dForce;
        }
        dForce = dForce / counter;
        return (dForce - vector) / 8;
    }

    void move()
    {
        transform.position = vector * speedConst + transform.position;
    }

    public Vector3 getVector()
    {
        return vector;
    }
}
