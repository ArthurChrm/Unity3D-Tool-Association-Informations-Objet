
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
        GetWindow<OutilCDRIN>("Outil CDRIN");
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
        //Debug.Log(Input.mousePosition);
        checkSiSelectionVide();
        checkSiSelectionValide();

        if (selectionEnvironnement && selectionNonNULL)
            selectionValide = true;
        else
            selectionValide = false;

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
        }else{
            EditorUtility.DisplayDialog ("Erreur de sélection", "Votre sélection est invalide", "Ok");
        }
    }

    void actionBoutonSelectionZone()
    {
        Debug.Log("Action selection zone");
        // SceneView sv = SceneView.currentDrawingSceneView;
        // Vector3 mousePosition = Event.current.mousePosition;
        // mousePosition.y = sv.camera.pixelHeight - mousePosition.y;
        // mousePosition = sv.camera.ScreenToWorldPoint(mousePosition);
        // mousePosition.y = -mousePosition.y;
        // Debug.Log(mousePosition.x + " , " + mousePosition.y + " , " + mousePosition.z);

        // GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        // Debug.Log(Input.mousePosition);
        GameObject selecteur = GameObject.CreatePrimitive(PrimitiveType.Cube);
        selecteur.name = "Selecteur";
        //selecteur.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        selecteur.GetComponent<Renderer>().material.color.a = 0;
        EditorUtility.DisplayDialog ("Séléction d'une zone", "Séléctionnez une zone avec le cube.", "Ok");
        selecteur.transform.position = new Vector3(0, 0.5f, 0);

    }


}
