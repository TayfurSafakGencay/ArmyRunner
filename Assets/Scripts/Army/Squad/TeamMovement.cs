using Managers;
using UnityEngine;

namespace Army.Squad
{
  public class TeamMovement : MonoBehaviour
  {
    private ArmyManager _armyManager;

    private GameState _gameState;

    public float _moveSpeed = 2;

    [SerializeField]
    private float _xLimit = 5f;

    private Vector2 touchStartPos;

    private void Awake()
    {
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;

      OnGameStateChanged(GameManager.Instance.GameState);

      _armyManager = ArmyManager.Instance;
    }

    private void OnGameStateChanged(GameState gameState)
    {
      _gameState = gameState;
    }

    private void Update()
    {
      if (_gameState != GameState.StartGame)
        return;

      if (Input.touchCount > 0)
      {
        Move();
      }
    }

    private void HandleMovement()
    {
      if (Input.touchCount <= 0) return;
      Touch touch = Input.GetTouch(0);

      switch (touch.phase)
      {
        case TouchPhase.Began:
          touchStartPos = touch.position;
          break;
        case TouchPhase.Moved:
        case TouchPhase.Stationary:
        {
          Vector2 touchCurrentPos = touch.position;
          float touchDeltaX = (touchCurrentPos.x - touchStartPos.x) / Screen.width;

          float move = touchDeltaX * _moveSpeed * Time.deltaTime;
          Vector3 newPosition = transform.position + new Vector3(move, 0, 0);

          newPosition.x = Mathf.Clamp(newPosition.x, -_xLimit, _xLimit);
          transform.position = newPosition;
          touchStartPos = touchCurrentPos;
          break;
        }
      }
    }

    private void Move()
    {
      Touch touch = Input.GetTouch(0);

      float halfScreen = Screen.width / 2;
      float xPos = (touch.position.x - halfScreen) / halfScreen;
      float finalXPos = Mathf.Clamp(xPos * _xLimit, -_xLimit, _xLimit);

      transform.position = Vector3.MoveTowards(transform.position, new Vector3(finalXPos, 0.22f, 0), Time.deltaTime * _moveSpeed);
    }
  }
}