using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChangeCatColor : MonoBehaviour, IInteractable
{
 
    public Animator anim;
    private bool IsWhite = false;
    public void Interact()
    {
       

        //add ug open nga chest choy
        if (!IsWhite)
        {
            Debug.Log("You made the cat WHITE.");
            anim.SetBool("isWhite", true);
            IsWhite = true;
        }
        else
        {
            Debug.Log("You turned the cat BLACK");
            anim.SetBool("isWhite", false);
            IsWhite = false;
        }


    }
}
