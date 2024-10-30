using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //texto
using UnityEngine.UI;
using Unity.VisualScripting;

public class pegarCoins : MonoBehaviour
{
    [SerializeField] private Button[] Button;
    public GameObject otherObject;
    public TMP_Text questao;
    public GameObject painel;
    public TMP_Text[] textButtons;
    public TMP_Text coinsText;

    int value;
    bool cricou;

    Pergunta p1 = new Pergunta();

    void Awake()
    {
        // Adicionando listeners aos botões e passando o índice correspondente
        for (int i = 0; i < Button.Length; i++)
        {
            int index = i; // Captura do índice localmente para uso na expressão lambda
            Button[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Player_input player_Input = otherObject.GetComponent<Player_input>();
        SetupQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        value = Player_input.coinsCont;
    }

    // Função para configurar a pergunta e as respostas
    void SetupQuestion()
    {
        p1.Questao = "Analisando a equação do segundo grau x² – 2x +1 = 0, podemos afirmar que ela possui:";
        p1.listaAlt[0] = "Nenhuma solução real";
        p1.listaAlt[1] = "Uma única solução real";
        p1.listaAlt[2] = "Duas soluções reais.";
        p1.listaAlt[3] = "Infinitas soluções reais.";
        p1.IndexRes = 2;

        questao.text = p1.Questao;
        textButtons[0].text = p1.listaAlt[0];
        textButtons[1].text = p1.listaAlt[1];
        textButtons[2].text = p1.listaAlt[2];
        textButtons[3].text = p1.listaAlt[3];
    }

    // Função para lidar com o clique do botão, recebendo o índice do botão clicado
    private void OnButtonClick(int index)
    {
        bool res = (index == p1.IndexRes);
        if (res)
        {
            Debug.Log("Resposta correta!");
            SetButtonColor(Button[index], "#54B84F");
           // StartCoroutine(HandleCorrectAnswer()); // Inicia a corrotina
        }
        else
        {
            Debug.Log("Errou!");
            SetButtonColor(Button[index], "#DC5049");// Cor vermelha em hexadecimal
        }
    }

    // Função para alterar a cor do botão usando uma string hexadecimal
    void SetButtonColor(Button button, string hexColor)
    {
        Color color;
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ColorBlock cb = button.colors;
            cb.normalColor = color;
            cb.highlightedColor = color;
            cb.pressedColor = color;
            cb.selectedColor = color;
            button.colors = cb;
        }
        else
        {
            Debug.LogError("Cor hexadecimal inválida: " + hexColor);
        }
    }

    // Corrotina para lidar com a resposta correta
    IEnumerator HandleCorrectAnswer()
    {
        Debug.Log("Iniciando a espera de 1 segundo...");
        yield return new WaitForSeconds(1f); // Espera por 1 segundo
        Debug.Log("1 segundo passado, atualizando o painel.");
        Player_input.coinsCont += 1; // Incrementa coinsCont
        coinsText.text = "X" + Player_input.coinsCont;

        // Confirma se o painel ainda está ativo antes de desativar
        if (painel.activeSelf)
        {
            Debug.Log("Painel ainda está ativo, desativando agora.");
            painel.SetActive(false); // Desativa o painel
        }
        else
        {
            Debug.Log("Painel já está desativado.");
        }
    }

    public class Pergunta
    {
        public string Questao;
        public string[] listaAlt = new string[4];
        public int IndexRes;
    }
}
