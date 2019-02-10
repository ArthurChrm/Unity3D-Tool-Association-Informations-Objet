
using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{


    private string ERREUR_AUCUNE_SELECTION = "Séléctionnez un ou plusieurs éléments !";
    private string ERREUR_PAS_ENVIRONNEMENT = "Votre séléction comporte un ou plusieurs éléments ne contenant pas de composant \"Environnement\"";
    private string labelSelectionObjet = "";
    private string labelErreurSelection = "";

    [MenuItem("Outil CRIN/Association Informations-Environnement")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("Example");
    }
    void OnGUI()
    {
        // Window code
        GUILayout.Label(labelSelectionObjet);
        GUILayout.Label(labelErreurSelection);
    }
    void Update()
    {

        checkSiSelectionVide();
        checkSiSelectionValide();



        // On va vérifier si un objet est séléctionné
        foreach (GameObject obj in Selection.gameObjects)
        {
            Debug.Log(obj.GetComponent<Environnement>());
            //Debug.Log(obj.ToString());
        }
    }

    void checkSiSelectionVide()
    {
        // Va vérifier si l'utilisateur a séléctionné un élément
        if (Selection.gameObjects.Length == 0)
        {
            labelSelectionObjet = ERREUR_AUCUNE_SELECTION;
        }
        else
        {
            labelSelectionObjet = null;
        }
    }

    void checkSiSelectionValide()
    {
        // Va vérifier si la séléction ne comporte que des gameObjects ayant le type "Environnement"
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<Environnement>() == null)
            {
                labelErreurSelection = ERREUR_PAS_ENVIRONNEMENT;
                return;
            }
        }
        labelErreurSelection = null;
    }

}
