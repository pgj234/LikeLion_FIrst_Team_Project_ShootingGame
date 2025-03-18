using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Bullet bullet;
    


    void Start()
    {
        InvokeRepeating("shoot",0.5f,0.5f);
    }

    
    void Update()
    {
        
    }

    void shoot()
    {
        //Instantiate(bullet, transform.position, Quaternion.identity);
    }
}
