using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public CinemachineVirtualCamera activeCam = null;
    public static CameraManager instance;
    Transform mainCam;
    CinemachineBrain brain;
    public NoiseSettings SixDShake;
    public NoiseSettings normalHandHeldMild;

    [Header("All Cameras")]
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera LureCam;
    public CinemachineVirtualCamera CampfireCam;

    private void Awake()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        mainCam = Camera.main.transform;
        instance = this;
        activeCam = normalCam;
    }

    private void Update()
    {
        if (CampfireCam == null && SceneTracker.instance.currentScene == Scenes.Cabin)
            CampfireCam = GameObject.Find("CamFire").GetComponent<CinemachineVirtualCamera>();
    }


    public bool IsActiveCamera(CinemachineVirtualCamera cam)
    {
        return cam == activeCam;
    }

    public void SetNoiseSetting(NoiseSettings settings)
    {
        activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = settings;
    }
    public bool HasReachedDestination()
    {
        return mainCam.position == activeCam.transform.position;
    }

    public void SwitchCamera(CinemachineVirtualCamera cam, CinemachineBlendDefinition.Style blendStyle)
    {
        if(activeCam != null)
            activeCam.Priority = 0;

        cam.Priority = 10;
        activeCam = cam;
        brain.m_DefaultBlend.m_Style = blendStyle;
        //foreach (CinemachineVirtualCamera c in cameras)
        //{
        //    if(c != cam)
        //    {
        //        c.Priority = 0;
        //    } 
        //}
    }
}
