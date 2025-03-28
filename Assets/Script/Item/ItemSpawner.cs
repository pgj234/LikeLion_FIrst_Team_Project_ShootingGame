using System;
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


    List<GameObject> gunItems = new List<GameObject>();
    List<GameObject> peopleItems = new List<GameObject>();

    public static event Action onGunItemAllDestroy;
    public static event Action onPeopleItemAllDestroy;
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
        StartCoroutine(WaitForGunItemAllDestroy());
        StartCoroutine(WaitForPeopleItemAllDestroy());
    }

    IEnumerator WaitForGunItemAllDestroy()
    {
        while (true)
        {
            yield return null;
            int nullCount = 0;
            for (int i = 0; i < gunItems.Count; i++)
            {
                if (gunItems[i] == null) nullCount++;
            }
            if (nullCount == gunItems.Count)
            {
                break;
            }
        }
        gunItems.Clear();
        onGunItemAllDestroy?.Invoke();
    }

    IEnumerator WaitForPeopleItemAllDestroy()
    {
        while (true)
        {
            yield return null;
            int nullCount = 0;
            for (int i = 0; i < peopleItems.Count; i++)
            {
                if (peopleItems[i] == null) nullCount++;
            }
            if (nullCount == peopleItems.Count)
            {
                break;
            }
        }
        peopleItems.Clear();
        onPeopleItemAllDestroy?.Invoke();
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
        peopleFenceCount = StageManager.instance.GetPeopleFenceCount();
        co = StartCoroutine(SpawnCo());

    }

    //몬스터 안잡고, 업글만 노릴수있으니, 스테이지마다 갯수제한을 둠.
    int fenceCount;
    int peopleFenceCount;
    IEnumerator SpawnCo()
    {
        while (fenceCount > 0 || peopleFenceCount > 0)
        {
            yield return new WaitForSeconds(spawnFreq);

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                if (!TrySpawnGun())
                {
                    TrySpawnPeople();
                }
            }
            else
            {
                if (!TrySpawnPeople())
                {
                    TrySpawnGun();
                }
            }

        }
    }
    bool TrySpawnGun()
    {
        if (fenceCount <= 0) return false;
        fenceCount--;
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                AddGun(); AddEmpty();
                break;
            case 1:
                AddEmpty(); AddGun();
                break;
        }
        return true;
    }
    bool TrySpawnPeople()
    {
        if (peopleFenceCount <= 0) return false;
        peopleFenceCount--;
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                AddPeople(); AddEmpty();
                break;
            case 1:
                AddEmpty(); AddPeople();
                break;
        }
        return true;
    }
    void AddGun()
    {
        GameObject go = Instantiate(gunPrefab);
        go.transform.position = points[pointIndex].position;
        go.GetComponent<GunItem>().Init();
        gunItems.Add(go);

        NextPoint();
    }

    void AddPeople()
    {
        GameObject go = Instantiate(peoplePrefab);
        go.transform.position = points[pointIndex].position;
        go.GetComponent<PeopleItem>().Init();
        peopleItems.Add(go);

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
}
