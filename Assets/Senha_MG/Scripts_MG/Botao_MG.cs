using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Senha_Game game; // Refer�ncia ao jogo principal
    public int number = -1; // N�mero associado a este bot�o (-1 se n�o for um bot�o num�rico)

    private Button button;

    private void Start()
    {
        // Obt�m a refer�ncia ao componente Button
        button = GetComponent<Button>();

        // Adiciona um ouvinte de clique para chamar o m�todo apropriado com base no tipo de bot�o
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Verifica o tipo do bot�o e chama o m�todo apropriado do jogo principal
        if (number >= 0)
        {
            // Bot�o num�rico
            game.OnNumberButtonClicked(number);
        }
        else if (gameObject.name == "DeleteButton")
        {
            // Bot�o de apagar
            game.OnDeleteButtonClicked();
        }
        else if (gameObject.name == "CheckButton")
        {
            // Bot�o de verificar
            game.OnCheckButtonClicked();
        }
    }
}
