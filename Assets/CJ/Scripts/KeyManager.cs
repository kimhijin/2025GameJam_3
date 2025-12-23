using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }
    
    [SerializeField]private int keyCount = 0;
    public System.Action<int> OnKeyCountChanged;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void AddKey(int amount = 1)
    {
        keyCount += amount;
        OnKeyCountChanged?.Invoke(keyCount);
    }
    
    public bool UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            OnKeyCountChanged?.Invoke(keyCount);
            return true;
        }
        return false;
    }
    
    public int GetKeyCount() => keyCount;
}