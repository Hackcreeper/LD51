﻿using System.Linq;
using Ai.Enum;
using Cooking;
using Feeding;
using UnityEngine;

namespace Ai
{
    public class YeetBot : Bot
    {
        public YeetPlatform[] yeetPlatforms;
        public MonsterFeeder feeder;

        private YeetBotState _yeetState = YeetBotState.Waiting;
        private Plate _targetPlate;
        private YeetPlatform _targetYeet;
        private float _yeetDelay;

        protected override void Update()
        {
            if (Sucked)
            {
                transform.position = Vector3.Lerp(
                    transform.position, SuckTarget, 5f * Time.deltaTime
                );
                return;
            }
            
            _yeetDelay -= Time.deltaTime;
            
            switch (_yeetState)
            {
                case YeetBotState.Waiting:
                    HandleWaiting();
                    break;

                case YeetBotState.PickingUpPlate:
                    HandlePickingUpPlate();
                    break;
                
                case YeetBotState.MovePlateToYeetStation:
                    HandleMovePlateToYeetStation();
                    break;
                
                case YeetBotState.Yeet:
                    HandleYeet();
                    break;
            }
        }
        
        private void HandleWaiting()
        {
            var mealToYeet = yeetPlatforms.FirstOrDefault(platform => !platform.IsFree());
            if (mealToYeet && feeder.HowMuchTimeLeft() < 2f && _yeetDelay <= 0f)
            {
                Agent.SetDestination(mealToYeet.transform.position);
                _targetYeet = mealToYeet;
                _yeetState = YeetBotState.Yeet;
                return;
            }

            var finishedPlate = FindObjectsOfType<Plate>()
                .Where(plate => plate.GetPickableMeal() != null && !plate.IsPreparedForTheYeet() && !plate.IsPickedUp())
                .FirstOrDefault(plate => plate.GetPickableMeal().IsReadyToDeliver());

            if (!finishedPlate)
            {
                return;
            }

            Agent.SetDestination(finishedPlate.transform.position);
            _targetPlate = finishedPlate;
            _yeetState = YeetBotState.PickingUpPlate;
        }

        private void HandlePickingUpPlate()
        {
            if (_targetPlate.IsPickedUp())
            {
                Agent.SetDestination(StartPosition);
                _targetPlate = null;
                _yeetState = YeetBotState.Waiting;
                return;
            }
            
            if (Agent.remainingDistance > 0.5f)
            {
                return;
            }

            _targetPlate.Interact(Player);
            _targetPlate.PrepareForYeet();
            _yeetState = YeetBotState.MovePlateToYeetStation;

            var station = yeetPlatforms.First(platform => platform.IsFree());
            Agent.SetDestination(station.standingTarget.position);
            _targetYeet = station;
        }
        
        private void HandleMovePlateToYeetStation()
        {
            if (Agent.remainingDistance > 0.2f)
            {
                return;
            }

            _targetYeet.Interact(Player);
            _yeetState = YeetBotState.Waiting;

            Agent.SetDestination(StartPosition);
        }
        
        private void HandleYeet()
        {
            if (Agent.remainingDistance > 0.5f)
            {
                return;
            }

            _targetYeet.Interact(Player);
            _yeetState = YeetBotState.Waiting;
            _yeetDelay = 5f;

            Agent.SetDestination(StartPosition);
        }
    }
}