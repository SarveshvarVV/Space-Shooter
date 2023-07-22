using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text score_text;
    [SerializeField]
    Text gameover_text;
    [SerializeField]
    Text restart_text;

    private GameManager _gm;

    [SerializeField]
    Image LivesImg;
    [SerializeField]
    Sprite[] _liveSprite;
    // Start is called before the first frame update
    void Start()
    {
        score_text.text = "Score: " + 0;
        gameover_text.gameObject.SetActive(false);
        restart_text.gameObject.SetActive(false);
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScore(int player_score)
    {
        score_text.text = "Score: " + player_score.ToString();
    }

    public void UpdateLives(int CurrentLives)
    {
        LivesImg.sprite = _liveSprite[CurrentLives];
        if(CurrentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gm.GameOver();
        gameover_text.gameObject.SetActive(true);
        restart_text.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        StartCoroutine(RestartFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        while(true)
        {
            gameover_text.text = "Game Over";
            yield return new WaitForSeconds(0.25f);
            gameover_text.text = "";
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator RestartFlicker()
    {
        while (true)
        {
            restart_text.text = "Press the 'R' key to restart";
            yield return new WaitForSeconds(0.5f);
            restart_text.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
