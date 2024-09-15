using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //씬에 다시시작 버튼 함수 호출 위함-(%) 


public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI enemyKilledLabel;
    public TextMeshProUGUI timeLeftLabel;

    void Start()
    {
        Cursor.visible = true;  //게임씬에서 마우스 커서 숨겼던거 창에서 다시 보이게 함
        Cursor.lockState = CursorLockMode.None;// 마우스 락걸린거 풀어줌

        int enemyLeft = GameManager.instance.enemyLeft;
        float timeLeft = GameManager.instance.timeLeft;   //겜매니저에 있는 남은 적수랑 시간 변수를 가져옴
    
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

        Destroy(GameManager.instance.gameObject);//겜매니저 없앰->씬이 넘어가고 겜매니저가 여러개 생성 막기위함
    }

    public void PlayAgainPressed()//-(%)
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Quit() //-(%)
    {
        Application.Quit(); //에디터에서 작동X,하지만 게임 실행시 작동)
    }
}//GOVM
