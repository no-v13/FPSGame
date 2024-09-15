using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int enemyLeft = 10;  //# 적
    public float timeLeft = 60; //남은시간

    bool isPlaying = true;//처음에 게임중인 상태로 시작

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject); //게임매니저옵젝이 씬 사라질때 파괴X->다음씬 넘어ㅏ서 데이터 사용O
    }

    void Update()
    {
        if (isPlaying)
        {//겜 플레잉중일때만 시간을 뺀다!
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                GameOverScene();
            }
        }
    }

    public void EnemyDied()
    {
        enemyLeft--;

        if (enemyLeft <= 0)
        {
            GameOverScene();
        }
    }

    public void GameOverScene()
    {
        isPlaying = false; //게임오버씬으로 넘어갈때 게임 진행X
        SceneManager.LoadScene("GameOverScene"); 
    }

}
