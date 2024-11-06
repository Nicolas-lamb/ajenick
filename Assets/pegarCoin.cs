using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // texto
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class pegarCoin : MonoBehaviour
{
    public GameObject painelVencer;
    [SerializeField] private Button[] Button;
    public GameObject otherObject;
    public TMP_Text questao;
    public GameObject painel;
    public TMP_Text[] textButtons;
    public TMP_Text coinsText;
    public SpawnManager spawnManager;
    [SerializeField] private Button VoltarHome;
    public GameObject light;

    int value;
    bool cricou;
    int errou = 0;
    private List<Pergunta> perguntas = new List<Pergunta>();
    private int indicePerguntaAtual = 0;

    void Awake()
    {
        for (int i = 0; i < Button.Length; i++)
        {
            int index = i;
            Button[i].onClick.AddListener(() => OnButtonClick(index));
        }
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Start()
    {
        VoltarHome.onClick.AddListener(OnButtonClickVenceu);
        StartCoroutine(GetQuestionsFromAPI());
        string id_jogo = PlayerPrefs.GetString("id_jogo", "ID não definido");
        Debug.Log(id_jogo);
        
    }

    void Update()
    {
        value = Player_input.coinsCont;
        
    }

    IEnumerator GetQuestionsFromAPI()
    {
        string url = "http://127.0.0.1:6000/get_questions?id_jogo=" + PlayerPrefs.GetString("id_jogo");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            // Deserializando o JSON para a lista de perguntas
            PerguntaList perguntaList = JsonUtility.FromJson<PerguntaList>("{\"perguntas\":" + json + "}");
            perguntas = perguntaList.perguntas;

            // Debug para verificar o conteúdo de perguntas
            foreach (var pergunta in perguntas)
            {
                Debug.Log("Questão: " + pergunta.questao);
                Debug.Log("Alternativas: " + string.Join(", ", pergunta.listaAlt));
                Debug.Log("Resposta Correta: " + pergunta.resposta);
            }

            SetupQuestion();
        }
        else
        {
            Debug.LogError("Erro ao buscar perguntas: " + request.error);
        }
    }

    void SetupQuestion()
    {
        if (indicePerguntaAtual < perguntas.Count)
        {
            Pergunta perguntaAtual = perguntas[indicePerguntaAtual];
            questao.text = perguntaAtual.questao;
            textButtons[0].text = perguntaAtual.listaAlt[0];
            textButtons[1].text = perguntaAtual.listaAlt[1];
            textButtons[2].text = perguntaAtual.listaAlt[2];
            textButtons[3].text = perguntaAtual.listaAlt[3];
        }
        else
        {
            
            painelVencer.SetActive(true);
        }
        
    }
    private void OnButtonClickVenceu()
    {
        SceneManager.LoadScene("home");
    }

    private void OnButtonClick(int index)
    {
        Pergunta perguntaAtual = perguntas[indicePerguntaAtual];
        bool res = (index == perguntaAtual.resposta);
        if (res)
        {
            Debug.Log("Resposta correta!");
            light.SetActive(true);
            Player_input.coinsCont += 1;
            coinsText.text = "X" + Player_input.coinsCont;
            indicePerguntaAtual++;
            SetupQuestion();
            spawnManager.GeneratePrefab();

            if (painel.activeSelf)
            {
                painel.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Errou!" + errou);
            spawnManager.GeneratePrefab();
            painel.SetActive(false);

            errou++;
        }
    }

    [System.Serializable]
    public class Pergunta
    {
        public string questao;
        public string alternativa1;
        public string alternativa2;
        public string alternativa3;
        public string alternativa4;
        public int resposta;

        public string[] listaAlt => new string[] { alternativa1, alternativa2, alternativa3, alternativa4 };
    }

    [System.Serializable]
    public class PerguntaList
    {
        public List<Pergunta> perguntas;
    }
}
