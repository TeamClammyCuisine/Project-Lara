using UnityEngine;

public class RenderSortingOrder : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private int baseOrder = 5000;
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnce = false;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();
    }
    
    public void FixedUpdate()
    {
        _renderer.sortingOrder = (int) (baseOrder - transform.position.y - offset);
        if(runOnce)
            Destroy(this);
    }
}