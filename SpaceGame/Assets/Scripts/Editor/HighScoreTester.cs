
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

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y+=padding/2;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Write Test Data"))
        {
            FileUtil.DeleteFileOrDirectory("Test.highscore.txt");
            
            ReadWriteLeaderBoard.WriteScore("Jakob",5,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Jakob",15,"Test.highscore.txt");
            
            ReadWriteLeaderBoard.WriteScore("Tom",11,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Bob",20,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Josef",9,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Paul",18,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Hans",14,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Lukas",11,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Thomas",12,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Sepp",14,"Test.highscore.txt");
            ReadWriteLeaderBoard.WriteScore("Christoph",10,"Test.highscore.txt");
            
            ReadWriteLeaderBoard.WriteScore("Jaqueline",5,"Test.highscore.txt");

            m_path = "Test.highscore.txt";
        }

        m_path = EditorGUILayout.TextField("Targeted Highscore File", m_path);
        if (GUILayout.Button("Read Data"))
        {
            m_currentHighscoreData = ReadWriteLeaderBoard.ReadScores(m_path);
        }
        
        if(m_currentHighscoreData != null)
            foreach (var (name,score) in m_currentHighscoreData)
            {
                string name_with_padding = name + "                           ";
                GUILayout.Label($"Player:\t{name_with_padding.Substring(0,20)}\t with score:\t{score}");
                DrawUILine(Color.white);
            }
    }
}
