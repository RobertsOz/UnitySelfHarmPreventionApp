using UnityEngine;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.IO;
using sqliteConnection = Mono.Data.Sqlite.SqliteConnection;
using sqliteCommand = Mono.Data.Sqlite.SqliteCommand;

public class createDB : MonoBehaviour {
    public string dbName;

    private sqliteConnection conn = null;


    // Use this for initialization
    void Start () {
        
        //make the database
        if (File.Exists(Application.persistentDataPath + "/" + dbName))
        {
            Debug.Log("A database exists.");
            conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True");
            conn.Open();
        }
        else
        {
            try
            {
                Debug.Log("A database doesn't exist. Creating one...");
                sqliteConnection.CreateFile(Application.persistentDataPath + "/" + dbName);

                conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True");

                conn.Open();
                //create table for Contacts1
                var command = new sqliteCommand("create table contact1 (name varchar(20), relationship varchar(20), mobile varchar(20), home varchar(20) )", conn);
                command.ExecuteNonQuery();
                //create table for Contacts2
                command = new sqliteCommand("create table contact2 (name varchar(20), relationship varchar(20), mobile varchar(20), home varchar(20) )", conn);
                command.ExecuteNonQuery();
                //create table for PatientInfo
                command = new sqliteCommand("create table patient_info (page varchar(20), date varchar(20), feeling varchar(20), urge varchar(20), intensity varchar(20), thoughts TEXT)", conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to add to DB " + ex);
            }
        }
        conn.Close(); //Close Db
    }
	


	

	
	
	// Update is called once per frame
	void Update () {
	
	}
}