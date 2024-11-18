using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PerguntaManager : MonoBehaviour
{
    public GameObject perguntaPrefab; // Prefab da pergunta
    public Transform contentPanel; // Painel de conteúdo do ScrollView
    public Button adicionarPergunta; // Botão para adicionar novas perguntas
    public Button salvarPerguntas; // Botão para salvar as perguntas
    public PainelScript painelScript; // Referência ao PainelScript
    public DataUploader dataUploader;// Referência ao DataUploader
    public GameObject painelPerguntas;
    float alturaTotal;

    private int numeroPergunta = 1; // Contador de perguntas
    private List<GameObject> perguntasList = new List<GameObject>(); // Lista de perguntas

    private float tamanhoInicial = 106f; // Tamanho inicial do contentPanel
    private float spacing = 23f; // Espaçamento entre perguntas

    public Color erroColor = new Color(1f, 0f, 0f); // Cor vermelha (RGB: 255, 0, 0)
    public Color inputFieldOriginalColor = new Color(239f, 239f, 239f); // Cor original do TMP_InputField

    public string titulo;
    public string descricao;
    public string materia;

    void Start()
    {

        titulo = painelScript.titulo;
        descricao = painelScript.descricao;
        materia = painelScript.textoSelecionado;
        Debug.Log(materia);

        if (adicionarPergunta != null)
        {
            adicionarPergunta.onClick.AddListener(AdicionarNovaPergunta);
        }
        else
        {
            Debug.LogError("O botão 'adicionarPergunta' não foi atribuído.");
        }

        if (salvarPerguntas != null)
        {
            salvarPerguntas.onClick.AddListener(SalvarPerguntas);
        }
        else
        {
            Debug.LogError("O botão 'salvarPerguntas' não foi atribuído.");
        }


        AdicionarNovaPergunta();
    }

    void AdicionarNovaPergunta()
    {
        GameObject novaPergunta = Instantiate(perguntaPrefab, contentPanel);
        if (novaPergunta == null)
        {
            Debug.LogError("Falha ao instanciar o prefab da pergunta.");
            return;
        }

        TMP_Text perguntaText = novaPergunta.transform.Find("PerguntaText")?.GetComponent<TMP_Text>();
        if (perguntaText != null)
        {
            perguntaText.text = "Pergunta " + numeroPergunta;
        }
        else
        {
            Debug.LogError("Não foi possível encontrar 'QuestaoInputField' no prefab da pergunta.");
        }

        numeroPergunta++;
        perguntasList.Add(novaPergunta);
        AtualizarAlturaContentPanel();
    }

    void AtualizarAlturaContentPanel()
    {
        RectTransform contentRect = contentPanel.GetComponent<RectTransform>();
        if (contentRect == null)
        {
            Debug.LogError("O 'contentPanel' não tem um componente 'RectTransform'.");
            return;
        }

        alturaTotal = tamanhoInicial + (300 + spacing) * perguntasList.Count;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, alturaTotal);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    void SalvarPerguntas()
    {
        List<Pergunta> perguntas = new List<Pergunta>();
        bool todosCamposPreenchidos = true;

        foreach (GameObject perguntaGO in perguntasList)
        {
            TMP_InputField questaoField = perguntaGO.transform.Find("OpcoesPanel/QuestaoInputField")?.GetComponent<TMP_InputField>();
            TMP_InputField alternativa1Field = perguntaGO.transform.Find("OpcoesPanel/Alternativa1InputField")?.GetComponent<TMP_InputField>();
            TMP_InputField alternativa2Field = perguntaGO.transform.Find("OpcoesPanel/Alternativa2InputField")?.GetComponent<TMP_InputField>();
            TMP_InputField alternativa3Field = perguntaGO.transform.Find("OpcoesPanel/Alternativa3InputField")?.GetComponent<TMP_InputField>();
            TMP_InputField alternativa4Field = perguntaGO.transform.Find("OpcoesPanel/Alternativa4InputField")?.GetComponent<TMP_InputField>();

            Toggle toggle1 = perguntaGO.transform.Find("OpcoesPanel/Toggle1")?.GetComponent<Toggle>();
            Toggle toggle2 = perguntaGO.transform.Find("OpcoesPanel/Toggle2")?.GetComponent<Toggle>();
            Toggle toggle3 = perguntaGO.transform.Find("OpcoesPanel/Toggle3")?.GetComponent<Toggle>();
            Toggle toggle4 = perguntaGO.transform.Find("OpcoesPanel/Toggle4")?.GetComponent<Toggle>();

            if (questaoField != null && alternativa1Field != null && alternativa2Field != null && alternativa3Field != null && alternativa4Field != null && toggle1 != null && toggle2 != null && toggle3 != null && toggle4 != null)
            {
                if (!string.IsNullOrWhiteSpace(questaoField.text) &&
                    !string.IsNullOrWhiteSpace(alternativa1Field.text) &&
                    !string.IsNullOrWhiteSpace(alternativa2Field.text) &&
                    !string.IsNullOrWhiteSpace(alternativa3Field.text) &&
                    !string.IsNullOrWhiteSpace(alternativa4Field.text))
                {
                    Pergunta pergunta = new Pergunta
                    {
                        questao = questaoField.text,
                        alternativa1 = alternativa1Field.text,
                        alternativa2 = alternativa2Field.text,
                        alternativa3 = alternativa3Field.text,
                        alternativa4 = alternativa4Field.text,
                        indexRes = GetRespostaCorreta(toggle1, toggle2, toggle3, toggle4)
                    };
                    perguntas.Add(pergunta);
                    HighlightEmptyFields(questaoField, alternativa1Field, alternativa2Field, alternativa3Field, alternativa4Field);
                }
                else
                {
                    HighlightEmptyFields(questaoField, alternativa1Field, alternativa2Field, alternativa3Field, alternativa4Field);
                    todosCamposPreenchidos = false;
                }
            }
            else
            {
                Debug.LogError("Não foi possível encontrar todos os campos necessários no prefab da pergunta.");
                todosCamposPreenchidos = false;
            }
        }

        if (todosCamposPreenchidos)
        {
            if (dataUploader != null)
            {
                StartCoroutine(dataUploader.EnviarDadosParaAPI(titulo, descricao, perguntas, () =>
                {
                    SceneManager.LoadScene("home");
                }, materia));

            }
            else
            {
                Debug.LogError("O componente DataUploader não foi encontrado no GameObject 'dataUploader'.");
            }
        }
        else
        {
            Debug.LogError("Alguns campos estão vazios. Não foi possível salvar as perguntas.");
        }
    }


    void HighlightEmptyFields(params TMP_InputField[] fields)
    {
        foreach (TMP_InputField field in fields)
        {
            Image backgroundImage = field.GetComponent<Image>();
            if (backgroundImage != null)
            {
                if (string.IsNullOrWhiteSpace(field.text))
                {
                    backgroundImage.color = erroColor;
                }
                else
                {
                    backgroundImage.color = inputFieldOriginalColor;
                }
            }
        }
    }

    int GetRespostaCorreta(Toggle toggle1, Toggle toggle2, Toggle toggle3, Toggle toggle4)
    {
        if (toggle1.isOn) return 0;
        if (toggle2.isOn) return 1;
        if (toggle3.isOn) return 2;
        if (toggle4.isOn) return 3;
        return -1; // Nenhuma resposta correta selecionada
    }

    

    // Wrapper para serializar a lista de perguntas
    [System.Serializable]
    private class PerguntasWrapper
    {
        public List<Pergunta> perguntas;
    }

    [System.Serializable]
    public class Pergunta
    {
        public string questao;
        public string alternativa1;
        public string alternativa2;
        public string alternativa3;
        public string alternativa4;
        public int indexRes;
        public int id_jogo;
    }

    
}
