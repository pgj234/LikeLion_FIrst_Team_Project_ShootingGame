using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float speed = 3f;
    public GameObject[] tileGroupA;
    public GameObject[] tileGroupB;
    public float gapY = 10f;
    private bool isMovingGroupA;

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
            tile.transform.Translate(Vector2.down * speed * Time.deltaTime);
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
