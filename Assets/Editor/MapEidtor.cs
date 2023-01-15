using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEidtor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //CustomEditor 키워드로 이 에디터 스크립트가 다룰 것이라고 선언한 오브젝트는
        //target으로 접근할 수 있게 자동으로 설정된다.
        MapGenerator map = target as MapGenerator;

        map.GenerateMap();

    }
}
