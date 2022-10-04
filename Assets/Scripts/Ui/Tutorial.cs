using System;
using Ui.Enum;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial Instance;
        public static TutorialState State = TutorialState.Step0_MainMenu;

        public GameObject tutorialStep1;
        public GameObject tutorialStep2;
        public GameObject tutorialStep3;
        public GameObject tutorialStep4;
        public GameObject tutorialStep5;
        public GameObject tutorialStep6;
        public GameObject tutorialStep7;
        public GameObject tutorialStep8;
        public GameObject tutorialStep9;
        public GameObject tutorialStep10;
        public GameObject recipeUi;
        public GameObject mealUi;
        public GameObject timerUi;
        public GameObject rageMeterUi;

        private void Awake()
        {
            Instance = this;
        }

        public void StartTutorial()
        {
            State = TutorialState.Step1_Welcome;
            tutorialStep1.SetActive(true);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (State == TutorialState.Step1_Welcome && context.performed)
            {
                tutorialStep1.SetActive(false);
                tutorialStep2.SetActive(true);
            
                recipeUi.SetActive(true);
                mealUi.SetActive(true);

                State = TutorialState.Step2_SwitchToBurger;
                
                return;
            }
            
            if (State == TutorialState.Step9_Congrats && context.performed)
            {
                tutorialStep9.SetActive(false);
                tutorialStep10.SetActive(true);
            
                State = TutorialState.Step10_LastAdvice;
                
                return;
            }
            
            if (State == TutorialState.Step10_LastAdvice && context.performed)
            {
                tutorialStep10.SetActive(false);
            
                State = TutorialState.Step11_Done;
                
                timerUi.SetActive(true);
                rageMeterUi.SetActive(true);
            }
        }

        public void BurgerRecipeSelected()
        {
            if (State != TutorialState.Step2_SwitchToBurger)
            {
                return;
            }
            
            tutorialStep2.SetActive(false);
            tutorialStep3.SetActive(true);
            
            State = TutorialState.Step3_LockRecipe;
        }

        public void LockedRecipe()
        {
            if (State != TutorialState.Step3_LockRecipe)
            {
                return;
            }
            
            tutorialStep3.SetActive(false);
            tutorialStep4.SetActive(true);
            
            State = TutorialState.Step4_TakeBun;
        }

        public void TakenBun()
        {
            if (State != TutorialState.Step4_TakeBun)
            {
                return;
            }
            
            tutorialStep4.SetActive(false);
            tutorialStep5.SetActive(true);
            
            State = TutorialState.Step5_BunToOven;
        }

        public void BunInOven()
        {
            if (State != TutorialState.Step5_BunToOven)
            {
                return;
            }

            tutorialStep5.SetActive(false);
            tutorialStep6.SetActive(true);
            
            State = TutorialState.Step6_BringBunToPlate;
        }

        public void PlacedBunOnPlate()
        {
            if (State != TutorialState.Step6_BringBunToPlate)
            {
                return;
            }

            tutorialStep6.SetActive(false);
            tutorialStep7.SetActive(true);
            
            State = TutorialState.Step7_TakeSalad;
        }
        
        public void LettuceTaken()
        {
            if (State != TutorialState.Step7_TakeSalad)
            {
                return;
            }

            tutorialStep7.SetActive(false);
            tutorialStep8.SetActive(true);
            
            State = TutorialState.Step8_CutSalad;
        }

        public void PlacedLettuceOnPlate()
        {
            if (State != TutorialState.Step8_CutSalad)
            {
                return;
            }

            tutorialStep8.SetActive(false);
            tutorialStep9.SetActive(true);
            
            State = TutorialState.Step9_Congrats;
        }
    }
}