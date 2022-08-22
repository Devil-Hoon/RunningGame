using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using GooglePlayGames;

public class MainBehaviour : MonoBehaviour
{

    public Button settingButton;
    public Button characterButton;
    public Button startButton;
    public Button storeButton;
    public GameObject settingModal;
    private bool settingModalStatus = false;

    private User user;

    void Start()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
        this.settingModal.SetActive(this.settingModalStatus);

        this.settingButton.onClick.AddListener(() => this.toggleSettingModal());
        //this.characterButton.onClick.AddListener(() => this.loadScene("Character"));
        this.startButton.onClick.AddListener(() => this.loadScene("GamePlay"));
        //this.storeButton.onClick.AddListener(() => this.loadScene("Store"));

        this.user = StaticClass.UserInfo;

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void toggleSettingModal()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
		if (auth != null)
		{
            auth.SignOut();
        }

        PlayGamesPlatform platform = Social.Active as PlayGamesPlatform;
		if (platform != null)
		{
            platform.SignOut();
		}

        Debug.Log("Sign Out");
        //this.settingModal.SetActive(!this.settingModalStatus);
        //this.settingModalStatus = !this.settingModalStatus;
    }



    private void loadScene(string name)
    {
        if (this.settingModalStatus)
        {
            this.toggleSettingModal();
        } 
        else
        {
            SceneManager.LoadScene(name);
        }
    }

}
