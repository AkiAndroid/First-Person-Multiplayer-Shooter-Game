using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isdead = false;
    public bool isdead
    {
        get { return _isdead; }
        protected set { _isdead = value; }
    }



    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    public GameObject deathEffect;

    [SerializeField]
    public GameObject[] disableGameObjectOnDeath;



    [SyncVar]
    private int currentHealth;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];

        for(int i=0 ; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefault();
    }

    //[ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isdead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + "health" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isdead = true;

        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        GameObject gfcIns= (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gfcIns, 3f);


        Debug.Log(transform.name + "is DEAD");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.gameSettings.respawnTime);

        SetDefault();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }

    public void SetDefault()
    {
        isdead = false;

        currentHealth = maxHealth;

        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }


        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }
    }



}
