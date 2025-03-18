using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnMinX;
    public float spawnMaxX;

    public GameObject gunPrefab;//gun++
    public GameObject peoplePrefab;//people++

    private float spawnY;

    private void Start()
    {
        StartCoroutine(SpawnCo());
        spawnY = transform.position.y;
    }


    IEnumerator SpawnCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SpawnGun();

            //yield return new WaitForSeconds(1f);
            //SpawnPeople();
        }
    }

    void SpawnGun()
    {
        GameObject go = Instantiate(gunPrefab);
        go.transform.position = new Vector3(Random.Range(spawnMinX, spawnMaxX), spawnY, 0);
        go.GetComponent<GunItem>().Init(3, 1);
    }

    void SpawnPeople()
    {
        GameObject go = Instantiate(gunPrefab);
        go.transform.position = new Vector3(Random.Range(spawnMinX, spawnMaxX), spawnY, 0);
    }
}
