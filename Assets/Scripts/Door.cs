using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Door : MonoBehaviour, IInteractable
{
   public Animator anim;
   private bool isOpen = false;

    public void Interact()
    {
        if (!isOpen)
        {
            Debug.Log("You opened the door!");
            anim.SetBool("IsOpen", true);
            isOpen = true;
        }
        else
        {
            Debug.Log("You closed the door!");
            anim.SetBool("IsOpen", false);
            isOpen = false;
        }
    }
}
