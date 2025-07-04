using UnityEngine;
using DG.Tweening;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingData _buildingData;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public virtual void Click()
    {
        UIController.Instance.ShowInfoButton(_buildingData);

        transform.DOKill();
        
        transform.localScale = _originalScale;

        transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, vibrato: 10, elasticity: 1f)
            .OnComplete(() => transform.localScale = _originalScale);
    }
}