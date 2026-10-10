using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Camera))]
public class PixelatedCamera : MonoBehaviour{
    public RenderTexture RenderTexture;
    private Camera _camera;

    void Awake(){
        _camera = GetComponent<Camera>();
        if(this.RenderTexture == null){
            Debug.LogError("PixelatedCamera is missing a RenderTexture reference.");
            return;
        }
        this.RenderTexture.filterMode = FilterMode.Point;
        this.RenderTexture.antiAliasing = 1;
    }

    void OnEnable(){
        InputSystem.onEvent += OnEvent;
        if(_camera != null && this.RenderTexture != null){
            _camera.targetTexture = this.RenderTexture;
        }
    }

    void OnDisable(){
        InputSystem.onEvent -= OnEvent;
        if(_camera != null){
            _camera.targetTexture = null;
        }
    }

    private void OnEvent(InputEventPtr eventPtr, InputDevice device){
        if(device != Mouse.current) return;
        if(!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

        var positionControl = Mouse.current.position;
        if(positionControl == null) return;

        bool positionChanged = eventPtr.IsA<StateEvent>();

        // if its only a partial update check if the event contains a position update
        if(!positionChanged){
            foreach(var control in eventPtr.EnumerateChangedControls()){
                if(control == positionControl){
                    positionChanged = true;
                    break;
                }
            }
        }

        // we have an event with a position so we can use it
        if(positionChanged){
            int targetWidth = this.RenderTexture != null ? this.RenderTexture.width : Screen.width;
            int targetHeight = this.RenderTexture != null ? this.RenderTexture.height : Screen.height;
            // 1. get raw mouse from event (probably screen space [0, 1920])
            Vector2 position = positionControl.ReadValueFromEvent(eventPtr);
            // 2. normalize to [0, 1] and 3. scale to targetResolution [0, targetWidth/targetHeight]
            position.x = (position.x / Screen.width) * targetWidth;
            position.y = (position.y / Screen.height) * targetHeight;
            // 4. set the event mouse position with the newly scaled position
            positionControl.WriteValueIntoEvent(position, eventPtr);
        }
    }
}
