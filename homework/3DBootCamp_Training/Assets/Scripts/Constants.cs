using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
    public static readonly string Input_Horizontal = "Horizontal";
    public static readonly string Input_Vertical = "Vertical";

    public static readonly string Animation_Start = "Start";
    public static readonly string Animation_Slide = "Slide";
    public static readonly string Animation_Jump = "Jump";
    public static readonly string Animation_Grounded = "Grounded";

    public static readonly string Tag_Player = "Player";
    public static readonly string Tag_MainCamera = "MainCamera";
    public static readonly string Tag_GameManager = "GameManager";

    public static readonly float TimeSec_Reflife = 3.0f;
    public static readonly float TimeSec_CoinBurst_Destory = 1f;


    public static readonly string AudioFile_Loop = "Assets/Sounds/FutureWorld_Resolution_Loop.ogg";
    public static readonly string AudioFile_Death = "Assets/Sounds/CHARGE Sci-Fi High Pass Sweep 12 Semi Down 4000ms (stereo).wav";
    public enum GameState
    {
        Playing,Dead
    }
}
