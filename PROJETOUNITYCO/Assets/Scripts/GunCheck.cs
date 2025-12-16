using UnityEngine;
using TMPro;
public class GunCheck : MonoBehaviour
{
    public GameObject Teleport;
    TMP_Text tmpText;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (tmpText.text != "Gun")
        {
            Invoke("NoGun", 1f);
        }
    }

    public void NoGun()
    {
        Teleport.gameObject.SetActive(false);
    }
}
