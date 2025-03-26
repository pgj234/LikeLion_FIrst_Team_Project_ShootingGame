using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject gunPrefab;//gun++
    public GameObject peoplePrefab;//people++
    public Transform[] points;
    public float spawnFreq = 1f;
    
    [Range(0f, 1f)]
    public float gunSpawnProbability = 0.7f; // GunItem 스폰 확률 (기본값 70%)
    
    private int pointIndex = 0;

    private void Start()
    {
        EventManager.instance.stageEvents.onChangeStage += ChangeStage;

    }
    private void OnDestroy()
    {
        EventManager.instance.stageEvents.onChangeStage -= ChangeStage;
    }


    Coroutine co = null;
    private void ChangeStage(int stage)
    {
        if (co != null)
        {
            StopCoroutine(co);
        }
        if (stage >= Constants.BOSS_STAGE) return;

        fenceCount = StageManager.instance.GetFenceCount();
        if (fenceCount > 0)
        {
            co = StartCoroutine(SpawnCo());
        }
    }

    int fenceCount;
    IEnumerator SpawnCo()
    {
        while (fenceCount-- > 0)
        {
            yield return new WaitForSeconds(spawnFreq);

            // 스폰 확률에 따라 아이템 생성
            if (Random.value < gunSpawnProbability)
            {
                AddGun();
            }
            else
            {
                AddEmpty();
            }
            
            // 두 번째 포인트에 대해서도 스폰 확률 적용
            if (Random.value < gunSpawnProbability)
            {
                AddGun();
            }
            else
            {
                AddEmpty();
            }
        }
    }
    void AddGun()
    {
        GameObject go = Instantiate(gunPrefab);
        go.transform.position = points[pointIndex].position;
        go.GetComponent<GunItem>().Init();

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
