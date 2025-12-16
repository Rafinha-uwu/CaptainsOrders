using UnityEngine;
using System.Text;
using TMPro;

public class FlagLogic : MonoBehaviour
{
    // ===== ENUMS =====

    public enum OtherColor { Black, Yellow, Purple }
    public enum OtherCharge { Cross, X, Smile, S, Snake }

    public enum OurColor { LightBlue, Blue, Green, Red }

    // FIXED: no normal Square
    public enum OurShape
    {
        BigSquare,          // 0
        SmallSquare,        // 1
        VerticalRectangle,  // 2
        HorizontalRectangle // 3
    }

    public enum OurCharge
    {
        HorizontalRectangle, // 0
        VerticalRectangle,   // 1
        Cross,               // 2
        ThreeSquares         // 3
    }

    // ===== TMP OUTPUT =====

    [Header("UI")]
    public TMP_Text otherShipText;

    // ===== OTHER SHIP =====

    private OtherColor otherColor;
    private OtherCharge[] otherCharges;
    private int otherCount;

    // ===== OUR FLAG (PLAYER INPUT) =====

    private OurColor ourColor;
    private OurShape ourShape;
    private OurCharge ourCharge;

    // ===== EXPECTED RESULT =====

    private OurColor expectedColor;
    private OurShape expectedShape;
    private OurCharge expectedCharge;

    void Start()
    {
        RandomizeOtherShip();
        ComputeExpectedFlag();
    }

    // ===== RANDOMIZATION =====

    public void RandomizeOtherShip()
    {
        otherColor = (OtherColor)Random.Range(0, 3);
        otherCount = Random.Range(1, 4);

        otherCharges = new OtherCharge[otherCount];
        for (int i = 0; i < otherCount; i++)
        {
            otherCharges[i] = (OtherCharge)Random.Range(0, 5);
        }

        WriteOtherShipText();
    }

    void WriteOtherShipText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Color: {otherColor}");
        sb.AppendLine($"Number: {otherCount}");
        sb.Append("Charges: ");

        for (int i = 0; i < otherCharges.Length; i++)
        {
            sb.Append(otherCharges[i]);
            if (i < otherCharges.Length - 1)
                sb.Append(", ");
        }

        if (otherShipText != null)
            otherShipText.text = sb.ToString();

        Debug.Log(sb.ToString());
    }

    // ===== RULE ENGINE =====

    void ComputeExpectedFlag()
    {
        // Rule 1 — Initial Color
        if (otherColor == OtherColor.Black)
            expectedColor = OurColor.Blue;
        else if (otherColor == OtherColor.Yellow)
            expectedColor = OurColor.Green;
        else
            expectedColor = OurColor.Red;

        // Rule 2 — Charge Interference
        foreach (var c in otherCharges)
        {
            if (c == OtherCharge.Cross || c == OtherCharge.X)
            {
                expectedColor = OurColor.LightBlue;
                break;
            }
        }

        // Rule 3 — Quantity Disruption
        if (otherCount == 3)
            expectedColor = InvertColor(expectedColor);

        // Rule 4 — Shape by Count
        if (otherCount == 1)
            expectedShape = OurShape.BigSquare;
        else if (otherCount == 2)
            expectedShape = OurShape.VerticalRectangle;
        else
            expectedShape = OurShape.HorizontalRectangle;

        // Rule 5 — Emotional Charges (FIXED)
        if (HasCharge(OtherCharge.Smile))
            expectedShape = ReduceShape(expectedShape);

        // Rule 6 — Charge Conversion
        int sCount = CountCharge(OtherCharge.S);
        int snakeCount = CountCharge(OtherCharge.Snake);

        if (sCount > snakeCount)
            expectedCharge = OurCharge.Cross;
        else if (snakeCount > sCount)
            expectedCharge = OurCharge.ThreeSquares;
        else
            expectedCharge = OurCharge.VerticalRectangle;
    }

    OurColor InvertColor(OurColor c)
    {
        return c switch
        {
            OurColor.Blue => OurColor.Red,
            OurColor.Red => OurColor.Blue,
            OurColor.Green => OurColor.LightBlue,
            OurColor.LightBlue => OurColor.Green,
            _ => c
        };
    }

    OurShape ReduceShape(OurShape s)
    {
        return s switch
        {
            OurShape.BigSquare => OurShape.SmallSquare,
            OurShape.VerticalRectangle => OurShape.SmallSquare,
            OurShape.HorizontalRectangle => OurShape.SmallSquare,
            _ => s
        };
    }

    bool HasCharge(OtherCharge c)
    {
        foreach (var ch in otherCharges)
            if (ch == c) return true;
        return false;
    }

    int CountCharge(OtherCharge c)
    {
        int count = 0;
        foreach (var ch in otherCharges)
            if (ch == c) count++;
        return count;
    }

    // ===== BUTTON INPUT =====

    public void SetOurColor(int index)
    {
        ourColor = (OurColor)index;
    }

    public void SetOurShape(int index)
    {
        ourShape = (OurShape)index;
    }

    public void SetOurCharge(int index)
    {
        ourCharge = (OurCharge)index;
    }

    // ===== PEACE SIGN CONFIRMATION =====

    public bool ConfirmFlag()
    {
        bool correct =
            ourColor == expectedColor &&
            ourShape == expectedShape &&
            ourCharge == expectedCharge;

        Debug.Log(correct ? "FLAG CORRECT" : "FLAG INCORRECT");
        return correct;
    }

    public void ConfirmFlag_UnityEvent()
    {
        bool correct = ConfirmFlag();

        if (correct)
        {
            // Solve logic here (or call another script)
            Debug.Log("MODULE SOLVED");
        }
        else
        {
            // Strike logic here
            Debug.Log("STRIKE");
        }
    }
}
