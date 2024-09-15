using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //���� �ٽý��� ��ư �Լ� ȣ�� ����-(%) 


public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI enemyKilledLabel;
    public TextMeshProUGUI timeLeftLabel;

    void Start()
    {
        Cursor.visible = true;  //���Ӿ����� ���콺 Ŀ�� ������� â���� �ٽ� ���̰� ��
        Cursor.lockState = CursorLockMode.None;// ���콺 ���ɸ��� Ǯ����

        int enemyLeft = GameManager.instance.enemyLeft;
        float timeLeft = GameManager.instance.timeLeft;   //�׸Ŵ����� �ִ� ���� ������ �ð� ������ ������
    
        if(enemyLeft <= 0)
        {
            titleLabel.text = "Cleared!";
        }
        else
        {
            titleLabel.text = "Game Over...";
        }

        enemyKilledLabel.text = "Enemy Killed: " + (10 - enemyLeft);
        timeLeftLabel.text = "Time Left: " + timeLeft.ToString("#.#");

        Destroy(GameManager.instance.gameObject);//�׸Ŵ��� ����->���� �Ѿ�� �׸Ŵ����� ������ ���� ��������
    }

    public void PlayAgainPressed()//-(%)
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Quit() //-(%)
    {
        Application.Quit(); //�����Ϳ��� �۵�X,������ ���� ����� �۵�)
    }
}//GOVM
