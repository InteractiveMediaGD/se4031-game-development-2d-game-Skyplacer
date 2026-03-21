using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // Offsets the texture over time to create the "moving" effect
        Vector2 offset = new Vector2(Time.time * scrollSpeed, 0);
        meshRenderer.material.mainTextureOffset = offset;
    }
}