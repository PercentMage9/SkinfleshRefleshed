using UnityEngine;

public class BrainRotter : MonoBehaviour
{
    public float decreaseAmountPerSecond = 10f;

    private PlayerBrainRot playerBrainRot;
    public bool isColliding;

    private void Start()
    {
        playerBrainRot = FindObjectOfType<PlayerBrainRot>();
    }

    private void Update()
    {
        if (isColliding)
        {
            Debug.Log("Decreasing brain health");
            playerBrainRot.DecreaseBrainHealth(decreaseAmountPerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Is colliding");
        //isColliding = true;
        playerBrainRot.StopIncreaseCoroutine();
        playerBrainRot.SetIsColliding(true);
    }

    private void OnTriggerExit(Collider other)
    {
        //isColliding = false;
        playerBrainRot.SetIsColliding(false);
    }
}