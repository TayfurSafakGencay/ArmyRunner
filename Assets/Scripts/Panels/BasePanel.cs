using UnityEngine;
using UnityEngine.Rendering;

namespace Panels
{
  [RequireComponent(typeof(SortingGroup))]
  public abstract class BasePanel : MonoBehaviour
  {
    protected SortingGroup SortingGroup { get; private set; }

    public virtual void Awake()
    {
      SortingGroup = gameObject.GetComponent<SortingGroup>();
    }

    protected abstract void ChangePanelLayer(int layer);
  }
}