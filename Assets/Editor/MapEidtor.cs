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

        //CustomEditor Ű����� �� ������ ��ũ��Ʈ�� �ٷ� ���̶�� ������ ������Ʈ��
        //target���� ������ �� �ְ� �ڵ����� �����ȴ�.
        MapGenerator map = target as MapGenerator;

        map.GenerateMap();

    }
}
