using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEidtor : Editor
{
    public override void OnInspectorGUI()
    {
        //이건 스크립트가 적용되어 있으면 계속 바뀐다
        //하지만 오트젝트가 생성되는게 큰 사이즈라면
        //성능이 안 좋아지진다. 따라서, 맵이 수정되었을 때 만
        //수정되게 바뀌어야 한다
        //base.OnInspectorGUI();


        //CustomEditor 키워드로 이 에디터 스크립트가 다룰 것이라고 선언한 오브젝트는
        //target으로 접근할 수 있게 자동으로 설정된다.
        MapGenerator map = target as MapGenerator;

        //수정되었을 때 만
        if (DrawDefaultInspector())
        {
            map.GenerateMap();
        }

        //위에 있는 건, 인스팩터창
        //아래는 스크립트에 수정했을 때 갱신하고 싶을 때 사용
        if(GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }


    }
}
