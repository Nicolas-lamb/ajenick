using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Use TMP se estiver usando TextMeshPro

public class PainelScript : MonoBehaviour
{
    public TMP_InputField tituloInput; // Arraste o InputField T�tulo aqui
    public TMP_InputField descricaoInput; // Arraste o InputField Descri��o aqui
    public Button proximoButton;// Arraste o bot�o Pr�ximo aqui
    public GameObject panelCriarPerguntas;
    public GameObject informacoesPanel;

    public string titulo { get; private set; } // Tornar p�blico com apenas getter
    public string descricao { get; private set; } // Tornar p�blico com apenas getter

    void Start()
    {
   
        proximoButton.onClick.AddListener(OnProximoClicked);
    }

    void OnProximoClicked()
    {
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

}
