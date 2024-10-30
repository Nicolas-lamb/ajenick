using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Use TMP se estiver usando TextMeshPro

public class PainelScript : MonoBehaviour
{
    public TMP_InputField tituloInput; // Arraste o InputField Título aqui
    public TMP_InputField descricaoInput; // Arraste o InputField Descrição aqui
    public Button proximoButton;// Arraste o botão Próximo aqui
    public GameObject panelCriarPerguntas;
    public GameObject informacoesPanel;

    public string titulo { get; private set; } // Tornar público com apenas getter
    public string descricao { get; private set; } // Tornar público com apenas getter

    void Start()
    {
   
        proximoButton.onClick.AddListener(OnProximoClicked);
    }

    void OnProximoClicked()
    {
        // Obtém o texto dos inputs
         titulo = tituloInput.text;
         descricao = descricaoInput.text;

        // Verifica se o título e a descrição não estão vazios
        if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrWhiteSpace(descricao))
        {
            // Se ambos os campos estão preenchidos, fecha o painel de informações e abre o painel de criar perguntas
            informacoesPanel.SetActive(false);
            panelCriarPerguntas.SetActive(true);
        }
        else
        {
            // Se qualquer um dos campos estiver vazio, exibe uma mensagem de erro ou aviso
            Debug.LogWarning("Título e descrição devem ser preenchidos antes de avançar.");
            // Opcional: Adicione um feedback visual ao usuário, como um texto de erro na interface
        }
    }

}
