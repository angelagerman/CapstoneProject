using UnityEngine;
using UnityEngine.UI;

public class AllyStatusEntry : MonoBehaviour
{
    public Text nameText;
    public Image hpFill;
    public Image mpFill;
    public Text HPText;
    public Text MPText;

    private bool isHighlighted;
    private Vector3 normalScale = Vector3.one;
    private Vector3 highlightScale = new Vector3(1.05f, 1.05f, 1f);
    private float highlightSpeed = 6f;

    private AllyBattleActions ally;

    public void Initialize(AllyBattleActions ally)
    {
        this.ally = ally;
        nameText.text = ally.stats.characterName;
        Refresh();
    }

    public void Refresh()
    {
        if (ally.stats == null) return;

        float hpPercent = (float)ally.stats.currentHealth / ally.stats.maxHealth;
        float mpPercent = (float)ally.stats.currentMagic / ally.stats.maxHealth;

        hpFill.fillAmount = hpPercent;
        mpFill.fillAmount = mpPercent;

        HPText.text = "HP " + ally.stats.currentHealth + " / ";
        MPText.text = " " + ally.stats.currentMagic + " MP";
    }
    
    void Update()
    {
        // Smoothly animate the entire entry’s scale
        Vector3 targetScale = isHighlighted ? highlightScale : normalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * highlightSpeed);
    }

    public void SetHighlighted(bool state)
    {
        isHighlighted = state;
    }

    public AllyBattleActions GetAlly()
    {
        return ally;
    }
}
