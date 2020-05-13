using System;
using System.IO;
using System.Text;

public static class WriteFeedbackController 
{
    private const string FILENAME = "feedback.txt";
    
    public static void WriteFeedback(int score)
    {
        var now = DateTime.Now;
        var time = now.ToString("HH:mm:ss tt zz");
        var date = now.ToString("dd-MM-yyyy");
        string row = $"{date},{time},{score}";

        if (!File.Exists(AndroidUtils.GetFriendlyPath()+FILENAME))
        {
            byte[] header = new ASCIIEncoding()
                .GetBytes("sep=,\nDate,Time,Score\n");
            
            var file = File.Create(AndroidUtils.GetFriendlyPath()+FILENAME); 
            file.Write(header,0,header.Length);
            file.Close();
        }
        using (StreamWriter writer = new StreamWriter("feedback.txt", append: true))
        {
            writer.WriteLine(row);   
            writer.Close();
        }

    }
}
