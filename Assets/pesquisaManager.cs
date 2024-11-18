using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class pesquisaManager : MonoBehaviour
{

    public TMP_InputField palavraInput; // Arraste o InputField
    public Button pesquisar;// Arraste o bot�o Pr�ximo aqui
    public GameObject scrollView;
    public GameObject containerToggles;
    public ToggleGroup toggleGroup;
    public string textoSelecionado;

    public ItemListManager itemListManager;



    public string palavra;
    public string materia;
    // Start is called before the first frame update
    void Start()
    {
        pesquisar.onClick.AddListener(OnPesquisarClicked);
        palavraInput.onSelect.AddListener (OnAbrirMateriasClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAbrirMateriasClicked(string inputText)
    {
        containerToggles.SetActive(true);
        scrollView.SetActive(false);
    }

    void OnPesquisarClicked()
    {

        ConfirmarSelecao();
        // Obt�m o texto dos inputs
        palavra = palavraInput.text;
        materia = textoSelecionado;
 

        // Verifica se o t�tulo e a descri��o n�o est�o vazios
        
            // Se ambos os campos est�o preenchidos, fecha o painel de informa��es e abre o painel de criar perguntas
            containerToggles.SetActive(false);
            scrollView.SetActive(true);

        StartCoroutine(itemListManager.GetJogosFromServer());

    }
    public void ConfirmarSelecao()
    {
        // Itera por todos os Toggles dentro do ToggleGroup
        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn) // Verifica se o Toggle est� ativo
            {
                // Obt�m o texto do Label dentro do Toggle
                TMP_Text labelTexto = toggle.GetComponentInChildren<TMP_Text>();
                if (labelTexto != null)
                {
                    textoSelecionado = labelTexto.text;
                    Debug.Log("Texto selecionado: " + textoSelecionado);
                    return; // Sai do m�todo ap�s encontrar o Toggle ativo
                }
                else
                {
                    Debug.LogWarning("N�o foi poss�vel encontrar o TMP_Text no Toggle ativo.");
                }
            }
        }

        // Se nenhum Toggle estiver ativo
        Debug.LogWarning("Nenhum Toggle foi selecionado.");
    }
}
