using UnityEngine;

public class FlagLogic : MonoBehaviour
{
    // ================= ENUMS =================

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

    // ================= OTHER SHIP VISUALS =================

    [Header("Other Ship - Colors")]
    public GameObject[] otherColorObjects;   // size 4

    [Header("Other Ship - Charges")]
    public GameObject[] otherChargeObjects;  // size 4

    [Header("Other Ship - Masts (1–3)")]
    public GameObject[] otherMastObjects;    // size 3

    // ================= OTHER SHIP DATA =================

    private FlagColor otherColor;
    private FlagCharge otherCharge;
    private int otherMasts;

    // ================= PLAYER INPUT =================

    private FlagColor ourColor;
    private FlagCharge ourCharge;
    private FlagShape ourShape;

    // ================= EXPECTED RESULT =================

    private FlagColor expectedColor;
    private FlagCharge expectedCharge;
    private FlagShape expectedShape;

    void Start()
    {
        RandomizeOtherShip();
        ComputeExpectedFlag();
    }

    // ====================================================
    // RANDOMIZATION + VISUAL APPLICATION
    // ====================================================

    public void RandomizeOtherShip()
    {
        otherColor = (FlagColor)Random.Range(0, 4);
        otherCharge = (FlagCharge)Random.Range(0, 4);
        otherMasts = Random.Range(1, 4);

        ApplyOtherShipVisuals();
    }

    void ApplyOtherShipVisuals()
    {
        for (int i = 0; i < otherColorObjects.Length; i++)
            otherColorObjects[i].SetActive(i == (int)otherColor);

        for (int i = 0; i < otherChargeObjects.Length; i++)
            otherChargeObjects[i].SetActive(i == (int)otherCharge);

        for (int i = 0; i < otherMastObjects.Length; i++)
            otherMastObjects[i].SetActive(i < otherMasts);
    }

    // ====================================================
    // RULE ENGINE — THIS IS THE IMPORTANT PART
    // ====================================================

    void ComputeExpectedFlag()
    {
        // ---------- Rule 1 ----------
        if (otherColor == FlagColor.White)
            expectedColor = FlagColor.Violet;
        else if (otherColor == FlagColor.BlueRed)
            expectedColor = FlagColor.White;
        else
            expectedColor = FlagColor.PurpleBlack;

        // ---------- Rule 2 ----------
        if (otherCharge == FlagCharge.FilledCross)
            expectedColor = FlagColor.BlueRed;
        else if (otherCharge == FlagCharge.HollowCross)
            expectedColor = FlagColor.White;
        // Crest / HorizontalLines -> no change

        // ---------- Rule 3 ----------
        if (otherMasts == 3)
        {
            expectedColor = InvertColor(expectedColor);
        }

        // ---------- Rule 4 ----------
        if (otherMasts == 1)
            expectedShape = FlagShape.Square;
        else if (otherMasts == 2)
            expectedShape = FlagShape.Rectangle;
        else
            expectedShape = FlagShape.BigRectangle;

        // ---------- Rule 5 ----------
        if (otherCharge == FlagCharge.HorizontalLines)
            expectedShape = FlagShape.Triangle;

        // ---------- Rule 6 ----------
        if (otherMasts > 1)
            expectedCharge = FlagCharge.FilledCross;
        else if (otherCharge == FlagCharge.Crest)
            expectedCharge = FlagCharge.HollowCross;
        else
            expectedCharge = FlagCharge.HorizontalLines;

        Debug.Log($"EXPECTED → Color:{expectedColor}, Shape:{expectedShape}, Charge:{expectedCharge}");
    }

    FlagColor InvertColor(FlagColor c)
    {
        return c switch
        {
            FlagColor.White => FlagColor.Violet,
            FlagColor.Violet => FlagColor.White,
            FlagColor.BlueRed => FlagColor.PurpleBlack,
            FlagColor.PurpleBlack => FlagColor.BlueRed,
            _ => c
        };
    }

    // ====================================================
    // PLAYER INPUT (BUTTONS)
    // ====================================================

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

    // ====================================================
    // CONFIRMATION (PEACE SIGN)
    // ====================================================

    bool ConfirmFlag_Internal()
    {
        return
            ourColor == expectedColor &&
            ourCharge == expectedCharge &&
            ourShape == expectedShape;
    }

    // UnityEvent-safe
    public void ConfirmFlag_UnityEvent()
    {
        bool correct = ConfirmFlag_Internal();

        Puzzle puzzle = GetComponent<Puzzle>();

        if (puzzle == null)
        {
            Debug.LogError("Puzzle component not found on this GameObject!");
            return;
        }

        if (correct)
        {
            Debug.Log("FLAG CORRECT");
            puzzle.CompletePuzzle();
        }
        else
        {
            Debug.Log("FLAG INCORRECT");
            puzzle.FailPuzzle();
        }
    }
}
