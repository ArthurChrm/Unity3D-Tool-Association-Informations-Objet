
using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{


    private string ERREUR_AUCUNE_SELECTION = "Séléctionnez un ou plusieurs éléments !";
    private string ERREUR_PAS_ENVIRONNEMENT = "Votre séléction comporte un ou plusieurs éléments ne contenant pas de composant \"Environnement\"";
    private string labelSelectionObjet = "";
    private string labelErreurSelection = "";
    private bool selectionNonNULL = false;
    private bool selectionEnvironnement = false;
    private bool selectionValide;

    //Variables objet
    private string nom = "saisir un nom";
    private string description = "saisir une description";
    private float prix = 0f;
    private float resistance = 0f;

    [MenuItem("Outil CDRIN/Association Informations-Environnement")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("Example");
    }
    void OnGUI()
    {
        // Window code
        GUILayout.Label(labelSelectionObjet);
        GUILayout.Label(labelErreurSelection);

        EditorGUILayout.Separator();

        // Nom
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Nom : ");
        nom = EditorGUILayout.TextArea(nom);
        EditorGUILayout.EndHorizontal();

        //Description
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Description : ");
        description = EditorGUILayout.TextArea(description);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Prix : ");
        prix = EditorGUILayout.FloatField(prix);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Resistance : ");
        resistance = EditorGUILayout.FloatField(resistance);
        EditorGUILayout.EndHorizontal();
    }
    void Update()
    {

        checkSiSelectionVide();
        checkSiSelectionValide();

        if (selectionEnvironnement && selectionNonNULL)
            selectionValide = true;

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
