using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using System.IO;
using System.Text;
using sqliteConnection = Mono.Data.Sqlite.SqliteConnection;
using sqliteCommand = Mono.Data.Sqlite.SqliteCommand;


public class SendEmail : MonoBehaviour {
    public string dbName;
    public Text email;

    private bool exportColumnHeaders;
    private sqliteConnection conn = null;
    public void MakeCvsThenSend()
    {
        //Make CVS file
        string fileName = Application.persistentDataPath + "/" + "data.csv";
        //page, date, feeling, urge, intensity, thoughts
        using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
        {
            //write the headers
            writer.Write("Page | Date | Feeling | Urge | Intensity | Thoughts");
            writer.Write(Environment.NewLine);
            
            //write data from the the db
            conn = new sqliteConnection("URI=file:" + Application.persistentDataPath + "/" + dbName + ";Version=3;FailIfMissing=True;mode=cvs");
            conn.Open();
            var Sqlcommand = new sqliteCommand("select * from patient_info", conn);
            var reader = Sqlcommand.ExecuteReader();
            while (reader.Read())
            {
                writer.Write(reader["page"]+"|"+reader["date"] + "|" + reader["feeling"] + "|" + reader["urge"] + "|" + reader["intensity"] + "|" + reader["thoughts"]);
                writer.Write(Environment.NewLine);
            }
    
            conn.Close();

        }
        
        
        //SendMail
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("selfharmpreventionapp@gmail.com");
        mail.To.Add(email.text);
        mail.Subject = "Patients Data from App";
        mail.Body = "The File attached is the patients data (every entry is seperated by '|' when importing into excel or google sheets that custom seporator should be used) ";
        mail.Attachments.Add(new Attachment(fileName));

        SmtpClient smtpServer = new SmtpClient();
        smtpServer.Host = "smtp.gmail.com";
        smtpServer.Port = 25;
        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpServer.Credentials = new System.Net.NetworkCredential("selfharmpreventionapp@gmail.com", "HeartRateAppFxu2018") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
    }
}
