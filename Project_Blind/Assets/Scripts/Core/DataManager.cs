﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// Json을 파싱하는것을 도와주는 클래스
    /// </summary>
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }
    public class DataManager : Manager<DataManager>
    {
        public Dictionary<string, Data.Conversation> ConversationDict { get; private set; } = new Dictionary<string, Data.Conversation>();
        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        public void Init()
        {
            ConversationDict = LoadJson<Data.ConversationData, string, Data.Conversation>("ConversationData").MakeDict();
        }
        Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>($"Data/{path}");
            return JsonUtility.FromJson<Loader>(textAsset.text);
        }

        #region Load, Save Data
        string FileName = "/GameData.json";
        GameData _gameData;
        public GameData GameData
        {
            get
            {
                if (_gameData == null)
                {
                    LoadGameData();
                    SaveGameData();
                }
                return _gameData;
            }
        }
        public void LoadGameData()
        {
            string filePath = Application.persistentDataPath + FileName;

            Debug.Log(Application.persistentDataPath);

            if (File.Exists(filePath))
            {
                Debug.Log("Data Load Success!");
                string JsonData = File.ReadAllText(filePath);
                _gameData = JsonUtility.FromJson<GameData>(JsonData);
            }
            else
            {
                Debug.Log("New Data Created!");
                _gameData = new GameData();
            }
        }
        public void SaveGameData()
        {
            string JsonData = JsonUtility.ToJson(GameData);
            string filePath = Application.persistentDataPath + FileName;
            File.WriteAllText(filePath, JsonData);
            Debug.Log("Save Completed!");
        }
        #endregion
    }
}

