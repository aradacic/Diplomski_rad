using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

public class ChessAI : MonoBehaviour
{
    const int WIDTH = 8;
    const int HEIGTH = 8;

    GridManager gm;
    public List<Piece> myPieces = new List<Piece>();
    public List<Piece> aiPieces = new List<Piece>();
    public List<Vector2Int> listOfPossibleMoves = new List<Vector2Int>();
    public bool waitFlag = false;

    // varijable za evaluaciju ploce
    int totalScore = 0;
    int currPieceValue;
    int maxDepth = 3;

    // za odigrati
    Piece chosenPiece = null;
    int xValue;
    int yValue;

    void Start()
    {
        gm = gameObject.GetComponent<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!gm.isWhitesTurn && !waitFlag)
        {
            
            waitFlag = true;
            
           
            StartCoroutine(DecideForMove());
        }
    }

    IEnumerator DecideForMove()
    {
        totalScore = 0;
        //chosenPiece = null;
        yield return new WaitForSeconds(.5f);
        gm.availableMoves.Clear();

        if (gm.moveCounter == 1)
        {
            gm.currChoice = gm.chessPieces[3, 6];
            gm.MoveToAI(gm.currChoice, 3, 4);
        }
        else if (gm.moveCounter == 2)
        {
            gm.currChoice = gm.chessPieces[1, 7];
            gm.MoveToAI(gm.currChoice, 2, 5);
        }
        else if (gm.moveCounter == 3)
        {
            gm.currChoice = gm.chessPieces[6, 7];
            gm.MoveToAI(gm.currChoice, 5, 5);
        }
        else if (gm.moveCounter > 3)
        {
            int depth = maxDepth - 1;
            int bestVAlue = AlphaBeta(depth, true, System.Int32.MinValue, System.Int32.MaxValue);
            Debug.LogError(bestVAlue);
            gm.currChoice = chosenPiece;
         
            gm.availableMoves = chosenPiece.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
            gm.specialMove = chosenPiece.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
            gm.PreventCheck();
            gm.MoveToAI(chosenPiece, xValue, yValue);
            myPieces.Clear();
            aiPieces.Clear();
        }
        
       
        
        /*
        List<Piece> myPieces = new List<Piece>();
        for (int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGTH; y++)
            {
                if (gm.chessPieces[x, y] != null)
                {
                    if (gm.chessPieces[x, y].team == 1)
                    {
                        myPieces.Add(gm.chessPieces[x, y]);
                        totalScore += gm.chessPieces[x, y].value;
                    }
                }
            }
        }
        Debug.Log(totalScore);
        
        /////////////////////////////////////////////////////////
        List<Piece> piecesWithMoves = new List<Piece>();
        for(int i = 0; i < myPieces.Count; i++)
        {
            gm.currChoice = myPieces[i];
            gm.availableMoves = gm.currChoice.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
            gm.specialMove = gm.currChoice.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
            gm.PreventCheck();
           
            if (gm.availableMoves.Count > 0)
            {
                piecesWithMoves.Add(myPieces[i]);
            }
        }
  
        if(piecesWithMoves.Count == 0)
        {
            gm.CheckMate(0);
        }
        ///////////////////////////////////////////////////////////
        gm.currChoice = piecesWithMoves[Random.Range(0, piecesWithMoves.Count)];
        gm.availableMoves = gm.currChoice.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
        gm.specialMove = gm.currChoice.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
        gm.PreventCheck();


        int randomMove = Random.Range(0, gm.availableMoves.Count);
        
       
        int aiMoveX = gm.availableMoves[randomMove].y - 1;
        int aiMoveY = gm.availableMoves[randomMove].x - 1;
        bool validMove = gm.MoveToAI(gm.currChoice, aiMoveX, aiMoveY);
        piecesWithMoves.Clear();
        */
      
       
    }

    int AlphaBeta(int depth, bool isAIMove, int alpha, int beta)
    {
       
        if (depth == 0)
        {
            int value = totalScore;
            return value;
        }

        if (isAIMove)
        {
            int value = System.Int32.MinValue;
            totalScore = 0;
            // uvatia sve AI figurice
            //////////////////////////////////////
            //List<Piece> myPieces = new List<Piece>();
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGTH; y++)
                {
                    if (gm.chessPieces[x, y] != null)
                    {
                        if (gm.chessPieces[x, y].team == 1)
                        {
                            aiPieces.Add(gm.chessPieces[x, y]);
                            totalScore += gm.chessPieces[x, y].value;
                        }
                    }
                }

            }
            ////////////////////////////////////////
           // Debug.Log(aiPieces.Count);
            //nadi sve figurice koje se mogu pomaknit
            /////////////////////////////////////////////////////////
            List<Piece> piecesWithMoves = new List<Piece>();
            for (int i = 0; i < aiPieces.Count; i++)
            {
                gm.currChoice = aiPieces[i];
                gm.availableMoves = gm.currChoice.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
                gm.specialMove = gm.currChoice.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
                gm.PreventCheck();

                if (gm.availableMoves.Count > 0)
                {
                    piecesWithMoves.Add(aiPieces[i]);
                }
            }
            Debug.LogWarning(piecesWithMoves.Count);
            //Debug.Log(piecesWithMoves.Count);
            //if (piecesWithMoves.Count == 0) // ako je gotovo
            //{
               // gm.CheckMate(0);
            //}
            ///////////////////////////////////////////////////////////
            
            // provjera svakoga
            ///////////////////////////////////////////////////////////
            foreach(Piece p in piecesWithMoves)
            {
                int actualX = p.currX;
                int actualY = p.currY;
                gm.currChoice = p;
                gm.availableMoves = gm.currChoice.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
                gm.specialMove = gm.currChoice.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
                gm.PreventCheck();
                
                foreach(Vector2Int move in gm.availableMoves)
                {
                   // Debug.LogWarning(gm.availableMoves.Count);
                    SimulateMove(p, move);
                   
                    int thisMoveValue = AlphaBeta(depth - 1, !isAIMove, alpha, beta);
                    UndoMove(p, actualX, actualY);

                    if (value < thisMoveValue)
                    {
                        value = thisMoveValue;

                        if(depth == maxDepth - 1)
                        {
                            chosenPiece = p;
                            xValue = move.y - 1;
                            yValue = move.x - 1;
                        }
                    }

                    if(value > alpha)
                    {
                        alpha = value;
                    }

                    if(beta <= alpha)
                    {
                        break;
                    }
                }

                if(beta <= alpha)
                {
                    break;
                }
            }

            return value;
        }
        else
        {
            int value = System.Int32.MaxValue;

            // dohvati sve igraceve figurice
            ////////////////////////////////////////
           // List<Piece> myPieces = new List<Piece>();
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGTH; y++)
                {
                    if (gm.chessPieces[x, y] != null)
                    {
                        if (gm.chessPieces[x, y].team == 0)
                        {
                            myPieces.Add(gm.chessPieces[x, y]);
                            totalScore += gm.chessPieces[x, y].value;
                        }
                    }
                }

            }
            //////////////////////////////////////////
            List<Piece> piecesWithMoves = new List<Piece>();
            for (int i = 0; i < myPieces.Count; i++)
            {
                gm.currChoice = myPieces[i];
                gm.availableMoves = gm.currChoice.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
                gm.specialMove = gm.currChoice.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
                gm.PreventCheck();

                if (gm.availableMoves.Count > 0)
                {
                    piecesWithMoves.Add(myPieces[i]);
                }
            }

            //if (piecesWithMoves.Count == 0) // ako je gotovo
            //{
             //   gm.CheckMate(0);
            //}
            // provjera za svakoga
            //////////////////////////////////////////
            foreach (Piece p in piecesWithMoves)
            {
               // Debug.Log("1");
                gm.currChoice = p;
                gm.availableMoves = gm.currChoice.GetAvailableMoves(ref gm.chessPieces, WIDTH, HEIGTH);
                gm.specialMove = gm.currChoice.GetSpecialMoves(ref gm.chessPieces, ref gm.moveList, ref gm.availableMoves);
                gm.PreventCheck();
               
                foreach (Vector2Int move in gm.availableMoves)
                {
                    int actualX = p.currX;
                    int actualY = p.currY;
                    SimulateMove(p, move);
                    int thisMoveValue = AlphaBeta(depth - 1, !isAIMove, alpha, beta);
                    UndoMove(p, actualX, actualY);

                    if (value > thisMoveValue)
                    {
                        value = thisMoveValue;
                    }

                    if (value < beta)
                    {
                        beta = value;
                    }

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                if (beta <= alpha)
                {
                    break;
                }
            }
            //////////////////////////////////////////
            
            return value;
        }
    }

    void SimulateMove(Piece piece, Vector2Int move)
    {
        int simulateX = move.y - 1;
        int simulateY = move.x - 1;

        Piece[,] simulation = new Piece[WIDTH, HEIGTH];

        // simuliraj taj potez
        simulation[piece.currX, piece.currY] = null;
        piece.currX = simulateX;
        piece.currY = simulateY;
        simulation[simulateX, simulateY] = piece;

    }

    void UndoMove(Piece p, int actualX, int actualY)
    {
        p.currX = actualX;
        p.currY = actualY;
    }
}
