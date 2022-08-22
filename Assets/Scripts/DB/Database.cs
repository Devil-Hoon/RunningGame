using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using UnityEngine;
using MySqlConnector;

public static class Database
{
	public static string userID;
	public static string nickname;
	public static int point;
	public static int stamina;
	public static List<int> item = new List<int>();
	public static Dictionary<int, Dictionary<string, string>> itemData = new Dictionary<int, Dictionary<string, string>>();

	private static string sqlConnect = "Server=konggames.co.kr;uid=konggames;pwd=tkfkdgody!39;database=running_game;charset=utf8;TlsVersion=Tlsv1.2;";

	private static MySqlConnection conn = null;
	private static MySqlCommand cmd = null;
	private static MySqlDataReader rdr = null;


	public static bool LoggedIn { get { return userID != null; } }
	public static bool Login(in string id_input, in string password_input, out string alarmMessage)
	{
		string id = id_input;
		string password = password_input;
		string passwordHash = passwordEncryption(id + password);
		string sqlConnect = "Server=konggames.co.kr;uid=konggames;pwd=tkfkdgody!39;database=running_game;charset=utf8;TlsVersion=Tlsv1.2;";


		MySqlConnection conn = new MySqlConnection(sqlConnect);
		if (conn.State != System.Data.ConnectionState.Closed)
		{
			Debug.Log("connetion Failed");
			alarmMessage = "DataBase에 접근할 수 없습니다.";
			return false;
		}
		conn.Open();

		string quary = "SELECT * FROM member  WHERE(userid = '" + id + "' OR username='" + id + "') AND pw='" + password + "'";
		MySqlCommand command = new MySqlCommand(quary, conn);
		MySqlDataReader rdr = command.ExecuteReader();

		if (!rdr.HasRows)
		{
			rdr.Read();
			rdr.Close();
			conn.Close();
			alarmMessage = "아이디 또는 비밀번호가 일치하지 않습니다.";
			return false;
		}
		else
		{
			rdr.Read();
			string tempid = rdr["userid"].ToString();
			string tempnickname = rdr["username"].ToString();
			int tempgp = int.Parse(rdr["money"].ToString());

			if (int.Parse(rdr["out"].ToString()) == 1)
			{
				alarmMessage = "탈퇴 신청 중인 아이디입니다.";
				rdr.Close();
				conn.Close();
				return false;
			}
			else if (int.Parse(rdr["out"].ToString()) == 2)
			{
				alarmMessage = "회원 탈퇴된 아이디입니다.";
				rdr.Close();
				conn.Close();
				return false;
			}
			else if (int.Parse(rdr["out"].ToString()) == 3)
			{
				alarmMessage = "정지 된 아이디입니다.";
				rdr.Close();
				conn.Close();
				return false;
			}
			rdr.Close();
			string loginCheckQuary = "SELECT * FROM clientCheck WHERE userid = '" + tempid + "'";
			MySqlCommand loginCmd = new MySqlCommand(loginCheckQuary, conn);
			MySqlDataReader loginRdr = loginCmd.ExecuteReader();
			if (!loginRdr.HasRows)
			{
				loginRdr.Read();
				loginRdr.Close();
				string sql = "INSERT INTO clientCheck VALUES('" + tempid + "', 1)";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				MySqlDataReader datardr = cmd.ExecuteReader();
				datardr.Read();
				datardr.Close();

				userID = tempid;
				nickname = tempnickname;
				point = tempgp;

				conn.Close();

				alarmMessage = "";
				return true;
			}
			else
			{
				int flag = int.MinValue;

				loginRdr.Read();
				flag = loginRdr.GetInt32(1);
				loginRdr.Close();

				if (flag == 1)
				{
					alarmMessage = "동일한 아이디가 이미 로그인 되어있습니다.";
					return false;
				}

				string sql = "UPDATE clientCheck SET lcheck = 1 WHERE userid = '" + tempid + "'";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				MySqlDataReader datardr = cmd.ExecuteReader();
				datardr.Read();
				datardr.Close();

				userID = tempid;
				nickname = tempnickname;
				point = tempgp;

				conn.Close();

				alarmMessage = "";
				return true;
			}
		}

	}
	public static bool LogOut()
	{
		if (!LoggedIn)
		{
			return true;
		}

		string sqlConnect = "Server=konggames.co.kr;uid=konggames;pwd=tkfkdgody!39;database=running_game;charset=utf8;TlsVersion=Tlsv1.2;";

		MySqlConnection conn = new MySqlConnection(sqlConnect);
		if (conn.State != System.Data.ConnectionState.Closed)
		{
			Debug.Log("connetion Failed");
			return false;
		}
		conn.Open();

		string sql = "UPDATE clientCheck SET lcheck = 0 WHERE userid = '" + userID + "'";
		MySqlCommand cmd = new MySqlCommand(sql, conn);
		MySqlDataReader datardr = cmd.ExecuteReader();
		datardr.Read();
		datardr.Close();

		conn.Close();

		userID = null;
		nickname = null;
		point = 0;
		return true;
	}
	public static void LoadGP()
	{
		string id = userID;
		string sqlConnect = "Server=konggames.co.kr;uid=konggames;pwd=tkfkdgody!39;database=running_game;charset=utf8;TlsVersion=Tlsv1.2;";

		MySqlConnection conn = new MySqlConnection(sqlConnect);
		if (conn.State != System.Data.ConnectionState.Closed)
		{
			Debug.Log("connetion Failed");
			return;
		}
		conn.Open();

		MySqlCommand command = new MySqlCommand("SELECT * FROM member  WHERE userid = '" + id + "' OR username='" + id + "';", conn);
		MySqlDataReader rdr = command.ExecuteReader();

		if (!rdr.HasRows)
		{
			Debug.Log("DB Load Failed");
			conn.Close();
			return;
		}

		while (rdr.Read())
		{
			point = int.Parse(rdr["money"].ToString());
		}

		rdr.Close();
		conn.Close();
	}
	public static string passwordEncryption(string passwordString)
	{
		UTF8Encoding ue = new UTF8Encoding();
		byte[] bytes = ue.GetBytes(passwordString);


		SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
		byte[] hashBytes = sha256.ComputeHash(bytes);

		string hashString = System.Convert.ToBase64String(hashBytes);


		return hashString;
	}

	public static bool insertUser(string uid)
    {
		bool result = false;
		conn = new MySqlConnection(sqlConnect);
		if (conn.State != System.Data.ConnectionState.Closed)
		{
			Debug.Log("connetion Failed");
			return false;
		}
		conn.Open();

		string sql = "INSERT INTO USER(USER_ID, NAME) VALUES('" + uid + "', '')";
		cmd = new MySqlCommand(sql, conn);

		try
        {
			cmd.ExecuteNonQuery();
			result = true;
        } catch (Exception e)
        {
			string error = string.Format("{0}", e);
			Debug.LogError(error);
        }
		
		

		conn.Close();

		return result;
    }

	/// <summary>
	/// 유저 정보 가져오기
	/// </summary>
	/// <param name="uid">파이어베이스 유저 아이디로 가져옴</param>
	/// <returns>null / status:N / 유저 정보</returns>
	public static User getUserInfo(string uid)
    {
		User user = null;
		conn = new MySqlConnection(sqlConnect);
		if (conn.State != System.Data.ConnectionState.Closed)
		{
			Debug.Log("connetion Failed");
			return null;
		}
		conn.Open();

		// 유저 정보 하나 가져옴
		string sql = "select user.idx, user_id, user.name, status, score, item1, item2, item3, A.name as 'ITEM_NAME1', B.name as 'ITEM_NAME2', C.name as 'ITEM_NAME3'  " +
					   "from user  " +
					   "join item_list A ON (A.IDX = user.ITEM1)  " +
					   "join item_list B ON (B.IDX = user.ITEM2)  " +
					   "join item_list C ON (C.IDX = user.ITEM3)  " +
					  "where user_id = '" + uid +  "'";

		cmd = new MySqlCommand(sql, conn);
		rdr = cmd.ExecuteReader();

		if (!rdr.HasRows)
        {
			rdr.Close();
			conn.Close();
        }
		else
        {
			rdr.Read();
			if(rdr.GetString(3) == "N")
            {
				user = new User();
				user.Status = rdr.GetString(3);
            } 
			else
            {
				int idx = rdr.GetInt32(0);
				string userId = rdr.GetString(1);
				string name = rdr.GetString(2);
				string status = rdr.GetString(3);
				int score = rdr.GetInt32(4);
				int item1 = rdr.GetInt32(5);
				int item2 = rdr.GetInt32(6);
				int item3 = rdr.GetInt32(7);
				string itemName1 = rdr.GetString(8);
				string itemName2 = rdr.GetString(9);
				string itemName3 = rdr.GetString(10);

				user = new User(idx, userId, name, status, score, item1, item2, item3, itemName1, itemName2, itemName3);
			}

			rdr.Close();
			conn.Close();
        }


		return user;
    }


	public static ArrayList getRanking()
    {
		ArrayList arr = null;
		Ranking rank = null;

		int num;
		int idx;
		int userIdx;
		string name;
		int score;

		conn = new MySqlConnection(sqlConnect);
		
		if (conn.State != System.Data.ConnectionState.Closed)
        {
			return null;
        }
		conn.Open();

		string sql = "select @ROWNUM := @ROWNUM + 1 AS rownum, A.* " +
			"from (SELECT ranking.idx, ranking.user_idx, user.name, max(ranking.score) as score " +
			"from (SELECT @ROWNUM := 0) R, ranking " +
			"join user on (user.IDX = ranking.USER_IDX) " +
			"group by ranking.USER_IDX " +
			"order by score desc, IDX ASC " +
			"limit 0, 10) A " +
			"order by ROWNUM ASC;";

		cmd = new MySqlCommand(sql, conn);
		rdr = cmd.ExecuteReader();

		if (!rdr.HasRows)
		{
			rdr.Close();
			conn.Close();
		}
		else
        {
			while(rdr.Read())
            {
				num = (int)rdr["rownum"];
				idx = (int)rdr["idx"];
				userIdx = (int)rdr["user_idx"];
				name = (string)rdr["name"];
				score = (int)rdr["score"];
				rank = new Ranking(num, idx, userIdx, name, score);
				arr.Add(rank);
            }
			rdr.Close();
			conn.Close();
        }


		return arr;


    }

	public static ArrayList getItemList()
	{
		ArrayList arr = null;
		ItemList item = null;

		int idx;
		string name;
		string content;
		string effect;
		int price;
		string parts;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return null;
		}
		conn.Open();

		string sql = "select idx, name, content, effect, price, parts " +
			"from item_list " +
			"order by field(parts, 'FOOT', 'WING', 'HEAD', 'ALL') DESC, IDX ASC;";

		cmd = new MySqlCommand(sql, conn);
		rdr = cmd.ExecuteReader();

		if (!rdr.HasRows)
		{
			rdr.Close();
			conn.Close();
		}
		else
		{
			while (rdr.Read())
			{
				idx = (int)rdr["idx"];
				name = (string)rdr["name"];
				content = (string)rdr["content"];
				effect = (string)rdr["effect"];
				price = (int)rdr["score"];
				parts = (string)rdr["parts"];
				item = new ItemList(idx, name, content, effect, price, parts);
				arr.Add(item);
			}
			rdr.Close();
			conn.Close();
		}


		return arr;


	}

	public static bool insertRanking(string uid, int score)
    {

		bool result = false;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "insert into ranking(user_idx, score) values('" + uid + "',  " + score + ");";
		cmd = new MySqlCommand(sql, conn);

		try
		{
			cmd.ExecuteNonQuery();
			result = true;
		}
		catch (Exception e)
		{
			string error = string.Format("{0}", e);
			Debug.LogError(error);
		}



		conn.Close();

		return result;
    }

	public static bool updateUser(string uid, int score)
    {
		bool result = false;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "update user set score =  " + score + " where user_id = '" + uid + "';";
		cmd = new MySqlCommand(sql, conn);

		try
		{
			cmd.ExecuteNonQuery();
			result = true;
		}
		catch (Exception e)
		{
			string error = string.Format("{0}", e);
			Debug.LogError(error);
		}



		conn.Close();

		return result;
    }

	public static bool updateUserItem(string uid, int item1, int item2, int item3)
    {
		bool result = false;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "update user set item1 =  " + item1 + ", item2 =  " + item2 + ", item3 =  " + item3 + " where user_id = '" + uid + "';";
		cmd = new MySqlCommand(sql, conn);

		try
		{
			cmd.ExecuteNonQuery();
			result = true;
		}
		catch (Exception e)
		{
			string error = string.Format("{0}", e);
			Debug.LogError(error);
		}



		conn.Close();

		return result;
	}

	public static bool updateRanking(string uid, int score)
	{
		bool result = false;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "update ranking set score =  " + score + " where user_idx = (select idx from user where user_id = '" + uid + "');";
		cmd = new MySqlCommand(sql, conn);

		try
		{
			cmd.ExecuteNonQuery();
			result = true;
		}
		catch (Exception e)
		{
			string error = string.Format("{0}", e);
			Debug.LogError(error);
		}



		conn.Close();

		return result;
	
	}

	public static string getRankingExist(string uid)
	{
		string result = "failed"; // --> server error

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "select count(*) as cnt from ranking where user_idx = (select idx from user where user_id = '" + uid + "');";
		cmd = new MySqlCommand(sql, conn);
		rdr = cmd.ExecuteReader();

		if (rdr.HasRows)
		{
			rdr.Read();
			if (rdr.GetInt32(0) > 0)
			{
				result = "true"; // --> updateRanking
			}
			else
			{
				result = "false"; // --> insertRanking
			}
		}

		rdr.Close();
		conn.Close();

		return result;

	}

	public static string getNameExist(string name)
	{
		string result = "failed"; // --> server error

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "select count(*) as cnt from user where name = ('" + name + "');";
		cmd = new MySqlCommand(sql, conn);
		rdr = cmd.ExecuteReader();

			Debug.Log(rdr);
		if (rdr.HasRows)
		{
			rdr.Read();
			if (rdr.GetInt32(0) > 0)
			{
				result = "false"; // --> 닉네임 존재함
			}
			else
			{
				result = "true"; // --> 닉네임 없음, setUserName 호출하면됨
			}
		}

		rdr.Close();
		conn.Close();

		return result;

	}

	public static bool insertPurchaseLog(string uid, int itemIdx, string action)
    {
		bool result = false;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "insert into item_purchase_log(item_idx, user_idx, action) values( " + itemIdx + ", (select idx from user where user_id = '" + uid + "'), '" + action + "');";
		cmd = new MySqlCommand(sql, conn);

		try
		{
			cmd.ExecuteNonQuery();
			result = true;
		}
		catch (Exception e)
		{
			string error = string.Format("{0}", e);
			Debug.LogError(error);
		}



		conn.Close();

		return result;
	}

	public static bool setUserName(string uid, string name)
    {
		bool result = false;

		conn = new MySqlConnection(sqlConnect);

		if (conn.State != System.Data.ConnectionState.Closed)
		{
			return result;
		}
		conn.Open();

		string sql = "update user set name =  '" + name + "' where user_id = '" + uid + "';";
		cmd = new MySqlCommand(sql, conn);

		try
		{
			cmd.ExecuteNonQuery();
			result = true;
		}
		catch (Exception e)
		{
			string error = string.Format("{0}", e);
			Debug.LogError(error);
		}

		conn.Close();

		return result;
	}

}
