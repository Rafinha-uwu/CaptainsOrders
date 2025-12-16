using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class CannonLogic : MonoBehaviour
{
    // ===== ENUMS =====

    public enum BowColor { Red, White, Blue }
    public enum ShipPart { Deck, Stern, Bow, Mast }
    public enum FlagColor { Blue, Red, Yellow, Black }

    // ===== UI =====

    public TMP_Text otherShipText;

    // ===== OTHER SHIP (RANDOMIZED) =====

    private int masts;          // 2–4
    private int cannons;        // 3–5
    private BowColor bowColor;

    // ===== EXPECTED RESULT =====

    private List<ShipPart> expectedSequence = new();
    private bool requiresPeaceFallback;

    // ===== PLAYER INPUT =====

    private List<ShipPart> playerShots = new();

    void Start()
    {
        RandomizeOtherShip();
        ComputeFiringSolution();
    }

    // ===== RANDOMIZATION =====

    void RandomizeOtherShip()
    {
        masts = Random.Range(2, 5);
        cannons = Random.Range(3, 6);
        bowColor = (BowColor)Random.Range(0, 3);

        WriteOtherShipText();
    }

    void WriteOtherShipText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Masts: {masts}");
        sb.AppendLine($"Cannons Facing You: {cannons}");
        sb.AppendLine($"Bow Color: {bowColor}");

        if (otherShipText != null)
            otherShipText.text = sb.ToString();

        Debug.Log(sb.ToString());
    }

    // ===== RULE ENGINE =====

    void ComputeFiringSolution()
    {
        expectedSequence.Clear();
        requiresPeaceFallback = false;

        // Rule A
        if (masts == 2 && bowColor == BowColor.Blue && cannons == 3)
        {
            expectedSequence.AddRange(new[] {
                ShipPart.Mast,
                ShipPart.Bow,
                ShipPart.Deck
            });
            return;
        }

        // Rule B
        if (masts == 4 && cannons == 5)
        {
            expectedSequence.AddRange(new[] {
                ShipPart.Deck,
                ShipPart.Stern,
                ShipPart.Bow
            });
            return;
        }

        // Rule C
        if (bowColor == BowColor.Red && masts == 3 && cannons == 4)
        {
            expectedSequence.AddRange(new[] {
                ShipPart.Bow,
                ShipPart.Mast,
                ShipPart.Stern
            });
            return;
        }

        // Rule D
        if (bowColor == BowColor.White && cannons == 3)
        {
            expectedSequence.AddRange(new[] {
                ShipPart.Deck,
                ShipPart.Mast,
                ShipPart.Bow
            });
            return;
        }

        // Rule E
        if (cannons == 4 && bowColor == BowColor.Blue)
        {
            expectedSequence.AddRange(new[] {
                ShipPart.Stern,
                ShipPart.Deck,
                ShipPart.Mast
            });
            return;
        }

        // Fallback
        requiresPeaceFallback = true;
    }

    // ===== PLAYER INPUT =====

    public void RegisterShot(int partIndex)
    {
        playerShots.Add((ShipPart)partIndex);
    }

    public void ResetShots()
    {
        playerShots.Clear();
    }

    // ===== PEACE SIGN FALLBACK =====

    public bool ResolvePeaceFallback(int flagColorIndex)
    {
        if (!requiresPeaceFallback)
            return false;

        ShipPart expected = (FlagColor)flagColorIndex switch
        {
            FlagColor.Blue => ShipPart.Bow,
            FlagColor.Red => ShipPart.Stern,
            FlagColor.Yellow => ShipPart.Deck,
            FlagColor.Black => ShipPart.Mast,
            _ => ShipPart.Deck
        };

        bool correct = playerShots.Count == 1 && playerShots[0] == expected;
        Debug.Log(correct ? "CANNON SOLVED" : "CANNON STRIKE");
        return correct;
    }

    // ===== NORMAL CONFIRM =====

    public bool ConfirmSequence()
    {
        if (requiresPeaceFallback)
            return false;

        if (playerShots.Count != expectedSequence.Count)
            return false;

        for (int i = 0; i < expectedSequence.Count; i++)
        {
            if (playerShots[i] != expectedSequence[i])
                return false;
        }

        Debug.Log("CANNON SOLVED");
        return true;
    }

    // ===== UNITY EVENT WRAPPERS =====

    public void ConfirmSequence_UnityEvent()
    {
        ConfirmSequence();
    }

    public void ResolvePeaceFallback_UnityEvent(int flagColorIndex)
    {
        ResolvePeaceFallback(flagColorIndex);
    }
}
