#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DrawTransformLines : MonoBehaviour
{
    [SerializeField]public Texture2D texture;
    [SerializeField] string nameArray = "posicoes";
    [SerializeField] public Node<Component> node;
    [SerializeField] Transform[] posicoes;
    [SerializeField] int numVar = 3;


    private void OnDrawGizmos()
    {
        if (node.data == null) { posicoes = null; return; }

        SerializedObject seriaObject = new SerializedObject(node.data);
        SerializedProperty property = seriaObject.FindProperty(nameArray);
        Transform[] posArray = new Transform[property.arraySize];
        for (int i = 0; i < posArray.Length; i++)
        {
            posArray[i] = (Transform)property.GetArrayElementAtIndex(i).objectReferenceValue;
        }
        posicoes = posArray;
        int cont = 1;
        for (int i = 0; i < posicoes.Length; i++)
        {
            if ((float)((i + 1) / numVar) == cont)
            {
                Gizmos.DrawLine(posicoes[i].position, posicoes[i - (numVar - 1)].position);
                cont++;

            }
            else if (i + 1 < posicoes.Length)
            {
                Gizmos.DrawLine(posicoes[i].position, posicoes[i + 1].position);
            }

        }

    }

}


[CustomEditor(typeof(DrawTransformLines)),CanEditMultipleObjects]
public class DesenharLinhas_Editor : Editor
{
    private void OnEnable()
    {
        DrawTransformLines script = (DrawTransformLines)target;
        EditorGUIUtility.SetIconForObject(script, script.texture);
    }

}


[System.Serializable]
public class Node<T> where T : Component
{
    [SerializeField] public T data;
}
#endif