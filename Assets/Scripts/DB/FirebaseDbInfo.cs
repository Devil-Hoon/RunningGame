/*using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

private FirebaseDatabase database = null;

public class FirebaseDbInfo
{

    public string name = "";
    public int gold = 0;

}





private void InitializeFirebase()
{

    //Auth 이후

    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(https://test-6cbb5.firebaseio.com/);
     this.database = FirebaseDatabase.DefaultInstance;

    database.RootReference.GetValueAsync().ContinueWith(task = &gt;
    {
        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            // 데이터를 출력하고자 할때는 Snapshot 객체 사용함

            foreach (DataSnapshot data in snapshot.Children)
            {
                //IDictionary info = (IDictionary)data.Value;

                Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
            }
        }
    });
   }
   
       public void UpdateDbUserInfo()
{

    Debug.LogFormat("[Database] Insert !");
    FirebaseDbInfo info = new FirebaseDbInfo();
    info.name = "테스트!";
    info.gold = 1234;

    string json = JsonUtility.ToJson(info);

    string key = user.UserId;
    this.database.GetReference("users").Child(key).SetRawJsonValueAsync(json);

}*/