using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask interactionMask;

    int perspectiveIndex = 1;
    float[] rotationYValues = { -180 ,- 90  };

    InputSystem_Actions input;
    private void Awake()
    {
        input = new InputSystem_Actions();

        input.Player.Attack.Enable();
        input.Player.Attack.performed += TriggerButtonTest;

        input.Player.Move.Enable();
        input.Player.Move.performed += FlipCamInput;
    }
    private void Start()
    {
        
    }

    void TriggerButtonTest(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 6);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 15, interactionMask))
        {
            if(hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable component))
            {
                component.Interact();
            }
        }
    }

    void FlipCamInput(InputAction.CallbackContext context)
    {
        int temp = perspectiveIndex;
        float xInput = context.ReadValue<Vector2>().x;
        if (xInput < 0)
            perspectiveIndex--;
        else
            perspectiveIndex++;
        if(perspectiveIndex < 0)
            perspectiveIndex = rotationYValues.Length - 1;
        if(perspectiveIndex >= rotationYValues.Length)
            perspectiveIndex = 0;
        if(perspectiveIndex != temp)
            ChangePerspective();
    }       
    void ChangePerspective()
    {
        float rotValue = rotationYValues[perspectiveIndex];
        cam.transform.rotation = Quaternion.Euler(new Vector3(8.8f, rotValue, cam.transform.rotation.z));
    }
}
