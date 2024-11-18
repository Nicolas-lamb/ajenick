using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataUploader : MonoBehaviour
{
    private string baseUrl = "https://api-ajenick.onrender.com"; // URL base da API Flask

    public IEnumerator EnviarDadosParaAPI(string titulo, string descricao, List<PerguntaManager.Pergunta> perguntas, System.Action callback, string materia)
    {
        // Adicionar o jogo
        int id_usuario = PlayerPrefs.GetInt("id_usuario");
        var jogoInfo = new JogoInfo(titulo, descricao, materia, id_usuario);
        string jogoInfoJson = JsonUtility.ToJson(jogoInfo);
        Debug.Log(jogoInfoJson);

        UnityWebRequest request = new UnityWebRequest($"{baseUrl}/add_game", "POST");
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(jogoInfoJson);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao adicionar jogo: " + request.error);
            yield break;
        }

        int idJogo;
        if (int.TryParse(request.downloadHandler.text, out idJogo))
        {
            // Adicionar as perguntas
            foreach (var pergunta in perguntas)
            {
                pergunta.id_jogo = idJogo;
            }

            var perguntasWrapper = new PerguntasWrapper(perguntas);
            string perguntasWrapperJson = JsonUtility.ToJson(perguntasWrapper);
            UnityWebRequest perguntasRequest = new UnityWebRequest($"{baseUrl}/add_questions", "POST");
            byte[] perguntasJsonToSend = System.Text.Encoding.UTF8.GetBytes(perguntasWrapperJson);
            perguntasRequest.uploadHandler = new UploadHandlerRaw(perguntasJsonToSend);
            perguntasRequest.downloadHandler = new DownloadHandlerBuffer();
            perguntasRequest.SetRequestHeader("Content-Type", "application/json");

            yield return perguntasRequest.SendWebRequest();

            if (perguntasRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro ao adicionar perguntas: " + perguntasRequest.error);
            }
            else
            {
                Debug.Log("Perguntas adicionadas com sucesso!");
            }
        }
        else
        {
            Debug.LogError("Erro ao obter ID do jogo.");
        }
        // Chama o callback após a conclusão
    callback?.Invoke();
    }
    

    [System.Serializable]
    public class JogoInfo
    {
        public string Titulo;
        public string Descricao;
        public string Materia;
        public int Usuario;

        public JogoInfo(string titulo, string descricao, string materia, int usuario)
        {
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.Materia = materia;
            this.Usuario = usuario;
        }
    }

    [System.Serializable]
    public class PerguntasWrapper
    {
        public List<PerguntaManager.Pergunta> perguntas;

        public PerguntasWrapper(List<PerguntaManager.Pergunta> perguntas)
        {
            this.perguntas = perguntas;
        }
    }
}
