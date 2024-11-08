﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View<MainMenuView>
{
    private const string m_BestScorePrefix = "BEST SCORE ";

    public Text m_BestScoreText;
    public Image m_BestScoreBar;
    public GameObject m_BestScoreObject;
    public InputField m_InputField;
    public List<Image> m_ColoredImages;
    public List<Text> m_ColoredTexts;

    public GameObject m_BrushGroundLight;
    public GameObject m_BrushesPrefab;
    public int m_IdSkin = 0;
    public GameObject m_PointsPerRank;
    public RankingView m_RankingView;

    [SerializeField] private Feature m_SkinSelectFeature;
    [SerializeField] private GameObject[] m_SkinSelectObjectsOld;
    [SerializeField] private GameObject[] m_SkinSelectObjectsNew;

    [Header("Ranks")]
    public string[] m_Ratings;

    private StatsManager m_StatsManager;

    protected override void Awake()
    {
        base.Awake();

        m_StatsManager = StatsManager.Instance;
        m_IdSkin = m_StatsManager.FavoriteSkin;

        OnFeatureStateChanged(m_SkinSelectFeature.FeatureType, m_SkinSelectFeature);
    }

    private void Start()
    {
        FeatureManager.OnChangeFeatureState += OnFeatureStateChanged;
    }

    private void OnDestroy()
    {
        FeatureManager.OnChangeFeatureState -= OnFeatureStateChanged;
    }

    private void OnFeatureStateChanged(EFeatureType featureType, bool state)
    {
        if (featureType == m_SkinSelectFeature.FeatureType)
        {
            for (int i = 0; i < m_SkinSelectObjectsOld.Length; i++)
                m_SkinSelectObjectsOld[i].SetActive(!state);

            for (int i = 0; i < m_SkinSelectObjectsNew.Length; i++)
                m_SkinSelectObjectsNew[i].SetActive(state);
        }
    }

    public void OnPlayButton()
    {
        if (m_GameManager.currentPhase == GamePhase.MAIN_MENU)
            m_GameManager.ChangePhase(GamePhase.LOADING);
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        switch (_GamePhase)
        {
            case GamePhase.MAIN_MENU:
                m_BrushGroundLight.SetActive(true);
                Transition(true);
                break;

            case GamePhase.LOADING:
                m_BrushGroundLight.SetActive(false);

                    m_BrushesPrefab.SetActive(false);

                if (m_Visible)
                    Transition(false);
                break;
        }
    }

    public void SetTitleColor(Color _Color)
    {
        m_BrushesPrefab.SetActive(true);
        int favoriteSkin = Mathf.Min(m_StatsManager.FavoriteSkin, m_GameManager.m_Skins.Count - 1);
        m_BrushesPrefab.GetComponent<BrushMainMenu>().Set(m_GameManager.m_Skins[favoriteSkin]);
        string playerName = m_StatsManager.GetNickname();

        if (playerName != null)
            m_InputField.text = playerName;

        for (int i = 0; i < m_ColoredImages.Count; ++i)
            m_ColoredImages[i].color = _Color;

        for (int i = 0; i < m_ColoredTexts.Count; i++)
            m_ColoredTexts[i].color = _Color;
            
        m_RankingView.gameObject.SetActive(true);
        m_RankingView.RefreshNormal();
    }

    public void OnSetPlayerName(string _Name)
    {
        m_StatsManager.SetNickname(_Name);
    }

    public string GetRanking(int _Rank)
    {
        return m_Ratings[_Rank];
    }

    public int GetRankingCount()
    {
        return m_Ratings.Length;
    }

    public void LeftButtonBrush()
    {
        ChangeBrush(m_IdSkin - 1);
    }

    public void RightButtonBrush()
    {
        ChangeBrush(m_IdSkin + 1);
    }

    public void ChangeBrush(int _NewBrush)
    {
        _NewBrush = Mathf.Clamp(_NewBrush, 0, GameManager.Instance.m_Skins.Count);
        m_IdSkin = _NewBrush;
        if (m_IdSkin >= GameManager.Instance.m_Skins.Count)
            m_IdSkin = 0;
        GameManager.Instance.m_PlayerSkinID = m_IdSkin;
        int favoriteSkin = Mathf.Min(m_StatsManager.FavoriteSkin, m_GameManager.m_Skins.Count - 1);
        m_BrushesPrefab.GetComponent<BrushMainMenu>().Set(GameManager.Instance.m_Skins[favoriteSkin]);
        m_StatsManager.FavoriteSkin = m_IdSkin;
        GameManager.Instance.SetColor(GameManager.Instance.ComputeCurrentPlayerColor(true, 0));
    }
}
