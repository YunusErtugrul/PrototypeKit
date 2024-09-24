using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorScript : MonoBehaviour, IInteractable, IShowable
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float lerpValue;
    [SerializeField] private float cooldownTime = 0.3f;
    [SerializeField] private AudioSource audioS;
    [SerializeField] private AudioClip door_open;
    [SerializeField] private AudioClip door_close;
    private Quaternion initialRotation;
    private string currentValue = "Door";
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        initialRotation = doorTransform.localRotation;
    }

    void OpenDoor()
    {
        if (isAnimating) return;

        isAnimating = true;

        if (!isOpen)
        {
            isOpen = true;
            currentValue = "Door";
            SetDoorRotation(openRotation);
            audioS.PlayOneShot(door_open);
        }
        else
        {
            isOpen = false;
            currentValue = "Door";
            SetDoorRotation(initialRotation.eulerAngles); //Vector3.Zero
            StartCoroutine(WaitDoor());
        }
    }

    IEnumerator WaitDoor()
    {
        yield return new WaitForSeconds(.65f);
        audioS.PlayOneShot(door_close);
    }

    void SetDoorRotation(Vector3 targetRotation)
    {
        doorTransform.DOLocalRotate(targetRotation, lerpValue).OnComplete(() =>
        {
            StartCoroutine(CooldownCoroutine());
        });
    }

    IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        isAnimating = false;
    }

    public string value { get => currentValue; }

    public void Interact()
    {
        OpenDoor();
    }
}
