using System;
using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    [SerializeField] private Animator[] doorAnimators;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && GameController.controller.minimapaAlvo[0]) return;
        doorAnimators[0].SetBool("shouldOpen", true);
        doorAnimators[1].SetBool("shouldOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && GameController.controller.minimapaAlvo[0]) return;
        doorAnimators[0].SetBool("shouldOpen", false);
        doorAnimators[1].SetBool("shouldOpen", false);
    }
}