using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum InputSelectionState
{
    left,
    center,
    right,
}

public class InputSelection : MonoBehaviour
{
    private List<int> playerIndexes = new List<int>();

    private List<PlayerConfiguration> playerConfigs;

    private InputSelectionState player1State = InputSelectionState.center;
    private InputSelectionState player2State = InputSelectionState.center;

    private VisualElement root;

    private VisualElement player1Container;
    private VisualElement player2Container;
    private VisualElement player3Container;

    private VisualElement leftContainer;
    private VisualElement rightContainer;

    private InputAction navigateAction;
    private InputAction submitAction;
    private InputAction cancelAction;

    [SerializeField]
    private GameObject leftColorSelector;
    [SerializeField]
    private GameObject rightColorSelector;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        player1Container = root.Q<VisualElement>("P1-Container");
        player2Container = root.Q<VisualElement>("P2-Container");
        player3Container = root.Q<VisualElement>("P3-Container");

        leftContainer = root.Q<VisualElement>("Left-Container");
        rightContainer = root.Q<VisualElement>("Right-Container");

        player3Container.style.opacity = 0f;
    }

    public void AddPlayerIndexToList(int playerIndex)
    {
        playerIndexes.Add(playerIndex);
        playerConfigs = GameManager.instance.playerConfigurationManager.GetPlayerConfigurations();

        if(playerIndex == 0)
        {
            SubscribePlayer1Input(playerConfigs[playerIndex].Input);
        }

        if (playerIndex == 1)
        {
            SubscribePlayer2Input(playerConfigs[playerIndex].Input);
        }
    }
    
    public void HandlePlayerInput(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        //if(context.action.name == )
    }

    public void SubscribePlayer1Input(PlayerInput input)
    {
        input.SwitchCurrentActionMap("UI");

        var playerInputActionMap = input.currentActionMap;
        navigateAction = playerInputActionMap.FindAction("Navigate");
        submitAction = playerInputActionMap.FindAction("Submit");
        cancelAction = playerInputActionMap.FindAction("Cancel");

        navigateAction.performed += Navigate;
        submitAction.performed += Submit;
        cancelAction.performed += Cancel;
    }

    public void SubscribePlayer2Input(PlayerInput input)
    {
        input.SwitchCurrentActionMap("UI");

        var playerInputActionMap = input.currentActionMap;
        navigateAction = playerInputActionMap.FindAction("Navigate");
        submitAction = playerInputActionMap.FindAction("Submit");
        cancelAction = playerInputActionMap.FindAction("Cancel");

        navigateAction.performed += Navigate2;
        submitAction.performed += Submit2;
        cancelAction.performed += Cancel2;
    }

    private void Navigate(InputAction.CallbackContext context)
    {
        if(context.ReadValue<Vector2>() == Vector2.left)
        {
            if (player1State == InputSelectionState.left)
                return;

            if(player1State == InputSelectionState.center)
            {
                if (player2State == InputSelectionState.left)
                    return;

                player1State = InputSelectionState.left;
                player1Container.style.opacity = 0f;
                leftContainer.style.opacity = 1f;

                leftContainer.Q<TextElement>("PlayerNumber").text = "1";
            }

            if (player1State == InputSelectionState.right)
            {
                player1State = InputSelectionState.center;
                player1Container.style.opacity = 1f;
                rightContainer.style.opacity = 0f;
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.right)
        {
            if (player1State == InputSelectionState.right)
                return;

            if (player1State == InputSelectionState.center)
            {
                if (player2State == InputSelectionState.right)
                    return;

                player1State = InputSelectionState.right;
                player1Container.style.opacity = 0f;
                rightContainer.style.opacity = 1f;

                rightContainer.Q<TextElement>("PlayerNumber").text = "1";
            }

            if (player1State == InputSelectionState.left)
            {
                player1State = InputSelectionState.center;
                player1Container.style.opacity = 1f;
                leftContainer.style.opacity = 0f;
            }
        }
    }

    private void Submit(InputAction.CallbackContext context)
    {
        if(player1State == InputSelectionState.left)
        {
            leftColorSelector.gameObject.SetActive(true);
        }

        if (player1State == InputSelectionState.right)
        {
            rightColorSelector.gameObject.SetActive(true);
        }
    }

    private void Cancel(InputAction.CallbackContext context)
    {

    }

    private void Navigate2(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() == Vector2.left)
        {
            if (player2State == InputSelectionState.left)
                return;

            if (player2State == InputSelectionState.center)
            {
                if (player1State == InputSelectionState.left)
                    return;

                player2State = InputSelectionState.left;
                player2Container.style.opacity = 0f;
                leftContainer.style.opacity = 1f;

                leftContainer.Q<TextElement>("PlayerNumber").text = "2";
            }

            if (player2State == InputSelectionState.right)
            {
                player2State = InputSelectionState.center;
                player2Container.style.opacity = 1f;
                rightContainer.style.opacity = 0f;

                leftContainer.Q<TextElement>("PlayerNumber").text = "2";
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.right)
        {
            if (player2State == InputSelectionState.right)
                return;

            if (player2State == InputSelectionState.center)
            {
                if (player1State == InputSelectionState.right)
                    return;

                player2State = InputSelectionState.right;
                player2Container.style.opacity = 0f;
                rightContainer.style.opacity = 1f;

                rightContainer.Q<TextElement>("PlayerNumber").text = "2";
            }

            if (player2State == InputSelectionState.left)
            {
                player2State = InputSelectionState.center;
                player2Container.style.opacity = 1f;
                leftContainer.style.opacity = 0f;
            }
        }
    }

    private void Submit2(InputAction.CallbackContext context)
    {
        if (player2State == InputSelectionState.left)
        {
            leftColorSelector.gameObject.SetActive(true);
        }

        if (player2State == InputSelectionState.right)
        {
            rightColorSelector.gameObject.SetActive(true);
        }
    }

    private void Cancel2(InputAction.CallbackContext context)
    {

    }
}
