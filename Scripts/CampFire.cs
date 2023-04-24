using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampFire : MonoBehaviour, IInteractable
{
    GameObject cookingMenu;
    Slider progressBar;
    [SerializeField] float cookRate = 50;
    [SerializeField] float turnRate = 60;
    [SerializeField] GameObject salmonStickParent;
    [SerializeField] GameObject salmonStick;
    [SerializeField] MeshRenderer salmonColor;
    [SerializeField] Material salmonMaterial;
    bool isInterrupted = false;
    InputManager input;

    [Header("Food items")]
    public FoodItems roastedSalmon;
    public void Interact()
    {
        input = FindObjectOfType<InputManager>();
        CameraManager cm = CameraManager.instance;
        cm.SwitchCamera(cm.CampfireCam, Cinemachine.CinemachineBlendDefinition.Style.EaseInOut);
        StartCoroutine(TransitionToMinigame());
    }

    IEnumerator TransitionToMinigame()
    {
        CameraManager cm = CameraManager.instance;
        yield return new WaitUntil(() => cm.HasReachedDestination());
        GameObject canvas = GameObject.Find("Canvas");
        progressBar = canvas.transform.Find("FishingGameUI").gameObject.GetComponentInChildren<Slider>();
        cookingMenu = canvas.transform.Find("CookingMenu").gameObject;
        cookingMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindObjectOfType<InputManager>().isInterrupted = true;

    }

    public void RoastedSalmonRecipe()
    {
        StartCoroutine(RoastedSalmonMinigame());
    }

    IEnumerator RoastedSalmonMinigame()
    {
        isInterrupted = true;
        cookingMenu.SetActive(false);
        progressBar.transform.parent.gameObject.SetActive(true);
        GameObject salmon = Instantiate(salmonStick, salmonStickParent.transform);
        salmonColor = salmon.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        salmonStickParent.SetActive(true);
        progressBar.maxValue = 600;
        progressBar.value = 0;
        CameraManager cm = CameraManager.instance;
        float progress1 = 0;
        float progress2 = 0;
        float progress3 = 0;
        float progress4 = 0;
        float progress5 = 0;
        float progress6 = 0;
        float targetRotation = 0;
        do
        {
            targetRotation = Mathf.Clamp(targetRotation, 0, 360);
            if(input.turnDir < 0)
            {
                if (targetRotation == 0)
                    targetRotation = 360;
                targetRotation -= turnRate * Time.deltaTime;
            }
            else if(input.turnDir > 0)
            {
                if (targetRotation == 360)
                    targetRotation = 0;
                targetRotation += turnRate * Time.deltaTime;
            }
            salmon.transform.localEulerAngles = new Vector3(0, 0, targetRotation);
            float rot = salmon.transform.localEulerAngles.z;
            if (rot >= 0 && rot < 60)
            {
                progress1 += cookRate * Time.deltaTime;

                progress1 = Mathf.Clamp(progress1, 0, 100);
                if(progress1 == 100)
                {
                    Material[] materials = salmonColor.materials;
                    materials[2] = new Material(salmonMaterial);
                    salmonColor.materials = materials;
                }
            }
            else if (rot >= 60 && rot < 120)
            {
                progress2 += cookRate * Time.deltaTime;
                progress2 = Mathf.Clamp(progress2, 0, 100);
                if (progress2 == 100)
                {
                    Material[] materials = salmonColor.materials;
                    materials[1] = new Material(salmonMaterial);
                    salmonColor.materials = materials;
                }
            }
            else if (rot >= 120 && rot < 180)
            {
                progress3 += cookRate * Time.deltaTime;
                progress3 = Mathf.Clamp(progress3, 0, 100);
                if (progress3 == 100)
                {
                    Material[] materials = salmonColor.materials;
                    materials[0] = new Material(salmonMaterial);
                    salmonColor.materials = materials;
                }
            }
            else if (rot >= 180 && rot < 240)
            {
                progress4 += cookRate * Time.deltaTime;
                progress4 = Mathf.Clamp(progress4, 0, 100);
                if (progress4 == 100)
                {
                    Material[] materials = salmonColor.materials;
                    materials[7] = new Material(salmonMaterial);
                    salmonColor.materials = materials;
                }
            }
            else if (rot >= 240 && rot < 300)
            {
                progress5 += cookRate * Time.deltaTime;
                progress5 = Mathf.Clamp(progress5, 0, 100);
                if (progress5 == 100)
                {
                    Material[] materials = salmonColor.materials;
                    materials[8] = new Material(salmonMaterial);
                    salmonColor.materials = materials;
                }
            }
            else if (rot >= 300 && rot < 360)
            {
                progress6 += cookRate * Time.deltaTime;
                progress6 = Mathf.Clamp(progress6, 0, 100);
                if (progress6 == 100)
                {
                    Material[] materials = salmonColor.materials;
                    materials[3] = new Material(salmonMaterial);
                    salmonColor.materials = materials;
                }
            }

            progressBar.value = (progress1 + progress2 + progress3 + progress4 + progress5 + progress6);
            if ((progress1+ progress2+ progress3+ progress4+ progress5+ progress6) == progressBar.maxValue)
            {
                FindObjectOfType<InventorySystem>().PutInInventory(roastedSalmon, 1);
                FindObjectOfType<InputManager>().isInterrupted = false;
                isInterrupted = false;
                cm.SwitchCamera(cm.normalCam, Cinemachine.CinemachineBlendDefinition.Style.Cut);
                progressBar.transform.parent.gameObject.SetActive(false);
                salmonStickParent.SetActive(false);
                Destroy(salmon);
            }

            yield return null;
        } while (isInterrupted);
    }

    
}
