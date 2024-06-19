using UnityEngine;


public class CheckTouchGround : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
            GameController.controller.Damage(GameController.controller.carIntegrityMax);
    }
}