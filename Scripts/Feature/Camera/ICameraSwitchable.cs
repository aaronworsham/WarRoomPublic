using System;
using UnityEngine;

interface ICameraSwitchable
{
    Camera EnableThirdPersonCam();
    Camera EnableFirstPersonCam();
    Camera EnableDeepCam();
    Camera EnableIsoCam();
    Camera EnableTopDownCam();
    void DisableAllCams();

}
