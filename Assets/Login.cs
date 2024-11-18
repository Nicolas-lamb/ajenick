using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Login: MonoBehaviour
{
    public TMP_InputField emailInput; // Campo para o email
    public TMP_InputField senhaInput; // Campo para a senha
    public Button loginButton; // Botão de login
    private string apiUrl = "https://api-ajenick.onrender.com/login"; // URL da sua API Flask
    public string proximaCena = "home"; // Nome da cena para onde irá após o login
    public TMP_Text errorMessageText;
    public GameObject errorPanel;
    public Button fecharButton;

    public class DadosLogin
    {
        public string Email;
        public string Senha;
    }

    void Start()
    {
        loginButton.onClick.AddListener(() => RealizarLogin());
        fecharButton.onClick.AddListener(() => fecharErro());

      
    }

    public void RealizarLogin()
    {
        string Email = emailInput.text;
        string Senha = senhaInput.text;

        // Inicia a Coroutine para fazer o login
        StartCoroutine(LoginCoroutine(Email, Senha));
    }

    private IEnumerator LoginCoroutine(string email, string senha)
    {
        // Cria o JSON com os dados de login
        DadosLogin dados = new DadosLogin
        {
            Email = email,
            Senha = senha
        };

        string jsonData = JsonUtility.ToJson(dados);
        Debug.Log(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro ao fazer login: " + request.error);

                // Se a resposta for erro 401 (senha incorreta), mostramos o painel de erro
                if (request.responseCode == 401)
                {
                    ShowError("Senha incorreta. Tente novamente.");
                }
                else if (request.responseCode == 404)
                {
                    ShowError("Usuário não encontrado.");
                }
                else if(request.responseCode == 500)
                {
                    ShowError("Erro do servidor");
                }
                else
                {
                    ShowError("Erro ao tentar fazer login.");
                }
                
            }
            else
            {
                Debug.Log("Login bem-sucedido! Resposta: " + request.downloadHandler.text);

                // Pega o ID retornado e salva nas PlayerPrefs
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                PlayerPrefs.SetInt("id_usuario", response.id_usuario);

                // Carrega a próxima cena
                SceneManager.LoadScene(proximaCena);
            }
        
    }
    private void ShowError(string message)
    {
        errorMessageText.text = message; // Define a mensagem de erro
        errorPanel.SetActive(true); // Ativa o painel de erro
    }

    public void fecharErro()
    {
        errorPanel.SetActive(false);
    }

    [System.Serializable]
    public class LoginResponse
    {
        public int id_usuario;
    }
}
