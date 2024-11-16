using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePasswordVisibility : MonoBehaviour
{
    public TMP_InputField senhaInput;  // Campo de senha (InputField)
    public Button toggleButton;        // Botão que alterna a visibilidade
    public Image backgroundImage;      // A imagem de fundo que queremos alterar
    public Sprite imagemSenhaOculta;   // Imagem a ser mostrada quando a senha estiver oculta
    public Sprite imagemSenhaVisivel;  // Imagem a ser mostrada quando a senha estiver visível

    private bool isPasswordVisible = false;

    void Start()
    {
        // Inicializa o campo de senha com o tipo de conteúdo como "Password" (ocultando a senha)
        senhaInput.contentType = TMP_InputField.ContentType.Password;
        senhaInput.ForceLabelUpdate();

        // Configura o botão para alternar a visibilidade da senha
        toggleButton.onClick.AddListener(TogglePassword);

        // Define a imagem inicial para a senha oculta
        backgroundImage.sprite = imagemSenhaOculta;
    }

    // Função para alternar a visibilidade da senha e a imagem de fundo
    private void TogglePassword()
    {
        isPasswordVisible = !isPasswordVisible;

        // Altera o tipo de conteúdo da senha para exibir ou ocultar
        senhaInput.contentType = isPasswordVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        senhaInput.ForceLabelUpdate();

        // Altera a imagem do fundo dependendo da visibilidade da senha
        backgroundImage.sprite = isPasswordVisible ? imagemSenhaVisivel : imagemSenhaOculta;
    }
}
