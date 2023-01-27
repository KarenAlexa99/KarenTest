using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private EnemyController[] enemys;
    [SerializeField] private PlaceController[] places;
    [SerializeField] private KeyController[] keys;
    [SerializeField] private PlayerController player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InventorySystem inventory;

    [Header("Sounds")]
    [SerializeField] private AudioClip gameoverClip;
    [SerializeField] private AudioClip keyClip;

    private void Start()
    {
        player.OnTakeDamage += PlayerTakeDamage;
        player.OnGameOver += GameOver;

        uiManager.OnRestart += Restart;

        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].OnGetKey += PlayerGetKey;
        }
    }

    private void Update()
    {
        player.Initialized();

        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].Initialized();
        }
    }

    private void PlayerTakeDamage()
    {
        uiManager.CurrentPlayerLife(player.curHealth, player.maxiHealth);
    }

    private void PlayerGetKey()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].isObtained && keys[i].typeOfPlaceOpen == places[i].typeOfPlace)
            {
                inventory.GetKey();
                SoundManager.Instance.PlaySound(keyClip, 0.3f);
                places[i].isOpen = true;
            }
        }
    }

    private void PlayerDisable()
    {
        player.OnTakeDamage -= PlayerTakeDamage;
        player.OnGameOver -= GameOver;
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].OnGetKey -= PlayerGetKey;
        }
    }

    private void GameOver()
    {
        SoundManager.Instance.PlaySound(gameoverClip, 0.5f);
        uiManager.ShowGameOver();
        player.gameObject.SetActive(false);
        LeanTween.delayedCall(0.5f, () => PlayerDisable());
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
