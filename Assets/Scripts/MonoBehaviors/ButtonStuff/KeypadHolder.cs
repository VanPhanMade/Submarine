using UnityEngine;
using UnityEngine.Events;

public class KeypadHolder : MonoBehaviour
{
    string storedCode = string.Empty;
    [SerializeField] CodeMachine machine;
    private void Start()
    {
        int index = 1;
        foreach (TextButton button in GetComponentsInChildren<TextButton>())
        {
            if(index < 10)
                button.SetKeyNum(index.ToString());
            else
            {
                if (index == 10)
                    button.SetKeyNum("0");
                if (index == 11)
                    button.SetKeyNum("Enter");
            }
            index++;
        }
    }
    public void AddCharacter(string character)
    {
        if (character.Equals("Enter"))
        {
            SubmitCharacters();
            return;
        }
        storedCode += character;
    }
    public void SubmitCharacters()
    {
        machine.AttemptSolve(storedCode);
        Reset();
    }
    void Reset()
    {
        storedCode = string.Empty;
    }
}
