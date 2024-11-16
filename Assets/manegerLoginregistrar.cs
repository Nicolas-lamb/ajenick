using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class manegerLoginregistrar : MonoBehaviour  
{
    public Button loginButton;
    public Button registroButton;
    public GameObject registroPanel;
    public GameObject loginPanel;
    public GameObject tela;


    // Start is called before the first frame update
    void Start()
    {
        registroButton.onClick.AddListener(() => abrirRegistro());
        loginButton.onClick.AddListener(() => abrirLogin());
    }

    public void abrirLogin()
    {
        loginPanel.SetActive(true);
        tela.SetActive(false);
    }
    public void abrirRegistro()
    {
        registroPanel.SetActive(true);
        tela.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
