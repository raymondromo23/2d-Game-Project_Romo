using UnityEngine;

public class PettingCat : MonoBehaviour, IInteractable
{
    public CatMovements catScript;
    public void Interact()
    {
        Debug.Log("You give pats to the cat");
        //add animationg ari choy

        if (catScript != null)
            catScript.StartCoroutine(catScript.GetPetRoutine());
        //mo lingkod lang ang cat for pila ka seconds
    }
}
