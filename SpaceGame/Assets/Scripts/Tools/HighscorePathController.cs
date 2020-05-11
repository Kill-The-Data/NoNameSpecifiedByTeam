
using System;
using System.Globalization;
using UnityEngine;

public class HighscorePathController : MonoBehaviour
{
    [Header(" --- Setup ---")]
    
    [Tooltip("the file location for the leaderboard-file that stores the highscores of this week (format = ww.yy.<filepath>")]
    [SerializeField] private string m_highscoreWeeklyPath = "default.highscore.txt";
    [Tooltip("the file location for the leaderboard-file that stores the highscores of all time")]
    [SerializeField] private string m_highscoreAllTimePath = "alltime.highscore.txt";

    public void Start()
    {
        var week = GetIso8601WeekOfYear(DateTime.Today);

        PlayerPrefs.SetString("hs_daily",$"{DateTime.Today.DayOfWeek.ToString()}.{week:D2}.{DateTime.Today.Year:D2}.{m_highscoreWeeklyPath}");
        PlayerPrefs.SetString("hs_alltime", m_highscoreAllTimePath);
    }
    public static int GetIso8601WeekOfYear(DateTime time)
    {
        // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
        // be the same week# as whatever Thursday, Friday or Saturday are,
        // and we always get those right
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    } 
}
