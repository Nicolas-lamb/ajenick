using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

         //if (PlayerPrefs.HasKey("id_usuario"))
       // {
         //   SceneManager.LoadScene("home");
        //}
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
