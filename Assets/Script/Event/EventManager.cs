using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    public PlayerEvents playerEvents;
    public StageEvents stageEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("씬에 1개만 배치해주세요.");
        }
        instance = this;


        //이벤트 초기화
        playerEvents = new PlayerEvents();
        stageEvents = new StageEvents();
    }
}
