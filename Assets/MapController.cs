using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public string apiKey;
    public float lat = -33.85660618894087f;
    public float lon = 151.21500701957325f;
    public int zoom = 14;


    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;

    public enum type { roadmap, satellite, hybrid, terrain };
    public type mapType = type.roadmap;
    private string url = "";
    private int mapWidth = 640;
    private int mapHeight = 640;
    private Rect rect;
    private string apiKeyLast;
    private float latLast = -33.85660618894087F;
    private float lonLast = 151.21500701957325f;
    private int zoomLast = 14;
    private resolution mapResolutionLast = resolution.low;
    private type mapTypeLast = type.roadmap;
    private bool updateMap = true;
    private void Start()
    {
        StartCoroutine(GetGoogleMap());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);


    }
    private void Update()
    {
        if (updateMap && (apiKeyLast != apiKey || !Mathf.Approximately(latLast, lat) || !Mathf.Approximately(lonLast, lon) || zoomLast != zoom || mapResolutionLast != mapResolution || mapTypeLast != mapType))
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
        StartCoroutine(GetGoogleMap());
        updateMap = false;

    }
    IEnumerator GetGoogleMap()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?key=&signature=D4Xe_V98HAp-JIkozVdGPaD_PG0=" + lat + "," + lon + "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + mapResolution + "&mapType=" + mapType + "$key=" + apiKey;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WMW ERROR: " + www.error);
        }

        else
        {
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            apiKeyLast = apiKey;
            latLast = lat;
            lonLast = lon;
            zoomLast = zoom;
            mapResolutionLast = mapResolution;
            mapTypeLast = mapType;
            updateMap = true;
        }


    }


}
