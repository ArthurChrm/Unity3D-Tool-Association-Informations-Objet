
using System.Collections.Generic;
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
    private GameObject selecteur;
    private bool desactiverInputSelectionManuelle = false;
    private bool desactiverAppliquerModificationAZoneChoisie = false;

    //Variables objet
    private string[] infoStrManuel = { "", "", "0", "0" };
    private float[] infoFloatManuel = { 0f, 0f };
    private string[] infoStrZone = { "", "", "0", "0" };
    private float[] infoFloatZone = { 0f, 0f };

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

        GUILayout.Label("Séléction manuelle dans la hierarchy:", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(desactiverInputSelectionManuelle);
        // Nom
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Nom : ");
        infoStrManuel[0] = EditorGUILayout.TextArea(infoStrManuel[0]);
        EditorGUILayout.EndHorizontal();

        // Description
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Description : ");
        infoStrManuel[1] = EditorGUILayout.TextArea(infoStrManuel[1]);
        EditorGUILayout.EndHorizontal();

        // Prix
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Prix : ");
        infoFloatManuel[0] = EditorGUILayout.FloatField(infoFloatManuel[0]);
        EditorGUILayout.EndHorizontal();

        // Resistance
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Resistance : ");
        infoFloatManuel[1] = EditorGUILayout.FloatField(infoFloatManuel[1]);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Appliquer"))
        {
            actionBoutonAppliquer();
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(" ", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        GUILayout.Label("Séléction à l'aide d'une zone :", EditorStyles.boldLabel);

        if (GUILayout.Button("Séléctionner une zone"))
        {
            actionBoutonSelectionZone();
        }
        EditorGUI.BeginDisabledGroup(desactiverAppliquerModificationAZoneChoisie);
        EditorGUI.BeginDisabledGroup(desactiverInputSelectionManuelle);
        // Nom
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Nom : ");
        infoStrZone[0] = EditorGUILayout.TextArea(infoStrZone[0]);
        EditorGUILayout.EndHorizontal();

        // Description
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Description : ");
        infoStrZone[1] = EditorGUILayout.TextArea(infoStrZone[1]);
        EditorGUILayout.EndHorizontal();

        // Prix
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Prix : ");
        infoFloatZone[0] = EditorGUILayout.FloatField(infoFloatZone[0]);
        EditorGUILayout.EndHorizontal();

        // Resistance
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Resistance : ");
        infoFloatZone[1] = EditorGUILayout.FloatField(infoFloatZone[1]);

        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Appliquer les modifications à la zone choisie"))
        {
            actionBoutonValidationZone();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(" ", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

    }
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        checkSiSelectionVide();
        checkSiSelectionValide();

        if (selectionEnvironnement && selectionNonNULL)
        {
            selectionValide = true;
            desactiverInputSelectionManuelle = false;
        }
        else
        {
            selectionValide = false;
            desactiverInputSelectionManuelle = true;
        }

        desactiverAppliquerModificationAZoneChoisie = selecteur == null ? true : false;


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
                obj.GetComponent<Environnement>().nom = infoStrManuel[0];
                obj.GetComponent<Environnement>().description = infoStrManuel[1];
                obj.GetComponent<Environnement>().prix = float.Parse(infoStrManuel[2]);
                obj.GetComponent<Environnement>().resistance = float.Parse(infoStrManuel[3]);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Erreur de sélection", "Votre sélection est invalide", "Ok");
        }
    }

    void actionBoutonSelectionZone()
    {
        Debug.Log("Action selection zone");
        List<GameObject> parts = new List<GameObject>();
        GameObject[] allObjects = UnityEngine.GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy && go.name == "Selecteur")
                DestroyImmediate(go);

        }
        selecteur = GameObject.CreatePrimitive(PrimitiveType.Cube);
        selecteur.name = "Selecteur";
        EditorUtility.DisplayDialog("Séléction d'une zone", "Séléctionnez une zone avec le cube.", "Ok");
        selecteur.transform.position = new Vector3(0, 0, 0);

    }
    void actionBoutonValidationZone()
    {
        GameObject[] objetsChoisis = UnityEngine.GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in objetsChoisis)
        {
            if (go.activeInHierarchy && go.GetComponent<Environnement>() != null)
            {
                if (selecteur.GetComponent<Renderer>().bounds.Contains(go.GetComponent<Renderer>().bounds.center))
                    Debug.Log("okkkkkk :" + go);
            }
        }
    }


}
