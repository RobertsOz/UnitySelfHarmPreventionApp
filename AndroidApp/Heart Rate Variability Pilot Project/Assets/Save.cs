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
            sql += "('" + name1.textComponent.text + "'";
            sql += ",";
            sql += "'" + relationship1.textComponent.text + "'";
            sql += ",";
            sql += "'" + mobile1.textComponent.text + "'";
            sql += ",";
            sql += "'" + home1.textComponent.text + "'";
            sql += ")";

            var Sqlcommand = new sqliteCommand(sql, conn);
            Sqlcommand.ExecuteNonQuery();

            //insert into contact2
            sql = "insert into " + "contact2" + " (name, relationship, mobile, home) values ";
            sql += "('" + name2.textComponent.text + "'";
            sql += ",";
            sql += "'" + relationship2.textComponent.text + "'";
            sql += ",";
            sql += "'" + mobile2.textComponent.text + "'";
            sql += ",";
            sql += "'" + home2.textComponent.text + "'";
            sql += ")";

            Sqlcommand = new sqliteCommand(sql, conn);
            Sqlcommand.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            Debug.Log("Failed to add to DB " + ex);
        }
        conn.Close();
    }
    
    void Start()
    {
        conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True");
        conn.Open();

        try
        {
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
