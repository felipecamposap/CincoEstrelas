using UnityEngine;

public class AlvoMinimapa : MonoBehaviour
{
#if UNITY_EDITOR
    private bool check = false;
#endif
    public int index;
    private Vector3 correctedAlvoMinimapa;

    public void Start()
    {
#if UNITY_EDITOR
        try
        {
            GameController.controller.alvoMinimapa = this;
        }
        catch
        {
            check = true;
            return;
        }
#endif
        GameController.controller.alvoMinimapa = this;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (check)
            return;
#endif
        if (index == 2)
            index = 0;

        if (GameController.controller.minimapaAlvo[index] != null)
        {
            correctedAlvoMinimapa = new Vector3(
                GameController.controller.minimapaAlvo[index].transform.position.x,
                transform.position.y,
                GameController.controller.minimapaAlvo[index].transform.position.z);
            transform.LookAt(correctedAlvoMinimapa);
        }
    }
}