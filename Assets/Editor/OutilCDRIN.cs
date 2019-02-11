
using UnityEngine;
using UnityEditor;

public class OutilCDRIN : EditorWindow
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
        GetWindow<OutilCDRIN>("Example");
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

        // Description
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Description : ");
        description = EditorGUILayout.TextArea(description);
        EditorGUILayout.EndHorizontal();

        // Prix
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Prix : ");
        prix = EditorGUILayout.FloatField(prix);
        EditorGUILayout.EndHorizontal();

        // Resistance
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Resistance : ");
        resistance = EditorGUILayout.FloatField(resistance);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Appliquer"))
        {
            actionBoutonAppliquer();
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Séléctionner une zone"))
        {
            actionBoutonSelectionZone();
        }
    }
    void Update()
    {
        checkSiSelectionVide();
        checkSiSelectionValide();

        if (selectionEnvironnement && selectionNonNULL)
            selectionValide = true;

    }

    void checkSiSelectionVide()
    {
        // Va vérifier si l'utilisateur a séléctionné un élément
        if (Selection.gameObjects.Length == 0)
        {
            labelSelectionObjet = ERREUR_AUCUNE_SELECTION;
            selectionNonNULL = false;
        }
        else
        {
            labelSelectionObjet = null;
            selectionNonNULL = true;
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
                selectionNonNULL = false;
                return;
            }
        }
        selectionEnvironnement = true;
        labelErreurSelection = null;
    }

    void actionBoutonAppliquer()
    {
        if (selectionValide)
        {
            Debug.Log("Action bouton");
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.GetComponent<Environnement>().nom = nom;
                obj.GetComponent<Environnement>().description = description;
                obj.GetComponent<Environnement>().prix = prix;
                obj.GetComponent<Environnement>().resistance = resistance;
            }
        }
    }

    void actionBoutonSelectionZone(){
        Debug.Log("Action selection zone");
    }

}
