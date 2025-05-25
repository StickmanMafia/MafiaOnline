using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public abstract class Controller : MonoBehaviour
{
    [SerializeField]
    protected LocalizedStringTable LocalizedStringTable;

    protected GameObject PlayerParent;
    protected IEnumerable<PlayerData> Players;

    protected PlayerData Me => Players?.FirstOrDefault(n => n.IsMine);
    protected PlayerData Master => Players?.FirstOrDefault(n => n.PhotonPlayer.IsMasterClient);
    protected int currentMap;
    public GameObject MyCard;
    public bool insufficiently;

    protected virtual void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainLocation")
        {
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("language")];
        }
        
        PlayerParent = PlayerParentFetchService.Fetch();
        currentMap = PlayerPrefs.GetInt("currentMap");
    }
    public void CheckMyCard() {
        
            float minDistance = float.MaxValue;
            GameObject closestCard = null;
            foreach(GameObject card in GameObject.FindGameObjectsWithTag("Card")) {
                float distance = Vector3.Distance(transform.position, card.transform.position);
                if(distance < minDistance) {
                    minDistance = distance;
                    closestCard = card;
                }
            }
            if(closestCard == MyCard) return;
            MyCard = closestCard;
        
    }
    private void Update() {
        FetchPlayers();
        
        if(SceneManager.GetActiveScene().name != "MainLocation") return;
        foreach(var player in Players){
            /*if(player.GameObject.GetComponent<PhotonVoiceView>().IsSpeaking){
                player.GameObject.GetComponent<VoiceAnimation>().AnimLerp();
            }
            else{
                player.GameObject.GetComponent<VoiceAnimation>().DeanimLerp();
            }*/
        }
    }
    public string GetLocalizedString(string key) => LocalizedStringTable.GetTable().GetEntry(key).Value;

    protected void FetchPlayers()
    {
        /*if (PlayerParent != null)
        {
            Players = PlayerFetchService.FetchPlayerData(PlayerParent);
        }
        else
        {
            Debug.LogError("PlayerParent is not initialized.");
        }*/
    }

    protected void GoQueue()
    {
        Debug.LogError("GOQUEUE");
        if (Master != null)
        { 
           Master.PhotonView.RpcSecure("ActionQueueForward", RpcTarget.MasterClient, true);

        }
        else
        {
            Debug.LogError("Master is not initialized.");
        }
    }
}
