using System;
using System.Collections;
using Frictionless;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class GameshiftService : MonoBehaviour
{
    public delegate void onLogin();
    public onLogin OnLogin;
    
    public delegate void onLoadedAsset();
    public Action<AssetsResult> OnAssetsLoaded;
    public AssetsResult AllAssets;
    
    private string ShiftId;
    private string EmailAddress;
 
    private const string SHIFT_API_URI = "https://api.gameshift.dev/";
    private const string MY_API_URI = "https://cube-shift.vercel.app/api/";
    //private const string MY_API_URI = "https://e61b-77-10-59-99.ngrok-free.app/api/";
    private const string SHIFT_ID_KEY = "SHIFT_ID_KEY";
    private const string EMAIL_KEY = "EMAIL_KEY";
    private int currentRequests;

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        ShiftId = PlayerPrefs.GetString(SHIFT_ID_KEY);
    }

    public bool IsRequestRunning()
    {
        return currentRequests > 0;
    }
    
    private void Start()
    {
        if (!string.IsNullOrEmpty(ShiftId))
        {
            StartCoroutine(GetUserFromMy(ShiftId));   
        }
        else
        {
            ServiceFactory.Resolve<LoginScreen>().Open();
        }
    }

    public void LoginOrRegister(string email)
    {
        ShiftId = PlayerPrefs.GetString(SHIFT_ID_KEY);

        if (string.IsNullOrEmpty(ShiftId))
        {
            ShiftId = GameShiftUtils.CreateRandomId();
            StartCoroutine(Register(ShiftId, email));
        }
        else
        {
            StartCoroutine(GetAllAssets(ShiftId));
        }
    }
    
    public IEnumerator GetUserFromMy(string shiftId)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(MY_API_URI + "user?user="+shiftId))
        {
            Debug.Log("Get User");
            currentRequests++;
            yield return request.SendWebRequest();
            Debug.Log("Get User done");
            currentRequests--;
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection error register.");
            }
            else
            {
                if (request.responseCode == 200)
                {
                    string result = request.downloadHandler.text;
                    StartCoroutine(GetAllAssets(shiftId));
                    OnLogin?.Invoke();
                    ServiceFactory.Resolve<LoginScreen>().Close();

                    Debug.Log("Result: " + result);
                }
                else
                {
                    Debug.LogError(request.result);
                }
            }
        }
    }
    
    public IEnumerator GetUser(string shiftId)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(SHIFT_API_URI + "/users/"+shiftId))
        {
            request.SetRequestHeader("x-api-key", GameShiftUtils.API_KEY);
            currentRequests++;
            yield return request.SendWebRequest();
            currentRequests--;
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection error register.");
            }
            else
            {
                string result = request.downloadHandler.text;
                StartCoroutine(GetAllAssets(shiftId));
                OnLogin?.Invoke();
                ServiceFactory.Resolve<LoginScreen>().Close();

                Debug.Log("Result: " + result);
            }
        }
    }
    
    public IEnumerator Register(string shiftId, string email)
    {
        /*string json = @"
        {
            ""referenceId"": """ + shiftId + @""",
            ""email"": """ + email + @"""
        }";*/

        string json = "{\"referenceId\":\"" + shiftId + "\",\"email\":\"" + email + "\"}";

        var uri = MY_API_URI + "register";
        Debug.Log(uri);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, json, "application/json"))
        {
            request.timeout = 10;
            
            Debug.Log("Start request");
            currentRequests++;
            Debug.Log(request.result);
            var progress = request.SendWebRequest();
            while(!progress.isDone)
            {
                Debug.Log(progress.progress);
                yield return null;
            }
            currentRequests--;
            Debug.Log("Finished request");
            
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection error register.");
            }
            else
            {
                if (request.responseCode == 200)
                {
                    string result = request.downloadHandler.text;
                    PlayerPrefs.SetString(EMAIL_KEY, email);
                    PlayerPrefs.SetString(SHIFT_ID_KEY, shiftId);

                    Debug.Log($"Player Registered: {shiftId} {email}");
                
                    JSONNode jsonNode = JSON.Parse(result);
                    StartCoroutine(GetAllAssets(shiftId));
                    Debug.Log("Result: " + result);    
                    ServiceFactory.Resolve<LoginScreen>().Close();
                    OnLogin?.Invoke();
                }
                else
                {
                    Debug.LogError(request.result);
                }
            }
        }
    }
    
    public IEnumerator GetAllAssets(string shiftId)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(MY_API_URI + "assets?user=" + shiftId))
        {
            currentRequests++;
            yield return request.SendWebRequest();
            currentRequests--;
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection error register.");
            }
            else
            {
                if (request.responseCode == 200)
                {
                    string result = request.downloadHandler.text;
                    Debug.Log("res"+result);
                    AssetsResultJson assets = JsonUtility.FromJson<AssetsResultJson>(result);
                    AllAssets = assets.json;
                    OnAssetsLoaded.Invoke(AllAssets);
                    Debug.Log("Asset Result: " + result);
                }
                else
                {
                    Debug.LogError(request.result);
                }
            }
        }
    }
    
    public IEnumerator CreateAsset(CharacterId characterId)
    {
        /*string json = @"
        {
            ""referenceId"": """ + ShiftId + @""",
            ""assetId"": """ + characterId.ToString() + @"""
        }";*/
        
        string json = "{\"referenceId\":\"" + ShiftId + "\",\"assetId\":\"" + characterId.ToString() + "\"}";

        using (UnityWebRequest request = UnityWebRequest.Post(MY_API_URI + "mintAsset", json, "application/json"))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            currentRequests++;
            yield return request.SendWebRequest();
            currentRequests--;
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection error register.");
            }
            else
            {
                if (request.responseCode == 200)
                {
                    string result = request.downloadHandler.text;
                    StartCoroutine(GetAllAssets(ShiftId));
                    Debug.Log("Result: " + result);
                }
                else
                {
                    Debug.LogError(request.result);
                }
            }
        }
    }
}
