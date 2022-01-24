using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    DatabaseReference databaseRef;
    FirebaseStorage storageRef;
    GameHandler gh;
    UIManager ui;

    public string lobbyKey;

    // Start is called before the first frame update
    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        storageRef = FirebaseStorage.DefaultInstance;
        gh = FindObjectOfType<GameHandler>();
        ui = FindObjectOfType<UIManager>();
    }

    //Deletes lobby upon cloising
    private void OnApplicationQuit()
    {
        DeleteLobby();
    }

    //Opens new lobby
    public void NewLobby()
    {
        gh.playerType = false;
        sessionData user = new sessionData();
        lobbyKey = databaseRef.Child("Sessions").Push().Key;
        databaseRef.Child("Sessions").Child(lobbyKey).SetRawJsonValueAsync(JsonUtility.ToJson(user));
        databaseRef.Child("Sessions").Child(lobbyKey).Child("player2Connected").ValueChanged += ConnectionChanged;
        databaseRef.Child("Sessions").Child(lobbyKey).Child("player2Choice").ValueChanged += GetOtherPlayerChoice;
        databaseRef.Child("Sessions").Child(lobbyKey).ValueChanged += MonitorObject;
    }

    //Joins existing game
    public void JoinGame(string roomCode)
    {
        FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + roomCode)
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log(snapshot.Value);
                      
                      if(snapshot.Value != null)
                      {
                          lobbyKey = roomCode;
                          ui.SwitchMenu(4);
                          gh.gameState = -1;
                          gh.playerType = true;
                          databaseRef.Child("Sessions").Child(lobbyKey).Child("player2Connected").SetValueAsync(true);
                          databaseRef.Child("Sessions").Child(lobbyKey).Child("player1Connected").ValueChanged += ConnectionChanged;
                          databaseRef.Child("Sessions").Child(lobbyKey).Child("player1Choice").ValueChanged += GetOtherPlayerChoice;
                          databaseRef.Child("Sessions").Child(lobbyKey).ValueChanged += MonitorObject;
                          gh.bothConnected = true;
                      }
                  }
              });
    }

    //Uploads details
    public void UpdatePlayerDets(int choice, int moves, bool player)
    {
        if (player == false)
        {
            databaseRef.Child("Sessions").Child(lobbyKey).Child("player1Choice").SetValueAsync(choice);
            databaseRef.Child("Sessions").Child(lobbyKey).Child("player1Moves").SetValueAsync(moves);
        }
        else
        {
            databaseRef.Child("Sessions").Child(lobbyKey).Child("player2Choice").SetValueAsync(choice);
            databaseRef.Child("Sessions").Child(lobbyKey).Child("player2Moves").SetValueAsync(moves);
        }
    }

    //Gets other player choice
    public void GetOtherPlayerChoice(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (gh.playerType == false)
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + lobbyKey + "/player2Choice")
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log("Choice is "+snapshot.Value);

                      gh.player2Choice = int.Parse(snapshot.Value.ToString());
                      Debug.Log(gh.player2Choice);
                  }
              });
        }
        else
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + lobbyKey + "/player1Choice")
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log("Choice is "+snapshot.Value);

                      gh.player2Choice = int.Parse(snapshot.Value.ToString());
                  }
              });
        }
    }

    //Method to delete the players once the games stops
    private void DeleteLobby()
    {
        lobbyKey = "";
        databaseRef.Child("Sessions").Child(lobbyKey).RemoveValueAsync();
    }

    //Event listener for whole object
    void MonitorObject(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        sessionData sesh = JsonUtility.FromJson<sessionData>(args.Snapshot.GetRawJsonValue());
        if (sesh == null)
        {
            gh.EmergencyQuit();
        }
    }

    //Event listener for change in connections
    void ConnectionChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (gh.playerType == false)
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + lobbyKey + "/player2Connected")
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log("Choice is " + snapshot.Value);

                      if ((bool)snapshot.Value == true)
                      {
                          gh.bothConnected = true;
                      }
                      if (gh.bothConnected == true && (bool)snapshot.Value == false)
                      {
                          gh.EmergencyQuit();
                      }
                  }
              });
        }
        else
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + lobbyKey + "/player1Connected")
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log("Choice is " + snapshot.Value);

                      if ((bool)snapshot.Value == true)
                      {
                          gh.bothConnected = true;
                      }
                      if (gh.bothConnected == true && (bool)snapshot.Value == false)
                      {
                          gh.EmergencyQuit();
                      }
                  }
              });
        }
    }

    //Saves data to Firebase Storage
    public void SaveMatch()
    {
        if (lobbyKey == null || lobbyKey == "")
        {
            return;
        }

        matchData data = new matchData();
        data.key = lobbyKey;
        data.player1Moves = gh.totalmoves;
        if(gh.playerType == false)
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + lobbyKey + "/player2Moves")
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log("Choice is " + snapshot.Value);

                      data.player2Moves = (int)snapshot.Value;
                  }
              });
        }
        else
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Sessions/" + lobbyKey + "/player1Moves")
              .GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Error when getting values from database");
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      Debug.Log("Choice is " + snapshot.Value);

                      data.player2Moves = (int)snapshot.Value;
                  }
              });
        }
        data.winner = ui.winner;
        data.timeTaken = gh.totalTimer;

        byte[] customBytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        StorageReference saveRef = storageRef.RootReference.Child("SaveData/"+lobbyKey+".txt");

        saveRef.PutBytesAsync(customBytes)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                }
                else
                {
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });
    }
}

[Serializable]
public class matchData
{
    public string key = "";
    public int player1Moves = 0;
    public int player2Moves = 0;
    public string winner = "";
    public float timeTaken = 0;
}

[Serializable]
public class sessionData
{
    public bool player1Connected = true;
    public bool player2Connected = false;
    public int player1Choice = 0;
    public int player2Choice = 0;
    public int player1Moves = 0;
    public int player2Moves = 0;
}
