using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
 
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    HighScoreData _highScoreData;
    public static GameManager Instance;

    public TMP_InputField PlayerNameInput;
    public Button StartButton;
    public TMP_Text HighScoreTextbox;

   [HideInInspector]  public string PlayerName;



    private void Awake()
    {
       
        InitInstance();
        LoadHighScoreData();
    }

    //Init using Unity singleton pattern
    private void InitInstance()
    {
       if (GameManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        GameManager.Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerNameInputChanged()
    {
        PlayerName = PlayerNameInput.text.Trim();
        StartButton.interactable = !String.IsNullOrEmpty(PlayerName);
    }
 

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
  Application.Quit();
#endif
    }

    public void SetHighScoreIfHigherAndSave(int points)
    {
        if (points >= this._highScoreData.Score)
        {
            this._highScoreData.Player = PlayerName;
            this._highScoreData.Score = points;
            SaveHighScoreData();
        }
    }

    public void SaveHighScoreData()
    {
        var json = JsonUtility.ToJson(_highScoreData);
        File.WriteAllText(scoreDataPath(), json);
    }

    private string scoreDataPath()
    {
        return Application.persistentDataPath + "/highscoredata.json";
    }

    public void LoadHighScoreData()
    {
        try
        {
            var json = File.ReadAllText(scoreDataPath());
            _highScoreData = JsonUtility.FromJson<HighScoreData>(json);
            HighScoreTextbox.text = HighScoreText;
        }
        catch (Exception)
        {

            _highScoreData = new HighScoreData { };
        }
    }

    public string HighScoreText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_highScoreData.Player))
                return $"High Score: {_highScoreData.Player} ({_highScoreData.Score})";

            return "High Score: Be the first!";
        }
    }

    [Serializable]
    public class HighScoreData
    {
        public int Score;
        public string Player;
    }
}
