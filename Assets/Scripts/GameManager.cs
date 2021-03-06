﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public MatchSettings MatchSettings;
    //All players in game
    private Dictionary<string, Player> _players = new Dictionary<string, Player>();

    public bool IsMenuOpened = false;
    [SerializeField] private GameObject _sceneCamera;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("You have more than one game manager");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetSceneCameraState(bool state)
    {
        if (_sceneCamera == null)
        {
            Debug.LogError("GameManager: No scene camera found");
            return;
        }

        _sceneCamera.SetActive(state);
    }

    #region Player tracking

    public Player GetPlayer(string playerId)
    {
        return _players[playerId];
    }

    public void RegisterPlayer(string playerId, Player player)
    {
        _players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public void UnregisterPlayer(string playerId)
    {
        _players.Remove(playerId);
    }

    #endregion


}
