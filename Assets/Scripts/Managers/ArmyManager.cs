using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Army.Soldiers;
using Tools.Formations_main.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

namespace Managers
{
  public class ArmyManager : MonoBehaviour
  {
    public static ArmyManager Instance;

    [SerializeField]
    private Transform _soldierContainer;

    private readonly Dictionary<string, Soldier> _soldiers = new();
    
    private FormationBase _formation;
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);

      _formation = GetComponent<FormationBase>();

      for (int i = 0; i < 2; i++)
      {
        CreateSoldier();
      }
    }

    private const string _soldierAddressableKey = "Soldier";

    private const GunKey _standardGunKey = GunKey.MP5;

    public async Task CreateSoldier()
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle =
        Addressables.InstantiateAsync(_soldierAddressableKey, Vector3.zero, quaternion.identity, _soldierContainer);

      await asyncOperationHandle.Task;

      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
      {
        Soldier soldier = asyncOperationHandle.Result.GetComponent<Soldier>();
        soldier.SetKey(GenerateUniqueKey());

        soldier.EquipGun(_standardGunKey);
        
        AddSoldier(soldier);
      }
      else
      {
        Debug.LogError("Soldier could not created!");
      }
    }

    private void AddSoldier(Soldier soldier)
    {
      _soldiers.Add(soldier.Key, soldier);
      
      SoldierCountChanged();
    }

    public void RemoveSoldier(Soldier soldier)
    {
      if (_soldiers.ContainsKey(soldier.Key))
      {
        _soldiers.Remove(soldier.Key);
        
        // SoldierCountChanged();

        soldier.transform.parent = transform;
        Destroy(soldier.gameObject, 4);

        if (_soldiers.Count == 0)
        {
          GameManager.Instance.GameFinished(false);
        }
      }
      else
      {
        Debug.Log("There is no such soldier!");
      }
    }

    private void SoldierCountChanged()
    {
      _formation.SetAmount(_soldiers.Count);

      int index = 0;
      
      foreach (Vector3 pos in _formation.EvaluatePoints())
      {
        _soldiers.ElementAt(index).Value.ChangeTargetPosition(transform.position + pos);
        
        index++;
      }
    }

    public Transform GetArmyContainer()
    {
      return _soldierContainer;
    }
    
    public List<Soldier> GetSoldiers()
    {
      return _soldiers.Values.Select(soldier => soldier).ToList();
    }

    #region Creating Soldier Key

    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int keyLength = 8;
    private readonly List<string> generatedKeys = new();
    private readonly Random random = new();

    private string GenerateUniqueKey()
    {
      string newKey;

      do
      {
        newKey = GenerateRandomKey();
      } while (generatedKeys.Contains(newKey));

      generatedKeys.Add(newKey);
      return newKey;
    }

    private string GenerateRandomKey()
    {
      return new string(Enumerable.Repeat(characters, keyLength)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion
  }
}