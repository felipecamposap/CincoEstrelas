using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Senha_Game : MonoBehaviour
{
    public Text inputField;
    public Text messageText;
    //public Text targetNumberText; // Texto para exibir o n�mero-alvo
    //private int targetNumber;
    private string enteredNumber = "";

    private void Start()
    {
        // Inicia o jogo gerando um n�mero aleat�rio de 4 d�gitos
        //targetNumber = Random.Range(1000, 9999);

        // Exibe o n�mero-alvo na interface do usu�rio
        //targetNumberText.text = targetNumber.ToString();
    }

    public void OnNumberButtonClicked(int number)
    {
        // Verifica se o n�mero digitado j� possui 4 d�gitos
        if (enteredNumber.Length < 4)
        {
            // Adiciona o n�mero clicado ao campo de texto
            enteredNumber += number.ToString();
            inputField.text = enteredNumber;
        }
    }

    public void OnDeleteButtonClicked()
    {
        // Remove o �ltimo d�gito do campo de texto
        if (enteredNumber.Length > 0)
        {
            enteredNumber = enteredNumber.Substring(0, enteredNumber.Length - 1);
            inputField.text = enteredNumber;
        }
    }

    public void OnCheckButtonClicked()
    {
        // Verifica se o n�mero digitado � igual ao n�mero alvo
        int playerGuess;
        if (int.TryParse(enteredNumber, out playerGuess))
        {
            if (playerGuess == GameController.controller.passwordClient)
            {
                messageText.text = "Senha Correta";
                GameController.controller.passwordCorrect = true;
                enteredNumber = new string("");
                inputField.text = "";
                
            }
            else
            {
                messageText.text = "N�mero incorreto. Tente novamente.";
                enteredNumber = new string("");
                inputField.text = "";
            }
            StartCoroutine(CDMessage());
        }
    }

    private IEnumerator CDMessage()
    {
        yield return new WaitForSeconds(0.5f);
        messageText.text = "Insira o c�digo";
    }
}
