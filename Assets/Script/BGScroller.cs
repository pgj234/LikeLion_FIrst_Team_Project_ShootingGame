using System;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public GameObject[] tileGroupA;
    public GameObject[] tileGroupB;
    public float gapY = 10f;
    private bool isMovingGroupA;


    private float curSpeed = 0;
    void Start()
    {
        EventManager.instance.stageEvents.onChangeStage += ChangeStage;
        EventManager.instance.playerEvents.onPlayerDead += PlayerDead;
    }

    void OnDestroy()
    {
        EventManager.instance.stageEvents.onChangeStage -= ChangeStage;
        EventManager.instance.playerEvents.onPlayerDead -= PlayerDead;
    }

    private bool isPlayerDead = false;


    private void Update()
    {
        if (isPlayerDead) return;
        MoveBackground();
        CheckSwapGroup();
    }

    private void MoveBackground()
    {
        GameObject[] movingTiles = isMovingGroupA ? tileGroupB : tileGroupA;
        GameObject[] followingTiles = isMovingGroupA ? tileGroupA : tileGroupB;

        foreach (GameObject tile in movingTiles)
        {
            tile.transform.Translate(Vector2.down * curSpeed * Time.deltaTime);
        }

        for (int i = 0; i < followingTiles.Length; i++)
        {
            followingTiles[i].transform.position = movingTiles[i].transform.position + new Vector3(0, gapY, 0);
        }
    }

    private void CheckSwapGroup()
    {
        GameObject[] movingTiles = isMovingGroupA ? tileGroupB : tileGroupA;
        if (movingTiles[0].transform.position.y < -gapY)
        {
            isMovingGroupA = !isMovingGroupA;
        }
    }
    private void ChangeStage(int num)
    {
        curSpeed = StageManager.instance.GetCurSpeed();
    }


    private void PlayerDead()
    {
        isPlayerDead = true;
    }


}
