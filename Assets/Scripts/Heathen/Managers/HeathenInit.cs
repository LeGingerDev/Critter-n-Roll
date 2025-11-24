using Heathen.SteamworksIntegration;
using Heathen.SteamworksIntegration.API;
using UnityEngine;

public class HeathenInit : MonoBehaviour
{

    private void Awake()
    {
        App.evtSteamInitialized.AddListener(() =>
        {
            Debug.Log("Steamworks Initialized");
        });
        App.evtSteamInitializationError.AddListener((error) =>
        {
            Debug.LogError("Steamworks Initialization Error: " + error);
        });
        App.Client.Initialize(2069730);
        
    
    }


}
