using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using sqliteConnection = Mono.Data.Sqlite.SqliteConnection;
using sqliteCommand = Mono.Data.Sqlite.SqliteCommand;

public class SaveAndExit : MonoBehaviour {
    public string dbName;
    public string Page; //name page(20),  when varchar(20), feeling varchar(20), urge varchar(20), intensity varchar(20), thoughts TEXT
    public Text Date;
    public Slider Feeling;
    public Toggle Urge;
    public Toggle Intensity_L;
    public Toggle Intensity_M;
    public Toggle Intensity_H;
    public Text Thoughts;

    private string DateClean;
    private string UrgeClean;
    private string IntensityClean;
    private sqliteConnection conn = null;

    public void SaveThenExit()
    {
        //Prepare the data for insertion in the database, fields that are not on the page are null and should say "N/A"
        //Date (Only on the self harmed page)
        if (Date == null){ DateClean = "N/A"; }
        else{ DateClean = Date.text; }

        //Urge
        if(Urge == null){ UrgeClean = "N/A"; }
        else{
            if (Urge.isOn) { UrgeClean = "Yes"; }
            else { UrgeClean = "No"; }
        }
        //Intensity
        if(Intensity_L==null)//Doesn't matter which Intensity is checked here
        {
            IntensityClean = "N/A";
        }
        else
        {
            if (Intensity_L.isOn) { IntensityClean = "Low"; }
            if (Intensity_M.isOn) { IntensityClean = "Moderate"; }
            if (Intensity_H.isOn) { IntensityClean = "High"; }
        }
        

        conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True");
        conn.Open();
       
        try
        {   //insert into contact1
            var sql = "insert into " + "patient_info" + " (timestamp, page, date, feeling, urge, intensity, thoughts) values ";
            sql += "('" + DateTime.Now.ToString() + "'"; //Timestamp
            sql += ",";
            sql += "'" + Page + "'";                     //Page name from which the submission is happening
            sql += ",";
            sql += "@date";                              //Date of "When did you last self harm?"
            sql += ",";
            sql += "'" + Feeling.value.ToString() + "'"; //Slider value of "How are you feeling from 1-10"
            sql += ",";
            sql += "'" + UrgeClean + "'";                //Answer to "Do you have an urge to self-harm?"(Yes/No)
            sql += ",";
            sql += "'" + IntensityClean + "'";           //Answer to "The intensity of your urge to self harm"
            sql += ",";
            sql += "@thoughts";                          //Patients Thoughts and Feelings they have at this time
            sql += ")";
            
            var Sqlcommand = new sqliteCommand(sql, conn);
            //Prevent sql injection and just allow someone to do apostrpohes for example typing "Don't"
            Sqlcommand.Parameters.AddWithValue("@date", DateClean); 
            Sqlcommand.Parameters.AddWithValue("@thoughts", Thoughts.text);
            Sqlcommand.ExecuteNonQuery();


        }
        catch (Exception ex)
        {
            Debug.Log("Failed to add to DB " + ex);
        }
        conn.Close();
    }
    

}
