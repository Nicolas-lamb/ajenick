using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Registro : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField nomeUsuarioInput;
    public TMP_InputField senhaInput;
    public Button enviarButton;

    private string apiUrl = "http://localhost:6000/register"; // URL da sua API Flask
    public string proximaCena = "home"; // Nome da cena para qual você quer ir

    [System.Serializable]
    public class RespostaRegistro
    {
        public int id_usuario;
    }

    // Classe que representa os dados do registro
    [System.Serializable]
    public class DadosRegistro
    {
        public string Email;
        public string Senha;
        public string Nome;
    }

    void Start()
    {
       // if (PlayerPrefs.HasKey("id_usuario"))
       // {
        //    SceneManager.LoadScene(proximaCena);
        //}
        // Associa o evento do botão ao método EnviarDadosRegistro quando o botão é clicado
        enviarButton.onClick.AddListener(() => EnviarDadosRegistro());
    }

    // Função que será chamada ao clicar no botão para enviar os dados
    public void EnviarDadosRegistro()
    {
        string email = emailInput.text;
        string senha = senhaInput.text;
        string nomeUsuario = nomeUsuarioInput.text;

        // Inicia a coroutine para enviar os dados
        StartCoroutine(EnviarDadosRegistroCoroutine(email, senha, nomeUsuario));
    }

    // Coroutine que envia os dados para a API
    private IEnumerator EnviarDadosRegistroCoroutine(string email, string senha, string nomeUsuario)
    {
        // Cria um objeto DadosRegistro com os dados fornecidos
        DadosRegistro dados = new DadosRegistro
        {
            Email = email,
            Senha = senha,
            Nome = nomeUsuario
        };

        // Converte os dados para JSON
        string jsonData = JsonUtility.ToJson(dados);
        Debug.Log("Dados enviados para a API: " + jsonData);

        // Configura a requisição POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao adicionar usuario: " + request.error);
            yield break;
        }
        else
        {
            // Processa a resposta da API
            string responseText = request.downloadHandler.text;
            Debug.Log("Resposta da API: " + responseText);

            // Tenta extrair o ID do usuário da resposta
            try
            {
                // Classe para desserializar a resposta
                var resposta = JsonUtility.FromJson<RespostaRegistro>(responseText);

                if (resposta.id_usuario > 0)
                {
                    // Salva o ID do usuário nas PlayerPrefs
                    PlayerPrefs.SetInt("id_usuario", resposta.id_usuario);
                    PlayerPrefs.Save();
                    Debug.Log("ID do usuário salvo: " + resposta.id_usuario);

                    // Carrega a próxima cena
                    SceneManager.LoadScene(proximaCena);
                }
                else
                {
                    Debug.LogError("ID do usuário não retornado corretamente.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Erro ao processar a resposta: " + e.Message);
            }
        }
        SceneManager.LoadScene(proximaCena);
    }
}
