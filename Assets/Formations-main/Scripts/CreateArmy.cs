using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Formations_main.Scripts
{
  public class CreateArmy : MonoBehaviour
  {
    private FormationBase _formation;

    public FormationBase Formation
    {
      get
      {
        if (_formation == null) _formation = GetComponent<FormationBase>();
        return _formation;
      }
      set => _formation = value;
    }

    [SerializeField]
    private GameObject _unitPrefab;

    [SerializeField]
    private float _unitSpeed = 2;

    private readonly List<GameObject> _spawnedUnits = new();
    private List<Vector3> _points = new();

    private void Update()
    {
      SetFormation();
    }

    private void SetFormation()
    {
      _points = Formation.EvaluatePoints().ToList();

      if (_points.Count > _spawnedUnits.Count)
      {
        IEnumerable<Vector3> remainingPoints = _points.Skip(_spawnedUnits.Count);
        Spawn(remainingPoints);
      }
      else if (_points.Count < _spawnedUnits.Count)
      {
        Kill(_spawnedUnits.Count - _points.Count);
      }

      for (int i = 0; i < _spawnedUnits.Count; i++)
      {
        _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
      }
    }

    private void Spawn(IEnumerable<Vector3> points)
    {
      foreach (Vector3 pos in points)
      {
        GameObject unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, transform);
        _spawnedUnits.Add(unit);
      }
    }

    private void Kill(int num)
    {
      for (int i = 0; i < num; i++)
      {
        GameObject unit = _spawnedUnits.Last();
        _spawnedUnits.Remove(unit);
        Destroy(unit.gameObject);
      }
    }
  }
}