using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class OutilCDRIN : EditorWindow
{
    // Variables texte label
    private string labelSelectionObjet = "";
    private string labelErreurSelection = "";

    //  Variables messages d'erreurs
    private string AUCUNE_SELECTION = "Séléctionnez un ou plusieurs éléments !";
    private string SELECTION_PAS_TYPE_ENVIRONNEMENT = "Votre séléction comporte un ou plusieurs éléments ne contenant pas de composant \"Environnement\"";

    // Variables d'états
    private bool selectionNonNULL = false;
    private bool selectionEnvironnement = false;
    private bool selectionValide;
    private bool desactiverInputSelectionManuelle = false;
    private bool desactiverAppliquerModificationAZoneChoisie = false;

    //Variables objet
    private GameObject selecteur;
    private string[] infoStrManuel = { "", "" };
    private float[] infoFloatManuel = { 0f, 0f };
    private string[] infoStrZone = { "", "" };
    private float[] infoFloatZone = { 0f, 0f };
    private string[] infoStrSauvegarde = { "", "" };
    private float[] infoFloatSauvegarde = { 0f, 0f };
    private string nomFichier = "";

    // Autre
    string[] listeSauvegardes;
    int emplacementChoixRestauration = 0;

    [MenuItem("Outil CDRIN/Association Informations-Environnement")]
    public static void ShowWindow()
    {
        GetWindow<OutilCDRIN>("Outil CDRIN");
    }
    void OnGUI()
    {
        // Affichage des composants graphiques

        GUILayout.Label("Séléction manuelle dans la hierarchy", EditorStyles.boldLabel);
        GUILayout.Label(labelSelectionObjet);
        GUILayout.Label(labelErreurSelection);
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
            manuel_appliquer();
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(" ", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        GUILayout.Label("Séléction à l'aide d'une zone", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        if (GUILayout.Button("Séléctionner une zone"))
        {
            zone_selection();
        }

        EditorGUI.BeginDisabledGroup(desactiverAppliquerModificationAZoneChoisie);
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
            zone_appliquer();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(" ", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        // Partie sauvegarde/restauration
        GUILayout.Label("Sauvegarde", EditorStyles.boldLabel);

        // Nom
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Nom : ");
        infoStrSauvegarde[0] = EditorGUILayout.TextArea(infoStrSauvegarde[0]);
        EditorGUILayout.EndHorizontal();

        // Description
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Description : ");
        infoStrSauvegarde[1] = EditorGUILayout.TextArea(infoStrSauvegarde[1]);
        EditorGUILayout.EndHorizontal();

        // Prix
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Prix : ");
        infoFloatSauvegarde[0] = EditorGUILayout.FloatField(infoFloatSauvegarde[0]);
        EditorGUILayout.EndHorizontal();

        // Resistance
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Resistance : ");
        infoFloatSauvegarde[1] = EditorGUILayout.FloatField(infoFloatSauvegarde[1]);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        // Séléction nom fichier
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Nom de la sauvegarde : ");
        nomFichier = EditorGUILayout.TextArea(nomFichier);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Enregistrer"))
        {
            enregistrer();
        }

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(" ", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        GUILayout.Label("Restauration", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        emplacementChoixRestauration = EditorGUILayout.Popup("Sauvegarde à restaurer", emplacementChoixRestauration, listeSauvegardes);

    }

    void Awake()
    {
        miseAjoutListeSauvegardes();
    }
    void Update()
    {
        manuel_checkSelectionVide();
        manuel_checkSelectionValide();

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

    void manuel_checkSelectionVide()
    {
        // Va vérifier si l'utilisateur a séléctionné un élément
        selectionNonNULL = !(Selection.gameObjects.Length == 0);
        labelSelectionObjet = selectionNonNULL ? null : AUCUNE_SELECTION;
    }

    void manuel_checkSelectionValide()
    {
        // Va vérifier si la sélection ne comporte que des gameObjects ayant le type "Environnement"
        foreach (GameObject obj in Selection.gameObjects)
        {
            labelErreurSelection = obj.GetComponent<Environnement>() == null ? SELECTION_PAS_TYPE_ENVIRONNEMENT : null;
            selectionEnvironnement = obj.GetComponent<Environnement>() == null ? false : true;
        }
    }

    void manuel_appliquer()
    {
        if (!selectionValide)
        {
            EditorUtility.DisplayDialog("Erreur de sélection", "Votre sélection est invalide", "Ok");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            obj.GetComponent<Environnement>().nom = infoStrManuel[0];
            obj.GetComponent<Environnement>().description = infoStrManuel[1];
            obj.GetComponent<Environnement>().prix = float.Parse(infoStrManuel[2]);
            obj.GetComponent<Environnement>().resistance = float.Parse(infoStrManuel[3]);
        }
    }

    void zone_selection()
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
        EditorUtility.DisplayDialog("Sélection d'une zone", "Séléctionnez une zone avec le cube.", "Ok");
        selecteur.transform.position = new Vector3(0, 0, 0);

    }
    void zone_appliquer()
    {
        GameObject[] allObjects = UnityEngine.GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy && go.GetComponent<Environnement>() != null)
                if (selecteur.GetComponent<Renderer>().bounds.Contains(go.GetComponent<Renderer>().bounds.center))
                {
                    Debug.Log(go);
                    go.GetComponent<Environnement>().nom = infoStrZone[0];
                    go.GetComponent<Environnement>().description = infoStrZone[1];
                    go.GetComponent<Environnement>().prix = infoFloatZone[0];
                    go.GetComponent<Environnement>().resistance = infoFloatZone[1];
                }
        }
    }

    void enregistrer()
    {
        miseAjoutListeSauvegardes();
        if (nomFichier == "")
        {
            EditorUtility.DisplayDialog("Erreur", "Vous devez donner un nom au fichier.", "Ok");
            return;
        }
        SauvegardeEnvironnement temp = new SauvegardeEnvironnement();
        temp.nom = infoStrSauvegarde[0];
        temp.description = infoStrSauvegarde[1];
        temp.prix = infoFloatSauvegarde[0];
        temp.resistance = infoFloatSauvegarde[1];

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/SAV_" + nomFichier + ".dat", FileMode.OpenOrCreate);

        bf.Serialize(file, temp);
        file.Close();
        Debug.Log("Sauvegarde créée :" + Application.persistentDataPath);
        miseAjoutListeSauvegardes();
    }

    void miseAjoutListeSauvegardes()
    {
        string[] temp = Directory.GetFiles(Application.persistentDataPath);
        List<string> listeTemp = new List<string>();

        for (int i = 0; i < temp.Length; i++)
        {
            listeTemp.Add(Path.GetFileName(temp[i]));
        }

        listeSauvegardes = listeTemp.ToArray();
       
    }
}

[Serializable]
class SauvegardeEnvironnement
{
    public string nom;
    public string description;
    public float prix;
    public float resistance;
}

