using JetBrains.Annotations;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    // Update is called once per frame

    void Awake()
    {
        GameManager.gameManager = FindObjectOfType<GameManager>();
    }


    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -2f);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            //GameManager.Instance.EnemyDied(this);
            

           GameManager.gameManager.EnemyDied(this);
            
        }
    }
}
