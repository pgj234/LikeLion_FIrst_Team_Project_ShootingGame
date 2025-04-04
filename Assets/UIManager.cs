﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    StartStage,
    ClearStage,
    BossWarnning,
    BossClear
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [SerializeField] GameObject StartStageUI;
    [SerializeField] GameObject ClearStageUI;
    [SerializeField] GameObject BossWarnningUI;
    [SerializeField] GameObject BossClearUI;

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
            case UIType.BossClear:
                BossClearUI.SetActive(true);
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
            case UIType.BossClear:
                BossClearUI.SetActive(false);
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

    // UI창 떠있는지 여부 Get
    public bool UiOpenStateGet(UIType type)
    {
        switch (type)
        {
            case UIType.StartStage:
                return StartStageUI.activeSelf;
            case UIType.ClearStage:
                return ClearStageUI.activeSelf;
            case UIType.BossWarnning:
                return BossWarnningUI.activeSelf;
            case UIType.BossClear:
                return BossClearUI.activeSelf;
            default:
                return false;
        }
    }
}