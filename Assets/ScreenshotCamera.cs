using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScreenshotCamera : MonoBehaviour
{
    [FoldoutGroup("Screenshot Settings")]
    [SerializeField, Range(0f, 1f)] private float _minX = 0.2f;
    [FoldoutGroup("Screenshot Settings")]
    [SerializeField, Range(0f, 1f)] private float _maxX = 0.8f;
    [FoldoutGroup("Screenshot Settings")]
    [SerializeField, Range(0f, 1f)] private float _minY = 0.2f;
    [FoldoutGroup("Screenshot Settings")]
    [SerializeField, Range(0f, 1f)] private float _maxY = 0.8f;

    [FoldoutGroup("Screenshot Settings")]
    [SerializeField, FolderPath] private string _screenshotPath = "Screenshots";
    [FoldoutGroup("Screenshot Settings")]
    [SerializeField] private string _filePrefix = "Screenshot";
    [FoldoutGroup("Screenshot Settings")]
    [SerializeField] private bool _addTimestamp = true;

    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private bool _showVisualizer = true;
    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private bool _showGizmos = true;
    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private Color _visualizerColor = new Color(1f, 0f, 0f, 0.3f);
    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private Color _borderColor = Color.red;
    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private Color _gizmoColor = Color.yellow;
    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private float _borderWidth = 2f;
    [FoldoutGroup("Visualizer Settings")]
    [SerializeField] private float _gizmoDistance = 10f;

    [FoldoutGroup("Recent Screenshot Preview")]
    [SerializeField] private bool _showRecentScreenshot = true;
    [FoldoutGroup("Recent Screenshot Preview")]
    [SerializeField, Range(0f, 1f)] private float _previewScale = 0.3f;
    [FoldoutGroup("Recent Screenshot Preview")]
    [SerializeField] private Vector2 _previewPosition = new Vector2(20f, 20f);
    [FoldoutGroup("Recent Screenshot Preview")]
    [SerializeField] private Color _previewBorderColor = Color.white;
    [FoldoutGroup("Recent Screenshot Preview")]
    [SerializeField] private float _previewBorderWidth = 2f;
    [FoldoutGroup("Recent Screenshot Preview")]
    [SerializeField] private float _previewDisplayTime = 5f;

    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private PlayerController[] _playerControllers;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private bool[] _playerControllerOriginalStates;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private Camera _targetCamera;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private string _lastScreenshotPath;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private bool _mainCameraOriginalState;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private bool _canvasOriginalState;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private bool _visualizerOriginalState;

    private Camera _mainCamera;
    private GameObject _canvasGameObject;
    private Material _visualizerMaterial;
    private bool _isCapturing = false;
    private Texture2D _recentScreenshotTexture;
    private float _screenshotDisplayTimer = 0f;

    private void Awake()
    {
        GetCameraComponent();
        CreateVisualizerMaterial();
        ValidateScreenshotDirectory();
    }

    [FoldoutGroup("Setup")]
    [Button("Get Camera Component", ButtonSizes.Medium)]
    private void GetCameraComponent()
    {
        _targetCamera = GetComponent<Camera>();
        if (_targetCamera != null)
        {
            Debug.Log($"[ScreenshotCamera] Camera component found and assigned!");
        }
        else
        {
            Debug.LogError($"[ScreenshotCamera] No Camera component found on {gameObject.name}");
        }
    }

    [FoldoutGroup("Setup")]
    [Button("Choose Screenshot Folder", ButtonSizes.Medium)]
    private void ChooseScreenshotFolder()
    {
#if UNITY_EDITOR
        string currentPath = string.IsNullOrEmpty(_screenshotPath) ? Application.persistentDataPath : _screenshotPath;
        string selectedPath = EditorUtility.OpenFolderPanel("Choose Screenshot Folder", currentPath, "");

        if (!string.IsNullOrEmpty(selectedPath))
        {
            _screenshotPath = selectedPath;
            Debug.Log($"[ScreenshotCamera] Screenshot folder set to: {_screenshotPath}");
        }
#else
        Debug.LogWarning("[ScreenshotCamera] Folder selection only works in Editor mode");
#endif
    }
    // Add this method to find and cache references
    private void CacheGameObjectReferences()
    {
        // Find main camera if not already cached
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        // Find canvas gameobject if not already cached
        if (_canvasGameObject == null)
        {
            _canvasGameObject = GameObject.Find("Canvas");
            if (_canvasGameObject == null)
            {
                Debug.LogWarning("[ScreenshotCamera] Canvas gameobject not found!");
            }
        }
    }
    private void CreateVisualizerMaterial()
    {
        // Create material for visualizer overlay
        _visualizerMaterial = new Material(Shader.Find("Sprites/Default"));
        _visualizerMaterial.color = _visualizerColor;
    }

    private void ValidateScreenshotDirectory()
    {
        string fullPath = string.IsNullOrEmpty(_screenshotPath) ?
            Path.Combine(Application.persistentDataPath, "Screenshots") :
            _screenshotPath;

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
    }

    private void Update()
    {
        UpdateScreenshotDisplayTimer();
    }

    private void UpdateScreenshotDisplayTimer()
    {
        if (_screenshotDisplayTimer > 0f)
        {
            _screenshotDisplayTimer -= Time.deltaTime;

            if (_screenshotDisplayTimer <= 0f)
            {
                ClearRecentScreenshotTexture();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos || _targetCamera == null) return;

        DrawViewportGizmos();
    }

    private void DrawViewportGizmos()
    {
        // Set gizmo color
        Gizmos.color = _gizmoColor;

        // Get the four corners of the screenshot area in world space
        Vector3[] corners = GetScreenshotWorldCorners();

        // Draw the rectangle outline
        Gizmos.DrawLine(corners[0], corners[1]); // Bottom to Top Left
        Gizmos.DrawLine(corners[1], corners[2]); // Top Left to Top Right  
        Gizmos.DrawLine(corners[2], corners[3]); // Top Right to Bottom Right
        Gizmos.DrawLine(corners[3], corners[0]); // Bottom Right to Bottom Left

        // Draw corner markers for better visibility
        float markerSize = 0.2f;
        for (int i = 0; i < corners.Length; i++)
        {
            Gizmos.DrawWireCube(corners[i], Vector3.one * markerSize);
        }
    }

    private Vector3[] GetScreenshotWorldCorners()
    {
        if (_targetCamera == null) return new Vector3[4];

        // Convert viewport coordinates to world space
        // We use a fixed distance from the camera for visualization
        float distance = _gizmoDistance;

        // Get the four corners in viewport space
        Vector3 bottomLeft = new Vector3(_minX, _minY, distance);
        Vector3 topLeft = new Vector3(_minX, _maxY, distance);
        Vector3 topRight = new Vector3(_maxX, _maxY, distance);
        Vector3 bottomRight = new Vector3(_maxX, _minY, distance);

        // Convert to world space
        Vector3[] corners = new Vector3[4];
        corners[0] = _targetCamera.ViewportToWorldPoint(bottomLeft);
        corners[1] = _targetCamera.ViewportToWorldPoint(topLeft);
        corners[2] = _targetCamera.ViewportToWorldPoint(topRight);
        corners[3] = _targetCamera.ViewportToWorldPoint(bottomRight);

        return corners;
    }

    private void OnGUI()
    {
        if (_targetCamera == null) return;

        if (_showVisualizer)
        {
            DrawScreenshotArea();
        }

        if (_showRecentScreenshot && _recentScreenshotTexture != null && _screenshotDisplayTimer > 0f)
        {
            DrawRecentScreenshotPreview();
        }
    }

    private void DrawScreenshotArea()
    {
        // Convert viewport coordinates to screen coordinates
        Vector2 screenMin = _targetCamera.ViewportToScreenPoint(new Vector3(_minX, _minY, 0));
        Vector2 screenMax = _targetCamera.ViewportToScreenPoint(new Vector3(_maxX, _maxY, 0));

        // Unity's screen coordinates have (0,0) at bottom-left, but GUI uses top-left
        float screenHeight = Screen.height;
        screenMin.y = screenHeight - screenMin.y;
        screenMax.y = screenHeight - screenMax.y;

        // Calculate rectangle dimensions
        float width = screenMax.x - screenMin.x;
        float height = screenMin.y - screenMax.y;

        Rect screenshotRect = new Rect(screenMin.x, screenMax.y, width, height);

        // Draw filled rectangle
        GUI.color = _visualizerColor;
        GUI.DrawTexture(screenshotRect, Texture2D.whiteTexture);

        // Draw border
        GUI.color = _borderColor;
        DrawRectBorder(screenshotRect, _borderWidth);

        // Reset GUI color
        GUI.color = Color.white;
    }

    private void DrawRecentScreenshotPreview()
    {
        if (_recentScreenshotTexture == null) return;

        // Calculate preview dimensions based on scale
        float previewWidth = _recentScreenshotTexture.width * _previewScale;
        float previewHeight = _recentScreenshotTexture.height * _previewScale;

        // Create preview rect
        Rect previewRect = new Rect(_previewPosition.x, _previewPosition.y, previewWidth, previewHeight);

        // Draw border around preview
        GUI.color = _previewBorderColor;
        DrawRectBorder(previewRect, _previewBorderWidth);

        // Draw the screenshot preview
        GUI.color = Color.white;
        GUI.DrawTexture(previewRect, _recentScreenshotTexture);

        // Add fade effect based on remaining time
        float fadeAlpha = Mathf.Clamp01(_screenshotDisplayTimer / _previewDisplayTime);
        GUI.color = new Color(1f, 1f, 1f, fadeAlpha);
        GUI.DrawTexture(previewRect, _recentScreenshotTexture);

        // Reset GUI color
        GUI.color = Color.white;
    }

    private void DrawRectBorder(Rect rect, float thickness)
    {
        // Top border
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), Texture2D.whiteTexture);
        // Bottom border
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - thickness, rect.width, thickness), Texture2D.whiteTexture);
        // Left border
        GUI.DrawTexture(new Rect(rect.x, rect.y, thickness, rect.height), Texture2D.whiteTexture);
        // Right border
        GUI.DrawTexture(new Rect(rect.x + rect.width - thickness, rect.y, thickness, rect.height), Texture2D.whiteTexture);
    }

    [FoldoutGroup("Screenshot Settings")]
    [Button("Take Screenshot", ButtonSizes.Medium)]
    private void TakeScreenshotButton()
    {
        if (!_isCapturing)
        {
            StartCoroutine(CaptureScreenshotCoroutine());
        }
    }

    public void TakeScreenshot()
    {
        if (!_isCapturing)
        {
            StartCoroutine(CaptureScreenshotCoroutine());
        }
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        _isCapturing = true;

        // Prepare scene for screenshot (disable objects)
        PrepareForScreenshot();

        // Wait a frame to ensure changes take effect
        yield return null;

        yield return new WaitForSeconds(0.25f);

        // Wait for end of frame to ensure all rendering is complete
        yield return new WaitForEndOfFrame();

        string filePath = GenerateScreenshotPath();
        bool success = false;

        try
        {
            CaptureScreenshotArea(filePath);
            Debug.Log($"[ScreenshotCamera] Screenshot saved to: {filePath}");
            _lastScreenshotPath = filePath;
            success = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ScreenshotCamera] Failed to capture screenshot: {e.Message}");
        }
        finally
        {
            // Always restore the scene state, even if screenshot failed
            RestoreAfterScreenshot();
        }

        _isCapturing = false;

        // Load the screenshot for preview after a short delay
        if (success)
        {
            yield return new WaitForSeconds(0.1f);
            LoadRecentScreenshotForPreview();
        }
    }

    // Replace the existing PrepareForScreenshot method:
    private void PrepareForScreenshot()
    {
        CacheGameObjectReferences();
        CachePlayerControllerReferences();

        // Store and disable main camera
        if (_mainCamera != null)
        {
            _mainCameraOriginalState = _mainCamera.enabled;
            _mainCamera.enabled = false;
            Debug.Log("[ScreenshotCamera] Main camera disabled");
        }

        // Store and disable canvas
        if (_canvasGameObject != null)
        {
            _canvasOriginalState = _canvasGameObject.activeSelf;
            _canvasGameObject.SetActive(false);
            Debug.Log("[ScreenshotCamera] Canvas disabled");
        }

        // Store and disable all player controllers
        DisablePlayerControllers();

        // Store and disable visualizer
        _visualizerOriginalState = _showVisualizer;
        _showVisualizer = false;
        Debug.Log("[ScreenshotCamera] Visualizer disabled");
    }

    // Method to restore original states
    private void RestoreAfterScreenshot()
    {
        // Restore main camera
        if (_mainCamera != null)
        {
            _mainCamera.enabled = _mainCameraOriginalState;
            Debug.Log($"[ScreenshotCamera] Main camera restored to: {_mainCameraOriginalState}");
        }

        // Restore canvas
        if (_canvasGameObject != null)
        {
            _canvasGameObject.SetActive(_canvasOriginalState);
            Debug.Log($"[ScreenshotCamera] Canvas restored to: {_canvasOriginalState}");
        }

        // Restore all player controllers
        RestorePlayerControllers();

        // Restore visualizer
        _showVisualizer = _visualizerOriginalState;
        Debug.Log($"[ScreenshotCamera] Visualizer restored to: {_visualizerOriginalState}");
    }
    private void CachePlayerControllerReferences()
    {
        _playerControllers = FindObjectsOfType<PlayerController>();
        _playerControllerOriginalStates = new bool[_playerControllers.Length];

        if (_playerControllers.Length > 0)
        {
            Debug.Log($"[ScreenshotCamera] Found {_playerControllers.Length} PlayerController(s)");
        }
    }

    private void DisablePlayerControllers()
    {
        for (int i = 0; i < _playerControllers.Length; i++)
        {
            if (_playerControllers[i] != null)
            {
                _playerControllerOriginalStates[i] = _playerControllers[i].gameObject.activeSelf;
                _playerControllers[i].gameObject.SetActive(false);
            }
        }

        if (_playerControllers.Length > 0)
        {
            Debug.Log($"[ScreenshotCamera] Disabled {_playerControllers.Length} PlayerController GameObject(s)");
        }
    }

    private void RestorePlayerControllers()
    {
        for (int i = 0; i < _playerControllers.Length; i++)
        {
            if (_playerControllers[i] != null)
            {
                _playerControllers[i].gameObject.SetActive(_playerControllerOriginalStates[i]);
            }
        }

        if (_playerControllers.Length > 0)
        {
            Debug.Log($"[ScreenshotCamera] Restored {_playerControllers.Length} PlayerController GameObject(s)");
        }
    }
    private void CaptureScreenshotArea(string filePath)
    {
        // Convert viewport coordinates to pixel coordinates
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        int pixelMinX = Mathf.RoundToInt(_minX * screenWidth);
        int pixelMaxX = Mathf.RoundToInt(_maxX * screenWidth);
        int pixelMinY = Mathf.RoundToInt(_minY * screenHeight);
        int pixelMaxY = Mathf.RoundToInt(_maxY * screenHeight);

        int width = pixelMaxX - pixelMinX;
        int height = pixelMaxY - pixelMinY;

        // Create texture to store screenshot
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read pixels from screen
        screenshot.ReadPixels(new Rect(pixelMinX, pixelMinY, width, height), 0, 0);
        screenshot.Apply();

        // Save to file
        byte[] data = screenshot.EncodeToPNG();
        File.WriteAllBytes(filePath, data);

        // Clean up
        DestroyImmediate(screenshot);
    }

    private void LoadRecentScreenshotForPreview()
    {
        if (string.IsNullOrEmpty(_lastScreenshotPath) || !File.Exists(_lastScreenshotPath))
        {
            Debug.LogWarning("[ScreenshotCamera] Cannot load recent screenshot for preview - file doesn't exist");
            return;
        }

        StartCoroutine(LoadScreenshotTextureCoroutine(_lastScreenshotPath));
    }

    private IEnumerator LoadScreenshotTextureCoroutine(string filePath)
    {
        // Clear previous texture
        ClearRecentScreenshotTexture();

        try
        {
            // Load file data
            byte[] fileData = File.ReadAllBytes(filePath);

            // Create new texture and load image data
            _recentScreenshotTexture = new Texture2D(2, 2); // Size doesn't matter, will be replaced
            bool loadSuccess = _recentScreenshotTexture.LoadImage(fileData);

            if (loadSuccess)
            {
                _screenshotDisplayTimer = _previewDisplayTime;
                Debug.Log("[ScreenshotCamera] Recent screenshot loaded for preview");
            }
            else
            {
                Debug.LogError("[ScreenshotCamera] Failed to load screenshot texture");
                ClearRecentScreenshotTexture();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ScreenshotCamera] Error loading screenshot for preview: {e.Message}");
            ClearRecentScreenshotTexture();
        }

        yield return null;
    }

    private void ClearRecentScreenshotTexture()
    {
        if (_recentScreenshotTexture != null)
        {
            DestroyImmediate(_recentScreenshotTexture);
            _recentScreenshotTexture = null;
        }
    }

    [FoldoutGroup("Recent Screenshot Preview")]
    [Button("Clear Preview", ButtonSizes.Small)]
    private void ClearPreviewButton()
    {
        ClearRecentScreenshotTexture();
        _screenshotDisplayTimer = 0f;
    }

    private string GenerateScreenshotPath()
    {
        string directory = string.IsNullOrEmpty(_screenshotPath) ?
            Path.Combine(Application.persistentDataPath, "Screenshots") :
            _screenshotPath;

        string filename = _filePrefix;

        if (_addTimestamp)
        {
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            filename += $"_{timestamp}";
        }

        filename += ".png";

        return Path.Combine(directory, filename);
    }

    // Getter functions for external access
    public float GetMinX() => _minX;
    public float GetMaxX() => _maxX;
    public float GetMinY() => _minY;
    public float GetMaxY() => _maxY;
    public string GetLastScreenshotPath() => _lastScreenshotPath;
    public bool GetIsCapturing() => _isCapturing;
    public bool GetIsShowingRecentScreenshot() => _recentScreenshotTexture != null && _screenshotDisplayTimer > 0f;

    public void SetScreenshotArea(float minX, float maxX, float minY, float maxY)
    {
        _minX = Mathf.Clamp01(minX);
        _maxX = Mathf.Clamp01(maxX);
        _minY = Mathf.Clamp01(minY);
        _maxY = Mathf.Clamp01(maxY);

        ValidateAreaBounds();
    }

    public void SetPreviewSettings(bool showPreview, float scale, Vector2 position, float displayTime)
    {
        _showRecentScreenshot = showPreview;
        _previewScale = Mathf.Clamp01(scale);
        _previewPosition = position;
        _previewDisplayTime = Mathf.Max(0f, displayTime);
    }

    private void ValidateAreaBounds()
    {
        if (_minX >= _maxX)
        {
            Debug.LogWarning("[ScreenshotCamera] MinX should be less than MaxX");
        }
        if (_minY >= _maxY)
        {
            Debug.LogWarning("[ScreenshotCamera] MinY should be less than MaxY");
        }
    }

    private void OnDestroy()
    {
        if (_visualizerMaterial != null)
        {
            DestroyImmediate(_visualizerMaterial);
        }

        ClearRecentScreenshotTexture();
    }

    [FoldoutGroup("Debug")]
    [Button("Open Screenshots Folder")]
    private void OpenScreenshotsFolder()
    {
        string path = string.IsNullOrEmpty(_screenshotPath) ?
            Path.Combine(Application.persistentDataPath, "Screenshots") :
            _screenshotPath;

        if (Directory.Exists(path))
        {
#if UNITY_EDITOR
            EditorUtility.RevealInFinder(path);
#else
            System.Diagnostics.Process.Start(path);
#endif
        }
        else
        {
            Debug.LogWarning($"[ScreenshotCamera] Screenshots folder doesn't exist: {path}");
        }
    }
}