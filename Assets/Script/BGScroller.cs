using System;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public GameObject[] tileGroupA;
    public GameObject[] tileGroupB;
    public float gapY = 10f;
    private bool isMovingGroupA;
    public static event Action<float> onChangeSpeed;
    private static float _curSpeed;
    public static float curSpeed
    {
        get => _curSpeed;
        set
        {
            _curSpeed = value;
            onChangeSpeed?.Invoke(_curSpeed);
        }
    }

    private void Start()
    {
        BGScroller.curSpeed = 3f;
    }

    private void Update()
    {
        MoveBackground();
        CheckSwapGroup();
    }

    private void MoveBackground()
    {
        GameObject[] movingTiles = isMovingGroupA ? tileGroupB : tileGroupA;
        GameObject[] followingTiles = isMovingGroupA ? tileGroupA : tileGroupB;

        foreach (GameObject tile in movingTiles)
        {
            tile.transform.Translate(Vector2.down * _curSpeed * Time.deltaTime);
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
}
