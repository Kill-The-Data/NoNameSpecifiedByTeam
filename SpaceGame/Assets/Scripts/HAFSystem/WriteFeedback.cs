using System;
using System.IO;

public static class WriteFeedbackController 
{
    //where to write the feedback to 
    private const string FILENAME = "feedback.csv";
    
    public static void WriteFeedback(int score)
    {
        //generate time-stamp
        var now = DateTime.Now;
        var time = now.ToString("HH:mm:ss tt zz");
        var date = now.ToString("dd-MM-yyyy");
        
        //assemble feedback entry
        string row = $"{date},{time},{score}";

        //check if the feedback file already exists
        if (!File.Exists(AndroidUtils.GetFriendlyPath()+FILENAME))
        {
            using (StreamWriter writer = new StreamWriter(AndroidUtils.GetFriendlyPath() + FILENAME, append: false))
            {
                //assemble a header
                writer.WriteLine("sep=,");
                writer.WriteLine("Date,Time,Score");
                writer.Close();
            }
        }
        
        //open file as stream and write entry, then close
        using (StreamWriter writer = new StreamWriter(AndroidUtils.GetFriendlyPath()+FILENAME, append: true))
        {
            writer.WriteLine(row);   
            writer.Close();
        }

    }
}
