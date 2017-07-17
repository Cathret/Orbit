using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Runtime;
using UnityEngine;
using Orbit.Entity;

public class TowerAIManager : MonoBehaviour
{
    public static TowerAIManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<TowerAIManager>();
            return _instance;
        }
    }
    private static TowerAIManager _instance;

    #region Members
    private OpponentManager _opponentManager = null;

    // TODO: should find a way to optimize that. Also need to have a "override" so does not have to get pair, and also may change "uint" when AOpponentController gets hit
    private Dictionary<AOpponentController, uint> _opponentsDictionaryTopRight= new Dictionary<AOpponentController, uint>();
    private Dictionary<AOpponentController, uint> _opponentsDictionaryTopLeft = new Dictionary<AOpponentController, uint>();
    private Dictionary<AOpponentController, uint> _opponentsDictionaryBottomRight = new Dictionary<AOpponentController, uint>();
    private Dictionary<AOpponentController, uint> _opponentsDictionaryBottomLeft = new Dictionary<AOpponentController, uint>();
    #endregion

    #region Unity functions
    private void Awake()
    {
        _opponentManager = OpponentManager.Instance;
    }
    #endregion

    #region Private functions
    private Dictionary<AOpponentController, uint> GetListForQuarter( GameCell.Quarter quarter )
    {
        switch ( quarter )
        {
            case GameCell.Quarter.TopRight:
                return _opponentsDictionaryTopRight;
            case GameCell.Quarter.TopLeft:
                return _opponentsDictionaryTopLeft;
            case GameCell.Quarter.BottomRight:
                return _opponentsDictionaryBottomRight;
            case GameCell.Quarter.BottomLeft:
                return _opponentsDictionaryBottomLeft;
            default:
                throw new ArgumentOutOfRangeException( "quarter", quarter, null );
        }
    }
    #endregion

    #region Public functions
    // TODO: remove power and find another way to notify (see TODO on Dictionaries)
    public bool FindBestOpponent( GameCell cell, out Vector3 target, uint power )
    {
        AOpponentController bestOpponentController;
        List<AOpponentController> opponentList = _opponentManager.GetOpponentsInQuarter( cell.QuarterPosition );
        if ( opponentList.Count <= 0 )
        {
            target = new Vector3();
            return false;
        }
        Dictionary<AOpponentController, uint> usedDictionary = GetListForQuarter( cell.QuarterPosition );

        foreach ( AOpponentController opponentController in opponentList )
        {
            if ( !usedDictionary.ContainsKey( opponentController ) )
                usedDictionary.Add( opponentController, 0u );
        }

        usedDictionary = usedDictionary.Where( kv => kv.Key.Hp > kv.Value ).ToDictionary( p => p.Key, p => p.Value, usedDictionary.Comparer );
        
        if ( _opponentManager.FindClosestOpponentInList( usedDictionary.Keys, cell.transform, out bestOpponentController ) )
        {
            // Get target position
            target = bestOpponentController.transform.position;

            // Add power to uint so keep track if going to kill the unit or not
            KeyValuePair<AOpponentController, uint> keyValuePair = usedDictionary.Single( kv => kv.Key == bestOpponentController );
            usedDictionary[keyValuePair.Key] = keyValuePair.Value + power;

            return true;
        }

        target = new Vector3();
        return false;
    }
    #endregion
}
