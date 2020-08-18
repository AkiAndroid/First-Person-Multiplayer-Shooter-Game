using UnityEngine.Networking;
using UnityEngine;

public class playershooting : NetworkBehaviour
{
    public playerweapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

     void Start()
    {
        if (cam == null)
        {
            Debug.LogError("working");
            this.enabled = false;
        }
    }

   void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            if (hit.collider.tag == "Player")
            {
                playerShotCmd(hit.collider.name, weapon.damage);
            }
        }
    }

   
    
    void playerShotCmd(string _playerID, int _damage)
    {
        Debug.Log(_playerID + "has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);

    }



}
