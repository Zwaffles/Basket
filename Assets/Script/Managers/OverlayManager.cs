using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class OverlayManager : MonoBehaviour
{

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    private void OnEnable()
    {

        m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {

        if (pCallback.m_bActive != 0)
        {
            // Steam overlay activated
            GameManager.instance.uiManager.TogglePause(true);
        }
        else
        {
            // Deactivated
            GameManager.instance.uiManager.TogglePause(false);
        }

    }

}
