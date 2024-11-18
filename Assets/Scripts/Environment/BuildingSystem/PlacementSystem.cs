using System;
using UnityEngine;

/// <summary>
/// Manages the placement and removal of objects in the game world.
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private Grid grid; // The grid used for object placement.

    [SerializeField]
    private InputManager inputManager; // Handles user input for placement actions.

    [SerializeField]
    public GameObject player; // The player object in the scene.

    [SerializeField]
    private ObjectsDatabaseSO database; // Database containing the objects available for placement.

    [SerializeField]
    private GameObject gridVisualization; // Visual representation of the grid.

    private GridData floorData, wallData, wallDecorData, furnitureData; // Grid data for various object types.

    [SerializeField]
    private PreviewSystem preview; // System for showing placement previews.

    private Vector3Int lastDetectedPosition = Vector3Int.zero; // Last detected grid position.

    [SerializeField]
    private ObjectPlacer objectPlacer; // Handles the actual placement of objects.

    private IBuildingState buildingState; // Current state of the building system.

    public Camera cam; // Camera used for placement raycasting.

    public LayerMask placementMask; // Layer mask used for placement detection.

    private bool placementOn = true; // Indicates whether placement mode is active.

    public GameObject placementCanvas; // UI canvas for placement mode.

    public Action OnPlaced; 
    
    /// <summary>
    /// Initializes the placement system and sets up the grid data.
    /// </summary>
    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        wallData = new GridData();
        wallDecorData = new GridData();
        furnitureData = new GridData();
        inputManager.OnPlacementToggle += TogglePlacement;
        TogglePlacement();
    }

    /// <summary>
    /// Updates the placement system each frame, checking for changes in grid position.
    /// </summary>
    private void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    /// <summary>
    /// Toggles the placement mode on and off.
    /// </summary>
    private void TogglePlacement()
    {
        if (placementOn)
        {
            placementOn = false;
            placementCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StopPlacement();
        }
        else
        {
            placementOn = true;
            placementCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// Starts the placement mode for placing a specific object.
    /// </summary>
    /// <param name="ID">The ID of the object to place.</param>
    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID, grid, preview, this, database, floorData, wallData, wallDecorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        placementCanvas.SetActive(false);
    }

    /// <summary>
    /// Starts the removal mode for removing objects.
    /// </summary>
    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, this, floorData, wallData, wallDecorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        placementCanvas.SetActive(false);
    }

    /// <summary>
    /// Places the structure at the current grid position if conditions are met.
    /// </summary>
    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.OnAction(gridPosition);
      
    }

    /// <summary>
    /// Stops the current placement or removal mode.
    /// </summary>
    private void StopPlacement()
    {
        gridVisualization.SetActive(false);

        if (buildingState == null) return;

        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
}