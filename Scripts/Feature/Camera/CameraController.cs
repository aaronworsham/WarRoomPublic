using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : NetworkBehaviour
{

    readonly bool debug = false;
    [SerializeField] private NetworkLogger logger;
    [SerializeField] private FiveCameraSwitcher cameraSwitcher;
    [SerializeField] private Camera _currentCamera;
    [SerializeField] private bool _overheadMode = false;
    [SerializeField] private bool _forwardMode = true;
    public bool OverheadMode { get { return _overheadMode; } } 
    public bool ForwardMode { get { return _forwardMode; } } 
    public Camera CurrentCamera { get { return _currentCamera; } }


    //Mouse
    [SerializeField] private float speedH = 2f;
    [SerializeField] private float speedV = 2f;
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float pitch = 0f;
    [SerializeField] private float panLimit = 15f;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float facing = 0f;


    #region Callbacks & Events

    void OnValidate()
    {
        if (logger == null)
            logger = GetComponent<NetworkLogger>();
        if (cameraSwitcher == null)
            cameraSwitcher = GetComponent<FiveCameraSwitcher>();

    }

    public void Start()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
     }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (!hasAuthority) return; 
        SwitchToThirdPersonCam();
    }



    #endregion

    public void RotateCamera()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        _currentCamera.transform.eulerAngles = transform.eulerAngles + new Vector3(pitch, yaw, 0f);
    }

    public void ResetCameraRotation()
    {
        _currentCamera.transform.rotation = transform.rotation;
    }

    public void RotateOverheadCameraRight()
    {
        if (facing == 270)
            facing = 0;
        else
            facing += 90;
        transform.Rotate(0f, 90f, 0f, Space.World);

    }

    public void RotateOverheadCameraLeft()
    {
        if (facing == 0)
            facing = 270;
        else
            facing -= 90;
        transform.Rotate(0f, -90f, 0f, Space.World);

    }

    public void MoveCameraForward()
    {
        Vector3 pos = _currentCamera.transform.position;
        switch (facing)
        {
                    
        
            case 0:
                pos.z += panSpeed * Time.deltaTime;
                pos.z = Mathf.Clamp(pos.z, transform.position.z - panLimit, transform.position.z + panLimit);
                _currentCamera.transform.position = pos;
                break;
            case 90:
                pos.x += panSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, transform.position.x - panLimit, transform.position.x + panLimit);
                _currentCamera.transform.position = pos;
                break;
            case 180:
                pos.z -= panSpeed * Time.deltaTime;
                pos.z = Mathf.Clamp(pos.z, transform.position.z - panLimit, transform.position.z + panLimit);
                _currentCamera.transform.position = pos;
                break;
            case 270:
                pos.x -= panSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, transform.position.x - panLimit, transform.position.x + panLimit);
                _currentCamera.transform.position = pos;
                break;
        }
        

    }

    public void MoveCameraBack()
    {
        Vector3 pos = _currentCamera.transform.position;
        switch (facing)
        {
            case 0:
                pos.z -= panSpeed * Time.deltaTime;
                pos.z = Mathf.Clamp(pos.z, transform.position.z - panLimit, transform.position.z + panLimit);
                _currentCamera.transform.position = pos;
                break;
            case 90:
                pos.x -= panSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, transform.position.x - panLimit, transform.position.x + panLimit);
                _currentCamera.transform.position = pos;
                break;
            case 180:
                pos.z += panSpeed * Time.deltaTime;
                pos.z = Mathf.Clamp(pos.z, transform.position.z - panLimit, transform.position.z + panLimit);
                _currentCamera.transform.position = pos;
                break;
            case 270:
                pos.x += panSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, transform.position.x - panLimit, transform.position.x + panLimit);
                _currentCamera.transform.position = pos;
                break;
        }


    }

    public void ZoomCameraIn()
    {
        Vector3 pos = _currentCamera.transform.position ;
        pos += _currentCamera.transform.forward * panSpeed * Time.deltaTime;
        _currentCamera.transform.position = pos;
    }

    public void ZoomCameraOut()
    {
        Vector3 pos = _currentCamera.transform.position;
        pos -= _currentCamera.transform.forward * panSpeed * Time.deltaTime;
        _currentCamera.transform.position = pos;
    }

    public void MoveCameraLeft()
    {
        Vector3 pos = _currentCamera.transform.position;
        pos -= _currentCamera.transform.right * panSpeed * Time.deltaTime;
        pos.z = Mathf.Clamp(pos.z, transform.position.z - panLimit, transform.position.z + panLimit);
        pos.x = Mathf.Clamp(pos.x, transform.position.x - panLimit, transform.position.x + panLimit);
        _currentCamera.transform.position = pos;
    }

    public void MoveCameraRight()
    {
        Vector3 pos = _currentCamera.transform.position;
        pos += _currentCamera.transform.right * panSpeed * Time.deltaTime;
        pos.z = Mathf.Clamp(pos.z, transform.position.z - panLimit, transform.position.z + panLimit);
        pos.x = Mathf.Clamp(pos.x, transform.position.x - panLimit, transform.position.x + panLimit);
        _currentCamera.transform.position = pos;
    }

    public void SwitchToFirstPersonCam()
    {
        if (debug) logger.TLog(this.GetType().Name, "SwitchToFirstPersonCam");

        _currentCamera = cameraSwitcher.EnableFirstPersonCam();
        _overheadMode = false;
        _forwardMode = true;


    }    
    public void SwitchToThirdPersonCam()
    {
        if (debug) logger.TLog(this.GetType().Name, "SwitchToThirdPersonCam");
        _currentCamera = cameraSwitcher.EnableThirdPersonCam();
        _overheadMode = false;
        _forwardMode = true;

    } 
    
    public void SwitchToDeepCam()
    {
        if (debug) logger.TLog(this.GetType().Name, "SwitchToDeepCam");
        _currentCamera = cameraSwitcher.EnableDeepCam();
        _overheadMode = false;
        _forwardMode = true;

    }    
    public void SwitchToIsoCam()
    {
        if (debug) logger.TLog(this.GetType().Name, "SwitchToIsoCam");
        _currentCamera = cameraSwitcher.EnableIsoCam();
        _overheadMode = true;
        _forwardMode = false;

    }
    public void SwitchToTopDownCam()
    {
        if (debug) logger.TLog(this.GetType().Name, "SwitchToTopDownCam");
        _currentCamera = cameraSwitcher.EnableTopDownCam();
        _overheadMode = true;
        _forwardMode = false;
    }


}
