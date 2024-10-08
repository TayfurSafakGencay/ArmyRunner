using System.Collections.Generic;
using UnityEngine;

namespace Tools.Formations_main.Scripts
{
  public class RadialFormation : FormationBase
  {
    [SerializeField]
    private int _amount;

    [SerializeField]
    private float _radius = 1;

    [SerializeField]
    private float _radiusGrowthMultiplier = 0;

    [SerializeField]
    private float _rotations = 1;

    [SerializeField]
    private int _rings = 1;

    [SerializeField]
    private float _ringOffset = 1;

    [SerializeField]
    private float _nthOffset = 0;

    public override IEnumerable<Vector3> EvaluatePoints()
    {
      int amountPerRing = _amount / _rings;
      float ringOffset = 0f;
      for (int i = 0; i < _rings; i++)
      {
        for (int j = 0; j < amountPerRing; j++)
        {
          float angle = j * Mathf.PI * (2 * _rotations) / amountPerRing + (i % 2 != 0 ? _nthOffset : 0);

          float radius = _radius + ringOffset + j * _radiusGrowthMultiplier;
          float x = Mathf.Cos(angle) * radius;
          float z = Mathf.Sin(angle) * radius;

          Vector3 pos = new(x, 0, z);

          pos += GetNoise(pos);

          pos *= Spread;

          yield return pos;
        }

        ringOffset += _ringOffset;
      }
    }

    public override void SetAmount(int amount)
    {
      _amount = amount;
    }
  }
}