using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class XR_Interaction : MonoBehaviour
{

    // Start is called before the first frame update
    public float raycastDistance = 20f;
   private ActionBasedController controller;
    private bool isPointingAtMesh = false;
    private PuzzlePiece piece;
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
        controller.selectAction.action.performed += OnButtonPress;
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            
            HandleRaycastHit(hit);
            isPointingAtMesh = true;

        }
        else
        {
            isPointingAtMesh = false;
        }
        
        
    }
    private void OnButtonPress(InputAction.CallbackContext context) { if (context.performed && isPointingAtMesh) { piece.OnSelect(); } }

    private void HandleRaycastHit(RaycastHit hit)
    {
        // if the object we hit of a tag geometry
        // then select the parent object of this object
        if (hit.collider.gameObject.CompareTag("Geometry"))
        {
            piece = hit.collider.gameObject.GetComponentInParent<PuzzlePiece>();
            
        }
        
    }
}
