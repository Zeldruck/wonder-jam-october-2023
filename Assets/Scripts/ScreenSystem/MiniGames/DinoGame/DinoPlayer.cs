using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DinoPlayer : MonoBehaviour
{
    public MiniGame3Dino miniGame3Dino;
    public float jumpHeight = 1.5f;

    [Header("--- DEBUG -----")]
    public bool isGrounded = true;
    public float currentTime = 0f;
    public float timeStep = 0f;
    private void Awake()
    {
        timeStep = 3.0f / miniGame3Dino.gameSpeed;
    }
    private void Update()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
            isGrounded = false;
        }

        if(!isGrounded)
        {
            float newSpeed = miniGame3Dino.gameSpeed / transform.localScale.x;
            currentTime += Time.deltaTime;

            if (currentTime >= timeStep)
            {
                currentTime = 0f;
                transform.position = new Vector3(transform.position.x, transform.position.y - jumpHeight, transform.position.z);
                isGrounded = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            miniGame3Dino.EndMiniGame(false);
        }
    }

}
