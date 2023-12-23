using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Senha_Game game; // Referência ao jogo principal
    public int number = -1; // Número associado a este botão (-1 se não for um botão numérico)

    private Button button;

    private void Start()
    {
        // Obtém a referência ao componente Button
        button = GetComponent<Button>();

        // Adiciona um ouvinte de clique para chamar o método apropriado com base no tipo de botão
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Verifica o tipo do botão e chama o método apropriado do jogo principal
        if (number >= 0)
        {
            // Botão numérico
            game.OnNumberButtonClicked(number);
        }
        else if (gameObject.name == "DeleteButton")
        {
            // Botão de apagar
            game.OnDeleteButtonClicked();
        }
        else if (gameObject.name == "CheckButton")
        {
            // Botão de verificar
            game.OnCheckButtonClicked();
        }
    }
}
