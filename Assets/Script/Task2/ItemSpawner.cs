using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject gunPrefab;//gun++
    public GameObject peoplePrefab;//people++
    public Transform[] points;
    public float spawnFreq = 1f;
    private int pointIndex = 0;

    private void Start()
    {
        StartCoroutine(SpawnCo());
    }


    IEnumerator SpawnCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnFreq);

            switch (Random.Range(0, 3))
            {
                case 0:
                    Debug.Log("좌: 총 장애물, 우:빈공간");
                    AddGun(); AddEmpty();
                    break;
                case 1:
                    Debug.Log("좌: 총 장애물, 우:총 장애물");
                    AddGun(); AddGun();
                    break;
                case 2:
                    Debug.Log("좌: 빈공간, 우:총 장애물");
                    AddEmpty(); AddGun();
                    break;
            }
        }
    }
    void AddGun()
    {
        GameObject go = Instantiate(gunPrefab);
        go.transform.position = points[pointIndex].position;
        go.GetComponent<GunItem>().Init(3);

        NextPoint();
    }
    void AddEmpty()
    {
        NextPoint();
    }
    void NextPoint()
    {
        pointIndex++;
        pointIndex %= points.Length;
    }
    void AddPeople()
    {
        //
    }
}
