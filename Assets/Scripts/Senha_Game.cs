using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Senha_Game : MonoBehaviour
{
    public Text inputField;
    public Text messageText;
    //public Text targetNumberText; // Texto para exibir o número-alvo
    //private int targetNumber;
    private string enteredNumber = "";

    private void Start()
    {
        // Inicia o jogo gerando um número aleatório de 4 dígitos
        //targetNumber = Random.Range(1000, 9999);

        // Exibe o número-alvo na interface do usuário
        //targetNumberText.text = targetNumber.ToString();
    }

    public void OnNumberButtonClicked(int number)
    {
        // Verifica se o número digitado já possui 4 dígitos
        if (enteredNumber.Length < 4)
        {
            // Adiciona o número clicado ao campo de texto
            enteredNumber += number.ToString();
            inputField.text = enteredNumber;
        }
    }

    public void OnDeleteButtonClicked()
    {
        // Remove o último dígito do campo de texto
        if (enteredNumber.Length > 0)
        {
            enteredNumber = enteredNumber.Substring(0, enteredNumber.Length - 1);
            inputField.text = enteredNumber;
        }
    }

    public void OnCheckButtonClicked()
    {
        // Verifica se o número digitado é igual ao número alvo
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
                messageText.text = "Número incorreto. Tente novamente.";
                enteredNumber = new string("");
                inputField.text = "";
            }
            StartCoroutine(CDMessage());
        }
    }

    private IEnumerator CDMessage()
    {
        yield return new WaitForSeconds(0.5f);
        messageText.text = "Insira o código";
    }
}
