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
    
    // Runs when the app is opened on the first scene
    void Start () {
        //Path where the database is/is going to be
        string filePath = Application.persistentDataPath + "/" + dbName;

        //make the database If one doesn't exist already
        if (!File.Exists(filePath))
        {
            try
            {
                Debug.Log("A database doesn't exist. Creating one...");
                //Create the database file in the persisten data path of the application
                sqliteConnection.CreateFile(filePath);

                //Open the database
                conn = new sqliteConnection("URI=file:" + filePath + ";Version=3;FailIfMissing=True");
                conn.Open();
                //create table for Contacts1
                var command = new sqliteCommand("create table contact1 (name varchar(20), relationship varchar(20), mobile varchar(20), home varchar(20) )", conn);
                command.ExecuteNonQuery();
                //create table for Contacts2
                command = new sqliteCommand("create table contact2 (name varchar(20), relationship varchar(20), mobile varchar(20), home varchar(20) )", conn);
                command.ExecuteNonQuery();
                //create table for PatientInfo
                command = new sqliteCommand("create table patient_info (timestamp varchar(20), page varchar(20), date varchar(20), feeling varchar(20), urge varchar(20), intensity varchar(20), thoughts TEXT)", conn);
                command.ExecuteNonQuery();
                //Close Db
                conn.Close();
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to add to DB " + ex);
            }
        }
        else
        {
            Debug.Log(" A database exists.");
        }
        
    }
	


	

	
	
	// Update is called once per frame
	void Update () {
	
	}
}