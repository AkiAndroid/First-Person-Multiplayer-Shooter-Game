using UnityEngine.Networking;
using UnityEngine;
[RequireComponent(typeof(weaponManager))]
public class playershooting : NetworkBehaviour
{ 
    

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private weaponManager weaponManager;
    private playerweapon currentweapon;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("working");
            this.enabled = false;
        }

        weaponManager = GetComponent<weaponManager>();

        
    }

    void Update()
    {
        currentweapon = weaponManager.GetcurrentWeapon();
        if (currentweapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        else
        {
            if (Input.GetButtonDown("Fire1"))
                {
                InvokeRepeating("Shoot", 0f, 1f / currentweapon.fireRate);

            }
            else if (Input.GetButtonUp("Fire1")) {

                CancelInvoke("Shoot");
            }
        } 
    }

   

   

    
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        

        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentweapon.range, mask))
        {
            if (hit.collider.tag == "Player" && hit.collider.gameObject.layer==LayerMask.NameToLayer("RemotePlayer"))
            {
                playerShotCmd(hit.collider.name, currentweapon.damage);
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
