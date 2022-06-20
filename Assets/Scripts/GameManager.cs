using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Core Mechanics")]
    [Range(3, 8)]
    [SerializeField] private int numOfPieces;
    [SerializeField] private Peg startPeg, midPeg, goalPeg;
    [SerializeField] private Piece[] pieces;
    public int score;

    [Header("UI")]
    [SerializeField] private TMP_Text discNum;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text winScore;
    [SerializeField] private TMP_Text highScore;

    [Header("Audio")]
    [SerializeField] private AudioSource interact, win;
    [SerializeField] private AudioClip[] victoryClips;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private bool hasWon;
    // Start is called before the first frame update
    void Start()
    {
        startPeg.pieces = new Stack<Piece>();
        midPeg.pieces = new Stack<Piece>();
        goalPeg.pieces = new Stack<Piece>();

        discNum.text = numOfPieces.ToString();
        score = 0;
        hasWon = false;
        InitializePieces();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score + " moves";

        if (goalPeg.pieces.Count == numOfPieces && !hasWon)
        {
            hasWon = true;
            StartCoroutine(Win());
        }

    }

    public void IncreaseDiscs()
    {
        interact.Stop();
        interact.Play();
        if (numOfPieces < 8)
            numOfPieces++;
        else
            numOfPieces = 3;
        discNum.text = numOfPieces.ToString();
        Reset();
    }

    public void DecreaseDiscs()
    {
        interact.Stop();
        interact.Play();
        if (numOfPieces > 3)
            numOfPieces--;
        else
            numOfPieces = 8;
        discNum.text = numOfPieces.ToString();
        Reset();
    }

    public void Reset()
    {
        interact.Stop();
        interact.Play();

        winPanel.SetActive(false);

        ClearStacks();
        ResetPieces();
        InitializePieces();
        
        score = 0;
        hasWon = false;
    }

    void InitializePieces()
    {
        for (int i = pieces.Length - numOfPieces; i < pieces.Length; i++)
        {
            if (startPeg.CheckPushPiece(pieces[i]))
            {
                pieces[i].gameObject.SetActive(true);
                pieces[i].transform.position = startPeg.GetPiecePosition();

            }
        }
        score = 0;
    }

    void ClearStacks()
    {
        startPeg.ResetPeg();
        midPeg.ResetPeg();
        goalPeg.ResetPeg();
    }

    void ResetPieces()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].currentPeg = null;
            pieces[i].isTop = false;
            pieces[i].isGrabbed = false;
            pieces[i].gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        interact.Stop();
        interact.Play();
        Application.Quit(0);
    }

    IEnumerator Win()
    {
        win.clip = victoryClips[Random.Range(0, victoryClips.Length)];
        win.Stop();
        win.Play();

        if (PlayerPrefs.HasKey("Highscore_" + numOfPieces))
        {
            int hs = PlayerPrefs.GetInt("Highscore_" + numOfPieces, 0);
            if (score < hs)
            {
                PlayerPrefs.SetInt("Highscore_" + numOfPieces, score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Highscore_" + numOfPieces, score);
        }
        yield return new WaitForSeconds(3f);

        winPanel.gameObject.SetActive(true);
        winScore.text = "Completed in " + score + " moves.";
        highScore.text = "Best Score for " + numOfPieces + " discs: " + PlayerPrefs.GetInt("Highscore_" + numOfPieces, 0) + " moves";
        yield break;

    }

    public void ResetHiScore()
    {
        if (PlayerPrefs.HasKey("Highscore_" + numOfPieces))
        {
            PlayerPrefs.DeleteKey("Highscore_" + numOfPieces);
        }
    }
}
