using UnityEngine;
using System.Collections.Generic;

public class CannonLogic : MonoBehaviour
{
    // ================= ENUMS =================

    public enum ShipPart
    {
        Deck,   // 0
        Stern,  // 1
        Bow,    // 2
        Mast    // 3
    }

    public enum BowColor
    {
        Red,    // 0
        White,  // 1
        Blue    // 2
    }

    public enum FlagColor
    {
        Blue,   // 0
        Red,    // 1
        Yellow, // 2
        Black   // 3
    }

    // ================= VISUAL REFERENCES =================

    [Header("Other Ship - Masts (2–4)")]
    public GameObject[] otherMastObjects; // size 4

    [Header("Other Ship - Cannons Facing You (2–5)")]
    public GameObject[] otherCannonObjects; // size 5

    [Header("Other Ship - Bow (Material Based)")]
    public Renderer bowRenderer;
    public Material[] bowMaterials; // size 3 (Red, White, Blue)

    [Header("Fallback Flag (Material Based)")]
    public GameObject flagObject;
    public Renderer flagRenderer;
    public Material[] flagMaterials; // size 4 (Blue, Red, Yellow, Black)

    // ================= OTHER SHIP DATA =================

    private int mastCount;     // 2–4
    private int cannonCount;   // 2–5
    private BowColor bowColor;

    // ================= SOLUTION =================

    private List<ShipPart> expectedSequence = new();
    private ShipPart expectedSingleShot;
    private bool requiresFlag;
    private bool hasRaisedFlag;

    // ================= PLAYER STATE =================

    private ShipPart currentAim;
    private int shotIndex;

    void Start()
    {
        RandomizeOtherShip();
        ComputeSolution();
    }

    // ====================================================
    // RANDOMIZATION + VISUALS
    // ====================================================

    void RandomizeOtherShip()
    {
        mastCount = Random.Range(2, 5);   // 2–4
        cannonCount = Random.Range(2, 6); // 2–5
        bowColor = (BowColor)Random.Range(0, 3);

        ApplyVisuals();
    }

    void ApplyVisuals()
    {
        // Masts
        for (int i = 0; i < otherMastObjects.Length; i++)
            otherMastObjects[i].SetActive(i < mastCount);

        // Cannons
        for (int i = 0; i < otherCannonObjects.Length; i++)
            otherCannonObjects[i].SetActive(i < cannonCount);

        // Bow material
        bowRenderer.material = bowMaterials[(int)bowColor];

        // Flag hidden initially
        flagObject.SetActive(false);
    }

    // ====================================================
    // RULE ENGINE (A–E)
    // ====================================================

    void ComputeSolution()
    {
        expectedSequence.Clear();
        requiresFlag = false;
        hasRaisedFlag = false;
        shotIndex = 0;

        // Rule A
        if (mastCount == 2 && bowColor == BowColor.Blue && cannonCount == 3)
        {
            expectedSequence.AddRange(new[]
            {
                ShipPart.Mast,
                ShipPart.Bow,
                ShipPart.Deck
            });
            return;
        }

        // Rule B
        if (mastCount == 4 && cannonCount == 5)
        {
            expectedSequence.AddRange(new[]
            {
                ShipPart.Deck,
                ShipPart.Stern,
                ShipPart.Bow
            });
            return;
        }

        // Rule C
        if (bowColor == BowColor.Red && mastCount == 3 && cannonCount == 4)
        {
            expectedSequence.AddRange(new[]
            {
                ShipPart.Bow,
                ShipPart.Mast,
                ShipPart.Stern
            });
            return;
        }

        // Rule D
        if (bowColor == BowColor.White && cannonCount == 3)
        {
            expectedSequence.AddRange(new[]
            {
                ShipPart.Deck,
                ShipPart.Mast,
                ShipPart.Bow
            });
            return;
        }

        // Rule E
        if (cannonCount == 4 && bowColor == BowColor.Blue)
        {
            expectedSequence.AddRange(new[]
            {
                ShipPart.Stern,
                ShipPart.Deck,
                ShipPart.Mast
            });
            return;
        }

        // Final Condition
        requiresFlag = true;
    }

    // ====================================================
    // PLAYER INPUT
    // ====================================================

    public void SetCannonDirection(int index)
    {
        currentAim = (ShipPart)index;
    }

    // ====================================================
    // FIRE (SEQUENCE OR FLAG)
    // ====================================================

    public void Fire()
    {
        Puzzle puzzle = GetComponent<Puzzle>();

        // ----- FLAG MODE -----
        if (requiresFlag)
        {
            if (!hasRaisedFlag)
            {
                Debug.Log("Raise the flag first.");
                return;
            }

            if (currentAim != expectedSingleShot)
            {
                Debug.Log("WRONG FALLBACK SHOT"); puzzle.FailPuzzle();
                return;
            }

            Debug.Log("CANNON PUZZLE SOLVED (FLAG)"); puzzle.CompletePuzzle();
            return;
        }

        // ----- SEQUENCE MODE -----
        if (shotIndex >= expectedSequence.Count)
            return;

        if (currentAim != expectedSequence[shotIndex])
        {
            Debug.Log("WRONG SHOT");
            puzzle.FailPuzzle();
            return;
        }

        shotIndex++;
        Debug.Log("Correct shot");

        if (shotIndex == expectedSequence.Count)
            Debug.Log("CANNON PUZZLE SOLVED"); puzzle.CompletePuzzle();
    }

    // ====================================================
    // PEACE SIGN / FLAG
    // ====================================================

    public void PeaceSignTriggered()
    {
        Puzzle puzzle = GetComponent<Puzzle>();

        if (!requiresFlag)
        {
            Debug.Log("FLAG NOT REQUIRED — PENALTY"); puzzle.FailPuzzle();
            return;
        }

        if (hasRaisedFlag)
            return;

        hasRaisedFlag = true;

        FlagColor flag = (FlagColor)Random.Range(0, 4);
        ApplyFlag(flag);

        expectedSingleShot = flag switch
        {
            FlagColor.Blue => ShipPart.Bow,
            FlagColor.Red => ShipPart.Stern,
            FlagColor.Yellow => ShipPart.Deck,
            FlagColor.Black => ShipPart.Mast,
            _ => ShipPart.Deck
        };
    }

    void ApplyFlag(FlagColor flag)
    {
        flagObject.SetActive(true);
        flagRenderer.material = flagMaterials[(int)flag];
    }
}
