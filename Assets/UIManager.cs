using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    StartStage,
    ClearStage,
    BossWarnning
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [SerializeField] GameObject StartStageUI;
    [SerializeField] GameObject ClearStageUI;
    [SerializeField] GameObject BossWarnningUI;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("씬에 1개만 배치해주세요.");
        }
        instance = this;

    }
    public void OpenUI(UIType type)
    {
        switch(type)
        {
            case UIType.StartStage:
                StartStageUI.SetActive(true);
                break;
            case UIType.ClearStage:
                ClearStageUI.SetActive(true);
                break;
            case UIType.BossWarnning:
                BossWarnningUI.SetActive(true);
                break;
        }
    }
    public void CloseUI(UIType type)
    {
        switch(type)
        {
            case UIType.StartStage:
                StartStageUI.SetActive(false);
                break;
            case UIType.ClearStage:
                ClearStageUI.SetActive(false);
                break;
            case UIType.BossWarnning:
                BossWarnningUI.SetActive(false);
                break;
        }
    }
    public void CloseAllUI()
    {
        foreach(UIType type in Enum.GetValues(typeof(UIType)))
        {
            CloseUI(type);
        }
    }

    // 클리어 UI창 떠있는지 여부 Get
    public bool ClearStageUiOpenStateGet()
    {
        return ClearStageUI.activeSelf;
    }
}