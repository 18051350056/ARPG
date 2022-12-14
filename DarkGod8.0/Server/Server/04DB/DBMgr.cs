/********************************************************************
	file:		DBMgr.cs
	author:		矍铄的金先知
	created:	2022/06/24 10:00:41 
	mail:       2816387346@qq.com
	
	function:	数据库管理类
*********************************************************************/

using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr
{
	public static DBMgr instance = null;
	public static DBMgr Instance
    {
		get
        {
			if (instance == null)
            {
				instance = new DBMgr();
            }
			return instance;
        }
    }

    //定义数据库的连接
    private MySqlConnection conn;

    public void Init()
    {
		conn = new MySqlConnection("server = localhost; User Id = root; password = ;Database = darkgod; Charset = utf8");
		conn.Open();
		PECommon.Log("DB Init Done");

		//QueryPlayerData("xxx", "ooooo");
    }

	//查询账号(缓存层调用)
	public PlayerData QueryPlayerData(string acct, string pass)
    {
		PlayerData playerData = null;
		MySqlDataReader reader = null;

		bool isNew = true;
        try
        {
			MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
			cmd.Parameters.AddWithValue("acct", acct);
			reader = cmd.ExecuteReader();

			if (reader.Read())
            {
				isNew = false;
				string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))
                {
					//密码正确 返回玩家数据
					playerData = new PlayerData
					{
						id = reader.GetInt32("id"),
						name = reader.GetString("name"),
						lv = reader.GetInt32("level"),
						exp = reader.GetInt32("exp"),
						power = reader.GetInt32("power"),
						coin = reader.GetInt32("coin"),
						diamond = reader.GetInt32("diamond"),
						crystal = reader.GetInt32("crystal"),

                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),

                        guideid = reader.GetInt32("guideid")

                        //TODO
                    };
                    #region Strong
                    //数据：1#2#3#4#5#6#
                    string[] strongArr = reader.GetString("strong").Split('#');
                    int[] _strongArr = new int[6];
                    for (int i = 0; i < strongArr.Length; i++)
                    {
                        if (strongArr[i] == "") continue;
                        if (int.TryParse(strongArr[i], out int starLv))
                        {
                            _strongArr[i] = starLv;
                        }
                        else
                        {
                            PECommon.Log("Parse Strong Data Error", LogType.Error);
                        }
                    }
                    playerData.strongArr = _strongArr;
                    #endregion

                    //TODO
                }
            }
        }
		catch(Exception e)
        {
			PECommon.Log("Query PlayerData By Acct&Pass Error:" + e, LogType.Error);
        }
		//新账号的创建
        finally
        {
			if (reader != null)
            {
				reader.Close();
            }
			if (isNew)
            {
                //不存在账号数据，创建新的默认账号数据，并返回
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500,
                    crystal = 500,


                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,
                    guideid = 1001,

                    strongArr = new int[6],
                    //TOADD
                };

				playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }

		return playerData;
    }

	private int InsertNewAcctData(string acct, string pass,PlayerData pd)
    {
		int id = -1;
        try
        {
			MySqlCommand cmd = new MySqlCommand(
				"insert into account set acct=@acct,pass=@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond," +
                "crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical, guideid=@guideid," +
                "strong=@strong", conn);
			cmd.Parameters.AddWithValue("acct", acct);
			cmd.Parameters.AddWithValue("pass", pass);
			cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideid", pd.guideid);
            //数组信息转化为字符串
            string strongInfo = "";
            for (int i = 0; i < pd.strongArr.Length; i++)
            {
                strongInfo += pd.strongArr[i];
                strongInfo += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);

            //TOADD
            cmd.ExecuteNonQuery();
			id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
			PECommon.Log("Insert PlayerData Error" + e, LogType.Error);
        }
		return id;
    }

	public bool QueryNameData(string name)
    {
		bool exist = false;
		MySqlDataReader reader = null;
		try
        {
			MySqlCommand cmd = new MySqlCommand("select * from account where name= @name", conn);
			cmd.Parameters.AddWithValue("name", name);
			reader = cmd.ExecuteReader();
			if (reader.Read())
            {
				exist = true;
            }
        }
		catch(Exception e)
        {
			PECommon.Log("Query Name State Error: " + e, LogType.Error);
        }
        finally
        {
			if (reader != null)
            {
				reader.Close();
            }
        }
		return exist;
    }

	public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
    "update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond," +
    "crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce," +
    "critical=@critical,guideid=@guideid,strong=@strong where id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);
            cmd.Parameters.AddWithValue("crystal", playerData.crystal);

            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);
            //TOADD Others
            string strongInfo = "";
            for (int i = 0; i < playerData.strongArr.Length; i++)
            {
                strongInfo += playerData.strongArr[i];
                strongInfo += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);
            cmd.ExecuteNonQuery();
        }
		catch(Exception e)
        {
			PECommon.Log("Upadte PlayerData Error:" + e, LogType.Error);
			return false;
        }
		return true;
    }
}

