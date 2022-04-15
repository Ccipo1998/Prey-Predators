using System.Collections;
using UnityEngine;

namespace Assets.HierarchicalStateMachine
{
    public class prova : MonoBehaviour
    {
        private HSM hsm;

        // Use this for initialization
        void Start()
        {
            HSMstate A = new HSMstate("A", 2);
            HSMstate B = new HSMstate("B", 2);
            HSM C = new HSM("C", B, 1);
            HSMstate D = new HSMstate("D", 1);
            HSMstate E = new HSMstate("E", 1);
            HSMstate F = new HSMstate("F", 1);

            HSMtransition prima = new HSMtransition("uno", uno);
            B.AddTransition(prima, A);

            HSMtransition seconda = new HSMtransition("due", due);
            A.AddTransition(seconda, B);

            HSMtransition terza = new HSMtransition("tre", tre);
            C.AddTransition(terza, D);

            HSMtransition quarta = new HSMtransition("quattro", quattro);
            D.AddTransition(quarta, C);

            HSMtransition quinta = new HSMtransition("cinque", cinque);
            B.AddTransition(quinta, E);

            HSMtransition sesta = new HSMtransition("sei", sei);
            E.AddTransition(sesta, C);

            HSMtransition settima = new HSMtransition("sette", sette);
            C.AddTransition(settima, F);

            HSMtransition ottava = new HSMtransition("otto", otto);
            F.AddTransition(ottava, A);

            A.AddParent(C);
            B.AddParent(C);

            hsm = new HSM("HSM", C, 0);

            C.AddParent(hsm);
            D.AddParent(hsm);
            E.AddParent(hsm);
            F.AddParent(hsm);
        }

        private void FixedUpdate()
        {
            hsm.Update();
        }

        public bool uno()
        {
            return false;
        }
        public bool due()
        {
            return true;
        }
        public bool tre()
        {
            return false;
        }
        public bool quattro()
        {
            return true;
        }
        public bool cinque()
        {
            return true;
        }
        public bool sei()
        {
            return true;
        }
        public bool sette()
        {
            return false;
        }
        public bool otto()
        {
            return true;
        }
    }
}