using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class PainelScript : MonoBehaviour
{
    public TMP_InputField tituloInput; // Arraste o InputField T�tulo aqui
    public TMP_InputField descricaoInput; // Arraste o InputField Descri��o aqui
    public Button proximoButton;// Arraste o bot�o Pr�ximo aqui
    public GameObject panelCriarPerguntas;
    public GameObject informacoesPanel;
    public ToggleGroup toggleGroup;
    public string textoSelecionado;

    public string titulo { get; private set; } // Tornar p�blico com apenas getter
    public string descricao { get; private set; } // Tornar p�blico com apenas getter

    void Start()
    {
   
        proximoButton.onClick.AddListener(OnProximoClicked);
     

    }

    void OnProximoClicked()
    {

        ConfirmarSelecao();
        // Obt�m o texto dos inputs
        titulo = tituloInput.text;
         descricao = descricaoInput.text;

        // Verifica se o t�tulo e a descri��o n�o est�o vazios
        if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrWhiteSpace(descricao))
        {
            // Se ambos os campos est�o preenchidos, fecha o painel de informa��es e abre o painel de criar perguntas
            informacoesPanel.SetActive(false);
            panelCriarPerguntas.SetActive(true);
        }
        else
        {
            // Se qualquer um dos campos estiver vazio, exibe uma mensagem de erro ou aviso
            Debug.LogWarning("T�tulo e descri��o devem ser preenchidos antes de avan�ar.");
            // Opcional: Adicione um feedback visual ao usu�rio, como um texto de erro na interface
        }
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
