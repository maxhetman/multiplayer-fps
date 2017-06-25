using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour
{

    [SerializeField] private Text userNameText;

    [SerializeField] private Player player;
    [SerializeField] private RectTransform healthBarFill;

    void Update()
    {
        userNameText.text = player.Name;
        healthBarFill.localScale = new Vector3(player.GetHealthPct(), 1f, 1f);
    }
}
