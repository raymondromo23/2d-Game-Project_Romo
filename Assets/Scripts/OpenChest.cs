using UnityEngine;

public class OpenChest : MonoBehaviour, IInteractable
{
    public Animator anim;
    private bool isOpen = false;
    public void Interact()
    {
        
        //add ug open nga chest choy
        if (!isOpen)
        {
            Debug.Log("You opened the chest!");
            anim.SetBool("isOpen", true);
            isOpen = true;
        }
        else
        {
            Debug.Log("You closed the chest!");
            anim.SetBool("isOpen", false);
            isOpen = false;
        }

    }
}
