using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

/*
 * 
 *  DEPRECATED - Saving as a reference
 * 
 */

public class ScoreAchievements : MonoBehaviour
{
    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    private bool _ = true;

    private void OnEnable()
    {

        Debug.Log("Script is running.");

        if (!SteamManager.Initialized) return;

        Debug.Log("Steam manager has been initialized.");

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatReceived);

        SteamUserStats.RequestCurrentStats();

        //StartCoroutine(DoRunCallbacks());

    }

    private void OnUserStatReceived(UserStatsReceived_t pCallback)
    {

        Debug.Log(pCallback.m_eResult);

        if (pCallback.m_eResult != EResult.k_EResultOK) return;

        Debug.Log("User stats were received successfully.");

        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementsStored);

        //SteamUserStats.SetAchievement("ACH_THREE_CONSECUTIVE");

        SteamUserStats.StoreStats();

    }

    private void OnAchievementsStored(UserAchievementStored_t pCallback)
    {

        Debug.Log("Achievement " + pCallback.m_rgchAchievementName + " was stored.");

    }

    IEnumerator DoRunCallbacks()
    {

        for(;_;)
        {
            SteamAPI.RunCallbacks();

            yield return new WaitForSeconds(.5f);
        }

    }

}
