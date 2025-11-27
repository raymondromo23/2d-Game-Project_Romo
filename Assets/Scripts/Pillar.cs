using System.Collections;
using UnityEngine;

public class Pillar : MonoBehaviour, IInteractable
{
    public Animator anim;
    private bool turnedOn = true;
    public void Interact()
    {
        if (turnedOn)
        {
            Debug.Log("You activated the Rune Pillar");
            anim.SetBool("Off", true);
            turnedOn = false;


            //5 second cooldown
            StartCoroutine(Cooldown());
        }
        else
            Debug.Log("Rune Pillar is recharging...");
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5);

        anim.SetBool("Off", false);
        turnedOn = true;

        Debug.Log("Rune Pillar is active!");
    }

}

