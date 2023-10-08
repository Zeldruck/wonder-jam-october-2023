using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DinoGround : MonoBehaviour
{
    public MiniGame3Dino miniGame3Dino;

    [Header("----- DEBUG ------")]
    public float currentTime = 0f;
    public float timeStep = 0f;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        timeStep = 1.0f / miniGame3Dino.gameSpeed;
    }

    private void Update()
    {
        float newSpeed = miniGame3Dino.gameSpeed / transform.localScale.x;
        currentTime += Time.deltaTime;
        if (currentTime >= timeStep)
        {
            currentTime = 0f;
            meshRenderer.material.mainTextureOffset += Vector2.right * newSpeed;
        }
    }

}