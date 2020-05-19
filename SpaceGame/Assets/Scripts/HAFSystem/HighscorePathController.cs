
using System;
using System.Globalization;
using UnityEngine;

public class HighscorePathController : MonoBehaviour
{
    [Header(" --- Setup ---")]
    
    [Tooltip("the file location for the leaderboard-file that stores the highscores of this week (format = day.ww.yy.<filepath>")]
    [SerializeField] private string m_highscoreWeeklyPath = "default.highscore.txt";
    [Tooltip("the file location for the leaderboard-file that stores the highscores of all time")]
    [SerializeField] private string m_highscoreAllTimePath = "alltime.highscore.txt";

    public void Start()
    {
        //get the week of the year 
        var week = GetIso8601WeekOfYear(DateTime.Today);

        //the path of the weekly score is set to "Weekday.week.year.your-extension" so the 
        //default for the 14th of may 2020 would be Thursday.19.20.default.highscore.txt
        //Note we are using ISO8601 for the week of year so even though it is the 14th of may this is still week 19
        //check https://en.wikipedia.org/wiki/ISO_week_date for an explanation on ISO weeks
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
