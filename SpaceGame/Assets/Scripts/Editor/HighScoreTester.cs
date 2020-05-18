
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTester : EditorWindow
{
    private static string m_path;
    private List<(string, int)> m_currentHighscoreData;
    
    
    [MenuItem("Window/HighScoreTester")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<HighScoreTester>();
    }
    
    private void OnGUI()
    {
        if (GUILayout.Button("Write Test Data"))
        {
            WriteTestData();
        }

        m_path = EditorGUILayout.TextField("Targeted Highscore File", m_path);
        if (GUILayout.Button("Read Data"))
        {
           ReadData();
        }

        DisplayHighscoreData();
    }

    private void WriteTestData()
    {
        //delete the test highscore before writing new scores to it
        FileUtil.DeleteFileOrDirectory("Test.highscore.txt");
            
        //write some fake high-score data
        ReadWriteLeaderBoard.WriteScore("Jakob",        5, "Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Jakob",        15,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Tom",          11,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Bob",          20,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Josef",        9, "Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Paul",         18,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Hans",         14,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Lukas",        11,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Thomas",       12,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Sepp",         14,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Christoph",    10,"Test.highscore.txt");
        ReadWriteLeaderBoard.WriteScore("Jaqueline",    5, "Test.highscore.txt");
            
        //after writing these, some should have been dropped and others should have been removed
        //(those with the lowest Score of 5)
        //make sure to readback the highscore file 
            
        //set the readpath as a QoL
        m_path = "Test.highscore.txt";
    }

    private void ReadData()
    {
        m_currentHighscoreData = ReadWriteLeaderBoard.ReadScores(m_path);
    }

    private void DisplayHighscoreData()
    {
        if(m_currentHighscoreData != null)
            foreach (var (name,score) in m_currentHighscoreData)
            {
                //kind of a hack, add a tonne of padding to the name so that below we can substring it into something more manageable
                //this way we can use standard tab-stops instead of doing a complicated table system
                string name_with_padding = name + "                                                 ";
                
                //Display the Data, Substring the name (sorry if your name is longer than 20 characters... ?)
                GUILayout.Label($"Player:\t{name_with_padding.Substring(0,20)}\t with score:\t{score}");
                
                //draw a separator
                UIExtra.DrawUILine(Color.white);
            }
    }
}
