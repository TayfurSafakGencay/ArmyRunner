using System.Collections.Generic;
using UnityEngine;

namespace Formations_main.Scripts
{
  public abstract class FormationBase : MonoBehaviour
  {
    [SerializeField]
    [Range(0, 1)]
    protected float _noise = 0;

    [SerializeField]
    protected float Spread = 1;

    public abstract IEnumerable<Vector3> EvaluatePoints();

    public virtual void SetAmount(int amount) { }

    public Vector3 GetNoise(Vector3 pos)
    {
      float noise = Mathf.PerlinNoise(pos.x * _noise, pos.z * _noise);

      return new Vector3(noise, 0, noise);
    }
  }
}