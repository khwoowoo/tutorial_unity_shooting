using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEidtor : Editor
{
    public override void OnInspectorGUI()
    {
        //�̰� ��ũ��Ʈ�� ����Ǿ� ������ ��� �ٲ��
        //������ ��Ʈ��Ʈ�� �����Ǵ°� ū ��������
        //������ �� ����������. ����, ���� �����Ǿ��� �� ��
        //�����ǰ� �ٲ��� �Ѵ�
        //base.OnInspectorGUI();


        //CustomEditor Ű����� �� ������ ��ũ��Ʈ�� �ٷ� ���̶�� ������ ������Ʈ��
        //target���� ������ �� �ְ� �ڵ����� �����ȴ�.
        MapGenerator map = target as MapGenerator;

        //�����Ǿ��� �� ��
        if (DrawDefaultInspector())
        {
            map.GenerateMap();
        }

        //���� �ִ� ��, �ν�����â
        //�Ʒ��� ��ũ��Ʈ�� �������� �� �����ϰ� ���� �� ���
        if(GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }


    }
}
