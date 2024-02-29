using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Satyug.HardMode
{
    public class APIManager : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void RedirectWebSite();

        [DllImport("__Internal")]
        private static extern void Exit(string token, string collectibleID, string userID);

        //public static readonly string baseURl = "https://api.digitalrammandir.com/api/v1/satyug/";
        public static readonly string baseURl = "https://api.digitalrammandir.com/api/v1/satyug/";


        //Login

        [SerializeField] UnityEvent onTokenValidate;

        [SerializeField] TabData[] tabDatas;

        private void TokenValidation(string token, string collectiveID, string auserID)
        {
            StatueManager.StatueManagerInstance.userID = auserID;

            Debug.Log(token);
            TokenValidation tokenValidation = new TokenValidation
            {
                id = token,
                type = "GameAccessToken"
            };

            string json = JsonUtility.ToJson(tokenValidation);

            getResponse(baseURl + "users/validate-token", "POST", (String webResponse) =>
            {
                TokenValidationResponse tokenValidationResponse = JsonUtility.FromJson<TokenValidationResponse>(webResponse);

                if (tokenValidationResponse.status == 200 && (token != null || token != string.Empty))
                {
                    getResponse(baseURl + "collectable/" + collectiveID, "GET", (String WebResponse) => 
                    {
                        GetElementID getElementID = JsonUtility.FromJson<GetElementID>(WebResponse);

                        if(getElementID.status == 200) 
                        {
                            foreach (TabData tabName in tabDatas)
                            {
                                foreach (ElementData tokenData in getElementID.data)
                                {
                                    if (tabName.transform.name == tokenData.title)
                                    {
                                        tabName.id = tokenData.id;

                                        StartCoroutine(GetTexture(tokenData.image, tabName.rawImage));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.Log(WebResponse);
                        }

                    });
                }
                else if (tokenValidationResponse.status == 400 || token == null || token == string.Empty)
                {

                    Debug.Log(webResponse);
#if !UNITY_EDITOR
                RedirectWebSite();
#endif
                }
            },json);
        }

        //Logout
        public static void GenerateToken()
        {
            GenerateToken generateToken = new GenerateToken()
            {
                type = "GameVerifyToken"
            };

            string json = JsonUtility.ToJson(generateToken);
            getResponse(baseURl + "users/generate-token", "POST", (String webResponse) =>
            {

                GenerateTokenResponse generateTokenResponse = JsonUtility.FromJson<GenerateTokenResponse>(webResponse);

                if (generateTokenResponse.status == 200)
                {
                    var token = generateTokenResponse.data.token;
                    Debug.Log(webResponse);
                    //js
#if !UNITY_EDITOR
                    Exit(token, StatueManager.StatueManagerInstance.collectionID, StatueManager.StatueManagerInstance.userID);
#endif
                }
            }, json);
        }

        [SerializeField] int totalAPI = 0;

        IEnumerator GetTexture(string url, RawImage image)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                totalAPI ++;

                Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                image.texture = myTexture;

                if (totalAPI == 5)
                {
                    onTokenValidate?.Invoke();
                }
            }
        }


        private static void getResponse(string url, string method, Action<string> callBack, string json = null)
        {
            UnityWebRequest webRequest = new UnityWebRequest();
            webRequest.url = url;
            webRequest.method = method;

            if (json != null)
            {
                byte[] encodedData = new UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(encodedData);
            }

            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("cache-data", "no-code");

            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();

            asyncOperation.completed += delegate (AsyncOperation operation)
            {

                if (operation.isDone)
                {
                    if (asyncOperation.webRequest.result == UnityWebRequest.Result.Success)
                    {
                        var response = asyncOperation.webRequest.downloadHandler.text;
                        callBack(response);
                    }
                    else
                    {
                        callBack(asyncOperation.webRequest.downloadHandler.text);
                    }
                }
            };

        }

        //call by js
        public void GetToken(string parameters)
        {
            string[] paramsArray = parameters.Split(',');

            string token = paramsArray[0];
            string collectiveID = paramsArray[1];
            string userID = paramsArray[2];
            TokenValidation(token,collectiveID,userID);
        }

        public void GetVideoPlayer(string videoURL)
        {
            InteractionPoints.videoURl = videoURL;
        }

        //private void Start()
        //{
        //    TokenValidation("bb8095xk0t", "ca7a7478-4dfd-4fac-8079-edca2654904c", "11111");
        //}
    }

    #region API Params

    public struct TokenValidation
    {
        public string id;
        public string type;
    }


    public struct GenerateToken
    {
        public string type;
    }
    #endregion


    #region API Response

    public struct TokenValidationResponse
    {
        public bool error;
        public string message;
        public int status;
        public string data;
        public string timestamp;
    }

    [Serializable]
    public struct GenerateTokenResponse
    {
        public int status;
        public bool error;
        public string message;
        public Data data;
        public string timestamp;
    }

    [Serializable]
    public struct Data
    {
        public string token;
    }

    [Serializable]
    public struct GetElementID
    {
        public int status;
        public bool error;
        public string message;
        public ElementData[] data;
        public string timestamp;
    }

    [Serializable]
    public struct ElementData
    {
        public string id;
        public string title;
        public string image;
        public string srNo;
    }
}
#endregion