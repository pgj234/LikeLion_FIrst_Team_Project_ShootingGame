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
        EventManager.instance.stageEvents.onChangeStage += ChangeStage;
        EventManager.instance.stageEvents.onSpawnPause += SpawnPause;

    }
    private void OnDestroy()
    {
        EventManager.instance.stageEvents.onChangeStage -= ChangeStage;
        EventManager.instance.stageEvents.onSpawnPause -= SpawnPause;
    }

    void SpawnPause()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }
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

    //몬스터 안잡고, 업글만 노릴수있으니, 스테이지마다 갯수제한을 둠.
    int fenceCount;
    IEnumerator SpawnCo()
    {
        while (fenceCount-- > 0)
        {
            yield return new WaitForSeconds(spawnFreq);

            switch (Random.Range(0, 2))
            {
                case 0:
                    AddGun(); AddEmpty();
                    break;
                case 1:
                    AddEmpty(); AddGun();
                    break;
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
