using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBrainRot : MonoBehaviour
{
    private float maxBrainHealth = 255f;
    public float decreaseRatePerSecond = 8f;
    private float currentBrainHealth;
    private bool isColliding;
    private bool isIncreasing;
    public GameObject brainSprite;

    private Coroutine increaseCoroutine;
    private RectTransform brainRectTransform;

    private float targetBrainHealth;
    private float regenerationDuration = 4f;

    public float insanityThreshold = 200f;
    public RectTransform borderRectTransform;

    private float startingFOV;
    public Camera playerCamera;

    private float startingPlayerSpeed;
    public GameObject player;

    public void Start()
    {
        startingPlayerSpeed = player.GetComponent<PlayerMovement>().speed;
        startingFOV = Camera.main.fieldOfView;
        currentBrainHealth = maxBrainHealth;
        brainRectTransform = brainSprite.GetComponent<RectTransform>();
        UpdateBrainColorAndScale();
    }

    private void Update()
    {
        if (isColliding)
        {
            if (increaseCoroutine != null)
            {
                StopCoroutine(increaseCoroutine);
            }
            DecreaseBrainHealth(decreaseRatePerSecond * Time.deltaTime);
        }
        else if (currentBrainHealth < maxBrainHealth && !isIncreasing)
        {
            targetBrainHealth = currentBrainHealth;
            increaseCoroutine = StartCoroutine(IncreaseBrainHealthCoroutine(targetBrainHealth));
        }

        if (currentBrainHealth < insanityThreshold - 50)
        {
            player.GetComponent<PlayerMovement>().speed = startingPlayerSpeed - (startingPlayerSpeed / 4);
            playerCamera.fieldOfView = currentBrainHealth - 80;
            if (playerCamera.fieldOfView < 50)
            {
                playerCamera.fieldOfView = 50;
            }
        }
        else
        {
            player.GetComponent<PlayerMovement>().speed = startingPlayerSpeed;
            playerCamera.fieldOfView = startingFOV;
        }
    }

    private IEnumerator IncreaseBrainHealthCoroutine(float targetHealth)
    {
        isIncreasing = true;
        float startTime = Time.time;
        float endTime = startTime + regenerationDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / regenerationDuration;
            float easedT = Mathf.Pow(t, 2f);
            currentBrainHealth = Mathf.Lerp(targetHealth, maxBrainHealth, easedT);
            UpdateBrainColorAndScale();
            yield return null;
        }

        currentBrainHealth = maxBrainHealth;
        UpdateBrainColorAndScale();
        isIncreasing = false;
    }

    public void StopIncreaseCoroutine()
    {
        if (isIncreasing && increaseCoroutine != null)
        {
            StopCoroutine(increaseCoroutine);
            isIncreasing = false;
        }
    }

    private void UpdateBrainColorAndScale()
    {
        float normalizedRed = currentBrainHealth / maxBrainHealth;
        brainSprite.GetComponent<Graphic>().color = new Color(normalizedRed, 63f / 255f, 83f / 255f, 1f);

        float normalizedScale = currentBrainHealth / maxBrainHealth;
        brainRectTransform.localScale = new Vector3(1f, normalizedScale, 1f);

        if(currentBrainHealth < insanityThreshold)
        {
            borderRectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            borderRectTransform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    public void DecreaseBrainHealth(float amount)
    {
        currentBrainHealth -= amount;
        currentBrainHealth = Mathf.Clamp(currentBrainHealth, 0f, maxBrainHealth);
        UpdateBrainColorAndScale();
    }

    public void SetIsColliding(bool value)
    {
        isColliding = value;
    }
}