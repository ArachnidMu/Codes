using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    //切换场景后更改调用
    private void OnEnable() 
    {
        EventHandler.AfterSceneUnloadEvent += SwitchConfinerShape;
    }

    private void OnDisable() 
    {
        EventHandler.AfterSceneUnloadEvent -= SwitchConfinerShape;
    }
    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
        
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = confinerShape;

        // call this if the bounding shape's points change at runtime
        confiner.InvalidatePathCache();
    }
}