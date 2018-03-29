using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // 这个文件在手机上没有，需要使用条件编译
#endif
public class GameUtil {

    public static float GetRandomRate( )
    {
        return Random.Range(0, 100);
    }

    public static bool CheckAnimatorState(Animator m_anim, string stateName, float normalizedTime = 1f)
    {
        //获取动画层 0 指Base Layer.
        AnimatorStateInfo stateinfo = m_anim.GetCurrentAnimatorStateInfo(0);
        //如果正在播放walk动画.
        if ((stateinfo.normalizedTime < normalizedTime) && stateinfo.IsName("Base Layer." + stateName))
        {
            return true;
        }
        return false;
    }
    //hypotenuse 斜边
    public static float CosEulerAngle(float hypotenuse, float eulerAngle)
    {
        float value = hypotenuse * Mathf.Cos(eulerAngle * Mathf.Deg2Rad);
        //Debug.Log("[CosEulerAngle] value:" + value + ", hypotenuse:" + hypotenuse + ", eulerAngle:" + eulerAngle);
        return value;
    }

    //hypotenuse 斜边
    public static float SinEulerAngle(float hypotenuse, float eulerAngle)
    {
        float value = hypotenuse * Mathf.Sin(eulerAngle * Mathf.Deg2Rad);
        //Debug.Log("[SinEulerAngle] value:" + value + ", hypotenuse:" + hypotenuse + ", eulerAngle:" + eulerAngle);
        return value;
    }

    public static void PlayAudio(AudioSource audio_source, string fullPath)
    {
        AudioClip bgAudio = AssetDatabase.LoadAssetAtPath<AudioClip>(fullPath);
        Debug.Log("bgAudio loaded:" + bgAudio);
        audio_source.clip = bgAudio;
        audio_source.Play();
    }
}
