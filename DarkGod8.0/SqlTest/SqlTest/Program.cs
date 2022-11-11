using System;
using MySql.Data.MySqlClient;

class Program {
    static MySqlConnection conn = null;

    static void Main(string[] args) {
        conn = new MySqlConnection("server=localhost;User Id = root;passwrod=;Database=studymysql;Charset = utf8");
        conn.Open();

        //增
        //Add();

        //删
        Delete();

        //改
        //Update();

        //查
        //Query();

        Console.ReadKey();
        conn.Close();
    }

    static void Add() {
        MySqlCommand cmd = new MySqlCommand("insert into userinfo set name='xixi',age=96 ", conn);
        cmd.ExecuteNonQuery();
        int id = (int)cmd.LastInsertedId;
        Console.WriteLine("Sql Insert Key:{0}", id);
    }

    static void Delete() {
        MySqlCommand cmd = new MySqlCommand("delete from userinfo where id = 1", conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine("delete done");
    }

    static void Update() {
        MySqlCommand cmd = new MySqlCommand("update userinfo set name=@name,age=@age where id =@id", conn);
        cmd.Parameters.AddWithValue("name", "xoxo");
        cmd.Parameters.AddWithValue("age", 123);
        cmd.Parameters.AddWithValue("id", 2);


        cmd.ExecuteNonQuery();
        Console.WriteLine("update done");
    }

    static void Query() {
        //MySqlCommand cmd = new MySqlCommand("select * from userinfo", conn);
        MySqlCommand cmd = new MySqlCommand("select * from userinfo where age=66", conn);


        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read()) {
            int id = reader.GetInt32("id");
            string name = reader.GetString("name");
            int age = reader.GetInt32("age");

            Console.WriteLine(string.Format("sql result: id:{0} name:{1} age:{2}", id, name, age));
        }
    }
}
