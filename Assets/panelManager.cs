using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Use TMP se estiver usando TextMeshPro


public class panelManager : MonoBehaviour
{

    public GameObject materiaPanel;
    public Button materiaButton;
    public Button fecharMateria;
    // Start is called before the first frame update
    void Start()
    {
        materiaButton.onClick.AddListener(AbrirMateria);
        fecharMateria.onClick.AddListener(FecharMateria);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AbrirMateria()
    {
        materiaPanel.SetActive(true);
    }
    void FecharMateria()
    {
        materiaPanel.SetActive(false);
    }
}
