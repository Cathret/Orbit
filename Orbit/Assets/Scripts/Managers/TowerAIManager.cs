using System;
using System.Linq;
using System.Collections.Generic;
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

    #region Nested Class
    public class OpponentTarget
    {
        public OpponentTarget( AOpponentController opponentController )
        {
            OpponentController = opponentController;
        }
        
        #region Members
        public AOpponentController OpponentController
        {
            get { return _opponentController; }
            private set
            {
                if ( _opponentController )
                {
                    _opponentController.TriggerDestroy -= OnOpponentControllerDeath;
                    _opponentController = null;
                }

                if ( value == null ) return;

                _opponentController = value;
                _opponentController.TriggerDestroy += OnOpponentControllerDeath;
            }
        }
        private AOpponentController _opponentController = null;

        public uint DamagesTowards
        {
            get { return _damagesTowards; }
            private set { _damagesTowards = value; }
        }
        private uint _damagesTowards;

        public bool IsValid
        {
            get { return OpponentController != null; }
        }

        public bool NeedToBeFocused()
        {
            if ( OpponentController )
                return OpponentController.Hp > DamagesTowards;
            return false;
        }

        public void SetAsTargetFor( Projectile projectile )
        {
            // TODO: change to be a function and not a lambda. Need to have Sender as parameter
            projectile.TriggerDestroy += () =>
            {
                DamagesTowards -= projectile.Power;
            };
            DamagesTowards += projectile.Power;
        }
        #endregion

        #region Private functions
        private void OnOpponentControllerDeath()
        {
            _opponentController.TriggerDestroy -= OnOpponentControllerDeath;
            _opponentController = null;
        }
        #endregion
    }
    #endregion

    #region Members
    private OpponentManager _opponentManager = null;

    // TODO: should find a way to optimize that
    private List<OpponentTarget> _opponentsListTopRight = new List<OpponentTarget>();
    private List<OpponentTarget> _opponentsListTopLeft = new List<OpponentTarget>();
    private List<OpponentTarget> _opponentsListBottomRight = new List<OpponentTarget>();
    private List<OpponentTarget> _opponentsListBottomLeft = new List<OpponentTarget>();
    #endregion

    #region Unity functions
    private void Awake()
    {
        _opponentManager = OpponentManager.Instance;
    }
    #endregion

    #region Private functions
    private void CleanOneList( List<OpponentTarget> oneList )
    {
        oneList.RemoveAll( param => !param.IsValid );
    }

    private void CleanAllList()
    {
        CleanOneList( _opponentsListTopRight );
        CleanOneList( _opponentsListTopLeft );
        CleanOneList( _opponentsListBottomRight );
        CleanOneList( _opponentsListBottomLeft );
    }

    private bool GetListForQuarter( GameCell.Quarter quarter, out List<OpponentTarget> listOpponentTargets )
    {
        CleanAllList();

        switch ( quarter )
        {
            case GameCell.Quarter.TopRight:
                listOpponentTargets = _opponentsListTopRight;
                break;
            case GameCell.Quarter.TopLeft:
                listOpponentTargets = _opponentsListTopLeft;
                break;
            case GameCell.Quarter.BottomRight:
                listOpponentTargets = _opponentsListBottomRight;
                break;
            case GameCell.Quarter.BottomLeft:
                listOpponentTargets = _opponentsListBottomLeft;
                break;
            default:
                throw new ArgumentOutOfRangeException( "quarter", quarter, null );
        }

        return true;
    }
    #endregion

    #region Public functions
    // TODO: remove power and find another way to notify (see TODO on Dictionaries)
    public bool FindBestOpponent( GameCell cell, out OpponentTarget target )
    {
        AOpponentController bestOpponentController;
        List<AOpponentController> opponentList = _opponentManager.GetOpponentsInQuarter( cell.QuarterPosition );
        if ( opponentList.Count <= 0 )
        {
            target = new OpponentTarget( null );
            return false;
        }

        List<OpponentTarget> usedList;
        // Not checking if == false, will never happen without an exception
        GetListForQuarter( cell.QuarterPosition, out usedList );

        foreach ( AOpponentController opponentController in opponentList )
        {
            if ( !usedList.Exists( param => param.OpponentController == opponentController ) )
                usedList.Add( new OpponentTarget( opponentController ) );
        }

        List<OpponentTarget> needToBeFocusedList = usedList.Where( param => param.NeedToBeFocused() ).ToList();
        if ( _opponentManager.FindClosestOpponentInList( needToBeFocusedList.Select( param => param.OpponentController ), cell.transform, out bestOpponentController ) )
        {
            target = needToBeFocusedList.Find( param => param.OpponentController == bestOpponentController );
            return true;
        }

        target = new OpponentTarget( null );
        return false;
    }
    #endregion
}
