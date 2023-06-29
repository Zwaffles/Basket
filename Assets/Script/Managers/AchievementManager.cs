using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public enum Achievement
{
    FirstOfMany,
    SecondOfMany,
    ThirdOfMany,
    ComebackTime,
    LickTheRing,
    WhoseHoop,
    SeeYouInCourt,
    WeOnlyShoot3Pointers,
    KeepTheShotsComing
}

public enum Stat
{
    GamesPlayed
}

public class AchievementManager : MonoBehaviour
{
    protected Callback<UserStatsReceived_t> m_UserStatsReceived;

    private bool hasReceivedUserStats = false;
    private List<string> achievementQueue = new List<string>();

    private Dictionary<Stat, int> statsQueue = new Dictionary<Stat, int>();

    private void OnEnable()
    {

        if (!SteamManager.Initialized) return;

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatReceived);

        statsQueue.Add(Stat.GamesPlayed, 0);

        SteamUserStats.RequestCurrentStats();

    }

    private void OnUserStatReceived(UserStatsReceived_t pCallback)
    {

        if (pCallback.m_eResult != EResult.k_EResultOK) return;

        hasReceivedUserStats = true;

        ClearAchievementQueue();
        ClearStatQueue();

        /*

        Use this code to delete all your achievements

        SteamUserStats.ClearAchievement("ACH_COMEBACK");
        SteamUserStats.ClearAchievement("ACH_FIRST_WIN");
        SteamUserStats.ClearAchievement("ACH_SCORE_FIVE");
        SteamUserStats.ClearAchievement("ACH_NEAR_MISS");
        SteamUserStats.ClearAchievement("ACH_PLAY_MULTIPLAYER");
        SteamUserStats.ClearAchievement("ACH_THREE_CONSECUTIVE");
        SteamUserStats.ClearAchievement("ACH_SELF_GOAL");
        SteamUserStats.StoreStats();

        */

    }

    private void GiveAchievement(string achievement)
    {

        if (hasReceivedUserStats)
        {

            SteamUserStats.SetAchievement(achievement);

            SteamUserStats.StoreStats();

        }
        else
        {

            if (!achievementQueue.Contains(achievement))
                achievementQueue.Add(achievement);

        }

    }

    public void GiveAchievement(Achievement achievement)
    {

        switch (achievement)
        {

            case Achievement.ComebackTime:
                GiveAchievement("ACH_COMEBACK");
                break;
            case Achievement.FirstOfMany:
                GiveAchievement("ACH_FIRST_WIN");
                break;
            case Achievement.KeepTheShotsComing:
                GiveAchievement("ACH_SCORE_FIVE");
                break;
            case Achievement.LickTheRing:
                GiveAchievement("ACH_NEAR_MISS");
                break;
            case Achievement.SeeYouInCourt:
                GiveAchievement("ACH_PLAY_MULTIPLAYER");
                break;
            case Achievement.WeOnlyShoot3Pointers:
                GiveAchievement("ACH_THREE_CONSECUTIVE");
                break;
            case Achievement.WhoseHoop:
                GiveAchievement("ACH_SELF_GOAL");
                break;
            case Achievement.SecondOfMany:
                GiveAchievement("ACH_TENTH_GAME");
                break;
            case Achievement.ThirdOfMany:
                GiveAchievement("ACH_HUNDRETH_GAME");
                break;
            default:
                break;

        }

    }

    public void AddStat(Stat stat, int value)
    {

        string statReference = "Noel";

        switch (stat)
        {
            case Stat.GamesPlayed:
                statReference = "stat_games";
                break;
            default:
                break;
        }

        if (statReference.Equals("Noel")) return;

        if (hasReceivedUserStats)
        {

            int currentValue;
            SteamUserStats.GetStat(statReference, out currentValue);
            SteamUserStats.SetStat(statReference, currentValue + value);
            SteamUserStats.StoreStats();

            Debug.Log(currentValue);

        }
        else
        {

            if (statsQueue.ContainsKey(stat))
            {
                statsQueue[stat] += value;
            }

        }

    }

    public int GetStat(Stat stat)
    {

        string statReference = "Noel";

        switch (stat)
        {
            case Stat.GamesPlayed:
                statReference = "stat_games";
                break;
            default:
                break;
        }

        if (statReference.Equals("Noel")) return -1;

        if (hasReceivedUserStats)
        {

            int currentValue;
            SteamUserStats.GetStat(statReference, out currentValue);

            return currentValue;

        }
        else
        {

            return -1;

        }

    }

    private void ClearAchievementQueue()
    {

        foreach(string s in achievementQueue)
        {
            GiveAchievement(s);
        }

        achievementQueue.Clear();

    }

    private void ClearStatQueue()
    {

        if (statsQueue.ContainsKey(Stat.GamesPlayed)) AddStat(Stat.GamesPlayed, statsQueue[Stat.GamesPlayed]);

        statsQueue.Clear();

    }

}
