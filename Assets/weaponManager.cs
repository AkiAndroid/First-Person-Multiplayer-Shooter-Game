using UnityEngine.Networking;
using UnityEngine;

public class weaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    public playerweapon primaryweapon;

    private playerweapon currentweapon;

    private weaponGraphics currentGraphics;

    private void Start()
    {
        EquipWeapon(primaryweapon);
    }

    public playerweapon GetcurrentWeapon()
    {
        return currentweapon;
    }

    public weaponGraphics GetcurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(playerweapon _weapon)
    {
        currentweapon = _weapon;

        GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<weaponGraphics>();

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
           

        
        
           
    }
}
