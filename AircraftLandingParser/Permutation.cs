using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearOrdering.Parser
{
    public class Dados
    {
        public int idNodo;
        public int numNodos;
        public int[,] matrizCusto;

        public Dados()
        {
        }

        public Dados(int nodos)
        {
            this.numNodos = nodos;
            this.matrizCusto = new int[nodos, nodos];
        }
    }
}
