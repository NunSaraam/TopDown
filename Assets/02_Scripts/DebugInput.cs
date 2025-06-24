using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (PlayerStatsManager.Instance != null)
            {
                PlayerStatsManager.Instance.deathCount = 1;
            }
        }
    }

}
