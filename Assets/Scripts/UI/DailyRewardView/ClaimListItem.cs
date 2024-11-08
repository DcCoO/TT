using TMPro;
using UnityEngine;

public class ClaimListItem : MonoBehaviour
{
    [SerializeField] private GameObject _claimed;
    [SerializeField] private TMP_Text _dayLabel;
    [SerializeField] private TMP_Text _rewardLabel;
    
    private bool _isClaimed;
    
    public void Init(int day, int reward)
    {
        _dayLabel.text = $"DAY {day}";
        _rewardLabel.text = reward.ToString();
        SetClaimed(false);
    }
    
    public void SetClaimed(bool claimed)
    {
        _claimed.SetActive(claimed);
        _isClaimed = claimed;
    }
}
