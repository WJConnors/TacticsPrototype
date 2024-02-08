using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour
{
    private Camera cam;
    public LayerMask clickableLayer; // Layer mask to isolate clickable objects like units
    List<GameObject> selectedUnits = new();
    private Vector3 mouseDragStart;
    private GameObject[] allUnits;
    public GameObject selectionCirclePrefab; // Assign in the inspector

    public GameObject[] GetAllUnits()
    {
        allUnits = GameObject.FindGameObjectsWithTag("Unit");
        if (allUnits == null)
        {
            return new GameObject[0]; // Return an empty array to avoid null reference
        }
        return allUnits;
    }

    public static RTSController Instance { get; private set; }
    void Awake()
    {
        allUnits = new GameObject[0];
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cam = Camera.main; // Reference to the main camera
        // Retrieve all units with the tag "Unit"
        allUnits = GameObject.FindGameObjectsWithTag("Unit");
    }

    void Update()
    {
        HandleSelection();
        HandleMovement();     
    }

    private void HandleMovement()
    {
        if (selectedUnits.Count > 0 && Input.GetMouseButtonDown(1)) // Right mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var unit in selectedUnits)
                {
                    if (unit != null)
                    {
                        if (unit.TryGetComponent<NavMeshAgent>(out var agent))
                        {
                            agent.destination = hit.point;
                        }
                    }
                }
            }
        }
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDragStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float dragThreshold = 5f; // Threshold to distinguish between click and drag
            if (Vector3.Distance(mouseDragStart, Input.mousePosition) < dragThreshold)
            {
                SelectSingleUnit(); // Select a single unit
            }
            else
            {
                SelectUnitsInDrag(); // Select units in drag area
            }
        }
    }

    private void OnGUI()
    {
        if (Input.GetMouseButton(0))
        {
            // Create a rect from the start position to the current mouse position
            var rect = Utils.GetScreenRect(mouseDragStart, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, Color.blue);
        }
    }

    void SelectUnitsInDrag()
    {
        // Clear previous selections
        ClearSelectionCircles();

        selectedUnits.Clear();
        Rect selectionRect = Utils.GetViewportBounds(Camera.main, mouseDragStart, Input.mousePosition);

        foreach (var unit in allUnits)
        {
            if (unit.activeInHierarchy && selectionRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)))
            {
                selectedUnits.Add(unit);
                InstantiateSelectionCircle(unit); // Instantiate a circle for each selected unit
            }
        }
    }

    void SelectSingleUnit()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, clickableLayer))
        {
            if (hit.collider.CompareTag("Unit"))
            {
                // Clear previous selections
                ClearSelectionCircles();

                selectedUnits.Clear();
                selectedUnits.Add(hit.collider.gameObject);
                InstantiateSelectionCircle(hit.collider.gameObject); // Instantiate a circle for the selected unit
            }
        }
        else
        {
            // Clear selection if clicked on empty space
            ClearSelectionCircles();
            selectedUnits.Clear();
        }
    }
    void InstantiateSelectionCircle(GameObject unit)
    {
        GameObject circle = Instantiate(selectionCirclePrefab, unit.transform.position, Quaternion.identity);
        circle.transform.SetParent(unit.transform, false);
        circle.transform.localPosition = new Vector3(0, 0.01f, 0); // Adjust Y to avoid z-fighting
        //set the rotation of the circle to x = 180 so it faces the camera
        circle.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    void ClearSelectionCircles()
    {
        foreach (var unit in selectedUnits)
        {
            if (unit.TryGetComponent<NavMeshAgent>(out var agent))
            {
                foreach (Transform child in unit.transform)
                {
                    if (child.CompareTag("SelectionCircle")) // Ensure the tag matches your prefab
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }
}
