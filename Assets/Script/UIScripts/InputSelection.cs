using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum InputSelectionState
{
    left,
    center,
    right,
    lockedLeft,
    lockedRight,
}

public enum ColorSelectionState
{
    notSelecting,
    player1Selecting,
    player2Selecting,
    hasSelected,
}

public class InputSelection : MonoBehaviour
{
    private List<int> playerIndexes = new List<int>();

    private List<PlayerConfiguration> playerConfigs;

    private InputSelectionState player1State = InputSelectionState.center;
    private InputSelectionState player2State = InputSelectionState.center;

    private ColorSelectionState colorSelectionState = ColorSelectionState.notSelecting;

    private VisualElement root;

    private VisualElement leftRoot;
    private VisualElement rightRoot;

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

    [SerializeField]
    private Texture2D playerIcon;
    [SerializeField]
    private Texture2D playerConfirmIcon;

    private bool finalSelection = false;

    //0 = green, 1 = orange, 2 = blue, 3 = red
    private int currentColor = 0;
    private int firstColor = -1;

    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material orangeMaterial;
    [SerializeField]
    private Material blueMaterial;
    [SerializeField]
    private Material redMaterial;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        rightRoot = rightColorSelector.GetComponent<UIDocument>().rootVisualElement;

        player1Container = root.Q<VisualElement>("P1-Container");
        player2Container = root.Q<VisualElement>("P2-Container");
        player3Container = root.Q<VisualElement>("P3-Container");

        leftContainer = root.Q<VisualElement>("Left-Container");
        rightContainer = root.Q<VisualElement>("Right-Container");

        player3Container.style.opacity = 0f;

        playerConfigs = GameManager.instance.playerConfigurationManager.GetPlayerConfigurations();
        if(playerConfigs.Count == 1)
        {
            SubscribePlayer1Input(playerConfigs[0].Input);
        }

        if (playerConfigs.Count == 2)
        {
            SubscribePlayer1Input(playerConfigs[0].Input);
            SubscribePlayer2Input(playerConfigs[1].Input);
        }
    }

    private void OnDisable()
    {
        
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

    public void UnsubscribePlayerInputs()
    {
        playerConfigs = GameManager.instance.playerConfigurationManager.GetPlayerConfigurations();
        if (playerConfigs.Count == 1)
        {
            var playerInputActionMap = playerConfigs[0].Input.currentActionMap;
            navigateAction = playerInputActionMap.FindAction("Navigate");
            submitAction = playerInputActionMap.FindAction("Submit");
            cancelAction = playerInputActionMap.FindAction("Cancel");

            navigateAction.performed -= Navigate;
            submitAction.performed -= Submit;
            cancelAction.performed -= Cancel;
        }

        if (playerConfigs.Count == 2)
        {
            var playerInputActionMap = playerConfigs[0].Input.currentActionMap;
            navigateAction = playerInputActionMap.FindAction("Navigate");
            submitAction = playerInputActionMap.FindAction("Submit");
            cancelAction = playerInputActionMap.FindAction("Cancel");

            navigateAction.performed -= Navigate;
            submitAction.performed -= Submit;
            cancelAction.performed -= Cancel;

            playerInputActionMap = playerConfigs[1].Input.currentActionMap;
            navigateAction = playerInputActionMap.FindAction("Navigate");
            submitAction = playerInputActionMap.FindAction("Submit");
            cancelAction = playerInputActionMap.FindAction("Cancel");

            navigateAction.performed -= Navigate2;
            submitAction.performed -= Submit2;
            cancelAction.performed -= Cancel2;
        }
    }

    private void Navigate(InputAction.CallbackContext context)
    {
        if(colorSelectionState != ColorSelectionState.player1Selecting)
        {
            if (context.ReadValue<Vector2>() == Vector2.left)
            {
                if (player1State == InputSelectionState.left || player1State == InputSelectionState.lockedLeft)
                    return;

                if (player1State == InputSelectionState.center)
                {
                    if (player2State == InputSelectionState.left || player2State == InputSelectionState.lockedLeft)
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
                if (player1State == InputSelectionState.right || player1State == InputSelectionState.lockedRight)
                    return;

                if (player1State == InputSelectionState.center)
                {
                    if (player2State == InputSelectionState.right || player2State == InputSelectionState.lockedRight)
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

        else
        {
            if (!finalSelection)
            {
                if(context.ReadValue<Vector2>() == Vector2.left)
                {
                    switch (currentColor)
                    {
                        case 1:
                            currentColor = 0;
                            leftRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 2;
                            leftRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.right)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 1;
                            leftRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        case 2:
                            currentColor = 3;
                            leftRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.up)
                {
                    switch (currentColor)
                    {
                        case 2:
                            currentColor = 0;
                            leftRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 1;
                            leftRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.down)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 2;
                            leftRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        case 1:
                            currentColor = 3;
                            leftRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }
            }
            else
            {
                if (context.ReadValue<Vector2>() == Vector2.left)
                {
                    switch (currentColor)
                    {
                        case 1:
                            currentColor = 0;
                            rightRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 2;
                            rightRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.right)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 1;
                            rightRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        case 2:
                            currentColor = 3;
                            rightRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.up)
                {
                    switch (currentColor)
                    {
                        case 2:
                            currentColor = 0;
                            rightRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 1;
                            rightRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.down)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 2;
                            rightRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        case 1:
                            currentColor = 3;
                            rightRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }
            }
        }
    }

    private void Submit(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;


        if (player1State == InputSelectionState.left)
        {
            player1State = InputSelectionState.lockedLeft;
            leftContainer.Q<VisualElement>("LeftPlayer").style.backgroundImage = playerConfirmIcon;

            if(player2State == InputSelectionState.lockedRight)
            {
                ColorSelection(ColorSelectionState.player1Selecting);
            }

            return;
        }

        if (player1State == InputSelectionState.right)
        {
            player1State = InputSelectionState.lockedRight;
            rightContainer.Q<VisualElement>("RightPlayer").style.backgroundImage = playerConfirmIcon;

            if (player2State == InputSelectionState.lockedLeft)
            {
                ColorSelection(ColorSelectionState.player2Selecting);
            }

            return;
        }

        if(colorSelectionState == ColorSelectionState.player1Selecting)
        {
            if (currentColor == firstColor)
                return;

            switch (currentColor)
            {
                case 0:
                    playerConfigs[0].PlayerMaterial = greenMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkGreen").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("GreenColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player2Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkGreen").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;

                case 1:
                    playerConfigs[0].PlayerMaterial = orangeMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkOrange").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("OrangeColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player2Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkOrange").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;

                case 2:
                    playerConfigs[0].PlayerMaterial = blueMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkBlue").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("BlueColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player2Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkBlue").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;

                case 3:
                    playerConfigs[0].PlayerMaterial = redMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkRed").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("RedColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player2Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkRed").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;
            }
        }
    }

    private void Cancel(InputAction.CallbackContext context)    
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (player1State != InputSelectionState.lockedLeft && player1State != InputSelectionState.lockedRight)
        {
            UnsubscribePlayerInputs();
            foreach (PlayerConfiguration player in playerConfigs)
            {
                player.Input.SwitchCurrentActionMap("Player");
            }
            GameManager.instance.StartMenu();
            SceneManager.LoadScene("MainMenu");
        }

        if(player1State == InputSelectionState.lockedLeft)
        {
            player1State = InputSelectionState.left;
            leftContainer.Q<VisualElement>("LeftPlayer").style.backgroundImage = playerIcon;
            DisableColorSelection();
        }

        if (player1State == InputSelectionState.lockedRight)
        {
            player1State = InputSelectionState.right;
            rightContainer.Q<VisualElement>("RightPlayer").style.backgroundImage = playerIcon;
            DisableColorSelection();
        }
    }

    private void Navigate2(InputAction.CallbackContext context)
    {
        if (colorSelectionState != ColorSelectionState.player2Selecting)
        {
            if (context.ReadValue<Vector2>() == Vector2.left)
            {
                if (player2State == InputSelectionState.left || player2State == InputSelectionState.lockedLeft)
                    return;

                if (player2State == InputSelectionState.center)
                {
                    if (player1State == InputSelectionState.left || player1State == InputSelectionState.lockedLeft)
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
                }
            }
            if (context.ReadValue<Vector2>() == Vector2.right)
            {
                if (player2State == InputSelectionState.right || player2State == InputSelectionState.lockedRight)
                    return;

                if (player2State == InputSelectionState.center)
                {
                    if (player1State == InputSelectionState.right || player1State == InputSelectionState.lockedRight)
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

        else
        {
            if (!finalSelection)
            {
                if (context.ReadValue<Vector2>() == Vector2.left)
                {
                    switch (currentColor)
                    {
                        case 1:
                            currentColor = 0;
                            leftRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 2;
                            leftRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.right)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 1;
                            leftRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        case 2:
                            currentColor = 3;
                            leftRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.up)
                {
                    switch (currentColor)
                    {
                        case 2:
                            currentColor = 0;
                            leftRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 1;
                            leftRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.down)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 2;
                            leftRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        case 1:
                            currentColor = 3;
                            leftRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }
            }
            else
            {
                if (context.ReadValue<Vector2>() == Vector2.left)
                {
                    switch (currentColor)
                    {
                        case 1:
                            currentColor = 0;
                            rightRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 2;
                            rightRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.right)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 1;
                            rightRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        case 2:
                            currentColor = 3;
                            rightRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.up)
                {
                    switch (currentColor)
                    {
                        case 2:
                            currentColor = 0;
                            rightRoot.Q<VisualElement>("GreenColor").Focus();
                            return;
                        case 3:
                            currentColor = 1;
                            rightRoot.Q<VisualElement>("OrangeColor").Focus();
                            return;
                        default:
                            return;
                    }
                }

                if (context.ReadValue<Vector2>() == Vector2.down)
                {
                    switch (currentColor)
                    {
                        case 0:
                            currentColor = 2;
                            rightRoot.Q<VisualElement>("BlueColor").Focus();
                            return;
                        case 1:
                            currentColor = 3;
                            rightRoot.Q<VisualElement>("RedColor").Focus();
                            return;
                        default:
                            return;
                    }
                }
            }
        }
    }

    private void Submit2(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;


        if (player2State == InputSelectionState.left)
        {
            player2State = InputSelectionState.lockedLeft;
            leftContainer.Q<VisualElement>("LeftPlayer").style.backgroundImage = playerConfirmIcon;

            if (player1State == InputSelectionState.lockedRight)
            {
                ColorSelection(ColorSelectionState.player2Selecting);
            }

            return;
        }

        if (player2State == InputSelectionState.right)
        {
            player2State = InputSelectionState.lockedRight;
            rightContainer.Q<VisualElement>("RightPlayer").style.backgroundImage = playerConfirmIcon;

            if (player1State == InputSelectionState.lockedLeft)
            {
                ColorSelection(ColorSelectionState.player1Selecting);
            }

            return;
        }

        if (colorSelectionState == ColorSelectionState.player2Selecting)
        {
            if (currentColor == firstColor)
                return;

            switch (currentColor)
            {
                case 0:
                    playerConfigs[1].PlayerMaterial = greenMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkGreen").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("GreenColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player1Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkGreen").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;

                case 1:
                    playerConfigs[1].PlayerMaterial = orangeMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkOrange").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("OrangeColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player1Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkOrange").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;

                case 2:
                    playerConfigs[1].PlayerMaterial = blueMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkBlue").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("BlueColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player1Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkBlue").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;

                case 3:
                    playerConfigs[1].PlayerMaterial = redMaterial;

                    if (!finalSelection)
                    {

                        leftRoot.Q<VisualElement>("CheckmarkRed").style.opacity = 1f;
                        leftRoot.Q<VisualElement>("RedColor").style.opacity = .25f;

                        FinalColorSelection(ColorSelectionState.player1Selecting);
                    }
                    else
                    {
                        rightRoot.Q<VisualElement>("CheckmarkRed").style.opacity = 1f;
                        FinishSelection();
                    }
                    return;
            }
        }
    }

    private void Cancel2(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (player2State != InputSelectionState.lockedLeft && player2State != InputSelectionState.lockedRight)
        {
            UnsubscribePlayerInputs();
            foreach (PlayerConfiguration player in playerConfigs)
            {
                player.Input.SwitchCurrentActionMap("Player");
            }
            GameManager.instance.StartMenu();
            SceneManager.LoadScene("MainMenu");
        }

        if (player2State == InputSelectionState.lockedLeft)
        {
            player2State = InputSelectionState.left;
            leftContainer.Q<VisualElement>("LeftPlayer").style.backgroundImage = playerIcon;
            DisableColorSelection();
        }

        if (player2State == InputSelectionState.lockedRight)
        {
            player2State = InputSelectionState.right;
            rightContainer.Q<VisualElement>("RightPlayer").style.backgroundImage = playerIcon;
            DisableColorSelection();
        }
    }

    private void ColorSelection(ColorSelectionState state)
    {
        colorSelectionState = state;
        leftContainer.style.opacity = 0f;
        leftColorSelector.SetActive(true);
        leftRoot = leftColorSelector.GetComponent<UIDocument>().rootVisualElement;
        leftRoot.Q<VisualElement>("GreenColor").Focus();
    }

    private void FinalColorSelection(ColorSelectionState state)
    {
        colorSelectionState = state;
        rightContainer.style.opacity = 0f;
        rightColorSelector.SetActive(true);
        rightRoot = rightColorSelector.GetComponent<UIDocument>().rootVisualElement;
        firstColor = currentColor;
        switch (firstColor)
        {
            case 0:
                rightRoot.Q<VisualElement>("GreenColor").style.opacity = .25f;
                break;
            case 1:
                rightRoot.Q<VisualElement>("OrangeColor").style.opacity = .25f;
                break;
            case 2:
                rightRoot.Q<VisualElement>("BlueColor").style.opacity = .25f;
                break;
            case 3:
                rightRoot.Q<VisualElement>("RedColor").style.opacity = .25f;
                break;
        }
        currentColor = 0;
        finalSelection = true;
        rightRoot.Q<VisualElement>("GreenColor").Focus();
    }

    private void DisableColorSelection()
    {
        colorSelectionState = ColorSelectionState.notSelecting;
        if(player1State == InputSelectionState.left || player2State == InputSelectionState.left || player1State == InputSelectionState.lockedLeft || player2State == InputSelectionState.lockedLeft)
        {
            leftContainer.style.opacity = 1f;
        }

        if (player1State == InputSelectionState.right || player2State == InputSelectionState.right || player1State == InputSelectionState.lockedRight || player2State == InputSelectionState.lockedRight)
        {
            rightContainer.style.opacity = 1f;
        }

        firstColor = -1;
        currentColor = 0;
        finalSelection = false;

        if (rightColorSelector.activeInHierarchy)
        {
            rightRoot.Q<VisualElement>("GreenColor").style.opacity = 1f;
            rightRoot.Q<VisualElement>("OrangeColor").style.opacity = 1f;
            rightRoot.Q<VisualElement>("BlueColor").style.opacity = 1f;
            rightRoot.Q<VisualElement>("RedColor").style.opacity = 1f;

            rightRoot.Q<VisualElement>("CheckmarkGreen").style.opacity = 0f;
            rightRoot.Q<VisualElement>("CheckmarkOrange").style.opacity = 0f;
            rightRoot.Q<VisualElement>("CheckmarkBlue").style.opacity = 0f;
            rightRoot.Q<VisualElement>("CheckmarkRed").style.opacity = 0f;

            rightColorSelector.SetActive(false);
        }

        if (leftColorSelector.activeInHierarchy)
        {
            leftRoot.Q<VisualElement>("GreenColor").style.opacity = 1f;
            leftRoot.Q<VisualElement>("OrangeColor").style.opacity = 1f;
            leftRoot.Q<VisualElement>("BlueColor").style.opacity = 1f;
            leftRoot.Q<VisualElement>("RedColor").style.opacity = 1f;

            leftRoot.Q<VisualElement>("CheckmarkGreen").style.opacity = 0f;
            leftRoot.Q<VisualElement>("CheckmarkOrange").style.opacity = 0f;
            leftRoot.Q<VisualElement>("CheckmarkBlue").style.opacity = 0f;
            leftRoot.Q<VisualElement>("CheckmarkRed").style.opacity = 0f;

            leftColorSelector.SetActive(false);
        }
    }

    private void FinishSelection()
    {
        colorSelectionState = ColorSelectionState.hasSelected;
        UnsubscribePlayerInputs();
        foreach (PlayerConfiguration player in playerConfigs)
        {
            player.Input.SwitchCurrentActionMap("Player");
        }
        GameManager.instance.InitializeScene("Taher + Enviroment", isMultiplayer: true);
    }
}
