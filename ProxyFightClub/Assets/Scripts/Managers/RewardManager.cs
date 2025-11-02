using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    private int _statPoints { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GiveReward()
    {
        
    }


    public bool SpendPoints(int cost)
    {
        if (_statPoints < cost) return false;
        
        _statPoints -= cost;

        return true;
    }
}

