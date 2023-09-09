using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class Launcher : MonoBehaviourPunCallbacks, IDisposable
{
    [SerializeField] private Button _disconnectButton;

    [SerializeField] private Color _connectColor;
    [SerializeField] private Color _disconnectColor;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        _disconnectButton.onClick.AddListener(Disconnect);

        Connect();
    }

    private void Connect()
    {
        if (PhotonNetwork.IsConnected)
            return;

        _disconnectButton.image.color = _connectColor;
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = Application.version;
        
    }

    private void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            _disconnectButton.image.color = _disconnectColor;
            Debug.Log("Disconnect from the server");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }

    public void Dispose()
    {
        _disconnectButton.onClick?.RemoveAllListeners();
    }
}
