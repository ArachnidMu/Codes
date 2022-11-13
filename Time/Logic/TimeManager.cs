using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMintue, gameHour, gameDay, gameMonth, gameYear;
    private Season gameSeason = Season.春天;
    private int monthInSeason = 3;
    
    public bool gameClockPause;
    private float tikTime;

    private void Awake() 
    {
        NewGameTime();
    }

    private void Start() 
    {
        EventHandler.CallGameMinuteEvent(gameMintue, gameHour);
        EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
    }
    private void Update() 
    {
        if(!gameClockPause)
        {
            tikTime += Time.deltaTime;
            if(tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }

        if(Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMintue = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.春天;
    }

    private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMintue++;
            gameSecond = 0;

            if (gameMintue > Settings.minuteHold)
            {
                gameHour++;
                gameMintue = 0;
            
                if(gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if(gameDay > Settings.dayHold)
                    {
                        gameMonth++;
                        gameDay = 1;

                        if(gameMonth > 12)
                            gameMonth = 1;
                        
                        monthInSeason--;
                        if(monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if(seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;

                            if(gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMintue, gameHour);
        }
        //计时检查
        //Debug.Log("Second:" + gameSecond + " Mintue:" + gameMintue);
    }
}
