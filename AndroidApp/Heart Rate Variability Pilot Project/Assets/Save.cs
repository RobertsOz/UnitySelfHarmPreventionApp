using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using sqliteConnection = Mono.Data.Sqlite.SqliteConnection;
using sqliteCommand = Mono.Data.Sqlite.SqliteCommand;


public class Save : MonoBehaviour {
    public string dbName;
    public InputField name1;
    public InputField relationship1;
    public InputField mobile1;
    public InputField home1;

    public InputField name2;
    public InputField relationship2;
    public InputField mobile2;
    public InputField home2;

    private sqliteConnection conn = null;

    public void SaveIntoDatabase(){
        conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True");
        conn.Open();

        try //clear the table for contact 1 and contact 2 before rewriting the data
        {
            var Sqlcommand = new sqliteCommand("delete from contact1", conn);
            Sqlcommand.ExecuteNonQuery();
            Sqlcommand = new sqliteCommand("delete from contact2", conn);
            Sqlcommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Debug.Log("Failed to delete data from contact1/2 tables " + ex);
        }

        try
        {   //insert into contact1
            var sql = "insert into " + "contact1" + " (name, relationship, mobile, home) values ";
            sql += "(@name1";
            sql += ",";
            sql += "@relationship1";
            sql += ",";
            sql += "@mobile1";
            sql += ",";
            sql += "@home1";
            sql += ")";

            var Sqlcommand = new sqliteCommand(sql, conn);
            //Prevent Sql Injection 
            Sqlcommand.Parameters.AddWithValue("@name1", name1.text);
            Sqlcommand.Parameters.AddWithValue("@relationship1", relationship1.text);
            Sqlcommand.Parameters.AddWithValue("@mobile1", mobile1.text);
            Sqlcommand.Parameters.AddWithValue("@home1", home1.text);
            Sqlcommand.ExecuteNonQuery();

            //insert into contact2
            sql = "insert into " + "contact2" + " (name, relationship, mobile, home) values ";
            sql += "(@name2";
            sql += ",";
            sql += "@relationship2";
            sql += ",";
            sql += "@mobile2";
            sql += ",";
            sql += "@home2";
            sql += ")";

            Sqlcommand = new sqliteCommand(sql, conn);
            //Prevent Sql Injection 
            Sqlcommand.Parameters.AddWithValue("@name2", name2.text);
            Sqlcommand.Parameters.AddWithValue("@relationship2", relationship2.text);
            Sqlcommand.Parameters.AddWithValue("@mobile2", mobile2.text);
            Sqlcommand.Parameters.AddWithValue("@home2", home2.text);
            Sqlcommand.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            Debug.Log("Failed to add to DB " + ex);
        }
        conn.Close();
    }
    
    void Start() //When the Emergency Contacts Scene is opened Should display Contacts if they have been saved in the Database
    {   
       
        conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True");
        conn.Open();

        try
        {
            //Read from contact1 table and put that into text fields on the Emergency Contacts Screen
            var Sqlcommand = new sqliteCommand("select * from contact1", conn);
            var reader = Sqlcommand.ExecuteReader();
            reader.Read();
            if (reader.HasRows == true)
            {
                name1.text = reader["name"] as String;
                relationship1.text = reader["relationship"] as String;
                mobile1.text = reader["mobile"] as String;
                home1.text = reader["home"] as String;
            }
            else
            {
                Debug.Log("There are no rows in contact1");
            }

            //Read from contact2 table and put that into text fields on the Emergency Contacts Screen
            Sqlcommand = new sqliteCommand("select * from contact2", conn);
            reader = Sqlcommand.ExecuteReader();
            reader.Read();
            if (reader.HasRows == true)
            {
                name2.text = reader["name"] as String;
                relationship2.text = reader["relationship"] as String;
                mobile2.text = reader["mobile"] as String;
                home2.text = reader["home"] as String;
            }
            else
            {
                Debug.Log("There are no rows in contact2");
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Failed to read from Database " +ex);
        }
        conn.Close();
    }
    
}
