using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField, TextArea(5, 10)] private string _info;
    
    public virtual void Click()
    {
        
    }
}
