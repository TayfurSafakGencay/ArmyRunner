using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rewards
{
  public class RewardPair : MonoBehaviour
  {
    public List<GameObject> Rewards;
    
    private List<GameObject> _randomTwo;

    private void Awake()
    {
      _randomTwo = Rewards.OrderBy(x => Random.value).Take(2).ToList();

      int index = 0;
      foreach (GameObject reward in _randomTwo)
      {
        GameObject rewardObject = Instantiate(reward, transform);

        rewardObject.transform.localPosition = index switch
        {
          0 => new Vector3(-3f, 0, 0),
          1 => new Vector3(3f, 0, 0),
          _ => rewardObject.transform.localPosition
        };

        index++;
      }
    }
  }
}