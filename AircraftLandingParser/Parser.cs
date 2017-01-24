using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace LinearOrdering.Parser
{
    public class Parser
    {
        public Dados parametrosProblema;
        int nNodos;        
        FileManager fm;

        public Parser(string nomeArquivo)
        {
            
            fm = new FileManager(nomeArquivo);
        }

        public void InputLinearOrderingFile()
        {
            List<string> incoming = new List<string>();
            
            // pega número de aviões.
            incoming = fm.ReadFileLine();
            nNodos = Convert.ToInt32(incoming[0]);

            parametrosProblema = new Dados(nNodos);

            int linha = 0;
            while (linha < nNodos)
            {
                incoming = fm.ReadFileLine();

                for (int i = 0; i < incoming.Count; i++)
                {
                    parametrosProblema.matrizCusto[linha, i] = Convert.ToInt32(incoming[i]);
                }

                linha++;
            }           
           
        }

    }
}
