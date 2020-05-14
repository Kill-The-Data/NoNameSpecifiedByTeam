using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

//NOTE:THIS IS FOR TESTING ONLY
//ENTRIES CREATED THIS WAY WILL BE MARKED AS FORGED ALL THE TIME!
public class FeedbackForger : EditorWindow
{
    [MenuItem("Window/FeedbackForger")]
    public static void ShowWindow()
    {
        GetWindow<FeedbackForger>().Show();
    }

    private int m_value;
    
    private void OnGUI()
    {
        GUILayout.Label("Warning, Feedback created this way will be marked as forged, do not use in production!");

        //the feedback value (1 = bad, 3 = best)
        m_value = EditorGUILayout.IntField("Score", m_value);

        if (GUILayout.Button("Forge!"))
        {
            //write real feedback
            WriteFeedbackController.WriteFeedback(m_value);

            //get newline information
            bool crlf = Environment.NewLine.Length == 2;
            byte[] newl = new ASCIIEncoding().GetBytes(Environment.NewLine);
            
            //write and commit
            byte[] mark = {0x2C, 0x46};
            var file = File.OpenWrite("feedback.txt");
            file.Seek(-(crlf?2:1), SeekOrigin.End);
            file.Write(mark,0,mark.Length);
            file.Write(newl,0,newl.Length);
            file.Close();
        }
    }
}
