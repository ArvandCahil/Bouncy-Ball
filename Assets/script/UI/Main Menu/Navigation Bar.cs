using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Rendering;

public class NavigationBar : MonoBehaviour
{
    public enum State
    {
        shop,
        home,
        inventory
    }

    private Dictionary<State, float> stateInformation;
    private Dictionary<State, GameObject> objectInformation;
    private State state;
    private Color activeColor, inactiveColor;

    [SerializeField] private RectTransform selector;
    [SerializeField] private GameObject shopObject;
    [SerializeField] private GameObject homeObject;
    [SerializeField] private GameObject inventoryObject;

    private Camera mainCamera;
    private Camera overlayCamera;
    private Volume EnvironmentVolume;
    private Volume BallVolume;

    private void Start()
    {
        stateInformation = new Dictionary<State, float>();
        objectInformation = new Dictionary<State, GameObject>();

        stateInformation.Add(State.shop, -338f);
        stateInformation.Add(State.home, 0f);
        stateInformation.Add(State.inventory, 338f);

        objectInformation.Add(State.shop, shopObject);
        objectInformation.Add(State.home, homeObject);
        objectInformation.Add(State.inventory, inventoryObject);

        inactiveColor = new Color(0.905f, 0.905f, 0.905f);
        activeColor = new Color(0.196f, 0.196f, 0.196f);

        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            UniversalAdditionalCameraData cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            if (cameraData != null && cameraData.cameraStack.Count > 0)
            {
                overlayCamera = cameraData.cameraStack[0];
            }
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
        if (overlayCamera == null)
        {
            Debug.LogError("Overlay camera not found in the camera stack!");
        }

        GameObject[] cache = GameObject.FindGameObjectsWithTag("Post Processing");
        foreach (GameObject p in cache)
        {
            if(gameObject.layer.Equals("Post Processing"))
            {
                EnvironmentVolume = p.GetComponent<Volume>();
            }else if(gameObject.layer.Equals("Post Processing 1"))
            {
                BallVolume = p.GetComponent<Volume>();
            }
        }

        setState(State.home);
    }

    private void setState(State targetState)
    {
        if (state != targetState)
        {
            if (objectInformation.ContainsKey(state))
            {
                TextMeshProUGUI lastText = objectInformation[state].GetComponentInChildren<TextMeshProUGUI>();
                lastText.DOColor(inactiveColor, 1f);
            }

            State lastState = state;
            state = targetState;

            if (selector.localPosition.x != stateInformation[targetState])
            {
                StartCoroutine(WaveText(targetState));
                parallaxButtonEffect(targetState, lastState);

                selector.DOLocalMoveX(stateInformation[targetState], 0.3f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    TextMeshProUGUI targetText = objectInformation[targetState].GetComponentInChildren<TextMeshProUGUI>();
                    targetText.DOColor(activeColor, 0.3f);
                });
            }

            UpdateCameraEffect(targetState);
        }
    }

    private void UpdateCameraEffect(State targetState)
    {
        if (mainCamera != null && overlayCamera != null)
        {
            float targetFOV = 60f;
            if (targetState == State.inventory)
            {
                targetFOV = 30f;
            }else if (targetState == State.shop)
            {
                targetFOV = 90f;
            }
            else if(targetState == State.home)
            {
                targetFOV = 60f;
            }

            mainCamera.DOKill();
            overlayCamera.DOKill();
            mainCamera.DOFieldOfView(targetFOV, 0.5f).SetEase(Ease.InOutCubic);
            overlayCamera.DOFieldOfView(targetFOV, 0.5f).SetEase(Ease.InOutCubic);
        }
    }

    public void onButtonClick(int index)
    {
        switch (index)
        {
            case 0:
                setState(State.shop);
                break;
            case 1:
                setState(State.home);
                break;
            case 2:
                setState(State.inventory);
                break;
        }
    }

    private void parallaxButtonEffect(State targetState, State lastState)
    {
        float additiveY = 10f;
        float additiveScale = 0.2f;

        if (objectInformation.ContainsKey(lastState))
        {
            Image lastIcon = objectInformation[lastState].GetComponentsInChildren<Image>()[1];
            TextMeshProUGUI lastText = objectInformation[lastState].GetComponentInChildren<TextMeshProUGUI>();
            lastText.rectTransform.DOAnchorPosY(lastText.rectTransform.anchoredPosition.y + additiveY, 0.2f).SetEase(Ease.InOutCubic);
            lastIcon.rectTransform.DOScale(lastIcon.transform.localScale.y - additiveScale, 0.2f).SetEase(Ease.InOutCubic);
        }

        Image targetIcon = objectInformation[targetState].GetComponentsInChildren<Image>()[1];
        TextMeshProUGUI targetText = objectInformation[targetState].GetComponentInChildren<TextMeshProUGUI>();
        targetIcon.rectTransform.DOScale(targetIcon.transform.localScale.y + additiveScale, 0.2f).SetEase(Ease.InOutCubic);
        targetText.rectTransform.DOAnchorPosY(targetText.rectTransform.anchoredPosition.y - additiveY, 0.2f).SetEase(Ease.InOutCubic);
    }

    private IEnumerator WaveText(State targetState)
    {
        TextMeshProUGUI targetText = objectInformation[targetState].GetComponentInChildren<TextMeshProUGUI>();
        targetText.ForceMeshUpdate();
        TMP_TextInfo textInfo = targetText.textInfo;

        Vector3[][] originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = (Vector3[])textInfo.meshInfo[i].vertices.Clone();
        }

        float startTime = Time.time;
        float duration = 0.5f;

        while (Time.time < startTime + duration)
        {
            float timeElapsed = Time.time - startTime;
            float waveFrequency = 5f;
            float waveAmplitude = 4f;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                for (int j = 0; j < 4; j++)
                {
                    Vector3 orig = originalVertices[charInfo.materialReferenceIndex][charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin((orig.x + Time.time * 5f) * waveFrequency) * waveAmplitude, 0);
                }
            }

            for (int k = 0; k < textInfo.meshInfo.Length; k++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[k];
                meshInfo.mesh.vertices = meshInfo.vertices;
                targetText.UpdateGeometry(meshInfo.mesh, k);
            }

            yield return null;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = originalVertices[i];
            targetText.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
