using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ItemListManager : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform contentPanel;

    public Button home;
    public Button criar;
    public Button perfil;
    public GameObject painel;
    float alturaTotal;
    private float tamanhoInicial = 20f; // Tamanho inicial do contentPanel
    private float spacing = 40f; // Espaçamento entre perguntas

    public pesquisaManager pesquisaManager;

    private int id_usuario;


    [System.Serializable]
    public class Item
    {
        public string id_jogo;
        public string nome;  // O campo 'nome' no banco de dados
        public string descricao;
        public string id_usuario;
        public string materia;
        public Sprite imagem;
    }

    [System.Serializable]
    public class ItemList
    {
        public List<Item> items;
    }

    void Start()
    {
       
        Screen.orientation = ScreenOrientation.Portrait;

        id_usuario = PlayerPrefs.GetInt("id_usuario");
        Debug.Log(id_usuario);

        // Inicia a chamada para pegar os itens do servidor
        StartCoroutine(GetJogosFromServer());

        // Adiciona um listener ao botão "criar" para ativar o painel
        criar.onClick.AddListener(AtivarPainel);

    }

    void AtivarPainel()
    {
        painel.SetActive(true); // Ativa o painel quando o botão "criar" é clicado
    }


    public IEnumerator GetJogosFromServer()
    {
        string url = "https://api-ajenick.onrender.com/get_items";

        string palavraChave = pesquisaManager.palavra;
        
        string materia = pesquisaManager.materia;
        

        if (!string.IsNullOrEmpty(palavraChave) || !string.IsNullOrEmpty(materia))
        {
            List<string> queryParams = new List<string>();

            if (!string.IsNullOrEmpty(palavraChave))
                queryParams.Add("nome=" + UnityWebRequest.EscapeURL(palavraChave));

            if (!string.IsNullOrEmpty(materia))
                queryParams.Add("materia=" + UnityWebRequest.EscapeURL(materia));

            url += "?" + string.Join("&", queryParams);
        }
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Recebe o JSON e deserializa para o objeto ItemList
            ItemList itemList = JsonUtility.FromJson<ItemList>("{\"items\":" + www.downloadHandler.text + "}");
            PopulateList(itemList.items);

            // Ajusta o tamanho do contentPanel após carregar os itens
            AjustarTamanhoContentPanelDinamicamente(itemList.items);
        }
    }

    void PopulateList(List<Item> itens)
    {

        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Item item in itens)
        {
            GameObject newItem = Instantiate(itemPrefab, contentPanel);

            TMP_Text titleText = newItem.transform.Find("ItemTitle").GetComponent<TMP_Text>();
            if (titleText != null)
            {
                titleText.text = item.nome;
            }

            TMP_Text descriptionText = newItem.transform.Find("ItemDescription").GetComponent<TMP_Text>();
            if (descriptionText != null)
            {
                descriptionText.text = item.descricao;
            }

            Image itemImage = newItem.transform.Find("ItemImage").GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.imagem;
            }

            Button button = newItem.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnItemClick(item.id_jogo));
            }
        }
    }

    void OnItemClick(string id_jogo)
    {
        PlayerPrefs.SetString("id_jogo", id_jogo);
        SceneManager.LoadScene("labirinto");
    }

    void AjustarTamanhoContentPanelDinamicamente(List<Item> items)
    {
        RectTransform contentRect = contentPanel.GetComponent<RectTransform>();
        if (contentRect == null)
        {
            Debug.LogError("O 'contentPanel' não tem um componente 'RectTransform'.");
            return;
        }

        alturaTotal = tamanhoInicial + (212 + spacing) * items.Count;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, alturaTotal);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }
}
