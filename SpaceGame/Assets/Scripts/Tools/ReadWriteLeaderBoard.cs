using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ReadWriteLeaderBoard
{
   private const char SEP = '|';
   private const int KEEP = 10;
   
   public static List<(string, int)> ReadScores(string path)
   {
      var scores = ReadScoreFile(path);
      return SortScores(scores);
   }

   public static void WriteScore(string name, int score, string path)
   {
      //get all scores
      var scores = ReadScores(path);
      
      //check if the score is on the leaderboard
      if (ScoreQualifies(score, scores))
      {
         
         //add score to leaderboard and write to file
         scores.Add((name,score));
         scores = SortScores(scores);
         WriteScores(scores,path);
      }
   }

   private static bool ScoreQualifies(int score,List<(string, int)> scores)
   {
      
      //as long as there is space on the leaderboard, you will always make it onto the list
      if (scores.Count < 10) return true;
      
      //check if the current score is higher then the lowest score
      int minValue = scores.Min(s => s.Item2);
      return score > minValue;
   }
   private static List<(string,int)> SortScores(List<(string,int)> scores)
   {
      return scores.OrderByDescending(x => x.Item2).ToList();
   }
   

   private static List<(string,int)> ReadScoreFile(string path)
   {
      List<(string,int)> scores = new List<(string,int)>(KEEP);

      try
      {
         //open file, if file does not exists an empty dictionary is returned
         using (StreamReader file = new StreamReader(path))
         {
            string line;

            //read scores
            while ((line = file.ReadLine()) != null)
            {
               //separate line by separator
               var tokens = line.Split(SEP);
               if (tokens.Length != 2) continue;

               //get name
               string name = tokens[0];

               //get score
               if (!int.TryParse(tokens[1], out int score)) continue;

               //add to score
               scores.Add((name, score));
            }
         }
      }catch (FileNotFoundException ex)
      {
         return new List<(string,int)>(1);
      }

      return scores;
   }

   private static void WriteScores(List<(string,int)> scores,string path)
   {
      using(StreamWriter writer = new StreamWriter(path,false))
      {
         int counter = 0;
         foreach (var (key,value) in scores)
         {
            counter++;
            if (counter > 10) break;
            writer.WriteLine($"{key}{SEP}{value}");
         }
      }
   }

   public static bool IsOnLeaderboard(int score,string path)
   {
      var scores = ReadScores(path);
      return ScoreQualifies(score, scores);
   }
}
