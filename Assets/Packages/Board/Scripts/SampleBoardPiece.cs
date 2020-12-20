using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBoardPiece : MonoBehaviour
{
    public SO_BoardSquare so_pieceData;

    [ContextMenu(nameof(UpdateSize))]
    public void UpdateSize() => so_pieceData.UpdateSize();
}
