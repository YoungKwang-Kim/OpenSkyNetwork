using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OpenSkyRequestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OpenSkyRequest());
    }

    IEnumerator OpenSkyRequest()
    {
        string url = "https://opensky-network.org/api/states/all?time=1458564121&icao24=3c6444";

        UnityWebRequest request = new UnityWebRequest();
        using(request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if(request.result != UnityWebRequest.Result.Success)
            {
                // API 호출이 실패하면 에러 표시
                Debug.Log(request.error);
            }
            else
            {
                string result = request.downloadHandler.text;
                SkyData skydata = JsonUtility.FromJson<SkyData>(result);
            }
        }
    }

    public DateTime ConvertFromUnixToDate(double timeStamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds(timeStamp);
    }

    public double ConvertFromDateToUnix(string dateString)
    {
        DateTime dateTime = DateTime.ParseExact(dateString, "MM/dd/yyyy HH:mm:ss", null);
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        TimeSpan diff = dateTime - origin;
        return Math.Floor(diff.TotalSeconds);
    }
}
