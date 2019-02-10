using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectObjectOfType : ScriptableWizard
{

    public string searchType = "null";
    public string nom;
    public string description;
    public float prix;
    public float resistance;
    private Object[] fo;

    [MenuItem("My Tools/SelectObjectOfType...")]
    static void SelectObjectOfTypeWizard()
    {
        ScriptableWizard.DisplayWizard<SelectObjectOfType>("Séléction des objets du type...", "Choisir", "test");
    }

    void OnWizardCreate()
    {
        var foundObjects = FindObjectsOfType<Environnement>();
        Debug.Log(foundObjects + " : " + foundObjects.Length);
        fo = foundObjects;

        OnWizardOtherButton();
    }

    void OnWizardOtherButton()
    {
        for (int i = 0; i < fo.Length; i++)
        {
            Debug.Log(fo[i].ToString());
        }
    }
}