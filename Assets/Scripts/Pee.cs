using DG.Tweening;
using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pee : MonoBehaviour, IShowable, IInteractable
{
    private string curruentValue = "Toilet";
    public PlayerController playerController;
    public ItemChange handHide;
    [SerializeField] private Vector3 endRotation;
    private bool isPee = false;
    public Transform toi;
    private float lerp = 2;
    private Quaternion initialRotation;
    public AudioSource footStep;

    private void Start()
    {
        initialRotation = toi.localRotation;
    }

    void Toilet()
    {
        if (!isPee)
        {
            
            curruentValue = "Toilet";
            setToiletRotation(endRotation);
            Debug.Log("Work!");
            Debug.Log("Peeing...");
            playerController.canMove = false;
            playerController.CanRunning = false;
            playerController.CroughKey = KeyCode.None;
            playerController.jumpSpeed = 0;
            footStep.enabled = false;
            handHide.Hide(true);
            StartCoroutine(peeing());
        }
        else
        {
            isPee = false;
            curruentValue = "Toilet";
            setToiletRotation(initialRotation.eulerAngles);
        }
    }


    IEnumerator peeing()
    {
        yield return new WaitForSeconds(5);
        isPee = true;
        Debug.Log("Finish");
        playerController.CroughKey = KeyCode.LeftControl;
        playerController.canMove = true;
        playerController.CanRunning = true;
        playerController.jumpSpeed = 6;
        footStep.enabled = true;
        handHide.Hide(false);
    }

    void setToiletRotation(Vector3 _endRotation)
    {
        toi.DOLocalRotate(_endRotation, lerp);
    }

    public string value { get => curruentValue; }

    public void Interact()
    {
        Toilet();
    }
}
