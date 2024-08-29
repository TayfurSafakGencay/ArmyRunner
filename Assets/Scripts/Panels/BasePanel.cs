using UnityEngine;
using UnityEngine.Rendering;

namespace Panels
{
  [RequireComponent(typeof(SortingGroup))]
  public abstract class BasePanel : MonoBehaviour
  {
    private SortingGroup _sortingGroup;

    public virtual void Awake()
    {
      _sortingGroup = gameObject.GetComponent<SortingGroup>();
    }

    protected void ChangePanelLayer(int layer)
    {
      _sortingGroup.sortingOrder = layer;
    }
  }
}