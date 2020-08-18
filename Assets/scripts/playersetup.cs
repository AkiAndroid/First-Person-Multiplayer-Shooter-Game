using UnityEngine.Networking;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Player))]
public class playersetup : NetworkBehaviour
{   [SerializeField] 
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayer="RemotePlayer";

    [SerializeField]
    string dontDrawLayer = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayer));
        }

        GetComponent<Player>().Setup();

    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        
        obj.layer = newLayer;

        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayer);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        GameManager.UnRegisterPlayer(transform.name);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }


}
