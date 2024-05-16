using TMPro;
using UnityEngine;

public enum GameState
{
    Idle,
    Playing,
    LevelEnd
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Castle { get; private set; }

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI shotsText;
    [SerializeField] private Vector3 castlePosition;
    [SerializeField] private GameObject[] castles;

    private int _level;
    private int _levelMax;
    private int _shotsTaken;
    private GameState _state = GameState.Idle;

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _levelMax = castles.Length;
        StartLevel();
    }

    private void Update()
    {
        if (_state == GameState.Playing && Goal.GoalMet)
        {
            _state = GameState.LevelEnd;
            FollowCamera.Instance.SwitchView(FollowCamera.View.Both);
            Invoke(nameof(NextLevel), 2f);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void StartLevel()
    {
        if (Castle) Destroy(Castle);

        Projectile.Clear();

        Castle = Instantiate(castles[_level], castlePosition, Quaternion.identity);
        Goal.GoalMet = false;

        UpdateGUI();

        _state = GameState.Playing;
        FollowCamera.Instance.SwitchView(FollowCamera.View.Both);
    }

    private void NextLevel()
    {
        _level = (_level + 1) % _levelMax;

        if (_level == 0) _shotsTaken = 0;

        StartLevel();
    }

    private void UpdateGUI()
    {
        levelText.text = $"Level: {_level + 1} of {_levelMax}";
        shotsText.text = $"Shots Taken: {_shotsTaken}";
    }

    public void ShotFired()
    {
        _shotsTaken++;
        UpdateGUI();
    }
}
