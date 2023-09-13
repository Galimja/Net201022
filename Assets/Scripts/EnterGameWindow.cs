using UnityEngine;
using UnityEngine.UI;

public class EnterGameWindow : MonoBehaviour
{
    [SerializeField] private Button _sigInButton;
    [SerializeField] private Button _createAccountButton;

    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private Canvas _signInCanvas;

    private void Start()
    {
        _sigInButton.onClick.AddListener(OpenSignInWindow);
        _createAccountButton.onClick.AddListener(CreateAccountWindow);
    }

    private void OpenSignInWindow()
    {
        _signInCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
    }
    private void CreateAccountWindow()
    {
        _createAccountCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
    }
}
