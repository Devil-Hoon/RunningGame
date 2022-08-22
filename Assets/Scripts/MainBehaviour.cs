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
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
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
