using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public Action OnRestart;

    [SerializeField] private Image playerLifeBarFill;
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private Button restartBtn;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClip;

    private void Awake()
    {
        gameOverPanel.alpha = 0;
        restartBtn.onClick.AddListener(Restart);
    }

    public void CurrentPlayerLife(float _curHealth, float _maxHealth)
    {
        playerLifeBarFill.fillAmount = _curHealth / _maxHealth;
    }

    public void ShowGameOver()
    {
        LeanTween.scale(gameOverPanel.gameObject, Vector3.one, 1).setEaseInOutBounce();
        gameOverPanel.alpha = 1;
    }

    private void Restart()
    {
        SoundManager.Instance.PlaySound(buttonClip, 0.5f);
        LeanTween.delayedCall(1, () =>
        {
            if (OnRestart != null)
            {
                OnRestart();
                gameOverPanel.gameObject.SetActive(false);
            }
        });
    }
}
