using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Data;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Android;
using GooglePlayGames.OurUtils;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


public class TitleBehaviour : MonoBehaviour {

	public GameObject panel;
	public Button anonymous;
	public Button google;
	public Button close;
	public Image bottomImage;
	public GameObject namePanel;

	private DatabaseReference reference = null;

	public string FireBaseId = string.Empty;

	public static TitleBehaviour Instance = null;
	
	//public string clientId = "349552215810-ienp316f8qo4nhcsip9p2405bvlje4m1.apps.googleusercontent.com";

	private int userIdx;
	public InputField input;
	public Text name;
	public Text msg;
	public Button exist;
	public Button setBtn;
	private bool check = false;

	private FirebaseAuth auth = null;
	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
	Firebase.Auth.FirebaseUser user;

	

	void Start () {

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
		/*
			// 로컬스토리지에서 유저아이디 가져오고, 파이어베이스 토큰이나 그런걸로 검증해야함.
		*/
		this.panel.SetActive(false);
		this.namePanel.SetActive(false);
		this.google.onClick.AddListener(() => this.loginFunction());
		this.anonymous.onClick.AddListener(() => this.anonymousSignIn());
		this.close.onClick.AddListener(() => {
			this.panel.SetActive(false);
			this.bottomImage.enabled = true;
		});
		this.exist.onClick.AddListener(() => this.callNameExist(this.name.text));
		this.input.onValueChanged.AddListener(checkInput);
		this.setBtn.onClick.AddListener(() => this.setName());

		//InvokeRepeating("changeAlpha", 0, 2);

		// m_saved_state = GoogleSavedFileState.GOOGLE_SAVED_STATE_NONE;


		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
			.Builder()
			.EnableSavedGames()
			.RequestIdToken()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		//var configBuilder = new PlayGamesClientConfiguration.Builder();
		//var config = configBuilder
		//	.RequestServerAuthCode(false)
		//	.Build();

		//PlayGamesPlatform.InitializeInstance(config);
		//PlayGamesPlatform.Activate();
		auth = FirebaseAuth.DefaultInstance;

		Instance = this;

		// 파이어베이스 구글플레이 버전 체크
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available)
			{
				// Create and hold a reference to your FirebaseApp,
				// where app is a Firebase.FirebaseApp property of your application class.
				// app = Firebase.FirebaseApp.DefaultInstance;
				InitializeFirebase();

				// Set a flag here to indicate whether Firebase is ready to use by your app.
			}
			else
			{
				UnityEngine.Debug.LogError(System.String.Format(
				  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				// Firebase Unity SDK is not safe to use here.
			}
		});

		reference = FirebaseDatabase.DefaultInstance.RootReference;

		/*DataSnapshot snapshot = null;
		string result = string.Empty;
		JArray json;

		FirebaseDatabase.DefaultInstance.GetReference("ranking").Child("idx").GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted)
			{
				// handle err
				Debug.LogError("reference getValueAsync ::: ");
			}
			else if (task.IsCompleted)
			{
				// handle complete
				snapshot = task.Result;
			}

		});

		result = snapshot.GetRawJsonValue();

		json = JArray.Parse(result);
*/


	}

	

	void Update () {


		if (this.panel.activeSelf == false && this.namePanel.activeSelf == false && Input.GetMouseButtonDown(0))
        {

			Debug.Log(user);

			this.bottomImage.enabled = false;

			// 로그인 및 게임 실행
			if(user == null || string.IsNullOrWhiteSpace(user.UserId) || user.UserId == "")
            {
				// 로그인이 안됐거나 검증 실패 시 Panel 나오게함.
				this.panel.SetActive(true);
            }
			else
            {
				bool result = getUserInfo();
				if (!result)
				{
					bool result2 = Database.insertUser(user.UserId);
					if (result2)
					{
						User userInfo = Database.getUserInfo(user.UserId);
						StaticClass.UserInfo = userInfo;
						this.namePanel.SetActive(true);
						return;
					}
					else
					{
						this.panel.SetActive(true);
						return;
					}
				}
				if(StaticClass.UserInfo.Name.Length == 0)
                {
					// 닉네임 비었으면 닉네임설정
					this.namePanel.SetActive(true);
                }
				else
                {
					// 메인화면으로 이동
					MoveMain();
                }

			}
        }
	}

	void checkInput(string text)
    {
		this.name.text = text;
		this.msg.text = "";
		this.check = false;
    }

	private void callNameExist(string name)
    {

		string str = Database.getNameExist(name);

		if(name.Length < 1)
        {
			this.msg.text = "닉네임을 입력해주세요.";
			this.msg.color = Color.green;
			this.check = false;
			return;
		}

		// 서버에러
		if(str == "failed")
        {
			this.msg.text = "서버와 통신에 실패했습니다.";
			this.msg.color = Color.red;
		}
		else if (str == "false")
        {
			this.msg.text = "중복된 닉네임입니다.";
			this.msg.color = Color.red;
		}
		else
        {
			this.msg.text = "사용 가능한 닉네임입니다.";
			this.msg.color = Color.green;
			this.check = true;
		}

    }

	private void setName()
    {
		string uid = StaticClass.UserInfo.UserId;
		string name = this.name.text;

		if(this.check == false)
        {
			this.msg.text = "중복 확인을 해주세요.";
			this.msg.color = Color.red;
			return;
		}

		bool result = Database.setUserName(uid, name);

		if(result)
        {
			MoveMain();
        } 
		else
        {
			this.msg.text = "서버와 통신에 실패했습니다.";
			this.msg.color = Color.red;
			return;
		}
    }

	private bool getUserInfo()
    {
		bool result = false;

		User userInfo = Database.getUserInfo(user.UserId);
		StaticClass.UserInfo = userInfo;

		if(userInfo != null)
        {
			result = true;
        }

		return result;
    }


    void OnDestroy()
    {
		auth.StateChanged -= AuthStateChanged;
		auth = null;
    }

    private void InitializeFirebase()
    {
		Debug.Log("Setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);

    }

	private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
		if (auth.CurrentUser != user)
        {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null)
            {
				Debug.Log("Signed out " + user.UserId);
            }
			user = auth.CurrentUser;
			if (signedIn)
            {
				Debug.LogFormat("Signed in : <color='red'>{0}</color>", user.UserId);
            }
        }
    }

	private async void anonymousSignIn()
    { 
		await Task.Run(() =>
		{

			auth.SignInAnonymouslyAsync().ContinueWith(task =>
			{
				if (task.IsCanceled)
				{
					Debug.LogError("SignInAnonymouslyAsync was canceled.");
					return;
				}
				if (task.IsFaulted)
				{
					Debug.LogError("SignInAnonymouslyAsync encountered an errr : " + task.Exception);
					return;
				}

				user = task.Result;
				Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);

				bool result = Database.insertUser(user.UserId);

				Debug.Log("done");

				DataSnapshot snapshot = null;
				// MoveMain();
				if (result)
				{
					bool result2 = getUserInfo();
					if (!result2) return;

					this.namePanel.SetActive(true);

					return;
				}
				else
				{

				}
			});
		});

	}

	public async void ClickLogin()
    {
		Debug.Log("[Auth] Sign Login ");

		await Task.Run(() =>
		{
			auth.SignInAnonymouslyAsync().ContinueWith(task =>
			{
				if (task.IsCanceled)
				{
					Debug.LogError("SignInAnonymouslyAsync was canceled.");
					return;
				}
				if (task.IsFaulted)
				{
					Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
					return;
				}

				user = task.Result;
				Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
				bool result = Database.insertUser(user.UserId);

				Debug.Log("done");

				DataSnapshot snapshot = null;
				// MoveMain();
				if (result)
				{
					User userInfo = Database.getUserInfo(user.UserId);
					StaticClass.UserInfo = userInfo;
					this.namePanel.SetActive(true);
					return;
				}
			});

		});
		
    }

	public void ClickLogOut()
    {
		Debug.Log("[Auth] Sign Out ");
		auth.SignOut();
    }

	void loginFunction()
    {
		// 게임센터 로그인 시도
		Debug.Log("GameCenter Login");
		if (!Social.localUser.authenticated)
        {
			Social.localUser.Authenticate((bool success,string error) =>
			{
				if (success)
				{
					Debug.Log("google game service success");
					StartCoroutine(TryFirebaseLogin());
				}
				else
				{
					Debug.Log("google game service Fail (" + error + ")");
				}
			});
        }
	}

	public void TryGoogleLogout()
    {
		if (Social.localUser.authenticated)
        {
			PlayGamesPlatform.Instance.SignOut();
			auth.SignOut();
        }
    }


	IEnumerator TryFirebaseLogin()
    {
		while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
			yield return null;
		string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

		Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
		auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
		{
			if (task.IsCanceled)
            {
				Debug.Log("SignInWithCredentialAsync was canceled");
				return;
            }
			if (task.IsFaulted)
            {
				Debug.Log("SignInWithCredentialAsync encounted an error : " + task.Exception);
				return;
            }

			Firebase.Auth.FirebaseUser newUser = task.Result;
			FireBaseId = newUser.UserId;
			bool result = Database.insertUser(FireBaseId);

			Debug.Log("done");

			DataSnapshot snapshot = null;
			// MoveMain();
			if (result)
			{
				bool result2 = getUserInfo();
				if (!result2) return;

				this.namePanel.SetActive(true);
				return;
			}
			else
			{

			}
			MoveMain();
		});
    }

	private void MoveMain()
    {
		AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
    }

	int getLastUserIdx()
	{
		int idx = 0;
		DataSnapshot snapshot = null;
		string result = string.Empty;
		JObject json;
		// 마지막 유저 idx 가져옴 ==> 신규가입 시 필요
		FirebaseDatabase.DefaultInstance.GetReference("ranking").Child("idx").OrderByChild("userIdx").LimitToLast(1).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted)
			{
				// handle err
				Debug.LogError("reference getValueAsync ::: ");
			}
			else if (task.IsCompleted)
			{
				Debug.LogError("123812478954723895789237589");
				// handle complete
				snapshot = task.Result;
				string raw = snapshot.GetRawJsonValue().ToString();
				int splitedLength = snapshot.GetRawJsonValue().ToString().Split(':')[0].Length + 1;
				string splited = snapshot.GetRawJsonValue().ToString().Substring(splitedLength, raw.Length - splitedLength - 1);
				result = snapshot.GetRawJsonValue().ToString();
				json = JObject.Parse(splited);
				idx = ((int)json["userIdx"]);
			}

		});
		// idx 가져오기 끝

		return idx;
	}

	private void addUser(string id)
    {
		int idx = 0;
			//Task.Factory.StartNew(getLastUserIdx());
		Debug.LogError(idx);
		User user = new User();
		string json = JsonUtility.ToJson(user);

		Debug.LogError(json);
		reference.Child("user").Child("idx").Child(idx.ToString()).SetRawJsonValueAsync(json);
		Debug.LogError("done2");
    }

	private void CreateNewUser(string uid)
    {

		DataSnapshot snapshot = null;

		reference.GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted)
			{
				// handle err
				Debug.LogError("reference getValueAsync ::: ");
			}
			else if (task.IsCompleted)
            {
				// handle complete
				snapshot = task.Result;
            }
		});
		//User user = new User()
    }

}
