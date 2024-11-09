using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ClaimListItem : MonoBehaviour
{
    [SerializeField] private GameObject _claimed;
    [SerializeField] private TMP_Text _dayLabel;
    [SerializeField] private TMP_Text _rewardLabel;
    [SerializeField] private List<GameObject> _coinsIcons;
    
    private bool _isClaimed;
    
    public Vector3 GetPosition() => _coinsIcons[0].transform.position;
    
    public void Init(int day, int reward, int amountOfCoinsIcons)
    {
        _dayLabel.text = $"DAY {day}";
        _rewardLabel.text = reward.ToString();
        SetCoinsIcons(amountOfCoinsIcons);
        SetClaimed(false);
    }
    
    public void SetClaimed(bool claimed)
    {
        _claimed.SetActive(claimed);
        _isClaimed = claimed;
    }

    private void SetCoinsIcons(int amountOfCoinsIcons)
    {
        for (var i = 0; i < _coinsIcons.Count; i++)
        {
            var icon = _coinsIcons[i];
            icon.SetActive(i < amountOfCoinsIcons);
        }
    }
}

