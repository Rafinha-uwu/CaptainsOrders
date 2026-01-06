using UnityEngine;

public class FlagLogicV3 : MonoBehaviour
{
    // ===== ENUMS =====

    public enum FlagColor
    {
        White,        // 0
        BlueRed,      // 1
        Violet,       // 2
        PurpleBlack   // 3
    }

    public enum FlagCharge
    {
        Crest,           // 0
        HorizontalLines, // 1
        FilledCross,     // 2
        HollowCross      // 3
    }

    public enum FlagShape
    {
        Square,       // 0
        Rectangle,    // 1
        Triangle,     // 2
        BigRectangle  // 3
    }

    // =========================================================
    // OTHER SHIP VISUAL REFERENCES
    // =========================================================

    [Header("Other Ship - Colors (paired, match enum order)")]
    public GameObject[] otherColorObjects;   // size 4

    [Header("Other Ship - Charges (match enum order)")]
    public GameObject[] otherChargeObjects;  // size 4

    [Header("Other Ship - Flag Count (1,2,3)")]
    public GameObject[] otherFlagCountObjects; // size 3

    // =========================================================
    // OTHER SHIP DATA
    // =========================================================

    private FlagColor otherColor;
    private FlagCharge otherCharge;
    private int otherCount;

    // =========================================================
    // OUR FLAG (PLAYER INPUT)
    // =========================================================

    private FlagColor ourColor;
    private FlagCharge ourCharge;
    private FlagShape ourShape;

    // =========================================================
    // EXPECTED RESULT (RULE OUTPUT)
    // =========================================================

    private FlagColor expectedColor;
    private FlagCharge expectedCharge;
    private FlagShape expectedShape;

    void Start()
    {
        RandomizeOtherShip();
        ComputeExpectedFlag(); // rules later
    }

    // =========================================================
    // RANDOMIZATION + VISUALS
    // =========================================================

    public void RandomizeOtherShip()
    {
        otherColor = (FlagColor)Random.Range(0, 4);
        otherCharge = (FlagCharge)Random.Range(0, 4);
        otherCount = Random.Range(1, 4);

        ApplyOtherShipVisuals();
    }

    void ApplyOtherShipVisuals()
    {
        // Colors (paired)
        for (int i = 0; i < otherColorObjects.Length; i++)
            otherColorObjects[i].SetActive(i == (int)otherColor);

        // Charges
        for (int i = 0; i < otherChargeObjects.Length; i++)
            otherChargeObjects[i].SetActive(i == (int)otherCharge);

        // Flag count
        for (int i = 0; i < otherFlagCountObjects.Length; i++)
            otherFlagCountObjects[i].SetActive(i < otherCount);
    }

    // =========================================================
    // RULE ENGINE (PLACEHOLDER)
    // =========================================================

    void ComputeExpectedFlag()
    {
        // TEMP safe defaults
        expectedColor = FlagColor.White;
        expectedCharge = FlagCharge.Crest;
        expectedShape = FlagShape.Square;
    }

    // =========================================================
    // PLAYER INPUT
    // =========================================================

    public void SetOurColor(int index)
    {
        ourColor = (FlagColor)index;
    }

    public void SetOurCharge(int index)
    {
        ourCharge = (FlagCharge)index;
    }

    public void SetOurShape(int index)
    {
        ourShape = (FlagShape)index;
    }

    // =========================================================
    // CONFIRMATION
    // =========================================================

    bool ConfirmFlag_Internal()
    {
        return
            ourColor == expectedColor &&
            ourCharge == expectedCharge &&
            ourShape == expectedShape;
    }

    // UnityEvent-safe wrapper
    public void ConfirmFlag_UnityEvent()
    {
        bool correct = ConfirmFlag_Internal();
        Debug.Log(correct ? "FLAG CORRECT" : "FLAG INCORRECT");
    }
}
