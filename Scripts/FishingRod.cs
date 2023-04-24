using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Threading;
using static Unity.VisualScripting.Member;

public class FishingRod : MonoBehaviour
{
    public LayerMask waterLayer;
    public GameObject chargeSliderUI;
    public GameObject fishSliderUI;
    public Slider chargeSlider;
    public Slider fishSlider;
    public float chargeRate = 1;
    public float fishRate = 10;

    InputManager input;
    PlayerMovement pm;

    float minRot = 3;
    float maxRot = 15;
    float minTimeToBait = 0.2f;
    float maxTimeToBait = 20f;
    [SerializeField] float currentRot;
    float inverseCurrentRot;

    bool doMinus;
    public bool isCharging;
    public bool isFishing;
    public bool isThrowing;
    [SerializeField] Transform fishingPoint;

    [SerializeField] Transform testObject;
    [SerializeField] LineRenderer lineRendere;
    [SerializeField] Transform lineStart;
    [SerializeField] AudioClip baitClip;
    [SerializeField] ParticleSystem waterSplash;
    Vector3 deltaPos;
    Vector3 propPos;

    bool waitingForFish = false;
    bool hasBaited = false;
    bool isInterrupted = false;
    public Vector2 fishPull;
    Vector3 storedLureRotation;
    AudioSource source;

    [SerializeField] FishItems[] fishs;
    InventorySystem inventory;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
        input = GetComponent<InputManager>();
        pm = GetComponent<PlayerMovement>();
        inventory = GetComponent<InventorySystem>();
    }

    
    void Update()
    {
        if (input.doPullOutFish && !hasBaited)
        {
            StartCoroutine(PullProp(propPos, deltaPos, fishingPoint.position));
            StopCoroutine(WaitForFish());
        }
        lineRendere.SetPosition(0, lineStart.position);
        lineRendere.SetPosition(1, testObject.position);
        if (input.doChargeFishing)
        {
            if(isFishing || isThrowing)
                chargeSliderUI.SetActive(false);
            else
                chargeSliderUI.SetActive(true)
                    ;
            if (isFishing || isThrowing)
                return;

                isCharging = true;
            currentRot = Mathf.Clamp(currentRot, minRot, maxRot);
            inverseCurrentRot = Mathf.Clamp(inverseCurrentRot, minRot, maxRot);
            if (!doMinus)
            {
                currentRot += chargeRate * Time.deltaTime;
                inverseCurrentRot -= chargeRate * Time.deltaTime;
                currentRot = Mathf.Clamp(currentRot, minRot, maxRot);
                inverseCurrentRot = Mathf.Clamp(inverseCurrentRot, minRot, maxRot);
                if (currentRot == maxRot)
                    doMinus = true;
                
            }
            else
            {
                currentRot -= chargeRate * Time.deltaTime;
                inverseCurrentRot += chargeRate * Time.deltaTime;
                currentRot = Mathf.Clamp(currentRot, minRot, maxRot);
                inverseCurrentRot = Mathf.Clamp(inverseCurrentRot, minRot, maxRot);
                if (currentRot == minRot)
                    doMinus = false;
            }
            chargeSlider.value = inverseCurrentRot;
        }
        else
        {
            chargeSliderUI.SetActive(false);
            if (isCharging)
            {
                if(inverseCurrentRot / maxRot > 0.95f)
                {
                    currentRot = minRot;

                    fishingPoint.localEulerAngles = new Vector3(currentRot, 0, 180);
                    print("Perfect!");
                }
                RaycastHit hit;
                if(Physics.Raycast(fishingPoint.position,fishingPoint.up,out hit, 20, waterLayer))
                {
                    deltaPos = transform.position - hit.point;
                    deltaPos = new Vector3(hit.point.x + deltaPos.x / 2, transform.position.y + 10, hit.point.z + deltaPos.z / 2);
                    propPos = hit.point;
                    StartCoroutine(ThrowProp(fishingPoint.position, deltaPos, hit.point));
                }
                else
                {
                    print("hit nothing");
                }
                isCharging = false;
            }

            currentRot = maxRot;
            inverseCurrentRot = minRot;
            doMinus = false;
        }
        fishingPoint.localEulerAngles = new Vector3(currentRot, 0, 180);

        if (isFishing && !waitingForFish && !isInterrupted)
        {
            StartCoroutine(WaitForFish());
        }
    }

    IEnumerator WaitForFish()
    {
        waitingForFish = true;
        isInterrupted = true;
        float timeForFish = UnityEngine.Random.Range(minTimeToBait, maxTimeToBait);
        yield return new WaitForSeconds(timeForFish);
        if(isFishing)
            StartCoroutine(Bait());

    }
    IEnumerator Bait()
    {
        source.PlayOneShot(baitClip);
        float timeElapsed = 0;
        float timeForBait = 0.75f;
        bool failed = true;
        hasBaited = true;
        do
        {

            if (input.doPullOutFish)
            {
                failed = false;
                hasBaited = false;
                CameraManager cm = CameraManager.instance;
                cm.SwitchCamera(cm.LureCam, CinemachineBlendDefinition.Style.EaseInOut);
                StartCoroutine(TransitionToMinigame());
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        } while (timeElapsed < timeForBait);

        if (failed)
        {
            hasBaited = false;
            StartCoroutine(WaitForFish());
        }
       
    }

    IEnumerator TransitionToMinigame()
    {
        CameraManager cm = CameraManager.instance;
        yield return new WaitUntil(() => cm.HasReachedDestination());
        StartCoroutine(FishMinigame());
    }
    IEnumerator FishMinigame()
    {
        fishSliderUI.SetActive(true);
        fishSlider.maxValue = 100;
        float progress = fishSlider.maxValue / 2;
        fishSlider.value = progress;
        fishPull = NewFishDirection();
        CameraManager cm = CameraManager.instance;
        StartCoroutine(ChangeFishDirection());
        waterSplash.Play();
        do
        {
            if(input.pullInput == -fishPull)
            {
                cm.SetNoiseSetting(cm.SixDShake);
                progress += fishRate * Time.deltaTime * 1.25f;
            }
            else
            {
                cm.SetNoiseSetting(cm.normalHandHeldMild);
                progress -= fishRate * Time.deltaTime;
            }

            progress = Mathf.Clamp(progress, fishSlider.minValue, fishSlider.maxValue);
            fishSlider.value = progress;
            if (progress == fishSlider.maxValue)
            {
                StopCoroutine(ChangeFishDirection());
                fishSliderUI.SetActive(false);
                StartCoroutine(PullProp(propPos, deltaPos, fishingPoint.position));
                FishItems fish = fishs[UnityEngine.Random.Range(0, fishs.Length)];
                inventory.PutInInventory(fish, 1);
            }
            else if (progress == fishSlider.minValue)
            {
                StopCoroutine(ChangeFishDirection());
                fishSliderUI.SetActive(false);
                StartCoroutine(PullProp(propPos, deltaPos, fishingPoint.position));
            }
            yield return null;
        } while (isInterrupted);

    }
    IEnumerator ChangeFishDirection()
    {
        float minWait = 0.5f;
        float maxWait = 2f;

        while (isInterrupted)
        {
            float timeToWait = UnityEngine.Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(timeToWait);
            fishPull = NewFishDirection();
        }

    }
    Vector2 NewFishDirection()
    {
        int direction = UnityEngine.Random.Range(1, 9);

        switch (direction)
        {
            case 1:
                testObject.GetChild(0).localEulerAngles = new Vector3(0, 25, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(0, -90, 0);
                return new Vector2(0, 1).normalized;
            case 2:
                testObject.GetChild(0).localEulerAngles = new Vector3(-25, 0, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(90, 0, 0);
                return new Vector2(1, 0).normalized;
            case 3:
                testObject.GetChild(0).localEulerAngles = new Vector3(25, 0, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(-90, 0, 0);
                return new Vector2(-1, 0).normalized;
            case 4:
                testObject.GetChild(0).localEulerAngles = new Vector3(0, -25, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(0,  90, 0);
                return new Vector2(0, -1).normalized;
            case 5:
                testObject.GetChild(0).localEulerAngles = new Vector3(-25, 25, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(42.243f, -100.302f, -6.469f);
                return new Vector2(1, 1).normalized;
            case 6:
                testObject.GetChild(0).localEulerAngles = new Vector3(-25, -25, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(43.366f, 109.658f, 15.892f);
                return new Vector2(1, -1).normalized;
            case 7:
                testObject.GetChild(0).localEulerAngles = new Vector3(25, 25, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(-30.574f, -102.29f, -4.262f);
                return new Vector2(-1, 1).normalized;
            case 8:
                testObject.GetChild(0).localEulerAngles = new Vector3(25, -25, testObject.localEulerAngles.z);
                waterSplash.transform.localEulerAngles = new Vector3(-47.392f, 93.884f, -67.434f);
                return new Vector2(-1, -1).normalized;
            default:
                return new Vector2(0, 1).normalized;

        }
    }
    IEnumerator ThrowProp(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        waitingForFish = false;
        float timeElapsed = 0;
        float timeToGo = 1f;
        isThrowing = true;
        lineRendere.enabled = true;
        testObject.gameObject.SetActive(true);
        testObject.localEulerAngles = pm.body.transform.localEulerAngles - new Vector3(90, 0,0);
        testObject.GetChild(0).localEulerAngles = new Vector3(0, 0, testObject.localEulerAngles.z);
        while (timeElapsed < timeToGo)
        {
            float t = timeElapsed / timeToGo;
            Vector3 currentPos = p1 + Mathf.Pow(1 - t, 2) * (p0 - p1) + Mathf.Pow(t, 2) * (p2 - p1);
            testObject.position = currentPos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        testObject.position = p2;
        isThrowing = false;
        isFishing = true;
    }

    IEnumerator PullProp(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        isFishing = false;
        isInterrupted = false;
        float timeElapsed = 0;
        float timeToGo = 1f;
        lineRendere.enabled = false;
        CameraManager cm = CameraManager.instance;
        cm.SetNoiseSetting(cm.normalHandHeldMild);
        cm.SwitchCamera(cm.normalCam, CinemachineBlendDefinition.Style.Cut);
        isThrowing = true;
        while (timeElapsed < timeToGo)
        {
            float t = timeElapsed / timeToGo;
            Vector3 currentPos = p1 + Mathf.Pow(1 - t, 2) * (p0 - p1) + Mathf.Pow(t, 2) * (p2 - p1);
            testObject.position = currentPos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        testObject.position = p2;
        isThrowing = false;
        testObject.gameObject.SetActive(false);


    }
}
