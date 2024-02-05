using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using PuzzlePipes;

public class XR_Interaction : MonoBehaviour
{

    // Start is called before the first frame update
    public float raycastDistance = 100f;
    public LayerMask raycastMask;
    public Material highlightMaterial;
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
    private void OnButtonPress(InputAction.CallbackContext context) { if (context.performed) { piece.OnSelect(); } }

    private void HandleRaycastHit(RaycastHit hit)
    {
        Debug.Log("hitttt");
        // if the object we hit of a tag geometry
        // then select the parent object of this object
        if (hit.collider.gameObject.CompareTag("Geometry"))
        {
            piece = hit.collider.gameObject.GetComponentInParent<PuzzlePiece>();
            
        }
        
    }
}
