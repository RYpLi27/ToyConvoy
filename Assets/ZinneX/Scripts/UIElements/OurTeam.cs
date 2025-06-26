using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OurTeam : MonoBehaviour {
    [SerializeField] private List<TeamMemberSO> teamMembers;
    [SerializeField] private List<Image> dotFill;
    [SerializeField] private TMP_Text fullNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image ratImage;

    private int currentIndex;
    
    public void NextMember() {
        currentIndex = currentIndex == teamMembers.Count - 1 ? 0 : ++currentIndex;
        UpdateDisplay();
    }

    public void PreviousMember() {
        currentIndex = currentIndex == 0 ? teamMembers.Count - 1 : --currentIndex;
        UpdateDisplay();
    }

    public void FirstMember() {
        currentIndex = 0;
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        fullNameText.text = teamMembers[currentIndex].fullName;
        descriptionText.text = teamMembers[currentIndex].description;
        SetImage(teamMembers[currentIndex].sprite);
        
        dotFill.ForEach(dot => dot.enabled = false);
        dotFill[currentIndex].enabled = true;
    }
    
    private void SetImage(Sprite newSprite) {
        ratImage.sprite = newSprite;

        Vector2 spriteSize = newSprite.rect.size;
        Vector2 pivot = newSprite.pivot / spriteSize;

        ratImage.rectTransform.pivot = pivot;
        ratImage.rectTransform.sizeDelta = spriteSize;
    }
}
