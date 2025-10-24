using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   // Start is called before the first frame update
    public float InputHorinzontal;
    public float speed = 10.0f;
    public float xRange = 20.0f;
    public ObjectPool ProjectiledObject;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        Moveee();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

    }
   public void Moveee()
    {
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        InputHorinzontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * InputHorinzontal * Time.deltaTime * speed);

        
    }
    public void Fire()
    {
        PooledObject projectile = ProjectiledObject.GetPooledObject();
        projectile.transform.position = transform.position + new Vector3(0, 1, 0);
      // ProjectiledObject.GetPooledObject();
    }


}

