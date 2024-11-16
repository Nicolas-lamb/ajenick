using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePasswordVisibility : MonoBehaviour
{
    public TMP_InputField senhaInput;  // Campo de senha (InputField)
    public Button toggleButton;        // Bot�o que alterna a visibilidade
    public Image backgroundImage;      // A imagem de fundo que queremos alterar
    public Sprite imagemSenhaOculta;   // Imagem a ser mostrada quando a senha estiver oculta
    public Sprite imagemSenhaVisivel;  // Imagem a ser mostrada quando a senha estiver vis�vel

    private bool isPasswordVisible = false;

    void Start()
    {
        // Inicializa o campo de senha com o tipo de conte�do como "Password" (ocultando a senha)
        senhaInput.contentType = TMP_InputField.ContentType.Password;
        senhaInput.ForceLabelUpdate();

        // Configura o bot�o para alternar a visibilidade da senha
        toggleButton.onClick.AddListener(TogglePassword);

        // Define a imagem inicial para a senha oculta
        backgroundImage.sprite = imagemSenhaOculta;
    }

    // Fun��o para alternar a visibilidade da senha e a imagem de fundo
    private void TogglePassword()
    {
        isPasswordVisible = !isPasswordVisible;

        // Altera o tipo de conte�do da senha para exibir ou ocultar
        senhaInput.contentType = isPasswordVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        senhaInput.ForceLabelUpdate();

        // Altera a imagem do fundo dependendo da visibilidade da senha
        backgroundImage.sprite = isPasswordVisible ? imagemSenhaVisivel : imagemSenhaOculta;
    }
}
