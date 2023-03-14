//Fernando Cossio 01/03/2023  Name and date of creation is necessary 
//Diana Ramos 01/03/2023 renamed variables   Name of the last person edited, date and a very breif general explanation of what was changed.
//I made this code when I was drunk, only god knows how it works. Hours wasted here: 12.  

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{ //All code MUST be contained under namespace for our studio and our proyect 

    public class Example : MonoBehaviour //PascalCaseForClassNames
    {
        #region GeneralOrder
        /*
        Constant Fields
        Fields
        Constructors
        Finalizers(Destructors)
        Delegates
        Events
        Enums
        Interfaces(interface implementations)
        Properties
        Indexers
        Methods
        Structs
        Classes
         */
        #endregion

        #region ProtectionOrder
        /*
        public
        internal
        protected internal
        protected
        private

        For all levels of access
        static
        non-static
        */
        #endregion

        [HideInInspector] public float DamageToReceive; //All public variables must be hidden in the inspector, unless absolutelly necessary, 
        public float Speed; //PascalCaseOnPublicVariables

        [Header("Player")] //If using Serialized field, they MUST contain a Header even if only 1 variable is being used. 
        [Tooltip("Example tooltip")] //Tooltip can be used if wanted. 
        [SerializeField] private GameObject _playerController; //Public variables are discouraged. SerializedField are encouraged.  
        private GameObject _playerHand;  //_camelCaseForPrivateVariables starting with "_"

        #region UnityMethods
        public void Awake()
        {
            // Awake, Start, Update, FixedUpdate, LateUpdate and Prepare methods MUST come before any new method, in that specific order. 
            //This exception is specific to Unity.
        }

        public void Start()
        {

        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {

        }

        private void Prepare()
        {
            //All new methods MUST be private unless necessary.  
        }
        #endregion

        public void DoStuff()
        {
            //For methods, starting uppercase is mandatory. Verbs to name the methods is highly encouraged. Avoid any non-verb naming. 
        }

        //Private methods should be declare before any public methods. This to be in par with variable declaration. 
    }
}
