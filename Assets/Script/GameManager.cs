using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int enemyLeft = 10;  //# ��
    public float timeLeft = 60; //�����ð�

    bool isPlaying = true;//ó���� �������� ���·� ����

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject); //���ӸŴ��������� �� ������� �ı�X->������ �Ѿ�� ������ ���O
    }

    void Update()
    {
        if (isPlaying)
        {//�� �÷������϶��� �ð��� ����!
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
        isPlaying = false; //���ӿ��������� �Ѿ�� ���� ����X
        SceneManager.LoadScene("GameOverScene"); 
    }

}
