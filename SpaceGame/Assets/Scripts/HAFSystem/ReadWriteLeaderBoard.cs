using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ReadWriteLeaderBoard
{
   
   //the entry is seperated by a Pipe,
   //usually no one has a pipe symbol in their name
   private const char SEP = '|';
   
   //we only keep 10 entries, after that we discard
   //all scores
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
      //sort the scores by descending value of score
      return scores.OrderByDescending(x => x.Item2).ToList();
   }
   

   private static List<(string,int)> ReadScoreFile(string path)
   {
      List<(string,int)> scores = new List<(string,int)>(KEEP);

      try
      {
         //open file, if file does not exist we catch a FileNotFound and simply return an 
         //empty list
         using (StreamReader file = new StreamReader(AndroidUtils.GetFriendlyPath()+path))
         {
            string line;
            int counter = 0;
            
            //read scores
            while ((line = file.ReadLine()) != null)
            {
               //drop reading any more scores after we already reached KEEP amount
               if (counter++ > KEEP) break;
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
      } catch (FileNotFoundException ex)
      {
         //return a new List, and give it a capacity 
         //of one, playing on the fact that you are probably going to insert a new score 
         return new List<(string,int)>(1);
      }
      return scores;
   }

   private static void WriteScores(List<(string,int)> scores,string path)
   {
      using(StreamWriter writer = new StreamWriter(AndroidUtils.GetFriendlyPath() + path,false))
      {
         int counter = 0;
         foreach (var (key,value) in scores)
         {
            //if the score-board already has KEEP entires, drop any others
            if (counter++ > KEEP) break;
            
            //write the name (minus and SEPs we find in there) then a SEP then the value
            writer.WriteLine($"{key.Replace(SEP, ' ')}{SEP}{value}");
         }
      }
   }

   public static bool IsOnLeaderboard(int score,string path)
   {
      var scores = ReadScores(path);
      return ScoreQualifies(score, scores);
   }
}
