#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DrawTransformLines : MonoBehaviour
{
    [SerializeField]public Texture2D texture;
    [SerializeField] private string nameArray = "posicoes";
    [SerializeField] public Node<Component> node;
    [SerializeField] private Transform[] posicoes;
    [SerializeField] private int numVar = 3;


    private void OnDrawGizmos()
    {
        if (node.data == null) { posicoes = null; return; }

        var seriaObject = new SerializedObject(node.data);
        var property = seriaObject.FindProperty(nameArray);
        var posArray = new Transform[property.arraySize];
        for (var i = 0; i < posArray.Length; i++)
        {
            posArray[i] = (Transform)property.GetArrayElementAtIndex(i).objectReferenceValue;
        }
        posicoes = posArray;
        var cont = 1;
        for (var i = 0; i < posicoes.Length; i++)
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
        var script = (DrawTransformLines)target;
        EditorGUIUtility.SetIconForObject(script, script.texture);
    }

}


[System.Serializable]
public class Node<T> where T : Component
{
    [SerializeField] public T data;
}
#endif